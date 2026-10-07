# GdsMapControl 개발자 분석서

> 성능 개선 작업 순서와 완료 기준은 `GdsMapControl_성능개선_우선순위.md`를 참고한다.

## 1. 문서 목적

이 문서는 `GdsMapControl` 프로젝트의 GDSII 파일 파싱과 OpenGL 도면 표시 구조를 개발자가 유지보수하거나 확장할 수 있도록 정리한 자료이다.

분석 대상 경로는 다음과 같다.

`D:\1_프로젝트 문서\18.스테츠칩팩_POC\ChippacPOC\GdsMapControl`

프로젝트 명칭과 구현 대상은 GDP Map이 아니라 GDS Map이다. GDS는 반도체 레이아웃 도면에 널리 사용되는 GDSII 형식이다.

## 2. 프로젝트 개요

| 구분 | 내용 |
|---|---|
| 실행 형태 | .NET Framework 4.8 WinForms |
| 화면 엔진 | OpenTK 3.3.3 / OpenGL |
| 데이터베이스 | Oracle Managed Data Access 19.18.0 |
| 주 기능 | GDSII 읽기, 레이어별 도면 표시, 확대/이동/선택, 불량 표시, 일부 요소 Oracle 저장과 조회 |
| 빌드 호환성 | C# 7.3 기준 빌드 확인 완료 |

프로젝트는 하나의 WinForms 실행 프로그램으로 구성된다. `Form1`이 입력과 화면 조작을 담당하고 `GdsMapControl`이 OpenGL 렌더링을 담당한다.

```text
사용자 GDS 파일 선택
        |
        v
GdsReader.Read
        |
        v
GdsLibrary / GdsStructure / GdsLayer / GdsElement
        |
        v
GdsMapControl.ShowStructure
        |
        +-- SREF를 실제 좌표로 평면화
        +-- 화면용 GlSceneItem 생성
        +-- GPU Vertex 목록 생성
        |
        v
OpenGL VBO 업로드
        |
        v
OnPaint에서 레이어별 도형 출력
```

## 3. 시작점과 화면 구성

### `Program.cs`

애플리케이션 시작점이다. WinForms 시각 스타일을 설정하고 `Form1`을 실행한다.

### `Form1.cs`

업무 기능과 `GdsMapControl`을 연결하는 화면 코드이다.

| 메서드 | 역할 |
|---|---|
| `Form1_Load` | 정사각형 요소만 선택하도록 `OnlySelectSquareItems`를 설정 |
| `button1_Click` | GDS 파일 선택, 파싱, 기본 레이어 색상 적용, 도면 표시, 요소 Grid와 레이어 목록 생성 |
| `btnSave_Click` | 현재 Structure의 레이어 요소를 Oracle에 저장 |
| `btnLoad_Click` | Oracle에서 고정된 GDS 순번과 Structure명을 기준으로 요소를 읽어 표시 |
| `btnLayer_Click` | 체크 목록의 레이어 표시 상태를 화면에 반영 |

`button1_Click`은 C# 7.3 호환 `using` 블록으로 파일 선택 창을 생성한다. 현재 시험 목적의 `Defect` 2건도 이 메서드에서 직접 추가한다. 운영 데이터 연계가 필요한 경우 이 하드코딩된 시험 데이터는 별도 서비스 또는 조회 로직으로 분리해야 한다.

### `Form1.Designer.cs`

화면 배치를 담당한다.

| 화면 요소 | 역할 |
|---|---|
| `Load from File` 버튼 | GDS 파일 파싱 시작 |
| `Save DB` 버튼 | Boundary와 Path 저장 |
| `Load DB` 버튼 | DB 저장 요소 조회 |
| `checkedListBox1` | 레이어별 표시 여부 선택 |
| `Redraw` 버튼 | 선택한 레이어 상태 반영 |
| `map` | `GdsMapControl` OpenGL 도면 영역 |
| `dataGridView1` | 요소의 레이어, 종류, 경계, 텍스트 또는 Path 폭 표시 |

## 4. GDSII 파싱 구조

### 4.1 GDSII 데이터 모델

