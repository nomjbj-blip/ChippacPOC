# GDS Map 소스 분석

## 1. 분석 목적

이 문서는 `GdsMap` 프로젝트가 GDSII 파일을 읽고 화면에 표시하는 구조를 정리한 자료이다.

분석 대상 경로는 다음과 같다.

`D:\1_프로젝트 문서\0.본사제안\스테츠칩팩\GdsMap`

프로젝트 이름과 소스 기준으로 대상은 GDP Map이 아니라 GDS Map이다.

## 2. 프로젝트 개요

이 프로젝트는 .NET Framework 4.8 기반 WinForms 단일 실행 프로그램이다. GDSII 파일을 파싱하고 OpenTK/OpenGL로 도면을 그린다. 일부 도형은 Oracle `GDS_ELEMENT` 테이블에 저장하거나 다시 조회할 수 있다.

주요 외부 라이브러리는 다음과 같다.

| 라이브러리 | 용도 |
|---|---|
| OpenTK 3.3.3 | OpenGL 제어와 GPU 기반 도면 렌더링 |
| OpenTK.GLControl 3.3.3 | WinForms에서 OpenGL 화면을 표시하는 컨트롤 |
| Oracle.ManagedDataAccess 19.18.0 | Oracle 데이터 저장 및 조회 |

처리 흐름은 다음과 같다.

```text
GDS 파일 선택
  -> GdsReader가 GDSII 바이너리 파싱
  -> GdsLibrary / GdsStructure / GdsLayer / GdsElement 모델 생성
  -> GdsMapControl이 Structure 참조를 평면화
  -> OpenGL 버텍스 버퍼 생성
  -> 레이어별 도형과 불량 위치 화면 표시
```

## 3. 프로젝트 및 화면 구성

| 파일 | 역할 |
|---|---|
| `NexplantQMS.GdsMap.sln` | Visual Studio 솔루션 파일 |
| `NexplantQMS.GdsMap.csproj` | .NET Framework 4.8 WinForms 실행 프로젝트와 외부 라이브러리 참조 설정 |
| `Program.cs` | 프로그램 시작점. `Form1`을 실행 |
| `Form1.cs` | 파일 열기, DB 저장/조회, 레이어 표시 제어를 연결하는 화면 로직 |
| `Form1.Designer.cs` | 버튼, 레이어 목록, 도면 영역, 요소 목록 Grid의 화면 배치 |
| `App.config` | .NET 실행 환경과 Oracle Managed Driver 설정 |

화면에는 다음 기능이 있다.

| 화면 항목 | 기능 |
|---|---|
| `Load from File` | GDS 파일을 선택해 파싱하고 화면에 표시 |
| `Save DB` | 현재 Structure의 Boundary와 Path를 Oracle에 저장 |
| `Load DB` | Oracle에서 고정 조건의 데이터를 조회해 화면에 표시 |
| 레이어 체크 목록 | 레이어별 표시와 숨김 선택 |
| `Redraw` | 체크된 레이어 상태를 화면에 다시 반영 |
| 요소 목록 Grid | 레이어, 요소 종류, 경계, 텍스트 또는 Path 폭을 목록으로 표시 |

## 4. GDSII 파싱 영역

### `Gds/GdsReader.cs`

GDSII 바이너리 파일을 순차적으로 읽는 핵심 파서이다.

구현 내용은 다음과 같다.

- 1MB 스트림 버퍼로 파일을 순차 읽기
- GDSII 레코드 헤더에서 길이, 레코드 타입, 데이터 타입 읽기
- Big Endian 16비트와 32비트 정수 변환
- GDSII의 8바이트 실수 형식 변환
- 라이브러리명과 단위 정보 파싱
- Structure 시작과 종료 처리
- Boundary, Path, SREF, AREF, Text 요소 파싱
- 레이어, 데이터 타입, 좌표, 폭, 참조 Structure명, 회전, 배율, 반전 정보 반영
- Structure 종료 시 전체 요소 경계를 계산해 저장

파싱하는 GDSII 요소와 모델의 관계는 다음과 같다.

| GDSII 요소 | 생성 클래스 | 저장 정보 |
|---|---|---|
| `BOUNDARY` | `GdsBoundary` | 닫힌 폴리곤 좌표 |
| `PATH` | `GdsPath` | 선형 좌표, 폭, PathType |
| `SREF` | `GdsSRef` | 참조 Structure명, 원점 |
| `AREF` | `GdsARef` | 참조 Structure명, 행/열, 배치 벡터 |
| `TEXT` | `GdsText` | 텍스트, 위치, TextType |

