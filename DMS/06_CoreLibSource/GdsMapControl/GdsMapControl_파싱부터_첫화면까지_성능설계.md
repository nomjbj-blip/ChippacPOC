# GDS 파싱부터 첫 화면까지 성능 개선 설계

## 1. 범위와 현재 상태

대상은 `NexplantQMS.GdsMap.csproj`에서 파일 선택 직후부터 실제 Map의 첫 화면이 그려질 때까지다. 이 문서는 설계이며, 성능 개선 코드를 적용한 결과나 속도 향상 수치를 뜻하지 않는다. 기존 변경 사항인 Layer별 VBO 부분 갱신과 모든 Layer의 GDS TEXT 처리도 보존한다.

현재 흐름은 다음과 같다.

```text
Form1.button1_Click
  -> Task.Run(GdsReader.Read)
  -> GdsLayerList.AddElement / Structure 구성
  -> UI 스레드: GdsMapControl.ShowStructure
  -> FlattenStructure / GlSceneLayerList.AddSceneItem / TEXT 영역 계산
  -> BuildGpuBuffers / UploadAllGpuVertices
  -> BindElementList / RefreshLayerList
  -> 메시지 루프의 OnPaint / SwapBuffers / DrawTextLabels
```

`Form1`의 현재 완료 시간은 `EndMapLoad`에서 멈춘다. 첫 `OnPaint`가 그 뒤에 실행된다면 이 숫자는 실제 첫 화면 완료 시간을 포함하지 않는다. 첫 화면이 늦다는 현상을 정확히 재려면 별도 종료 시점이 필요하다.

### 사용자 측정 결과 반영

현재 확인된 시간 순위는 `FlattenStructure`가 1위, `BuildGpuBuffers`가 2위다. 각 구간의 ms/비율은 아직 기록되지 않았으며, `BuildGpuBuffers` 안에는 `UploadAllGpuVertices` 호출도 들어 있다. 따라서 정점 생성과 GPU 업로드를 분리하기 전에는 두 번째 구간이 CPU 정점 작업만의 시간이라고 판단하지 않는다.

## 2. 먼저 측정할 구간

| 구간 | 시작 / 종료 | 함께 기록할 값 |
|---|---|---|
| 파일 읽기 | `GdsReader.Read` 시작 / 반환 | 파일 byte, 레코드 수, Structure/Layer/요소 수 |
| 파싱 내부 | 레코드 읽기 / 요소 추가 / Structure 종료 | 각 구간 시간, 중복 판정 수 |
| Scene 구성 | `ShowStructure` 시작 / `FlattenStructure` 완료 | SREF 방문 수, Scene Item 수, TEXT 수 |
| 정점 구성 | `BuildGpuBuffers` 시작 / CPU 정점 완료 | 정점 수, Capacity, 최대 메모리 |
| GPU 업로드 | `UploadAllGpuVertices` 시작 / `GL.BufferData` 반환 | 업로드 byte, `ToArray` 복사 시간 |
| 화면 보조 작업 | `BindElementList` / `RefreshLayerList` 각각 시작 / 종료 | 표 행 수, Layer 수 |
| 첫 화면 | 첫 `OnPaint` 시작 / `SwapBuffers`와 TEXT 표시 완료 | Draw Call 수, 표시 TEXT 수, 프레임 시간 |
| 사용자가 체감하는 전체 시간 | 파일 선택 완료 / 첫 화면 완료 | 총 ms, UI 응답 가능 여부 |

- 실제 사용하는 동일 GDS 파일, 동일 화면 크기, 동일 Layer 상태에서 Release 빌드로 3회 이상 측정한다.
- 첫 실행과 같은 파일의 반복 실행을 나눠 적는다. OS 파일 캐시 효과를 개선 효과와 혼동하지 않는다.
- `Stopwatch` 측정은 짧은 구간에서 화면 갱신이나 로그 문자열 생성을 반복하지 않도록 한다.
- 첫 화면 완료 이벤트는 `DrawTextLabels`까지 끝난 뒤 한 번만 알린다. `EndMapLoad` 시간과 별도로 표시한다.
- 현재는 사용자가 확인한 1위/2위 순위를 우선 적용하되, 각 함수 안의 세부 원인은 추가 측정 전까지 가설로 둔다.

## 3. 개선 후보와 권장 순서