| 파일 | 클래스 | 역할 |
|---|---|---|
| `GdsLibrary.cs` | `GdsLibrary` | 라이브러리명, User Unit, Database Unit, Structure 목록 보관 |
| `GdsStructure.cs` | `GdsStructure` | 하나의 Cell 또는 Block. 이름, 레이어, 불량 목록, 전체 경계 보관 |
| `GdsLayer.cs` | `GdsLayer`, `GdsLayerList` | Layer ID별 요소 보관, 표시 여부 관리, 정렬 상태 삽입 |
| `GdsElement.cs` | `GdsElement` | 모든 요소의 공통 부모. 레이어, 데이터 타입, 변환, 경계 보관 |
| `GPoint.cs` | `GPoint` | 실수 X,Y 좌표 및 Oracle 저장용 바이트 배열 변환 |
| `GBox.cs` | `GBox` | 최소/최대 X,Y 경계, 폭/높이, 교차 판정 |
| `GTransform.cs` | `GTransform` | 회전, 배율, X축 반전 정보 |
| `UnitLength.cs` | `LengthUnit` | Meter, Millimeter, Micrometer, Nanometer 단위 정의 |

### 4.2 요소 클래스

| GDSII 요소 | 파일 | 화면 처리 의도 |
|---|---|---|
| Boundary | `Elements/GdsBoundary.cs` | 닫힌 폴리곤 영역 |
| Path | `Elements/GdsPath.cs` | 선형 경로와 폭 |
| Text | `Elements/GdsText.cs` | 문자열과 기준 위치 |
| SREF | `Elements/GdsSRef.cs` | 다른 Structure 한 건 참조 |
| AREF | `Elements/GdsARef.cs` | 다른 Structure를 행/열 배열로 참조 |

### 4.3 `Gds/GdsReader.cs`

`GdsReader`는 GDSII 바이너리 레코드를 순차 읽기 방식으로 파싱하는 핵심 클래스다.

#### 읽기 절차

1. `Read(path)`에서 1MB 버퍼를 가진 `FileStream`, `BufferedStream`, `BinaryReader`를 연다.
2. 각 레코드의 2바이트 길이와 1바이트 레코드 타입, 1바이트 데이터 타입을 읽는다.
3. 레코드 타입에 따라 현재 Library, Structure, Element 객체에 값을 저장한다.
4. `ENDEL` 레코드에서 `EndElement`를 호출해 요소 경계를 계산하고 해당 Layer에 삽입한다.
5. `ENDSTR` 레코드에서 Structure 전체 경계를 계산하고 Library에 추가한다.

#### 지원 레코드

| 레코드 타입 | 의미 | 처리 내용 |
|---|---|---|
| `0x02` | LIBNAME | 라이브러리명 저장 |
| `0x03` | UNITS | 사용자 단위와 DB 단위 저장 |
| `0x05` / `0x06` / `0x07` | BGNSTR / STRNAME / ENDSTR | Structure 생성, 이름, 종료 처리 |
| `0x08` | BOUNDARY | `GdsBoundary` 생성 |
| `0x09` | PATH | `GdsPath` 생성 |
| `0x0A` | SREF | `GdsSRef` 생성 |
| `0x0B` | AREF | `GdsARef` 생성 |
| `0x0C` | TEXT | `GdsText` 생성 |
| `0x0D` / `0x0E` | LAYER / DATATYPE | Layer ID와 데이터 타입 설정 |
| `0x0F` | WIDTH | Path 폭 설정 |
| `0x10` | XY | 요소 좌표 설정 |
| `0x12` / `0x13` | SNAME / COLROW | 참조 대상명과 AREF 행/열 설정 |
| `0x1A` / `0x1B` / `0x1C` | STRANS / MAG / ANGLE | 반전, 배율, 회전 설정 |
| `0x11` | ENDEL | 요소 완료 및 Layer 삽입 |

#### 바이트 변환과 단위 변환

GDSII 숫자는 Big Endian으로 저장된다. `I16`, `I32`는 PC의 Little Endian 메모리에서 읽은 값을 바이트 순서 변경으로 변환한다.

`Real8`은 GDSII의 8바이트 실수 형식을 처리한다. 이 값은 일반적인 IEEE 754 double 형식과 다르므로 별도 변환이 필요하다. `Pow16Table`은 지수 계산에 필요한 16의 거듭제곱을 미리 계산해 반복 `Math.Pow` 호출을 줄인다.

좌표와 Path 폭은 `DatabaseUnit`을 곱해 선택한 `LengthUnit` 기준으로 바꾼다. 화면에서 `Micrometer`를 설정하므로 일반적으로 화면 좌표는 마이크로미터 기준으로 사용된다.

## 5. Geometry와 경계 계산

### `Gds/GdsGeometry.cs`

| 메서드 | 역할 |
|---|---|
| `ElementBounds` | 요소 타입별 경계를 계산. Path는 폭의 절반을 경계에 추가 |
| `Bounds` | 좌표 목록의 최소/최대 X,Y 계산 |
| `CreateTransform` | MirrorX, Magnification, Rotation을 GDI+ Matrix로 생성 |
| `ToPoint` | GPoint를 화면 PointF로 변환 |