### `Gds/GdsLibrary.cs`

GDS 파일 전체를 나타내는 최상위 모델이다. 라이브러리 이름, User Unit, Database Unit, Structure 목록을 관리한다.

### `Gds/GdsStructure.cs`

하나의 Structure를 표현한다. Structure 이름, 레이어 목록, 불량 목록, 전체 경계 영역을 가진다. GDS 파일 내부에서 Cell 또는 Block 단위로 이해할 수 있다.

### `Gds/GdsLayer.cs`

레이어 ID별로 요소를 보관한다. 요소와 레이어는 정렬 상태로 넣으며, 동일 요소가 발견되면 중복 목록에 별도 보관한다. `Visible` 속성은 화면 표시 여부를 관리한다.

### `Gds/GdsGeometry.cs`, `Gds/GBox.cs`, `Gds/GPoint.cs`, `Gds/GTransform.cs`, `Gds/UnitLength.cs`

좌표와 도형 계산에 쓰이는 공통 영역이다.

| 파일 | 역할 |
|---|---|
| `GdsGeometry.cs` | 요소별 경계 계산, 확대/회전/반전 변환 행렬 생성 |
| `GBox.cs` | 최소/최대 X,Y 기반 경계 모델, 교차 여부와 폭/높이 계산 |
| `GPoint.cs` | GDS 좌표 모델. Oracle 저장용 `byte[]` 변환과 복원 제공 |
| `GTransform.cs` | 회전, 확대/축소, X축 반전 정보 보관 |
| `UnitLength.cs` | Meter, Millimeter, Micrometer, Nanometer 단위 정의 |

## 5. GDS 요소 모델 영역

### `Elements/GdsElement.cs`

모든 GDS 요소의 공통 부모 클래스이다. Layer ID, Data Type, 이름, 변환 정보, 경계 영역을 공통으로 가진다. 요소 정렬과 요소별 경계 계산 호출도 담당한다.

### 요소별 클래스

| 파일 | 역할 |
|---|---|
| `GdsBoundary.cs` | 닫힌 폴리곤 영역의 좌표 목록 보관 |
| `GdsPath.cs` | 선형 경로의 좌표, 폭, PathType 보관 |
| `GdsText.cs` | 텍스트, 위치, TextType 보관 |
| `GdsSRef.cs` | 한 개 Structure 참조 정보 보관 |
| `GdsARef.cs` | 배열 형태 Structure 참조 정보 보관 |

## 6. OpenGL 렌더링 영역

### `Controls/GdsMapControl.cs`

도면 표시의 중심 컨트롤이다. WinForms `GLControl`을 상속하고 OpenGL 셰이더, VAO, VBO를 생성해 GPU로 그린다.

주요 처리 순서는 다음과 같다.

1. `OnLoad`에서 OpenGL 초기화, 셰이더 컴파일, VAO/VBO 생성
2. `ShowStructure`에서 선택한 Structure를 화면 표시 대상으로 지정
3. `FlattenStructure`에서 참조 Structure를 실제 화면 좌표로 펼침
4. `BuildGpuBuffers`에서 폴리곤과 선을 OpenGL 버텍스 목록으로 변환
5. `UpdateGpuBuffers`에서 GPU 버퍼에 업로드
6. `OnPaint`에서 레이어와 불량 도형을 OpenGL로 그림

`BOUNDARY`는 삼각형 채움과 외곽선으로 렌더링한다. `PATH`는 선분 목록으로 렌더링한다. 선택된 도형은 셰이더에서 노란색으로 표시한다.

`SREF`는 참조 대상 Structure를 찾은 후, 원점 이동과 변환값을 적용해 재귀적으로 펼친다. 순환 참조 방지용 Structure 목록과 최대 깊이 64단계 제한도 있다.

### `Controls/GdsMapControl_Layer.cs`

렌더링 레이어의 표시 여부와 레이어 색상을 관리한다.

- `UpdateVisibleLayer`: GDS 레이어의 표시 상태를 화면 레이어에 반영
- `VisibleAllLayer`: 모든 화면 레이어 표시 여부 변경
- `SetLayerColor`: Layer ID별 색상 사전 저장
- `OnlySelectSquareItems`: 정사각형 항목만 선택할지 결정

### `Controls/GdsMapControl_Mouse.cs`