| 순서 | 후보 | 소스에서 확인된 이유 | 적용 조건 |
|---|---|---|---|
| 0 | 구간별 측정과 첫 화면 종료 시점 분리 | 현재 완료 시간이 첫 Paint를 포함하는지 불확실 | 선행 필수 |
| 1 | `FlattenStructure` 내부 병목 분리와 개선 | 사용자 측정에서 가장 오래 걸림 / Scene 중간 삽입, Matrix/좌표 배열, SREF 조회, 재귀별 Layer 색상 순회가 후보 | 우선 진행 |
| 2 | `BuildGpuBuffers` 내부 병목 분리와 개선 | 사용자 측정에서 두 번째 / 반복 정점 생성과 함수 끝의 전체 GPU 업로드가 후보 | 1번과 별도 변경 |
| 3 | 파싱 중 요소 삽입 방식 개선 | `GdsLayerList.AddElement`가 요소마다 `BinarySearch` 후 `List.Insert` 실행 | 파일 읽기 시간이 높은 경우 |
| 4 | 전체 표 바인딩 지연 또는 가상화 | `BindElementList`가 모든 요소의 표시 행을 `ToList()`로 즉시 생성 | Map보다 표 준비가 늦을 때 |
| 5 | 첫 Paint 비용 축소 | 표시 중인 모든 Item에 `GL.DrawArrays`, TEXT마다 화면 측정/충돌 검사 | 첫 화면 프레임이 클 때 |
| 6 | 레코드 읽기 루프 미세 최적화 | 레코드마다 `BufferedStream.Position` 조회와 진행률 검사, 이중 버퍼 사용 | 순수 읽기/해석 시간이 큰 경우 |

사용자가 측정한 순위에 따라 1번과 2번을 먼저 검토한다. 각 함수 안에서는 다시 나누어 측정한 뒤 가장 큰 하위 작업 하나씩 수정한다. 이미 설계된 P1-1, P1-3, P2-1, P2-2와 겹치는 변경은 같은 작업으로 관리한다.

## 4. 후보별 구현 경계

### 파싱 요소 삽입

Layer별로 파싱 순서대로 모으고 Structure 종료 시 정렬하는 방식을 비교한다. 지금의 `GdsElement.CompareTo` 결과가 같은 요소는 `DuplicatedItems`로 분리된다. 새 방식에서도 중복 수와 남는 요소가 같아야 한다. `List.Sort`만 적용하면 같은 키의 순서가 달라질 수 있으므로 원본 입력 순서를 보조 키로 보관하거나 동일 순서를 보장하는 정렬 방식을 선택한다. 파싱 오류를 감추지 않도록 GDS 레코드 길이/읽기 완료 검사도 함께 설계하되, 데이터 해석 변경은 성능 변경과 별도 단계로 둔다.

순수 레코드 읽기 자체가 느린 것으로 확인될 때만 `FileStream`/`BufferedStream`의 중복 버퍼, 매 레코드 `Position` 조회, `BinaryReader.Read` 호출 비용을 각각 비교한다. 짧게 읽힌 레코드를 잘못 처리하지 않도록 길이 검증을 먼저 갖춘다. 버퍼 크기 변경만으로 큰 개선을 단정하지 않는다.

### Scene 구성

Layer별 Scene Item을 일단 모은 후 현재 비교 기준대로 확정 정렬한다. SREF가 반복될 때 `GdsStructureList.TryGetValue`는 이름을 선형 탐색하므로, 이름별 조회 인덱스를 한 번 만드는 방식을 검토한다. 중복 Structure 이름이 있으면 현재의 첫 일치 항목 선택을 보존해야 한다. `FlattenStructure`의 Matrix/좌표 배열 생성과 재귀 종료 시 전체 Layer 색상 반복 설정은 측정 후 줄인다. 좌표 변환을 바꾸는 경우 TEXT 삽입점/SREF 회전/반전 결과를 기존 값과 비교한다.

`FlattenStructure` 안에서는 다음 시간을 따로 기록한다: 도형/TEXT/SREF 개수, `CreateTransform`과 `TransformPoints`, `AddSceneItem`, SREF 이름 조회와 재귀 방문, 재귀 종료 시 Layer 색상 순회. 먼저 Layer 색상을 전체 Flatten 종료 후 한 번만 설정하는 작은 변경을 검토한다. 그다음 `AddSceneItem`의 정렬 중간 삽입을 Layer별 수집/최종 정렬로 바꾸되, 반투명 그리기 순서와 중복 Item 처리를 비교한다. Matrix를 값 형식 변환으로 바꾸는 작업은 좌표 회귀 검증이 더 필요하므로 마지막에 둔다.