`GBox`는 선택, 확대, 화면 맞춤의 기준 데이터다. 도형의 세부 폴리곤 교차가 아니라 사각형 경계 기준으로 선택 후보를 찾는다.

## 6. OpenGL 설명

### 6.1 OpenGL을 사용하는 이유

WinForms의 기본 GDI+는 CPU가 도형을 하나씩 그린다. GDS 도면은 레이어와 폴리곤 수가 많을 수 있으므로, 이 프로젝트는 도형 좌표를 GPU로 전달하고 GPU가 그리도록 OpenGL을 사용한다.

```text
CPU 역할
GDS 파싱 / 좌표 변환 / 버텍스 목록 생성 / 선택 상태 변경

GPU 역할
버텍스 좌표를 화면 좌표로 변환 / 색상 처리 / 삼각형과 선 출력
```

### 6.2 OpenGL 핵심 용어

| 용어 | 의미 | 프로젝트 사용 위치 |
|---|---|---|
| Vertex | 도형을 구성하는 점 | `Vertex` 구조체의 Position, Color, IsSelected |
| VBO | GPU 메모리에 올리는 버텍스 배열 | `_vbo`, `_gpuVertices` |
| VAO | 버텍스 배열의 데이터 배치 설명 | `_vao` |
| Shader | GPU에서 실행되는 짧은 프로그램 | `VertexShaderCode`, `FragmentShaderCode` |
| Vertex Shader | 각 점의 화면 위치와 전달 색상 계산 | `uMatrix`를 이용해 좌표 변환 |
| Fragment Shader | 화면의 각 픽셀 색상 결정 | `vColor`를 최종 색상으로 출력 |
| Primitive | GPU가 그리는 기본 도형 | `Triangles`, `Lines` |
| Projection Matrix | 월드 좌표를 OpenGL 화면 좌표로 바꾸는 행렬 | `Matrix4.CreateOrthographicOffCenter` |

### 6.3 `Controls/GdsMapControl.cs`

이 클래스는 `GLControl`을 상속한 핵심 렌더링 컨트롤이다. 하나의 화면 표시 주기는 다음과 같다.

```text
GdsStructure
  -> FlattenStructure
  -> GlSceneLayer / GlSceneItem
  -> BuildGpuBuffers
  -> _gpuVertices
  -> UpdateGpuBuffers
  -> VBO
  -> OnPaint / GL.DrawArrays
```

#### `OnLoad`

OpenGL 초기화 단계다.

1. 배경색과 Alpha Blend를 설정한다.
2. Vertex Shader와 Fragment Shader를 컴파일하고 Program으로 연결한다.
3. VAO와 VBO를 생성한다.
4. 버텍스 데이터 배치를 등록한다.

버텍스 하나는 float 7개로 구성된다.

```text
Position X, Position Y, Color R, Color G, Color B, Color A, IsSelected
```

`IsSelected`가 1이면 Vertex Shader가 원래 레이어 색상 대신 노란색을 출력하도록 처리한다.

#### `ShowStructure`

표시할 Structure를 지정하고 다음 작업을 수행한다.

1. `Structure` 속성에 대상 Structure 저장
2. `FlattenStructure`로 화면용 항목 생성
3. `BuildGpuBuffers`로 버텍스 생성
4. 선택 변경 이벤트 발생
5. `ZoomToFit`으로 화면 범위 조정

#### `FlattenStructure`

원본 GDS 모델을 실제 화면에 그릴 수 있는 항목으로 바꾸는 단계다.

| 요소 | 처리 방식 |
|---|---|
| Boundary | 좌표 변환 후 `Closed=true`인 `GlSceneItem` 생성 |
| Path | 좌표 변환 후 `Closed=false`인 `GlSceneItem` 생성 |
| Text | 위치와 문자열을 가진 `GlSceneItem` 생성 |
| SREF | 참조 Structure를 찾아 변환 행렬을 합성한 뒤 재귀 호출 |
| Defect | 중심 X,Y와 폭/높이를 사각형 좌표로 바꿔 `GlDefectItem` 생성 |

SREF 재귀 호출에는 두 안전 장치가 있다.

- 동일 Structure가 다시 참조되면 중단하는 순환 참조 방지 목록
- 깊이 64를 넘으면 중단하는 최대 재귀 깊이 제한

#### `BuildGpuBuffers`

화면용 항목을 GPU에 전달할 Vertex 목록으로 변환한다.