마우스 상호작용을 처리한다.

| 동작 | 기능 |
|---|---|
| 보기 모드의 좌측 또는 가운데 드래그 | 화면 이동 |
| 마우스 휠 | 커서 기준 확대/축소 |
| 선택 모드의 클릭 | 클릭 위치와 겹치는 요소 선택 또는 해제 |
| 선택 모드의 드래그 | 선택 사각형에 완전히 포함된 요소 선택 또는 해제 |
| `OnlySelectSquareItems=true` | 폭과 높이가 같은 요소만 선택 |

### `Controls/GlSceneItem.cs`, `GlSceneLayer.cs`, `GlDefectItem.cs`

원본 GDS 모델을 GPU 렌더링에 맞는 중간 모델로 변환한다.

| 파일 | 역할 |
|---|---|
| `GlSceneItem.cs` | 원본 요소, 화면 좌표, 경계, 선택 상태, GPU 버텍스 위치 보관 |
| `GlSceneLayer.cs` | Layer ID별 색상, 표시 여부, 도형 목록, 경계 보관 |
| `GlDefectItem.cs` | 불량 정보를 화면용 사각형 좌표와 GPU 버텍스 위치로 변환 |

## 7. 불량 표시 영역

### `Defects/Defect.cs`

불량 위치와 크기를 나타내는 모델이다.

- X, Y: 불량 중심 좌표
- Width, Height: 불량 영역 크기
- DefectName, DefectCode: 불량 식별 정보

현재 `Form1.cs` 파일 열기 처리에는 시험용 불량 2건이 직접 추가되어 있다. 불량 데이터는 빨간 사각형으로 표시된다.

## 8. Oracle 저장 및 조회 영역

### `Oracle/OracleSaveUtil.cs`

`GDS_ELEMENT` 테이블에 `BOUNDARY`, `PATH` 요소를 저장한다.

- `OracleBulkCopy` 사용
- 50,000건 단위 배치 저장
- 좌표 배열을 `GPoint.ArrayToBytes`로 `byte[]` 변환 후 저장
- 저장 시 `GDS_SEQ=2`, `STRUCTURE_NAME=TOP` 값이 고정

### `Oracle/OracleLoadUtil.cs`

`GDS_ELEMENT` 테이블에서 `GDS_SEQ`, `STRUCTURE_NAME` 조건으로 요소를 조회한다.

- `BOUNDARY`, `PATH` 데이터를 모델 객체로 복원
- 좌표 `byte[]`를 `GPoint.BytesToArray`로 복원
- DataReader 방식과 DataTable 방식의 조회 구현이 함께 존재

## 9. 현재 소스 기준 확인 필요 사항

아래 항목은 소스 정적 분석 결과이며, 실제 GDS 샘플과 실행 화면으로 별도 검증이 필요하다.

| 구분 | 확인 내용 |
|---|---|
| AREF 렌더링 | `GdsReader`는 AREF를 파싱하지만 `FlattenStructure`에는 AREF 처리 분기가 없어 배열 참조는 화면에 그려지지 않음 |
| TEXT 렌더링 | Text 데이터와 좌표는 파싱하지만 글자를 화면에 그리는 OpenGL 또는 GDI 처리 없음 |
| PATH 폭 | Path 폭은 경계 계산에만 사용되고 렌더링은 중심선으로 처리됨 |
| 반복 파일 열기 | `ShowStructure` 이전에 화면 레이어와 불량 목록을 초기화하지 않아 이전 도형이 누적될 가능성 있음 |
| DB 직접 조회 | DB 조회를 먼저 실행하면 레이어 색상 사전이 초기화되지 않아 예외가 날 가능성 있음 |
| 전체 보기 | `ZoomToFit`에서 도형 중심 대신 원점 `(0,0)`을 화면 중심으로 사용해 원점에서 멀리 떨어진 도형은 화면 맞춤이 부정확할 수 있음 |
| DB 저장 조건 | GDS 순번과 Structure명이 고정되어 있으며 기존 데이터 삭제나 중복 방지 처리가 없음 |
| DB 연결 정보 | Oracle 연결 정보가 소스 코드에 직접 정의되어 있어 운영 적용 전 설정 분리가 필요함 |

## 10. 분석 범위

이번 문서는 소스 정적 분석 기준이다. 실제 GDS 샘플별 파싱 결과, OpenGL 화면 렌더링, Oracle 연결과 저장 결과는 실행 검증하지 않았다.