### 정점 생성과 GPU 업로드

P1-1의 고정 Capacity 제거를 먼저 다룬다. 그다음 실제 정점 수에 맞는 저장소를 만들고 `ToArray()` 없이 한 번 업로드하는 방식을 별도 변경으로 검증한다. P1-2가 사용하는 Item Offset/Count와 Layer 연속 구간은 유지해야 하며, 색상/선택 부분 업로드도 계속 동작해야 한다. `GL.BufferData`와 OpenGL 컨텍스트 작업은 UI 스레드에 남긴다. 정점 byte 계산은 `long`으로 검증한 뒤 API의 허용 범위에 맞춰 전달한다.

`BuildGpuBuffers`에서는 Layer/Item 순회, 채움 삼각형 생성, 외곽선 생성, `List<Vertex>.Add`에 따른 Capacity 변경, 진행률 호출, `UploadAllGpuVertices`를 분리한다. 5천만 정점 선할당은 컨트롤 생성 시점에 일어나므로 `BuildGpuBuffers`의 시간에 직접 포함되지 않는다. 다만 메모리 사용량과 GC 부담은 별도로 측정한다. 가장 작은 변경은 Layer 채움 색상을 Item/정점마다 만들지 않고 Layer마다 한 번 계산하는 것이다. 다음으로 실제 정점 수를 계산해 고정 선할당을 없애고, 마지막으로 `ToArray()` 복사를 제거한다. 마지막 단계는 CPU 정점 저장 형식과 P1-2 부분 업로드에 영향을 주므로 별도 검증한다.

### 표 바인딩과 UI 반응

Map 첫 화면에 필요 없는 전체 요소 표 생성은 첫 Paint 뒤로 미루거나 `DataGridView.VirtualMode`/페이지 표시로 바꾼다. 다만 현재 표의 정렬/선택/진단 기능이 있는지 확인한 뒤 동일한 조회 결과를 제공해야 한다. 백그라운드로 옮길 수 있는 것은 파싱과 CPU Scene 구성이다. WinForms 컨트롤과 OpenGL 컨텍스트는 UI 스레드에서 다루며, 취소된 조회 결과가 늦게 화면에 반영되지 않도록 조회 식별자를 둔다.

### 첫 화면 Draw

확대 상태의 화면 밖 Item 건너뛰기는 첫 화면이 전체 보기일 때 효과가 작을 수 있다. 먼저 첫 화면 Draw Call 수와 시간부터 잰다. Draw Call 통합은 반투명 도형의 채움/외곽선 순서가 달라질 수 있어 별도 시각 비교가 필요하다. TEXT는 화면 밖 후보를 문자열 측정 전에 제외하고, 측정 결과를 재사용할 수 있는지 확인한다. 글자 위치, 안전 영역, Layer 표시와 충돌 차단 결과는 유지한다.

## 5. 완료 판단

- 동일 파일에서 Structure/Layer/요소/중복/Scene Item/정점/TEXT 수가 기존과 일치한다.
- Layer 체크/색상, 선택, TEXT 좌표와 확대 상태의 표시 결과가 기존과 일치한다.
- 최초 Map 표시 시간과 파일 선택부터 첫 화면까지의 시간이 각각 개선 전보다 감소한다. 평균과 최대값을 함께 기록한다.
- 조회 중 최대 메모리와 UI 멈춤 시간도 함께 기록한다. 속도가 빨라져도 메모리 급증이나 `OutOfMemoryException`이 생기면 완료로 보지 않는다.
- 실제 GDS 화면 비교 없이 컴파일만 통과한 상태는 구현 완료가 아니라 화면 검증 대기로 기록한다.

## 6. 바로 다음 작업

첫 변경은 `FlattenStructure`와 `BuildGpuBuffers` 안에 하위 구간별 측정값을 추가하는 작은 작업이다. 기존 1위/2위 순위를 유지하면서 `AddSceneItem`과 정점 생성 중 실제 비중이 큰 작업을 찾는다. 첫 화면 완료 이벤트는 전체 체감 시간 검증용으로 함께 추가한다. 현재 ms 수치는 기록되지 않았으므로 개선 효과 수치를 제시하지 않는다.