| 요소 | Vertex 생성 방식 |
|---|---|
| 닫힌 Boundary | 첫 점을 기준으로 삼각형 Fan을 생성하고 외곽선 Line 목록도 생성 |
| Path | 인접 점 2개씩 Line Vertex 생성 |
| Defect | 사각형을 삼각형으로 생성 |

Boundary의 채움은 오목 폴리곤을 정확히 분할하는 Ear Clipping이 아니라 첫 점 기준 Triangle Fan이다. 오목한 Boundary가 입력될 수 있다면 삼각 분할 알고리즘을 별도로 교체해야 한다.

#### `UpdateGpuBuffers`

선택 상태 변경 후 각 Vertex의 `IsSelected` 값을 갱신하고 `GL.BufferData`로 VBO 전체를 다시 업로드한다.

선택 변경마다 전체 버퍼를 다시 올리므로, 대용량 도면에서 선택 성능 문제가 생기면 선택 전용 버퍼 분리 또는 선택 색상용 Uniform/인스턴싱 구조를 검토할 수 있다.

#### `OnPaint`

렌더링 단계다.

1. 현재 확대율과 Offset으로 직교 투영 행렬을 계산한다.
2. Shader Program과 VAO를 활성화한다.
3. 레이어별로 `GL.DrawArrays(Triangles)`와 `GL.DrawArrays(Lines)`를 호출한다.
4. 불량 사각형을 추가 출력한다.
5. `SwapBuffers`로 그려진 화면을 표시한다.

드래그 선택 사각형은 OpenGL이 아닌 GDI+ `CreateGraphics()`로 OpenGL 화면 위에 임시 표시한다.

### 6.4 화면 좌표와 확대/이동

| 메서드 | 역할 |
|---|---|
| `ZoomToFit` | 전체 레이어 경계로 확대율 계산 |
| `WorldToScreen` | 월드 좌표를 WinForms 화면 좌표로 변환 |
| `ScreenToWorld` | 마우스 화면 좌표를 GDS 월드 좌표로 변환 |
| `ZoomAt` | 마우스 위치를 기준으로 확대/축소 |
| `Clamp` | 확대율을 최소와 최대 범위로 제한 |

## 7. 레이어, 선택, 마우스 처리

### `GdsMapControl_Layer.cs`

`_layerList`는 OpenGL 화면용 레이어 목록이며 원본 `GdsLayer`와 별도로 관리된다.

| 메서드 | 역할 |
|---|---|
| `UpdateVisibleLayer` | 원본 Structure 레이어의 Visible 값을 화면 레이어에 반영 후 GPU 버퍼 재생성 |
| `VisibleAllLayer` | 모든 화면 레이어의 표시 여부 변경 |
| `SetLayerColor` | Layer ID별 색상 사전 저장 |

### `GdsMapControl_Mouse.cs`

| 동작 | View Mode | Select Mode |
|---|---|---|
| 왼쪽 드래그 | 화면 이동 | 선택 사각형 생성 |
| 가운데 드래그 | 화면 이동 | 화면 이동 |
| 마우스 휠 | 커서 위치 기준 확대/축소 | 동일 |
| 짧은 클릭 | 사용하지 않음 | 경계가 겹치는 요소 중 하나 선택 |
| 영역 선택 | 사용하지 않음 | 선택 사각형 안에 완전히 포함되는 요소 선택 |

선택은 실제 도형 내부 판정이 아니라 `GBox` 경계 기준이다. `OnlySelectSquareItems=true`이면 반올림한 폭과 높이가 같은 항목만 선택할 수 있다.

## 8. Oracle 저장과 조회

### `Oracle/OracleSaveUtil.cs`

`GDS_ELEMENT` 테이블로 요소를 저장한다.

- `OracleBulkCopy` 사용
- 기본 배치 크기 50,000건
- 지원 저장 요소는 Boundary와 Path
- 좌표 배열은 `GPoint.ArrayToBytes`를 사용해 `byte[]`로 저장
- 현재 `GDS_SEQ=2`, `STRUCTURE_NAME="TOP"` 값이 코드에 고정

### `Oracle/OracleLoadUtil.cs`

`GDS_ELEMENT` 테이블에서 `GDS_SEQ`, `STRUCTURE_NAME`으로 조회한 요소를 `GdsBoundary`, `GdsPath` 객체로 복원한다.

`OracleLoadUtil_DataTable_Version`은 같은 기능을 DataTable 방식으로 구현한 비교용 또는 이전 방식 코드다. 실제 호출은 DataReader 기반 `OracleLoadUtil.LoadElements`다.

## 9. 파일별 유지보수 기준

| 변경 목적 | 우선 확인 파일 |
|---|---|
| GDS 레코드 추가 지원 | `GdsReader.cs`, 관련 `Elements` 클래스, `GdsGeometry.cs` |
| 새 요소 화면 표시 | `GdsMapControl.FlattenStructure`, `BuildGpuBuffers`, `OnPaint` |
| 레이어 색상과 표시 방식 변경 | `GdsMapControl_Layer.cs`, `GlSceneLayer.cs` |
| 선택 조건 변경 | `GdsMapControl_Mouse.cs`, `GBox.cs`, `GlSceneItem.cs` |
| 확대/이동 기준 변경 | `GdsMapControl.cs`의 `ZoomToFit`, `ZoomAt`, 좌표 변환 메서드 |
| Oracle 스키마와 저장 조건 변경 | `OracleSaveUtil.cs`, `OracleLoadUtil.cs`, `GPoint.cs` |
| 불량 데이터 연계 | `Defect.cs`, `Form1.cs`, `GdsMapControl.FlattenStructure` |

## 10. 현재 구현 범위와 제한 사항

아래 내용은 소스 확인 결과다. 요구사항으로 단정하지 않으며, 확장 전에 GDS 샘플과 업무 규칙으로 재확인해야 한다.

| 구분 | 현재 상태 | 개발 시 고려 사항 |
|---|---|---|
| AREF | 파싱 모델은 있으나 `FlattenStructure`에서 화면 생성 분기 없음 | 행/열 복제와 배열 배치 벡터 계산 구현 필요 |
| TEXT | 문자열과 위치는 보관하나 문자 렌더링 없음 | OpenGL 텍스처 폰트, SDF 폰트 또는 GDI 오버레이 방식 결정 필요 |
| Path 폭 | 경계 계산에는 반영되지만 화면은 중심선만 그림 | 실제 폭을 가진 폴리곤 또는 GL Line Width 적용 검토 |
| Boundary 삼각화 | Triangle Fan 방식 | 오목 폴리곤 입력 시 Ear Clipping 같은 삼각화 필요 |
| 반복 표시 | 화면 Scene 목록과 불량 목록을 초기화하지 않음 | 파일 재선택 또는 Structure 재표시 전 목록 초기화 필요 |
| 레이어 색상 | `_layerColor`이 설정되지 않은 DB 직접 조회 경로에서 NullReference 가능 | 빈 사전 기본값 또는 null 검사 추가 필요 |
| 화면 맞춤 | `ZoomToFit`의 중심 좌표가 원점으로 고정 | 실제 GBox 중심으로 Offset 설정 필요 |
| 선택 성능 | 선택마다 VBO 전체 업로드 | 대규모 데이터는 선택 데이터 분리 검토 |
| DB 저장 범위 | Boundary와 Path만 저장 | Text, SREF, AREF와 변환 정보 저장 여부 결정 필요 |
| DB 키 | GDS 순번과 Structure명이 고정 | 화면 입력 또는 업무 키 전달 방식으로 변경 필요 |
| DB 접속 설정 | 접속 정보가 코드에 포함 | 운영 전 App.config 또는 보안 저장소로 분리 필요 |

## 11. 빌드와 점검 방법

### C# 7.3 호환 빌드

```powershell
dotnet build NexplantQMS.GdsMap.csproj --no-restore -p:LangVersion=7.3
```

이 문서 작성 시점에 위 조건으로 오류 0건 빌드를 확인했다. `GlDefectItem.LineVertexCount`에 값이 할당되지 않는다는 경고 1건은 남아 있다.

### 기능 확인 순서

1. Visual Studio에서 `NexplantQMS.GdsMap.sln`을 연다.
2. `Load from File`로 실제 GDS 파일을 연다.
3. 레이어 목록과 요소 Grid가 생성되는지 확인한다.
4. 휠 확대/축소와 보기 모드 화면 이동을 확인한다.
5. 우클릭 메뉴에서 선택 모드로 전환해 클릭 및 영역 선택을 확인한다.
6. SREF를 포함한 GDS 파일로 참조 Structure가 보이는지 확인한다.
7. AREF, TEXT, 오목 Polygon, 원점에서 멀리 떨어진 도형은 현재 제한 사항을 기준으로 별도 확인한다.
8. DB 기능은 실제 접속 정보와 `GDS_ELEMENT` 테이블 구조가 준비된 별도 환경에서 확인한다.

## 12. 분석 근거와 범위

이 문서는 소스 정적 분석과 C# 7.3 빌드 결과를 근거로 작성했다. 실제 장비 GDS 샘플의 모든 문법, OpenGL 화면 품질, Oracle 연결과 저장 결과는 실행 환경과 샘플 데이터로 별도 검증해야 한다.
