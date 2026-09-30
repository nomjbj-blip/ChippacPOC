# Chain Setup 기능 컨셉안

- 작성일: 2026-09-23
- 대상: 스테츠칩팩 AOI Defect / E-Test Correlation POC
- 문서 목적: GDS 화면에서 Input부터 Output까지의 전류 연결 경로를 Chain으로 설정하고, 추후 AOI Defect와 ET Fail의 연관관계를 분석하기 위한 기능 개념을 정리한다.
- 문서 상태: 컨셉안 / 실제 GDS와 고객 기준정보 확인 필요
- 현재 설계 기준: DB 저장/재조회는 17.38절, 대량 Service 전송/적재는 17.41절, 현재 배치 본문 형식은 17.43절을 따른다. 반복된 불필요 도형의 일괄 제외는 17.34절의 현재 Chain 내부 유사 도형 묶음 설계를 따른다. 17.32절의 범위 선택 설계와 17.33절의 범위 드래그 구현은 철회했다. Output 없는 열린 경로 탐색은 17.28절, Chain 미선택 시 단자 지정 차단은 17.27절, Chain 선택/표시/Layer 체크의 일원화는 17.26절을 따른다. 한 Chain의 Input은 1개 필수이고 Output은 최대 1개다. 여러 Chain의 독립 수정/삭제는 17.23절, Chain별 상태 분리와 구현 순서는 17.22절을 따른다. 경로 편집과 지도 마우스 지정 방식은 17.13절을 따른다. 앞 절의 복수 Input/Output 지정은 당시 구현 이력이다.

## 1. 작성 배경

엔지니어는 GDS 화면에서 각 전기적 연결 경로를 Chain으로 설정할 수 있어야 한다.

제공된 화면 설명을 기준으로 다음과 같이 이해한다.

| 구분 | 의미 |
| --- | --- |
| 상단의 원형 단자 | Chain의 Input |
| 하단의 원형 단자 | Chain의 Output |
| 주황색으로 표시된 연결 부분 | 연결 단자 / Ball이 안착되는 원형 위치 / 연결 배선 |
| Input과 Output 사이의 연결 | 하나의 Chain을 구성하는 전류 경로 |

화면에 추가된 빨간 원과 화살표는 Input과 Output을 설명하기 위한 표시이다. 빨간 화살표 자체를 실제 전류 경로 또는 GDS Element 연결로 사용하지 않는다.

## 2. 기능 목적

Chain Setup의 목적은 Input과 Output만 등록하는 것이 아니다.

Input부터 Output까지 전류가 통과하는 연결 단자, Ball 안착부, 배선 구간과 각 부위의 연결 순서를 기준정보로 저장하는 것이 핵심이다.

이 기준정보는 추후 다음 분석에 사용한다.

1. AOI Defect 좌표를 GDS 좌표로 변환한다.
2. Defect와 겹치는 연결 단자, Ball 안착부 또는 배선 구간을 찾는다.
3. 해당 부위가 속한 Chain을 찾는다.
4. Chain의 Input부터 Output까지 연결 경로를 표시한다.
5. ET Fail 결과와 함께 전류 차단 연관 후보를 제공한다.

Defect가 특정 부위와 겹쳤다는 사실은 전류 차단 원인 확정이 아니다. 시스템은 위치상 연관 후보와 연결 경로를 제공하고, ET 결과와 함께 검토할 수 있도록 해야 한다.

## 3. 핵심 용어

| 용어 | 설명 |
| --- | --- |
| Chain | 하나의 Input에서 Output까지 이어지는 전기적 연결 경로 |
| Input | Chain의 시작 단자 |
| Output | Chain의 종료 단자 |
| Connection Terminal | 배선, Ball 안착부 또는 다른 부위가 연결되는 단자 |
| Ball Site | Ball이 안착되는 원형 위치 |
| Wire Segment | 단자와 단자 또는 단자와 Ball Site 사이를 연결하는 배선 구간 |
| Chain Part | Input, Output, 연결 단자, Ball Site, 배선 구간을 포함하는 Chain 구성 부위 |
| Connection | 두 Chain Part 사이의 연결 관계 |
| Defect Hit | Defect 좌표 또는 영역이 Chain Part의 분석 영역과 겹치는 상태 |

## 4. 기본 관리 구조

기준정보는 다음 계층으로 관리한다.

```text
Device
  -> GDS Revision
    -> Chain
      -> Chain Part
        -> GDS Element
      -> Connection
```

하나의 Chain Part가 항상 GDS Element 하나와 일치한다고 가정하지 않는다.

예를 들어 하나의 Ball Site가 외곽 원, 내부 원, 접속 패턴 등 여러 GDS Element로 구성될 수 있다. 엔지니어는 여러 Element를 선택하여 하나의 Chain Part로 묶을 수 있어야 한다.

GDS Layer와 Chain은 서로 다른 개념이다. 하나의 Chain은 여러 Layer의 Element를 포함할 수 있다.

## 5. Chain 구성 예시

다음은 개념 설명용 예시이며 제공된 화면의 실제 연결 순서를 확정한 내용은 아니다.

```text
Input
  -> Wire Segment 01
  -> Ball Site 01
  -> Wire Segment 02
  -> Connection Terminal 01
  -> Wire Segment 03
  -> Ball Site 02
  -> Wire Segment 04
  -> Output
```

각 부위는 이전 부위와 다음 부위 정보를 가진다. 이를 이용하여 Defect가 발견된 부위의 앞뒤 연결과 전체 Chain을 추적한다.

## 6. 엔지니어 설정 절차

### 6.1 Device와 GDS 선택

1. 설정할 Device를 선택한다.
2. 해당 Device에 적용된 GDS Revision을 선택한다.
3. GDS를 화면에 표시한다.
4. 필요한 Layer만 표시하여 작업 범위를 줄인다.

### 6.2 Chain 생성

1. 신규 Chain을 생성한다.
2. Chain ID와 Chain Name을 입력한다.
3. 화면 표시 색상을 선택한다.
4. 상태를 `작성 중`으로 생성한다.

### 6.3 Input과 Output 지정

1. 상단의 시작 단자를 선택한다.
2. 선택한 부위의 역할을 `Input`으로 지정한다.
3. 하단의 종료 단자를 선택한다.
4. 선택한 부위의 역할을 `Output`으로 지정한다.
5. 화면에 `IN`과 `OUT`을 고정 표시한다.

Input과 Output은 각각 하나의 GDS Element 또는 여러 GDS Element의 묶음으로 설정할 수 있어야 한다.

### 6.4 중간 연결 부위 설정

엔지니어는 Input에서 Output 방향으로 다음 부위를 순서대로 설정한다.

1. 연결 단자를 선택한다.
2. Ball 안착부를 선택한다.
3. 두 부위를 연결하는 배선 구간을 선택한다.
4. 선택한 Element를 하나의 Chain Part로 묶는다.
5. Chain Part의 종류와 순서를 지정한다.
6. 이전 Chain Part와 다음 Chain Part의 연결을 확인한다.

### 6.5 연결 검사

설정 완료 전 시스템은 다음 내용을 검사한다.

| 검사 항목 | 확인 내용 |
| --- | --- |
| Input 존재 | Input이 정확히 지정되었는가 |
| Output 존재 | Output이 정확히 지정되었는가 |
| 경로 완성 | Input에서 Output까지 연결되는가 |
| 단절 | 이전 또는 다음 연결이 없는 중간 부위가 있는가 |
| 중복 연결 | 같은 Element가 의도하지 않게 여러 부위에 등록되었는가 |
| 분기 | 한 부위에서 연결이 여러 방향으로 나뉘는가 |
| 교차 Layer | Layer 간 연결 규칙에 맞게 연결되었는가 |
| 빈 부위 | GDS Element가 없는 Chain Part가 있는가 |
| Revision 일치 | 설정에 사용한 GDS Revision이 현재 Revision과 같은가 |

### 6.6 저장과 확정

Chain의 상태는 다음과 같이 관리한다.

| 상태 | 의미 |
| --- | --- |
| 작성 중 | 설정 작업이 끝나지 않은 상태 |
| 검증 필요 | 저장은 되었지만 단절, 분기 또는 확인 항목이 남은 상태 |
| 확정 | 연결 검사를 통과하고 엔지니어가 확인한 상태 |
| 사용 중지 | GDS 변경 또는 기준 변경으로 분석에 사용하지 않는 상태 |

`작성 중`과 `검증 필요` 상태의 Chain은 운영 Correlation 결과에 사용하지 않는 것을 기본 원칙으로 한다.

## 7. 화면 구성 컨셉

| 화면 영역 | 주요 기능 |
| --- | --- |
| 상단 | Device / GDS Revision / Chain 생성 / 임시 저장 / 연결 검사 / 확정 |
| 왼쪽 | Chain 목록 / 검색 / 상태 / 색상 / 표시 여부 |
| 중앙 | GDS Map / Input / Output / Chain Part / 연결 경로 표시 |
| 오른쪽 | 선택 부위의 유형 / 이름 / 순서 / GDS Element / 이전 연결 / 다음 연결 |
| 하단 | 단절 / 중복 / 분기 / 확인 필요 항목과 오류 위치 이동 |

화면 표시 규칙은 다음과 같이 제안한다.

| 표시 대상 | 표시 방법 |
| --- | --- |
| 현재 Chain | 지정한 Chain 색상으로 강조 |
| Input | `IN` 표시와 시작점 아이콘 |
| Output | `OUT` 표시와 종료점 아이콘 |
| 현재 선택 부위 | 굵은 외곽선 또는 점멸 표시 |
| 연결 후보 | 반투명 후보 색상 |
| 단절 위치 | 경고 색상과 오류 아이콘 |
| Defect Hit 부위 | Defect 표시와 별도 강조 색상 |
| 주변 도형 | 흐리게 표시하거나 선택적으로 숨김 |

Chain 색상과 현재 선택 색상은 구분해야 한다. 현재 부위를 선택하더라도 원래 Chain의 색상을 확인할 수 있어야 한다.

## 8. 설정 편의 기능

### 8.1 선택 기능

- 클릭 선택
- 영역 선택
- 선택 추가 모드
- 선택 제외 모드
- 여러 GDS Element를 하나의 Chain Part로 묶기
- 겹친 Element 목록에서 Layer와 Element를 확인하여 선택
- 화면 선택과 목록 선택의 양방향 연동

### 8.2 경로 설정 기능

- Input부터 다음 연결 후보 강조
- 시작 부위와 종료 부위를 지정하여 중간 경로 후보 찾기
- 이전 부위 / 다음 부위 순차 이동
- 연결이 끊긴 위치에서 후보 찾기 중지
- 후보가 여러 개인 경우 엔지니어가 직접 선택
- 수동 연결과 시스템 제안 연결을 구분하여 저장

시스템은 단순히 좌표가 가깝다는 이유로 다른 도형을 자동 연결하면 안 된다. 동일 Layer의 접촉 조건, 다른 Layer 사이의 접속 규칙, 허용 오차가 확인된 경우에만 연결 후보를 제안한다.

### 8.3 반복 패턴 지원

반복되는 Ball Site와 배선 구조는 다음 방식으로 복사할 수 있어야 한다.

1. 기준 Chain 또는 기준 구간을 선택한다.
2. X/Y 이동 간격과 반복 개수를 지정한다.
3. Chain 이름과 부위 번호 생성 규칙을 지정한다.
4. 이동한 위치에서 실제 GDS Element를 다시 찾는다.
5. 누락 또는 구조 차이가 있는 복사 결과를 `검증 필요`로 표시한다.
6. 엔지니어가 확인한 결과만 확정한다.

원본 Chain의 좌표만 복사하여 저장하지 않는다. 복사 대상 위치의 실제 GDS Element와 다시 연결해야 한다.

### 8.4 작업 보호 기능

- 실행 취소 / 다시 실행
- 임시 저장
- 확정 Chain 잠금
- 수정 전후 비교
- 변경자 / 변경일 / 변경 사유 저장
- GDS Revision 변경 시 기존 설정 영향 확인

## 9. 저장 정보 컨셉

### 9.1 Chain 정보

| 항목 | 설명 |
| --- | --- |
| Device ID | 대상 Device |
| GDS Revision | 설정에 사용한 GDS 버전 |
| Chain ID | Chain 식별자 |
| Chain Name | 엔지니어가 확인할 수 있는 이름 |
| Input Part ID | Input 부위 식별자 |
| Output Part ID | Output 부위 식별자 |
| Display Color | 화면 표시 색상 |
| Status | 작성 중 / 검증 필요 / 확정 / 사용 중지 |
| Revision | Chain 설정 버전 |
| Created By / At | 최초 작성자 / 작성일 |
| Updated By / At | 최종 수정자 / 수정일 |

### 9.2 Chain Part 정보

| 항목 | 설명 |
| --- | --- |
| Part ID | Chain 구성 부위 식별자 |
| Part Type | Input / Output / Terminal / Ball Site / Wire Segment |
| Part Name | 부위 이름 |
| Sequence | Input에서 Output 방향의 기본 순서 |
| Element References | 부위를 구성하는 GDS Element 목록 |
| Analysis Bounds | 빠른 Defect 검색에 사용하는 영역 |
| Center X/Y | 부위 중심 좌표 |
| Layer 정보 | 부위가 사용하는 Layer 목록 |

### 9.3 Connection 정보

| 항목 | 설명 |
| --- | --- |
| From Part ID | 연결 시작 부위 |
| To Part ID | 연결 종료 부위 |
| Connection Type | 직접 연결 / Layer 간 연결 / 수동 지정 등 |
| Direction | Input에서 Output 방향의 조회 순서 |
| Validation Status | 검증 완료 / 확인 필요 |
| Remark | 연결 근거와 엔지니어 메모 |

GDS 화면에서 반복 배치된 동일 원본 Element가 있을 수 있으므로, Element 식별자는 원본 정보만 저장해서는 안 된다. GDS Revision, Structure 경로, 배치 인스턴스, Layer, DataType, 도형 위치 등을 조합하여 실제 화면상의 배치 위치를 다시 찾을 수 있어야 한다.

## 10. Defect 연관 분석 컨셉

### 10.1 기본 처리 순서

```text
AOI Defect 수신
  -> AOI 좌표를 GDS 좌표로 변환
  -> Offset과 Match Tolerance 적용
  -> 겹치는 Chain Part 후보 검색
  -> 소속 Chain 조회
  -> Input부터 Output까지 관련 경로 표시
  -> ET Fail 결과와 함께 연관 후보 제공
```

### 10.2 분석 영역

Chain 전체를 감싸는 하나의 큰 Rectangle만 사용하면 배선 사이의 빈 공간까지 Chain 영역으로 판정될 수 있다.

POC에서는 다음 두 단계로 관리하는 안을 제안한다.

| 영역 | 용도 |
| --- | --- |
| Chain 전체 Bounding Box | Chain 후보를 빠르게 찾고 전체 화면으로 이동 |
| Chain Part별 Bounding Box | 연결 단자, Ball Site, 배선 구간의 Defect 겹침 후보 판정 |

Ball Site는 원형에 가까운 영역이고 배선은 길고 폭이 좁은 영역이다. Bounding Box만으로 오검출이 많으면 실제 Geometry 판정을 후속 단계로 검토한다.

### 10.3 분석 결과 표시

분석 결과에는 다음 정보를 제공한다.

- Defect ID와 좌표
- 겹친 Chain Part의 종류와 이름
- 해당 Chain ID와 Chain Name
- Chain Part의 이전 연결과 다음 연결
- Input부터 Output까지 전체 경로
- 관련 ET Parameter와 Pass/Fail
- 위치 겹침 판정에 사용한 Offset과 Match Tolerance
- 후보가 여러 개인 경우 전체 후보 목록

결과 상태는 다음처럼 구분한다.

| 결과 | 의미 |
| --- | --- |
| 위치 겹침 | Defect가 특정 Chain Part의 분석 영역에 포함됨 |
| ET Fail 동시 발생 | 위치 겹침 Chain과 관련된 ET Fail이 존재함 |
| 전류 차단 연관 후보 | 위치 겹침과 ET Fail을 함께 검토할 대상 |
| 확인 필요 | 좌표 또는 Chain 설정이 불완전하여 확정할 수 없음 |

## 11. 분기와 공용 부위 처리

Chain이 항상 하나의 직선형 경로라고 가정하지 않는다.

실제 구조에 분기 또는 공용 부위가 있다면 Connection을 이용하여 그래프 형태로 저장한다. 기본 Sequence는 화면 조회 순서로만 사용하고, 실제 연결 관계는 From Part와 To Part를 기준으로 판단한다.

하나의 Chain Part 또는 GDS Element가 여러 Chain에 포함될 수 있는지는 고객 기준 확인이 필요하다. 초기 구현에서는 중복 등록 시 경고를 표시하고, 엔지니어가 공용 부위인지 잘못 선택한 것인지 확인하도록 한다.

## 12. POC 단계 구현 범위

### 12.1 1단계 / 수동 설정 검증

- 대표 Chain 1개 생성
- Input / Output 지정
- 연결 단자 / Ball Site / 배선 구간 등록
- Chain Part 간 연결 순서 등록
- 저장 후 같은 위치로 재조회
- 임의 좌표 선택 시 겹치는 Chain Part와 Chain 표시

완료 기준은 저장 후 GDS를 다시 열었을 때 동일한 배치 위치의 Element가 복원되고, Input에서 Output까지의 연결 관계가 동일하게 표시되는 것이다.

### 12.2 2단계 / 작업 편의성과 검증

- 연결 단절 / 중복 / 분기 검사
- Chain 목록과 GDS 화면 연동
- 현재 Chain 강조
- 실행 취소 / 다시 실행
- 확정 Chain 잠금
- 반복 구간 복사

### 12.3 3단계 / 연결 후보 지원

- 실제 샘플로 확인된 접촉 조건 적용
- 동일 Layer 연결 후보 찾기
- Layer 간 접속 규칙 적용
- 후보가 여러 개인 위치의 사용자 선택
- 자동 제안 결과와 수동 확정 결과 구분

### 12.4 후속 단계 / AOI와 ET 연계

- AOI와 GDS 좌표 정합
- Defect와 Chain Part의 겹침 판정
- Chain과 ET Parameter 연결
- ET Fail Chain과 AOI Defect 동시 표시
- 고객 선정 Defect Sample을 이용한 결과 검증

## 13. 정상 동작 확인 시나리오

| 순서 | 확인 내용 | 기대 결과 |
| --- | --- | --- |
| 1 | 대표 GDS와 Chain을 연다 | GDS Revision과 Chain 상태가 표시됨 |
| 2 | 상단 단자를 Input으로 지정한다 | 해당 위치에 `IN`이 표시됨 |
| 3 | 하단 단자를 Output으로 지정한다 | 해당 위치에 `OUT`이 표시됨 |
| 4 | 중간 단자, Ball Site, 배선 구간을 등록한다 | 각 부위가 지정한 Chain 색상으로 표시됨 |
| 5 | 연결 검사를 실행한다 | Input에서 Output까지 도달 여부와 오류 위치가 표시됨 |
| 6 | Chain을 저장하고 화면을 다시 연다 | 동일 Element와 연결 관계가 복원됨 |
| 7 | 임의의 부위에 Defect 좌표를 입력한다 | 겹친 부위와 소속 Chain이 표시됨 |
| 8 | ET Fail을 연결한다 | 위치 겹침과 ET Fail이 함께 조회됨 |

## 14. 구현 전 확인 필요 사항

다음 항목은 실제 샘플과 고객 기준을 받아 확정해야 한다.

| 확인 항목 | 확인할 내용 |
| --- | --- |
| 대표 Chain | 실제 Input / Output 한 쌍과 전체 연결 경로 |
| 부위 구분 | 연결 단자, Ball Site, 배선 구간을 나누는 기준 |
| Layer 연결 | 서로 다른 Layer 사이가 전기적으로 연결되는 조건 |
| 접촉 판정 | 도형 접촉, 겹침, 근접 허용값의 기준 |
| 연결 분기 | 분기와 우회 경로의 존재 여부와 처리 기준 |
| 공용 부위 | 하나의 부위가 여러 Chain에 속할 수 있는지 |
| Chain 이름 | ET CSV의 Chain Name과 GDS Chain의 연결 방법 |
| Element 식별 | GDS Revision 변경 후 동일 부위를 찾는 방법 |
| AOI 좌표 | 원점, 방향, 단위, 회전, 반전, Die 기준 좌표 |
| Match Tolerance | AOI Defect와 Chain Part 위치 겹침 허용값 |
| 연결 허용 오차 | GDS Element 사이의 연결 후보 판단 허용값 |
| Defect 크기 | 점 좌표인지 폭과 높이를 가진 영역인지 |
| 판정 기준 | 위치 겹침과 ET Fail을 어떤 규칙으로 연관 후보 처리할지 |

연결 후보 판단에 사용하는 허용 오차와 AOI Defect 위치 매칭에 사용하는 Match Tolerance는 목적이 다르므로 별도 값으로 관리한다.

## 15. 현재 결론

Chain Setup은 다음 세 가지를 함께 설정하는 기능이어야 한다.

1. 상단 Input과 하단 Output
2. 그 사이의 연결 단자, Ball 안착부, 배선 구간
3. 각 부위가 Input에서 Output까지 이어지는 연결 관계

POC는 엔지니어가 수동으로 확정할 수 있는 기능부터 구현하고, 연결 후보 찾기와 반복 패턴 복사를 편의 기능으로 추가하는 방향이 적절하다.

최초 검증은 대표 Chain 1개를 대상으로 수행한다. 실제 Chain의 Input, Output, 구성 부위와 Layer 간 연결 규칙이 확인되기 전에는 GDS 형상만으로 Chain을 자동 생성하거나 전기적 연결을 확정하지 않는다.

## 16. DB 저장 컨셉

Chain 설정이 완료되면 해당 정보를 DB에 저장한다.

DB 저장의 목적은 다음과 같다.

- Device와 GDS Revision별 Chain 기준정보 관리
- 저장된 Chain을 GDS 화면에서 동일하게 복원
- AOI Defect가 겹친 Chain Part와 전체 Chain 조회
- ET Fail Chain과 AOI Defect의 연관관계 분석
- Chain 설정 변경 이력과 적용 시점 추적

### 16.1 저장 시점

Chain 설정 화면에서는 `임시 저장`과 `확정 저장`을 구분한다.

| 저장 구분 | 처리 내용 |
| --- | --- |
| 임시 저장 | 작성 중인 Chain과 선택한 부위를 저장한다. 분석 기준정보로 사용하지 않는다. |
| 확정 저장 | 연결 검사를 통과한 Chain을 확정 버전으로 저장한다. Correlation 분석에 사용할 수 있다. |

확정 저장 시 다음 조건을 확인한다.

1. Device와 GDS Revision이 지정되어 있어야 한다.
2. Input과 Output이 지정되어 있어야 한다.
3. Input에서 Output까지 연결 가능한 경로가 있어야 한다.
4. 모든 Chain Part에 실제 GDS Element가 연결되어 있어야 한다.
5. 단절, 중복, 분기 등 확인이 필요한 항목을 엔지니어가 검토해야 한다.
6. 동일 Device / GDS Revision / Chain ID의 중복 저장을 확인해야 한다.

### 16.2 논리 테이블 구성

아래 테이블명은 컨셉 설명용이다. 실제 명칭과 컬럼 타입은 적용할 DB 표준을 확인한 후 결정한다.

| 논리 테이블 | 저장 내용 |
| --- | --- |
| CHAIN_MASTER | Device, GDS Revision, Chain ID, 이름, Input, Output, 상태, 버전 |
| CHAIN_PART | Input, Output, Terminal, Ball Site, Wire Segment 등 Chain 구성 부위 |
| CHAIN_PART_ELEMENT | 하나의 Chain Part를 구성하는 실제 GDS Element 목록 |
| CHAIN_CONNECTION | Chain Part 사이의 From / To 연결 관계 |
| CHAIN_HISTORY | Chain 생성, 수정, 확정, 사용 중지 이력 |

논리 관계는 다음과 같다.

```text
CHAIN_MASTER 1
  -> N CHAIN_PART
       -> N CHAIN_PART_ELEMENT

CHAIN_MASTER 1
  -> N CHAIN_CONNECTION

CHAIN_MASTER 1
  -> N CHAIN_HISTORY
```

### 16.3 CHAIN_MASTER 주요 정보

| 항목 | 설명 |
| --- | --- |
| Device ID | Chain이 적용되는 Device |
| GDS Revision | Chain 설정 당시 사용한 GDS 버전 |
| Chain ID | Device 내 Chain 식별자 |
| Chain Name | ET 결과 또는 엔지니어가 사용하는 Chain 이름 |
| Input Part ID | 시작 단자에 해당하는 Chain Part |
| Output Part ID | 종료 단자에 해당하는 Chain Part |
| Status | 작성 중 / 검증 필요 / 확정 / 사용 중지 |
| Version | Chain 설정 버전 |
| Effective From | 해당 버전의 적용 시작 시점 |
| Effective To | 해당 버전의 적용 종료 시점 |
| Created By / At | 최초 작성자 / 작성일 |
| Updated By / At | 최종 수정자 / 수정일 |

### 16.4 CHAIN_PART 주요 정보

| 항목 | 설명 |
| --- | --- |
| Part ID | Chain 내부의 구성 부위 식별자 |
| Chain ID / Version | 소속 Chain과 버전 |
| Part Type | Input / Output / Terminal / Ball Site / Wire Segment |
| Part Name | 엔지니어가 확인할 수 있는 부위 이름 |
| Sequence | 화면 조회를 위한 기본 순서 |
| Center X / Y | 부위 중심 좌표 |
| Min X / Min Y | 부위 분석 영역의 최소 좌표 |
| Max X / Max Y | 부위 분석 영역의 최대 좌표 |
| Validation Status | 검증 완료 / 확인 필요 |

Sequence는 화면에서 Input부터 Output 방향으로 부위를 정렬하기 위한 값이다. 실제 전기적 연결은 CHAIN_CONNECTION의 From Part와 To Part를 기준으로 판단한다.

### 16.5 CHAIN_PART_ELEMENT 주요 정보

하나의 Ball Site, Terminal 또는 배선 구간이 여러 GDS Element로 구성될 수 있으므로 Chain Part와 GDS Element를 분리하여 저장한다.

| 항목 | 설명 |
| --- | --- |
| Part ID | 소속 Chain Part |
| Element Reference ID | 화면에서 동일한 배치 Element를 찾기 위한 식별자 |
| Structure Path | GDS Structure와 하위 참조 경로 |
| Instance Path | SREF / AREF에 의해 배치된 인스턴스 경로 |
| Layer ID | GDS Layer |
| Data Type | GDS DataType |
| Element Type | Boundary / Path / Text 등 |
| Element Bounds | 실제 배치된 Element의 Min X / Min Y / Max X / Max Y |
| Element Signature | Element 변경 여부를 확인하기 위한 식별값 |

Element Reference ID를 현재 화면의 순번이나 메모리 객체 기준으로 저장하면 안 된다. 파일을 다시 열어도 같은 배치 Element를 찾을 수 있는 식별 기준이 필요하다.

### 16.6 CHAIN_CONNECTION 주요 정보

| 항목 | 설명 |
| --- | --- |
| Chain ID / Version | 소속 Chain과 버전 |
| From Part ID | 연결 시작 부위 |
| To Part ID | 연결 종료 부위 |
| Connection Type | 직접 연결 / Layer 간 연결 / 수동 지정 등 |
| Validation Status | 검증 완료 / 확인 필요 |
| Remark | 연결 근거 또는 엔지니어 메모 |

분기가 존재할 수 있으므로 이전 Part ID와 다음 Part ID를 CHAIN_PART에 한 개씩만 저장하지 않는다. 연결 정보를 별도 테이블로 관리해야 한 부위에서 여러 경로로 나뉘는 구조를 표현할 수 있다.

### 16.7 저장 처리 단위

Chain 한 건의 저장은 하나의 DB Transaction으로 처리한다.

```text
Transaction 시작
  -> CHAIN_MASTER 저장
  -> CHAIN_PART 저장
  -> CHAIN_PART_ELEMENT 저장
  -> CHAIN_CONNECTION 저장
  -> 연결 무결성 검사
  -> CHAIN_HISTORY 저장
Transaction Commit
```

중간 단계에서 오류가 발생하면 전체 저장을 Rollback한다. MASTER만 저장되고 구성 부위나 연결 정보가 누락되는 상태가 발생하면 안 된다.

### 16.8 저장 전 검증

| 검증 항목 | 검증 내용 |
| --- | --- |
| 필수값 | Device, GDS Revision, Chain ID, Input, Output 존재 여부 |
| 참조 무결성 | 모든 Connection의 From / To Part가 현재 Chain에 존재하는지 |
| Element 존재 | 모든 Chain Part에 연결된 GDS Element가 존재하는지 |
| 경로 검사 | Input에서 Output까지 도달 가능한지 |
| 순환 검사 | 의도하지 않은 무한 순환 연결이 있는지 |
| 중복 검사 | 동일 Element가 같은 Chain Part에 중복 저장되는지 |
| 버전 검사 | 수정 대상 Chain Version이 DB의 최신 버전과 같은지 |
| Revision 검사 | 현재 화면의 GDS Revision과 저장 대상 Revision이 같은지 |

### 16.9 수정과 버전 관리

확정된 Chain을 수정할 때는 기존 확정 데이터를 직접 덮어쓰지 않고 새 Version을 생성하는 방향을 제안한다.

예를 들어 Version 1이 분석에 사용된 후 배선 구간이 변경되었다면 Version 1을 유지하고 Version 2를 신규 생성한다. 이를 통해 과거 Defect 분석이 당시 어떤 Chain 설정을 사용했는지 다시 확인할 수 있다.

| 상황 | 처리 방식 |
| --- | --- |
| 작성 중 Chain 수정 | 현재 작성 버전 수정 가능 |
| 확정 Chain 수정 | 새 Version 생성 후 재검증 |
| GDS Revision 변경 | 기존 Chain 복사 후 Element 재연결 및 검증 |
| Chain 사용 중지 | 데이터 삭제 대신 상태 변경 |
| 잘못 저장된 임시 Chain | 권한과 사용 이력을 확인한 후 처리 |

### 16.10 DB 조회와 화면 복원

저장된 Chain을 조회할 때 다음 순서로 화면을 복원한다.

1. Device와 GDS Revision에 해당하는 확정 Chain을 조회한다.
2. Chain Part와 Connection을 조회한다.
3. Chain Part별 Element Reference를 이용하여 현재 GDS 화면의 실제 배치 Element를 찾는다.
4. 찾은 Element를 Chain 색상으로 표시한다.
5. Input과 Output을 표시한다.
6. 찾지 못한 Element 또는 변경된 Element는 `검증 필요`로 표시한다.

DB에 좌표가 있다는 이유만으로 현재 GDS의 가까운 Element에 자동 연결하면 안 된다. GDS Revision과 Element 식별정보가 일치하지 않으면 엔지니어 확인 대상으로 처리한다.

### 16.11 Correlation 결과와 Chain Version

AOI Defect와 ET Fail의 Correlation 결과를 저장할 경우, 분석에 사용한 Chain ID뿐 아니라 Chain Version도 함께 저장해야 한다.

이를 통해 다음 정보를 추적할 수 있다.

- 어떤 GDS Revision을 사용했는가
- 어떤 Chain 설정 버전을 사용했는가
- Defect가 어떤 Chain Part와 겹쳤는가
- 당시 적용한 Offset과 Match Tolerance는 무엇인가
- 재분석으로 결과가 변경되었는가

### 16.12 DB 설계 전 추가 확인 사항

실제 DDL 작성 전 다음 내용을 확인해야 한다.

| 확인 항목 | 내용 |
| --- | --- |
| 적용 DB | Oracle 등 실제 DBMS와 버전 |
| 스키마 | 테이블을 생성할 Schema와 명명 규칙 |
| 키 생성 | Sequence / Identity / GUID 등 프로젝트 표준 |
| 좌표 타입 | 좌표 정밀도와 NUMBER 자릿수 |
| 대량 저장량 | Device별 Chain 수 / Part 수 / Element 수 |
| 이력 정책 | 기존 Version의 보존 기간과 조회 방식 |
| 권한 | 작성 / 검증 / 확정 / 사용 중지 권한 구분 |
| 동시 수정 | 두 엔지니어가 같은 Chain을 수정할 때 처리 기준 |
| GDS 저장 방식 | GDS 파일 경로, 파일 해시, DB 저장 여부 |
| Correlation 결과 | 결과 저장 테이블과 재분석 이력 관리 여부 |

DB 테이블 DDL은 위 확인 사항과 실제 프로젝트의 기존 테이블 명명 규칙을 확인한 후 별도 설계한다.

## 17. 구현 진행 현황

### 17.1 2026-09-23 / 1차 기반 구현

여러 Layer에 걸친 Element를 사용자가 하나씩 지정하지 않도록 Chain 후보 추적 기반을 추가하였다.

구현된 범위는 다음과 같다.

- Input Element와 Output Element를 지정한 후보 경로 탐색
- 사용자가 지정한 작업 영역 안에서만 Element 검색
- Device별 Layer 연결 규칙이 있는 조합만 연결
- 같은 Layer 연결과 다른 Layer 연결 규칙 분리
- Element Bounding Box와 연결 허용 오차를 사용한 1차 후보 판정
- 공간 격자를 이용한 주변 Element 후보 검색
- Input에서 Output까지 BFS 후보 경로 생성
- 현재 GDS 화면의 Boundary / Path를 추적 모델로 변환하는 API
- 허용 Layer / 금지 Layer / 작업 영역 / 연결 허용 오차 Probe 검증

현재 결과는 전기적 연결 확정 결과가 아니라 엔지니어가 확인할 후보 경로이다.

다음 구현 범위는 다음과 같다.

1. 후보 경로의 별도 색상 표시
2. 분기 위치와 복수 후보 표시
3. 실제 GDS를 이용한 Layer 연결 규칙 검증
4. 영구 Element 식별자 확정
5. Chain 확정과 DB 저장

### 17.2 2026-09-23 / Chain 설정 화면 연결

1차 후보 추적 기능을 실제 GDS Map 화면에서 사용할 수 있도록 다음 설정 기능을 연결하였다.

- GDS 조회 전에는 Chain 설정 버튼을 비활성화한다.
- GDS 조회 후 Input / Output / 작업 영역 지정 버튼을 활성화한다.
- Input은 녹색 원과 `IN` 문자로 표시한다.
- Output은 주황색 원과 `OUT` 문자로 표시한다.
- 작업 영역은 파란색 점선 사각형과 모서리 표시로 구분한다.
- 화면 확대와 이동 후에도 지정 좌표를 기준으로 표식을 다시 그린다.
- 새로운 GDS를 조회하면 기존 Input / Output / 작업 영역과 후보 강조를 초기화한다.
- 화면 상단을 3행으로 구성하여 설정 상태 문구가 잘리지 않도록 한다.

### 17.3 실제 화면 검사 결과

샘플 파일 `SampleFile/OMM all layer2.gds/OMM all layer2.gds`를 사용하여 다음 항목을 확인하였다.

| 검사 항목 | 결과 | 확인 내용 |
| --- | --- | --- |
| GDS 조회 | 정상 | 파일 읽기와 첫 화면 그리기 완료 |
| 설정 버튼 활성화 | 정상 | GDS 조회 완료 후 버튼 활성화 |
| Input 지정 | 정상 | 선택한 상단 원에 녹색 `IN` 표시 |
| Output 지정 | 정상 | 선택한 하단 원에 주황색 `OUT` 표시 |
| 작업 영역 지정 | 정상 | 두 모서리를 연결한 파란색 점선 사각형 표시 |
| 동시 표시 | 정상 | Input / Output / 작업 영역이 같은 화면에 유지됨 |
| 상태 문구 | 정상 | 3행 화면에서 안내 문구가 잘리지 않음 |

이번 화면 검사는 좌표 지정과 표시 기능을 검증한 것이다. 표시한 Input과 Output이 실제 전기적 Chain으로 연결되는지는 Layer 연결 규칙과 Element 식별 기준을 적용한 후보 경로 검증 단계에서 별도로 확인해야 한다.

### 17.4 다음 구현 범위

다음 단계에서는 사용자가 Layer 번호를 문자열로 직접 입력하지 않도록 Layer 연결 규칙 편집 화면을 먼저 구현한다.

1. 현재 GDS의 Layer 목록을 규칙 편집 화면에 표시한다.
2. From Layer / To Layer를 목록에서 선택한다.
3. Layer 사이의 연결 허용 오차를 입력한다.
4. 여러 규칙을 표 형태로 추가하거나 삭제한다.
5. 입력한 규칙으로 후보 경로를 계산한다.
6. 후보 경로와 분기 후보를 화면에서 구분하여 표시한다.

### 17.5 2026-09-23 / Layer 연결 규칙 편집 화면

Layer 연결 규칙을 문자열로 직접 입력하던 방식을 표 형태의 편집 화면으로 변경하였다.

- 현재 GDS에 존재하는 Layer만 From Layer / To Layer 선택 목록에 표시한다.
- 규칙 추가 버튼으로 Layer 조합과 연결 허용 오차를 한 행씩 등록한다.
- 여러 행을 선택하여 삭제할 수 있다.
- 연결 허용 오차는 0 이상의 숫자만 허용한다.
- Layer 연결은 양방향으로 판단하므로 `1 / 2`와 `2 / 1`의 중복 등록을 차단한다.
- 적용 전까지 메인 화면의 기존 규칙은 변경하지 않는다.
- 적용한 규칙 수와 앞쪽 세 개 조합을 메인 화면에서 요약하여 표시한다.
- 새로운 GDS를 조회하면 이전 GDS의 Layer 규칙을 초기화한다.

샘플 GDS 화면 검사 결과는 다음과 같다.

| 검사 항목 | 결과 | 확인 내용 |
| --- | --- | --- |
| Layer 목록 전달 | 정상 | Layer 1부터 Layer 8과 Layer 775 표시 |
| 규칙 행 추가 | 정상 | 기본값 `1 / 1 / 0`으로 한 행 추가 |
| 선택 목록 제한 | 정상 | 현재 GDS에 존재하는 Layer만 표시 |
| 규칙 적용 | 정상 | 메인 화면에 `1개 / 1-1:0`으로 반영 |
| 상태 안내 | 정상 | `Layer 연결 규칙 1개를 적용했습니다.` 표시 |

다음 단계에서는 Input / Output / 작업 영역과 Layer 규칙을 함께 사용하여 실제 후보 경로를 실행하고, 후보 경로 표시와 실패 원인을 화면에서 확인한다.

### 17.6 2026-09-23 / 실제 후보 경로 1차 실행

샘플 GDS에서 인접한 원 두 개를 Input과 Output으로 지정하고 작은 작업 영역을 설정한 뒤 `Layer 1 / Layer 1 / 연결 허용 오차 0` 규칙으로 후보 경로를 실행하였다.

실행 결과는 다음과 같다.

| 검사 항목 | 결과 | 확인 내용 |
| --- | --- | --- |
| Input 지정 | 정상 | 선택한 원에 `IN` 표시 |
| Output 지정 | 정상 | 인접한 원에 `OUT` 표시 |
| 작업 영역 지정 | 정상 | 두 원을 포함한 파란색 점선 영역 표시 |
| 후보 추적 실행 | 정상 | 화면 정지나 오류 없이 완료 |
| 연결 결과 | 연결 실패 | Input 후보에서 Output까지 도달하지 못함 |
| 적용 규칙 | 확인 | Layer 1과 Layer 1 사이의 오차 0 규칙만 사용 |

이 결과는 현재 선택한 두 원 사이에 Layer 1만으로 연결되지 않는 중간 Element가 있거나, 실제 도형 사이에 0보다 큰 간격이 있음을 의미한다. 실제 Layer 연결 규칙을 확인하기 전에는 임의로 다른 Layer 조합이나 큰 허용 오차를 적용하지 않는다.

실패 원인을 확인하기 쉽도록 후보 추적 결과에 다음 진단 정보를 추가하였다.

- 선택된 Input Element의 Layer
- 선택된 Output Element의 Layer
- 탐색 중 방문한 Element 수
- 탐색 중 방문한 Layer 목록
- 연결 실패 시 방문한 후보 Element 화면 강조

다음 단계에서는 진단 결과를 실제 화면에서 확인한 후 고객 또는 설계 자료에서 Layer 간 연결 규칙과 허용 오차를 확정한다.

### 17.7 2026-09-23 / Input과 Output 수동 Element 지정 설계

#### 17.7.1 설계 원칙

Input과 Output은 시스템이 형상을 분석하여 자동 판별하지 않는다. 엔지니어가 GDS 화면을 확인하고 각각 하나의 대표 GDS Element를 직접 지정한다.

캡처에서 확인한 원형 Ball 위쪽의 접속 형상과 인접 Wire는 엔지니어가 Input 또는 Output 단자를 판단하기 위한 시각 정보로 사용한다. 시스템의 자동 판별 조건이나 자동 확정 조건으로 사용하지 않는다.

1차 구현에서는 Input과 Output에 각각 한 개의 대표 Element를 지정한다. 하나의 단자가 여러 Element로 구성된 경우의 Element 묶음 기능은 대표 Element 지정이 검증된 후 별도 단계에서 추가한다.

#### 17.7.2 사용자 설정 순서

1. 엔지니어가 `Input Element 지정` 버튼을 누른다.
2. GDS 화면에서 Input 단자로 판단한 위치를 클릭한다.
3. 시스템은 클릭 위치와 겹치는 GDS Element를 후보 목록으로 표시한다.
4. 엔지니어가 후보의 Layer, DataType, Element 종류와 크기를 확인한다.
5. 후보 행을 선택하면 해당 Element만 화면에서 미리 강조한다.
6. 엔지니어가 `Input 확정`을 눌러 대표 Element를 지정한다.
7. 같은 방식으로 Output Element를 지정한다.
8. 지정된 Element 전체를 역할별 색상으로 강조하고 선택 위치에 `IN` 또는 `OUT`을 표시한다.

후보가 한 개만 검색된 경우에도 시스템이 자동 확정하지 않는다. 후보 한 개를 미리 선택한 상태로 보여 주고 엔지니어의 확정 동작을 받는다.

#### 17.7.3 겹친 Element 후보 목록

| 표시 항목 | 용도 |
| --- | --- |
| 후보 번호 | 화면과 목록에서 같은 후보를 찾기 위한 임시 번호 |
| Layer | 여러 Layer가 같은 위치에 겹친 경우 구분 |
| DataType | 같은 Layer의 용도 구분 |
| Element 종류 | BOUNDARY / PATH 구분 |
| Bounds | Min X/Y, Max X/Y와 Width/Height 확인 |
| Point 수 | 단순 도형과 복합 도형 확인 |
| PATH Width | PATH Element인 경우 배선 폭 확인 |
| Structure 배치 경로 | 반복 배치된 동일 원본 Element의 실제 위치 구분 |

목록에서 행을 바꾸면 이전 미리보기 강조를 해제하고 현재 후보만 강조한다. 더블 클릭 또는 `확정` 버튼으로 선택을 완료하고, `취소`하면 기존 Input 또는 Output 설정을 유지한다.

#### 17.7.4 클릭 후보 검색 기준

클릭 좌표는 Input 또는 Output 자체가 아니라 후보 Element를 찾는 기준으로만 사용한다.

- 현재 화면에서 표시 중인 Layer의 BOUNDARY와 PATH를 검색한다.
- 마우스 클릭 주변에 일정한 화면 픽셀 허용 범위를 적용한다.
- 1차 구현은 Bounding Box가 클릭 허용 범위와 겹치는 Element를 후보로 표시한다.
- 큰 Bounding Box 때문에 후보가 과도하게 검색되면 BOUNDARY 내부 판정과 PATH 선분 거리 판정을 추가한다.
- 후보 순서는 클릭점과 가까운 Element, 작은 Element, Layer 번호 순으로 정렬하되 시스템이 자동 확정하지 않는다.

화면 픽셀 기준 허용 범위를 사용하는 이유는 확대 배율이 달라져도 클릭 편의성을 일정하게 유지하기 위해서다.

#### 17.7.5 선택 결과 모델

Input과 Output의 선택 결과에는 다음 정보를 보관한다.

| 항목 | 설명 |
| --- | --- |
| Role | Input 또는 Output |
| Runtime Element Key | 현재 화면 세션에서 Element를 다시 찾기 위한 임시 키 |
| Element Reference | DB 저장과 GDS 재조회에 사용할 영구 참조 |
| Layer / DataType | 선택한 Element의 GDS 속성 |
| Element Type | BOUNDARY 또는 PATH |
| World Bounds | 선택한 배치 Element의 실제 화면 좌표 영역 |
| Pick X/Y | 엔지니어가 클릭한 위치와 IN/OUT 표시 위치 |
| Structure Instance Path | 반복 배치된 Element의 배치 경로 |

현재 구현의 `SCENE:번호`는 작업 영역을 순회한 순서에 따라 만들어지는 실행 중 키다. GDS를 다시 조회하면 같은 번호가 같은 Element를 보장하지 않으므로 DB의 Element Reference로 저장하면 안 된다.

영구 Element Reference는 최소한 다음 값을 조합하여 설계한다.

```text
GDS Revision
+ Structure Instance Path
+ Layer
+ DataType
+ Element Type
+ 배치 후 World 좌표 또는 Geometry Hash
```

좌표 문자열만 식별자로 사용하면 소수점 처리나 GDS Revision 변경에 취약하므로, 원본 Structure 경로와 배치 인스턴스 정보를 함께 보관한다.

#### 17.7.6 화면 표시와 변경 규칙

| 상태 | 화면 처리 |
| --- | --- |
| 후보 검색 | 겹친 Element를 번호와 반투명 색상으로 표시 |
| 후보 미리보기 | 목록에서 선택한 한 개 Element만 굵게 강조 |
| Input 확정 | 선택 Element 강조와 클릭 위치의 `IN` 표시 |
| Output 확정 | 선택 Element 강조와 클릭 위치의 `OUT` 표시 |
| 다시 지정 | 기존 역할을 새 Element로 교체하고 후보 경로를 초기화 |
| 선택 취소 | 확정 전 변경만 버리고 기존 지정 상태 유지 |
| GDS Revision 불일치 | 자동 복원하지 않고 `Element 재지정 필요` 표시 |

Input과 Output은 같은 Element로 지정할 수 없다. 숨겨진 Layer의 Element는 후보 목록에서 제외한다. Layer 표시 상태가 변경되어 확정 Element가 숨겨져도 설정값은 유지하고, 화면 상단에 숨김 상태를 안내한다.

#### 17.7.7 1차 구현 범위

1. 클릭 위치의 겹친 Element 후보 조회 API
2. 후보 목록 화면
3. 후보 행 선택과 Element 미리보기 강조
4. Input 또는 Output 확정과 다시 지정
5. 같은 Element 중복 지정 방지
6. 선택 Element 정보와 클릭 위치 표시
7. 기존 후보 추적이 좌표 재검색 대신 확정된 Input/Output Element Key를 사용하도록 변경

1차 구현에서는 DB 저장, 여러 Element 묶음, Input/Output 자동 판별, 원형 Ball 형상 인식은 포함하지 않는다.

#### 17.7.8 구현 순서

첫 번째 작업은 클릭 위치에서 겹친 Element 목록을 반환하는 조회 기능이다. 이 기능이 확인되면 후보 선택 화면을 연결한다. 그다음 확정된 Element Key를 후보 추적기에 전달하여 현재의 `가장 작은 Element 자동 선택` 로직을 제거한다.

### 17.8 2026-09-23 / 작업 영역 없는 Element 연결 추적 설계

#### 17.8.1 변경 결정

Input과 Output Element를 수동으로 확정한 후 별도의 사각형 작업 영역은 지정하지 않는다.

기존 1차 POC의 작업 영역은 탐색 대상을 줄이기 위해 임시로 사용한 기능이다. 최종 Chain 설정 흐름에서는 Input Element를 시작점으로 사용하여 서로 연결된 Element를 순서대로 탐색하고, Output Element에 도달한 경로를 Chain 후보로 선택한다.

```text
Input Element 수동 확정
  -> Input과 연결된 주변 Element 검색
  -> 각 Element 사이의 실제 연결 여부 검사
  -> 연결된 Element만 다음 탐색 대상으로 추가
  -> Output Element 도달 확인
  -> Input부터 Output까지의 Element 경로 선택
  -> 엔지니어 검토와 Chain 확정
```

Input과 Output은 엔지니어가 지정하지만, 두 Element 사이의 연결 객체는 시스템이 연결 조건을 검사하여 선택한다.

#### 17.8.2 Element 연결 판단 절차

각 Element의 연결 여부는 다음 두 단계로 판단한다.

1. Bounding Box와 공간 격자로 가까운 Element를 빠르게 찾는다.
2. 가까운 후보에 대해서만 실제 도형 영역의 교차 여부를 검사한다.
3. Layer 규칙 적용을 선택한 경우 허용된 Layer 조합인지 추가로 검사한다.

Bounding Box가 겹친다는 이유만으로 연결을 확정하지 않는다. Bounding Box는 주변 후보를 줄이는 1차 검색에만 사용한다.

| 연결 종류 | 판단 기준 |
| --- | --- |
| 같은 Layer의 BOUNDARY | 두 Polygon의 실제 점유 영역이 교차하는지 검사 |
| 같은 Layer의 PATH | 선분과 폭을 적용한 실제 점유 영역이 교차하는지 검사 |
| BOUNDARY와 PATH | PATH 폭을 포함한 실제 점유 영역이 BOUNDARY와 교차하는지 검사 |
| 다른 Layer | 두 Element의 실제 점유 영역이 교차하는지 검사 |
| Layer 규칙 적용 상태 | 영역이 교차하고 등록된 Layer 조합에도 포함된 경우만 연결 |

기본 연결 허용 오차는 `0`으로 사용한다. 가까이 있지만 실제 점유 영역이 교차하지 않는 Element는 연결하지 않는다.

Layer 규칙 기능은 삭제하지 않고 선택적으로 적용한다. 규칙 적용을 끄면 왼쪽에서 필터링한 Layer 안의 실제 영역 교차만 검사한다. 규칙 적용을 켜면 실제 영역 교차 조건을 만족한 Element 중 등록된 Layer 조합만 연결한다.

#### 17.8.3 탐색 방식

전체 GDS Element를 한 번에 서로 비교하지 않는다. GDS가 크면 Element 수의 제곱만큼 비교가 발생할 수 있기 때문이다.

1. 현재 GDS의 BOUNDARY와 PATH를 공간 격자에 등록한다.
2. Input Element를 탐색 대기 목록에 넣는다.
3. 현재 Element가 포함된 격자와 인접 격자에서만 주변 후보를 찾는다.
4. 연결 조건을 통과한 Element만 방문 목록과 다음 탐색 대상에 추가한다.
5. Output Element에 도달하면 부모 연결 정보를 역추적하여 경로를 만든다.
6. 더 이상 연결된 Element가 없으면 단절로 처리한다.

수동 작업 영역을 사용하지 않아도 공간 격자와 엔지니어가 왼쪽에서 필터링한 Layer를 이용하여 탐색 범위를 제한한다.

#### 17.8.4 탐색 대상 제한

| 제한 기준 | 처리 방법 |
| --- | --- |
| 연결 대상 Layer | 엔지니어가 왼쪽 Layer 목록에서 체크한 Layer만 사용 |
| Element 종류 | 1차 구현은 BOUNDARY와 PATH만 사용 |
| Layer 규칙 | 적용을 선택한 경우에만 등록된 Layer 조합을 추가 검사 |
| 영역 교차 | 실제 점유 영역이 교차하지 않는 Element는 탐색하지 않음 |
| 방문 중복 | 한 번 검사한 Element는 다시 방문하지 않음 |
| 최대 방문 수 | 비정상적으로 큰 연결망이 생기면 탐색을 중지하고 확인 요청 |
| 실행 취소 | 탐색 중 취소할 수 있도록 처리 |

최대 방문 수는 실제 GDS의 대표 Chain 규모를 확인한 후 설정값으로 결정한다. 임의의 고정값으로 확정하지 않는다.

경로 찾기를 시작할 때 왼쪽 Layer 체크 상태를 복사하여 한 번의 탐색에 사용한다. 탐색이 끝난 뒤 Layer 체크 상태가 변경되면 기존 결과를 초기화하고 다시 실행하도록 안내한다.

Input 또는 Output Element의 Layer가 왼쪽에서 체크되어 있지 않으면 경로 찾기를 시작하지 않고 해당 Layer를 선택하도록 안내한다.

#### 17.8.5 경로 결과 처리

| 결과 | 화면 처리 |
| --- | --- |
| 경로 없음 | 방문한 마지막 Element와 단절 위치 표시 |
| 경로 한 개 | 해당 경로의 Element를 Chain 후보로 자동 선택 |
| 경로 여러 개 | 공통 구간과 분기 구간을 다른 색으로 표시하고 엔지니어가 경로 선택 |
| 과도한 연결 | 탐색 중지 후 Layer 필터와 선택적 Layer 규칙 확인 요청 |
| Input과 Output 동일 | 설정 오류로 처리 |

경로가 한 개여도 결과는 바로 DB의 확정 Chain으로 저장하지 않는다. 시스템이 Element를 후보로 선택한 뒤 엔지니어가 전체 경로를 확인하고 확정한다.

#### 17.8.6 화면 설정 순서

최종 화면의 기본 순서는 다음과 같다.

1. `Input Element 지정`
2. 겹친 후보 중 Input Element 수동 확정
3. `Output Element 지정`
4. 겹친 후보 중 Output Element 수동 확정
5. 왼쪽 Layer 목록에서 탐색에 사용할 Layer 필터링
6. 필요한 경우 `Layer 규칙 적용` 선택과 규칙 확인
7. `연결 경로 찾기`
8. 시스템이 연결된 Element 경로 선택과 강조
9. 단절 또는 분기 검토
10. 엔지니어가 Chain 확정

기존 `영역 시작`과 `영역 끝` 버튼은 최종 설정 흐름에서 제거한다.

#### 17.8.7 기존 구현 변경 대상

| 현재 구현 | 변경 방향 |
| --- | --- |
| Input/Output 클릭 좌표 저장 | 엔지니어가 확정한 Element Reference 저장 |
| 가장 작은 겹침 Element 자동 선택 | 겹친 후보 목록에서 수동 선택 |
| 작업 영역 안의 Element 변환 | 왼쪽에서 체크한 Layer의 전체 BOUNDARY/PATH를 공간 격자에 등록 |
| Bounding Box만으로 연결 판정 | Bounding Box 1차 검색 후 실제 점유 영역 교차 검사 |
| 첫 번째 BFS 도달 경로 반환 | 단일 경로/분기 경로/단절 결과 구분 |
| 영역 시작/끝 Overlay | 제거 |

#### 17.8.8 구현 순서

1. Input과 Output의 수동 Element 지정 기능
2. 작업 영역 없이 왼쪽에서 체크한 Layer의 Element를 공간 격자에 등록하는 기능
3. 같은 Layer Element의 실제 점유 영역 교차 검사
4. 다른 Layer Element의 실제 점유 영역 교차 검사
5. Layer 규칙 선택 적용
6. Input에서 Output까지의 단일 경로 탐색
7. 분기와 복수 경로 표시
8. 엔지니어 확인과 Chain 확정

두 번째 작업의 시작점은 기존 `TraceChainCandidate`가 좌표와 작업 영역을 받는 구조를, 확정된 Input/Output Element Key, 왼쪽에서 체크한 Layer 목록, Layer 규칙 적용 여부를 받는 구조로 변경하는 것이다.

### 17.9 2026-09-23 / Layer 필터와 선택적 Layer 규칙 설계

#### 17.9.1 Layer 필터의 역할

왼쪽 Layer 목록의 체크 상태를 엔지니어가 Chain 탐색에 포함할 Layer를 지정하는 기준으로 사용한다.

```text
왼쪽에서 Layer 체크
  -> 체크된 Layer의 BOUNDARY/PATH만 공간 격자에 등록
  -> 실제 점유 영역이 교차하는 Element 연결
  -> Input에서 Output까지 탐색
```

체크하지 않은 Layer의 Element는 화면에 데이터가 존재하더라도 Chain 연결 탐색과 결과 선택에서 제외한다.

#### 17.9.2 Layer 규칙의 역할

Layer 규칙 편집 기능은 유지한다. 다만 1차 기본 동작에서는 필수 입력값으로 사용하지 않는다.

| 설정 | 연결 판단 |
| --- | --- |
| Layer 규칙 적용 해제 | 체크된 Layer 안에서 실제 점유 영역이 교차하면 연결 |
| Layer 규칙 적용 선택 | 실제 점유 영역이 교차하고 등록된 Layer 조합이면 연결 |

Layer 규칙은 겹치기만 하면 안 되는 Layer 조합이 실제 샘플에서 확인될 때 사용할 수 있다. Layer 규칙이 실제 영역 교차 조건을 대신할 수는 없다.

#### 17.9.3 화면 구성

| 화면 위치 | 항목 | 처리 |
| --- | --- | --- |
| 왼쪽 | Layer 체크 목록 | Chain 탐색 대상 Layer 지정 |
| 상단 | Layer 규칙 적용 | 선택한 경우에만 규칙 검사 |
| 상단 | Layer 규칙 편집 | Layer 조합 관리 기능 유지 |
| 상단 | 선택 Layer 요약 | `선택 3개 / L1, L2, L5` 형태로 표시 |
| 상태 영역 | 탐색 기준 | 선택 Layer 수, 규칙 적용 여부, 탐색 Element 수 표시 |

#### 17.9.4 실행 전 검사

1. Input Element가 확정되어 있어야 한다.
2. Output Element가 확정되어 있어야 한다.
3. 왼쪽에서 한 개 이상의 Layer가 체크되어 있어야 한다.
4. Input과 Output의 Layer가 모두 체크되어 있어야 한다.
5. Layer 규칙 적용을 선택한 경우 규칙이 한 개 이상 등록되어 있어야 한다.
6. Input과 Output은 서로 다른 Element여야 한다.

#### 17.9.5 탐색 결과와 설정 변경

탐색 결과에는 사용한 Layer ID 목록과 Layer 규칙 적용 여부를 함께 보관한다. 엔지니어가 왼쪽 Layer 체크 상태 또는 Layer 규칙을 변경하면 기존 후보 경로는 더 이상 같은 조건의 결과가 아니므로 화면에서 해제한다.

DB 저장 단계에서는 Chain을 확정할 때 적용한 Layer 필터 목록과 Layer 규칙 버전을 함께 기록하여 동일한 조건으로 재검증할 수 있게 한다.

### 17.10 2026-09-28 / Input과 Output 수동 Element 선택 1차 구현

Input/Output 지정 버튼을 누르고 GDS 위치를 클릭하면 현재 표시 중인 Layer의 BOUNDARY/PATH 후보 목록이 열린다. 목록에는 Layer, DataType, Element 종류, 배치 좌표, Point 수, PATH 폭을 표시한다. 목록 행을 선택하면 해당 Element를 지도에서 미리 강조하고, 엔지니어가 확정 버튼을 눌러야 Input 또는 Output으로 지정된다.

- 후보가 한 개여도 자동 확정하지 않는다.
- Input과 Output에 같은 Element를 지정할 수 없다.
- 취소하면 이전에 확정한 Element를 유지한다.
- 후보 추적은 클릭 좌표를 다시 검색하지 않고 확정한 현재 화면 Element 키를 사용한다.
- 새 GDS를 조회하면 확정한 Element 키를 초기화한다.
- 현재 화면 키 `SCENE:번호`는 세션용으로만 사용하며 DB에 저장하지 않는다.

이번 단계에서는 기존 작업 영역 입력과 필수 Layer 규칙이 후보 추적에 남아 있다. 다음 단계에서 왼쪽 Layer 필터와 실제 도형 영역 교차 기준으로 탐색을 변경한다.

검증 결과는 Debug 빌드 오류 0개, 기존 `ChainCandidateTracerProbe`와 `ChainSetupFormProbe` 통과다. 엔지니어가 실제 화면에서 Input/Output Element 지정 기능을 확인하였다.

### 17.11 2026-09-28 / 작업 영역 제거와 Layer 필터 기반 후보 추적

Input/Output 지정 확인 후 두 번째 구현 단계로 작업 영역 입력을 제거하였다.

- `영역 시작`과 `영역 끝` 버튼을 화면에서 제거했다.
- `후보 경로 찾기`를 누른 순간 왼쪽에서 체크된 Layer ID 목록을 복사하여 탐색에 사용한다.
- Input 또는 Output의 Layer가 체크되지 않았으면 실행 전에 안내한다.
- `Layer 규칙 적용`은 기본 해제 상태로 두고 편집 기능은 유지한다.
- 적용을 켠 경우에만 등록된 Layer 조합을 추가로 검사한다.
- Bounding Box는 공간 격자의 주변 도형 검색에만 사용한다.
- BOUNDARY의 다각형과 PATH의 선분/폭으로 실제 점유 영역의 교차를 검사한다. 도형 경계가 닿는 경우도 연결 후보에 포함한다.
- Layer 체크 상태 또는 규칙을 바꾸면 이전 후보 강조를 해제하고 Input/Output 선택은 유지한다.
- 넓은 도형이 과도한 격자 Cell을 생성하지 않도록 별도 목록에 보관한다.

아직 이 결과는 전기적 연결 확정 정보가 아니다. 여러 분기가 있으면 현재 BFS가 처음 도달한 경로 하나를 후보로 보여 주므로 엔지니어 확인이 필요하다. 상단 접속 형상 인식이나 사용자 가이드 선을 이용한 경로 우선순위도 아직 적용하지 않았다.

검증: Debug 빌드 오류 0개, 기존 `ChainCandidateTracerProbe` 통과, 새 `ChainLayerFilteredProbe.ps1` 통과. 새 Probe는 Layer 제외/포함, 선택적 Layer 규칙, 실제 Polygon 겹침, PATH 폭, 큰 도형 검색을 검사한다. 기존 `ChainSetupFormProbe.exe`는 삭제된 `영역 시작` 버튼을 필수로 찾는 이전 화면 검사라 현재 설계의 통과 기준에서 제외한다.

엔지니어 화면 확인 순서:

1. 새 Debug 실행 파일에서 샘플 GDS를 조회한다.
2. Input과 Output Element를 각각 확정한다.
3. 왼쪽에서 Input/Output Layer를 포함해 탐색할 Layer를 체크한다.
4. `Layer 규칙 적용`을 해제한 채 `후보 경로 찾기`를 누른다.
5. `영역 시작/끝` 입력 없이 경로 탐색이 실행되는지 확인한다.
6. 중간 연결 Layer를 하나 해제하고 다시 실행하여 후보 결과가 달라지는지 확인한다.
7. Layer 규칙을 등록한 후 적용 체크박스를 켜고 다시 실행하여 허용 조합만 통과하는지 확인한다.

실제 샘플의 연결 경로가 나오는지 여부와 후보의 전기적 타당성은 엔지니어 화면 확인 결과를 기다린다.

### 17.12 2026-09-28 / 복수 단자 Element, 겹침 전체 표시, 수동 경로 편집 설계

#### 17.12.1 캡처에서 확인한 차이와 설계 범위

제공된 비교 캡처의 위쪽은 실제 Chain으로 표시된 분홍색 배선/단자이고, 아래쪽은 현재 기능이 찾은 노란색 후보 결과다. 두 화면의 색상과 도형 포함 범위가 다르다. 캡처만으로는 각 도형의 GDS Layer, DataType, Element 식별자 또는 실제 전기적 연결을 확정할 수 없다.

현재 코드는 Input/Output에 각각 Element Key 한 개를 보관하고, BFS가 Output에 처음 도달하면 부모 연결을 역추적한 경로 한 개만 표시한다. 그래서 기본 경로와 겹치는 다른 Layer의 Ball/Terminal Element 또는 병렬 도형이 누락될 수 있다.

이번 설계의 목표는 다음 세 가지다.

1. 엔지니어가 Input과 Output에 각각 여러 Element를 직접 지정한다.
2. 기본 경로와 직접 겹치는 선택 Layer의 모든 Element를 함께 보여 준다.
3. 엔지니어가 지도에서 경로 구성 Element를 직접 추가하거나 제외한다.

#### 17.12.2 Input/Output을 복수 Element 묶음으로 지정

Input과 Output을 각각 `ElementKey` 한 개가 아니라 `ElementKey 집합`으로 관리한다.

| 항목 | 설계 |
| --- | --- |
| Input | 엔지니어가 확정한 Element 1개 이상 |
| Output | 엔지니어가 확정한 Element 1개 이상 |
| 선택 대상 | 왼쪽에서 체크한 Layer의 BOUNDARY/PATH만 |
| 후보 화면 | 클릭 위치에 겹친 Element를 Layer/DataType/종류/좌표와 함께 표시 |
| 선택 방식 | 후보 목록 다중 체크, 지도에서 추가 선택/선택 해제 |
| 확정 | 후보가 여러 개여도 엔지니어의 확정 동작 필요 |

한 번의 클릭으로 겹친 후보를 모두 체크하는 편의 버튼은 제공하되 자동 확정하지 않는다. 큰 배경 도형처럼 클릭 영역을 넓게 덮는 후보도 목록에 표시하고, 엔지니어가 포함 여부를 판단한다. 선택한 후보마다 지도에서 미리보기를 제공한다.

Input과 Output에 같은 배치 Element를 중복 지정하면 오류로 표시한다. Input 또는 Output 묶음 안에 서로 떨어진 Element가 있으면 확정 전 경고한다. 선택한 Layer가 나중에 해제되면 해당 단자 Element가 필터 밖에 있음을 표시하고 경로 찾기를 중지한다.

#### 17.12.3 기본 경로와 겹침 Element의 구분

화면과 내부 모델에서 다음 항목을 구분한다.

| 구분 | 의미 | 초기 포함 방법 |
| --- | --- | --- |
| 기본 경로 | Input 묶음에서 Output 묶음까지 연결된 Element 순서 | 연결 탐색 |
| 겹침 Element | 기본 경로 또는 Input/Output 묶음의 실제 점유 영역과 직접 겹치는 선택 Layer Element | 경로 계산 후 전체 검색 |
| 분기 후보 | 기본 경로에서 갈라지지만 Output 도달 경로에 포함되지 않은 Element | 별도 경고 표시 |
| 수동 추가 | 엔지니어가 지도에서 경로에 포함시킨 Element | 사용자 편집 |
| 수동 제외 | 자동 결과에서 엔지니어가 빼기로 한 Element | 사용자 편집 |

`겹치는 Element 전부`의 기본 범위는 기본 경로와 Input/Output 묶음의 어느 Element에든 **직접** 겹치는 선택 Layer의 Element 전부다. 겹침 Element에서 다시 다음 Element로 무제한 확장하면 인접 Chain 전체가 선택될 수 있으므로, 자동 확장은 한 단계에서 멈춘다. 이 범위 밖의 실제 Chain Element는 엔지니어가 직접 추가한다.

기본 경로 Element와 겹침 Element가 같은 배치 Element이면 한 번만 보관한다. 기본 경로의 순서는 유지하되 겹침 Element는 연결된 기본 경로 Element의 위치에 함께 표시한다. 경로와 무관한 방문 Element를 결과에 포함하지 않는다.

#### 17.12.4 복수 Input/Output 경로 찾기

1. Input 집합의 모든 Element를 탐색 시작점으로 넣는다.
2. 왼쪽에서 체크한 Layer의 BOUNDARY/PATH만 공간 격자에 등록한다.
3. 실제 도형 접촉을 검사하며 연결된 Element를 탐색한다.
4. Output 집합의 어느 Element에 도달하면 Input부터 Output까지의 기본 경로 후보를 만든다.
5. 기본 경로와 Input/Output 집합에 직접 겹치는 Element를 빠짐없이 조회한다.
6. 분기 후보를 별도 표시한다.
7. 엔지니어의 수동 추가/제외를 반영하고 연결 상태를 다시 검사한다.

첫 번째 도달 경로가 실제 Chain이라는 보장은 없다. 따라서 경로가 여러 개이거나 분기가 있으면 대체 경로를 검토할 수 있어야 한다. 별도의 방향 가이드 선은 이번 요구사항의 필수 입력으로 추가하지 않는다. 추후 실제 샘플에서 분기 선택을 단순화할 필요가 확인되면 선택 기능으로 검토한다.

#### 17.12.5 지도에서 수동 추가와 제외

화면에 `추가` / `제외` / `조회` 모드를 명확히 나누고 현재 모드를 표시한다.

| 동작 | 처리 |
| --- | --- |
| 추가 모드에서 클릭 | 클릭 위치에 겹친 선택 Layer Element 목록을 열고 포함할 항목을 지정 |
| 제외 모드에서 클릭 | 현재 Chain 후보에 들어 있는 겹친 Element 중 제외할 항목을 지정 |
| 여러 항목 편집 | Ctrl 선택 또는 작은 영역 선택 후 한 번에 추가/제외 |
| 겹친 항목이 여러 개 | Layer/종류/좌표를 보여 주고 한 항목 또는 여러 항목 선택 |
| 선택 Layer 밖의 항목 | 추가/제외할 수 없으며 Layer 필터를 변경하도록 안내 |

`제외`는 GDS 원본 도형을 삭제하는 동작이 아니라 현재 Chain 설정에서만 제거하는 동작이다. Input/Output 역할의 Element는 일반 제외 모드에서 제거하지 않고 단자 편집 화면에서 수정한다. 잘못된 클릭을 되돌릴 수 있도록 실행 취소/다시 실행을 제공한다.

수동으로 제외한 Element는 후보를 다시 계산해도 자동으로 되살리지 않는다. 명시적으로 제외를 취소하거나 Chain 설정을 초기화할 때만 다시 포함한다. 수동 추가한 Element도 재탐색 후 유지하고, 현재 Layer 필터 밖으로 벗어나면 확인 필요 상태로 표시한다.

#### 17.12.6 수동 편집 후 연결 검사

| 검사 | 처리 |
| --- | --- |
| 중간 연결 Element 제외 | Input/Output 연결을 다시 검사하고 단절되면 경고 |
| 연결되지 않은 Element 추가 | 소속은 후보로 보이되 연결이 없는 상태로 표시 |
| 중복 포함 | 같은 배치 Element는 한 번만 보관 |
| 다른 Chain에도 포함 | 자동 차단하지 않고 충돌 가능성을 표시하여 엔지니어 검토 |
| 여러 경로 또는 분기 | 후보를 구분하여 표시하고 경로 확정 전 검토 |
| Input/Output 집합 변경 | 기존 경로를 재계산하고 수동 편집 항목의 유효성을 재검사 |

도형이 화면에서 겹친다는 사실만으로 전기적 연결이 확정되지는 않는다. 수동으로 추가한 Element가 기존 경로와 실제로 닿지 않는 경우에는 연결 근거 또는 수동 연결 지정을 요구한다. 단절과 미확인 분기가 남은 상태는 `검증 필요`로 보관하고 운영 Correlation용 `확정` 상태로 전환하지 않는다.

#### 17.12.7 화면 표시와 AOI 분석 대상

| 표시 대상 | 제안 표시 |
| --- | --- |
| 확정된 Input/Output 묶음 | 역할 아이콘과 Element 외곽선 |
| 기본 경로 | 진한 Chain 색상 |
| 자동 겹침 Element | 같은 계열의 반투명 색상 |
| 분기 후보 | 별도 경고 색상 |
| 수동 추가 | 수동 표시가 있는 Chain 색상 |
| 수동 제외 | 얇은 취소 표시, 최종 Chain에서는 비포함 |

목록에는 `기본 경로 / 겹침 / 수동 추가 / 수동 제외 / 분기 후보`의 개수와 Layer별 개수를 표시한다. 지도에서 한 Element를 선택하면 목록의 해당 행으로 이동하고, 목록 행을 선택하면 지도에서 해당 도형만 강조한다.

AOI Defect 겹침 분석에는 엔지니어가 최종 확정한 Chain Element만 사용한다. 단순 방문 Element, 미확정 분기 후보, 수동 제외 Element는 분석 대상에서 뺀다.

#### 17.12.8 저장 모델과 재조회

DB 저장 시 Input/Output을 단일 Part ID가 아닌 각각의 Element 참조 집합으로 연결한다. Chain 구성 Element에는 `기본 경로 / 자동 겹침 / 수동 추가`의 포함 근거와 `수동 제외` 이력을 남긴다. 연결 순서와 겹침 관계도 별도로 저장하여 AOI Defect 위치에서 관련 Chain 구간을 찾을 수 있게 한다.

현재 `SCENE:번호`는 실행 세션용이므로 저장하지 않는다. GDS Revision, Structure 배치 경로, Layer/DataType, 배치 후 도형 좌표를 이용한 영구 Element 참조가 준비되어야 저장 후 재조회할 수 있다. Layer 필터, 선택적 Layer 규칙, 엔지니어 확인자, Chain 버전도 함께 기록한다.

#### 17.12.9 권장 구현 순서와 확인 기준

1. Input/Output을 `ElementKey 집합`으로 변경하고 후보 창의 다중 선택을 구현한다.
2. 복수 시작/종료 Element로 기본 경로를 찾는다.
3. 기본 경로와 직접 겹치는 선택 Layer Element를 전부 표시한다.
4. 기본 경로/겹침/분기를 구분하여 지도와 목록에 표시한다.
5. 지도에서 추가/제외와 실행 취소/다시 실행을 구현한다.
6. 수동 편집 후 연결 재검사와 검증 필요 상태를 구현한다.
7. 영구 Element 참조와 DB 저장/재조회를 연결한다.

대표 Chain 한 개에 대해 실제 Chain 화면과 나란히 비교한다. Input/Output의 여러 Element가 모두 선택되는지, 경로에 직접 겹친 Ball/Terminal이 누락되지 않는지, 옆 Chain Element가 자동 포함되지 않는지, 수동 추가/제외 뒤 결과가 유지되는지를 엔지니어가 확인한다.

### 17.13 2026-09-28 / Input/Output 지도 마우스 지정 설계

17.12.2절의 `후보 목록 다중 체크`는 이번 절의 지도 직접 선택 방식으로 대체한다. 속성 그리드는 선택 결과를 읽는 보조 화면으로만 사용할 수 있으며, Input/Output 지정에 필요하지 않다. Input/Output 역할은 원 안의 선 모양으로 자동 판별하지 않고 엔지니어가 선택한다.

#### 17.13.1 화면 동작

1. 엔지니어가 왼쪽에서 사용할 Layer를 체크하고 `Input 지정`을 누른다. 화면에 `Input 선택 중`과 현재 선택 개수를 표시한다.
2. 지도에서 Input에 속할 Element를 마우스로 클릭한다. 클릭할 때마다 해당 Element가 Input 임시 선택에 추가되고 지도에서 전용 색상으로 강조된다. 이미 선택한 Element를 다시 클릭하면 임시 선택에서 빠진다.
3. 필요한 Element를 모두 선택한 뒤 `Input 적용`을 누른다. `취소` 또는 Esc는 이번 편집만 버리고 이전 확정 상태로 되돌린다.
4. `Output 지정`도 같은 방식으로 진행한다. 두 역할의 색과 선택 개수를 동시에 표시한다. Input 편집과 Output 편집 모드는 서로 배타적이다.
5. `후보 경로 찾기`는 Input과 Output에 각각 한 개 이상의 확정 Element가 있을 때만 실행한다.

Input/Output 모드는 클릭 한 번으로 종료하지 않는다. 여러 Element를 연속 지정할 수 있도록 `적용` 또는 `취소`할 때까지 유지한다. 확대/축소는 기존 조작을 유지하고, 마우스를 누른 뒤 일정 거리 이상 움직인 동작은 화면 이동으로 처리하여 Element 선택으로 오인하지 않는다. 첫 단계는 단일 클릭의 반복으로 구현하고, 영역 드래그 선택은 실제 사용 시 필요성이 확인되면 추가한다.

#### 17.13.2 한 지점에 여러 Element가 겹칠 때

클릭 위치에 선택 Layer의 Element가 하나면 즉시 선택을 전환한다. 둘 이상이면 마우스 위치 옆에 작은 `겹친 Element` 메뉴를 띄운다. 메뉴에는 Layer/DataType, BOUNDARY 또는 PATH, 현재 Input/Output 포함 여부를 표시한다. 항목에 마우스를 올리면 지도에서 그 Element 하나만 외곽선으로 미리 보여 주고, 항목을 클릭하면 선택을 전환한다. 여러 항목이 필요하면 메뉴를 유지해 연속 클릭할 수 있다. `이 위치의 후보 모두 추가`는 별도 명령으로 제공하되, 큰 배경 도형까지 포함되는지 미리 보여 준 뒤 엔지니어가 실행한다.

후보 순서는 작은 도형과 클릭점에 가까운 도형을 먼저 보여 주는 편의 기능일 뿐이다. 겹친 도형 중 어느 것이 Input/Output인지는 시스템이 자동 확정하지 않는다. Input과 Output에 같은 배치 Element를 지정하려 하면 두 번째 지정을 막고 기존 역할을 안내한다.

#### 17.13.3 선택 판정과 Layer 조건

클릭 후보는 왼쪽에서 체크한 Layer의 BOUNDARY/PATH로 제한한다. 클릭점에서 실제 다각형 내부 또는 PATH 폭이 차지하는 영역을 판정하고, 화면상 몇 픽셀의 클릭 오차만 허용한다. 현재의 경계 상자 겹침 검색만 사용하면 빈 공간을 클릭해도 큰 Element가 후보가 될 수 있으므로 실제 형상 판정을 추가해야 한다. 같은 Element가 클릭 오차 범위에서 여러 번 검색되어도 메뉴에는 한 번만 표시한다. 많은 Element가 있는 GDS에서는 공간 인덱스로 주변 후보를 좁힌 뒤 형상 판정을 수행한다.

선택된 Element의 Layer를 왼쪽에서 해제하면 지도에 `필터 밖` 상태를 표시하고 경로 찾기를 중지한다. 다시 체크하거나 해당 Element를 지정에서 제거해야 한다. 현재 화면에서만 유효한 `SCENE:번호`는 DB 저장 식별자로 사용하지 않는다.

#### 17.13.4 기존 화면과 코드의 변경 범위

| 대상 | 현재 상태 | 변경 방향 |
| --- | --- | --- |
| `GdsMapTestForm_Chain.cs` | 지도 클릭 한 번 후 선택 모드 종료, 역할별 Element Key 하나 보관 | 역할별 임시/확정 Element 집합, 연속 클릭, 적용/취소 처리 |
| `ChainElementCandidateForm.cs` | 단일 후보를 DataGridView에서 확정 | Input/Output 지정 경로에서 제거하고 지도 옆 겹침 메뉴로 대체 |
| `GdsMapControl_Chain.cs` | 보이는 도형의 경계 상자와 클릭 주변 상자를 비교 | 선택 Layer 필터와 실제 형상 기준의 클릭 판정, 후보 미리보기 제공 |
| Chain 표시 기능 | Input/Output 각각 좌표 한 개와 공통 강조 | 역할별 여러 Element의 외곽선/선택 개수 표시 |
| Chain 추적 입력 | Input/Output Key 각각 한 개 | 17.12절의 복수 시작점/종료점 집합 사용 |

기존 화면 이동 및 일반 Element 조회와 Chain 선택이 충돌하지 않게 Chain 선택 모드에서만 지도 클릭을 가로챈다. 지도 선택, 경로 수동 추가/제외, 일반 조회는 화면에 현재 모드를 명확하게 표시하고 한 번에 한 모드만 작동하게 한다.

#### 17.13.5 첫 구현 단위와 엔지니어 확인 기준

첫 구현 단위는 `Input/Output 지도 연속 클릭 + 겹친 Element 메뉴 + 적용/취소 + 복수 Element 강조`다. 그 다음에 복수 시작점/종료점 경로 탐색을 연결한다. 다음 조건을 실제 GDS 화면에서 확인한다.

1. 그리드를 열지 않고 지도 클릭만으로 Input 2개 이상과 Output 2개 이상을 각각 지정할 수 있다.
2. Ball 위에서 단자와 배선이 겹치면 메뉴에서 각각 미리 보고 필요한 Element만 선택할 수 있다.
3. 이미 선택한 Element를 다시 클릭하면 해당 역할에서 빠지고, Esc를 누르면 이전 확정값이 복원된다.
4. 체크하지 않은 Layer의 Element는 선택되지 않고, 화면 이동이 Element 선택으로 기록되지 않는다.
5. Input/Output에 같은 Element를 넣을 수 없고, 적용 전에는 기존 확정 경로가 변경되지 않는다.

### 17.14 2026-09-28 / 지도 마우스 지정 1차 구현 기록

Input/Output 지정 버튼을 누른 뒤 지도에서 Element를 여러 번 클릭하여 임시 선택에 추가/제외할 수 있게 했다. `선택 적용`은 역할별 복수 Element를 확정하고, `선택 취소` 또는 Esc는 기존 확정값을 복원한다. 겹친 Element는 지도 옆 메뉴에서 Layer/DataType/종류를 보고 마우스로 한 개씩 고르거나 `이 위치의 후보 모두 추가`를 선택한다. 첫 구현에서는 메뉴 항목을 고른 뒤 메뉴가 닫히므로 같은 위치의 다른 Element를 추가하려면 지도를 다시 클릭한다. 선택된 Input/Output의 개수와 역할 Marker를 지도에 표시한다.

클릭 후보는 체크된 Layer의 BOUNDARY/PATH 실제 형상과 화면 클릭 오차로 판정한다. 기존 경계 상자만 겹치는 빈 공간은 후보에서 제외한다. 화면 이동을 위한 드래그는 선택으로 처리하지 않는다. 클릭마다 전체 배치 Element를 훑는 현재 구조는 큰 GDS에서 지연이 생길 수 있으므로, 필요하면 클릭 후보용 공간 인덱스를 추가한다.

이번 단위에서는 후보 경로 찾기가 Input/Output 각각 한 개일 때만 기존 방식으로 동작한다. 어느 한쪽이라도 두 개 이상이면 안내 메시지를 띄우고 계산을 중단한다. 복수 시작점/종료점을 이용한 탐색과 겹침 Element 전체 표시, 수동 경로 추가/제외는 다음 구현 단위다.

화면 확인 순서: GDS 조회 → 왼쪽 Layer 체크 → Input 지정 → 서로 다른 Element 두 개 클릭 → 선택 적용 → Output도 같은 방식으로 두 개 지정 → 지도 Marker와 상태 개수 확인. 겹친 Ball/단자는 메뉴에서 각각 선택되는지, 같은 Element 재클릭으로 해제되는지, Esc 후 이전 확정 상태가 돌아오는지, 마우스 드래그 후 개수가 늘지 않는지도 확인한다.

### 17.15 2026-09-28 / 복수 Input/Output 후보 경로 탐색 구현 기록

확정된 Input과 Output의 모든 Element Key를 경로 탐색 요청으로 전달한다. 탐색기는 모든 Input을 BFS 시작점으로 등록하고 선택 Layer의 실제 형상 접촉 관계를 따라 어느 Output이든 처음 도달하면 후보 경로 한 개를 반환한다. 결과에는 사용된 시작 Input과 도달 Output의 Element Key를 표시한다. 일부 단자 Element가 현재 Layer 필터에 없으면 나머지만으로 계산하지 않고 오류를 반환한다. 기존 Input/Output 각 한 개 호출도 유지한다.

이 단계에서 화면에 표시하는 것은 `가장 먼저 도달한 기본 경로 한 개`다. 복수로 지정한 모든 단자를 하나의 전기 경로에 포함한다고 보장하지 않는다. 기본 경로와 직접 겹치는 다른 Element 전체 표시, 분기 후보 구분, 수동 경로 추가/제외는 다음 단계에서 구현한다.

화면 확인 순서: 서로 다른 Input 두 개 이상과 Output 두 개 이상을 지도에서 지정하고 `후보 경로 찾기`를 누른다. 연결 가능한 조합이 있으면 경로와 사용된 시작/끝 Element Key가 표시되어야 한다. 연결 조합이 없으면 실패 메시지가 나타나야 한다. 지정한 단자의 Layer 체크를 해제하면 일부 단자를 무시한 경로가 나타나서는 안 된다.

### 17.16 2026-09-28 / Layer 규칙과 격자 크기 사용 여부 확인

화면의 `Layer 규칙 적용`은 현재 기본 해제 상태지만, 체크하면 탐색기가 등록된 Layer 조합에 없는 Element 간 연결을 차단한다. `Layer 규칙 편집`에서 만든 규칙은 이 검사에 전달된다. 규칙이 없는 상태에서 적용 체크를 켜면 경로 찾기 전에 안내 메시지로 중단한다. 따라서 해당 기능은 선택적으로 실제 사용되며 이번 점검에서 삭제하지 않는다. 왼쪽 Layer 체크 필터는 규칙 적용 여부와 관계없이 항상 사용한다.

`격자 크기` 값은 공간 인덱스의 Cell 크기로 매번 탐색 요청에 전달된다. 경로의 업무 규칙이 아니라 주변 Element 조회량을 조절하는 성능 설정이지만 현재 코드에서 사용 중이므로 이번 점검에서 삭제하지 않는다. 실제 도형 좌표가 있는 경우 Layer 규칙의 허용 오차가 전기적 간격을 연결해 주지는 않으며 실제 형상이 겹쳐야 한다.

캡처처럼 `Layer 규칙 적용`이 체크되어 있고 `규칙 없음`이면 경로를 찾을 수 없다. 왼쪽 Layer 체크와 형상 겹침만으로 검토하려면 `Layer 규칙 적용`을 해제한다. 이후 실제 현장에서 규칙 편집과 격자 크기 조정의 사용 사례가 없다면 UI 단순화 범위를 별도로 정할 수 있다.

### 17.17 2026-09-28 / 직접 겹침 전체 표시와 분기 후보 1차 구현 기록

기본 경로를 찾은 뒤 그 경로와 확정된 모든 Input/Output Element를 기준점으로 삼아, 왼쪽에서 체크한 Layer의 BOUNDARY/PATH 가운데 실제 형상이 직접 닿는 Element를 모두 모은다. 동일 배치 Element는 한 번만 표시하고, 기준점에서 직접 닿지 않으며 겹친 Element를 통해서만 이어지는 다음 단계 도형은 자동 포함하지 않는다. 직접 겹침을 찾을 때에는 Layer 조합 규칙과 관계없이 선택 Layer의 실제 도형 교차를 보여 준다.

직접 겹친 항목 가운데 기준 경로 밖의 다른 Element로 다시 이어지는 항목은 분기 검토 후보로 표시한다. 이때는 `Layer 규칙 적용` 여부를 연결 조건에 반영한다. 분기는 직접 겹침 목록의 부분집합이며 확정 Chain을 뜻하지 않는다. 지도는 기본 경로를 기존 노란색으로, 직접 겹침을 하늘색 외곽선으로, 분기 검토 후보를 주황색 점선 외곽선으로 표시한다. 상태 영역에 기본 경로/직접 겹침/분기 개수를 함께 표시한다.

화면 확인 순서: 실제 GDS에서 Input/Output과 Layer를 지정한 뒤 `후보 경로 찾기`를 누른다. 노란 기본 경로 주변의 선택 Layer Element가 하늘색 외곽선과 함께 빠짐없이 보이는지, 분기가 주황색 점선으로 구분되는지, 한 단계 더 떨어진 옆 Chain이 자동으로 선택되지 않는지 확인한다. Layer 체크를 바꿔 다시 찾았을 때 해당 Layer의 겹침 결과가 바뀌어야 한다. 실제 도형 겹침과 표시 범위는 엔지니어가 샘플 GDS로 최종 확인한다.

### 17.18 2026-09-28 / 지정 Input/Output의 Chain 후보 누락 수정

이전 결과는 다중 시작점/종료점 중 기본 경로에 실제로 사용된 단자만 경로 목록에 들어갔다. 동시에 직접 겹침 수집에서는 모든 지정 단자를 기준점으로 삼으며 겹침 목록에서 제외했다. 그래서 기본 경로에 쓰이지 않은 지정 Input/Output은 IN/OUT Marker만 보이고 노란 Chain 후보 강조에서는 빠질 수 있었다.

수정 후에는 기본 경로/모든 지정 Input과 Output/직접 겹침 Element를 Element Key 기준으로 중복 제거해 `CandidateElements`에 담는다. 지도는 연결 성공 시 이 후보 집합 전체를 강조하고, 연결 실패 시에도 지정 단자를 방문 결과와 함께 보여 준다. 기본 경로에 사용되지 않은 지정 단자는 `경로 밖 단자` 개수로 따로 알려 전기적 연결이 확정된 것으로 오해하지 않게 한다.

동일 좌표에 다른 원/사각형 Element가 겹쳐도 하나로 합치지 않고 각각 후보로 관리한다. 단, 왼쪽에서 체크한 Layer의 BOUNDARY/PATH여야 하고 실제 형상 교차가 확인되어야 자동 겹침 후보에 들어간다. 제공된 화면 캡처만으로 특정 원/사각형의 Layer, Element Key 또는 실제 교차 여부를 확정할 수 없으므로 개별 제외 원인은 샘플 GDS의 후보 목록과 Layer 체크 상태로 확인한다.

화면 확인 순서: Input을 여러 Element로 지정하고 Output도 지정해 경로를 찾는다. 경로에 사용되지 않은 지정 단자에도 노란 후보 강조와 IN/OUT Marker가 함께 남는지 확인한다. 같은 위치의 서로 다른 Layer 원/사각형이 체크되어 있다면 각각 후보로 표시되는지 확인한다. 상태의 `경로 밖 단자`가 0보다 크면 연결 여부는 엔지니어가 추가 검토한다.

### 17.19 2026-09-28 / 겹친 Layer의 후보 표시 순서 보정

실제 화면에서는 경로 41개, 지정 단자 12개, 직접 겹침 76개가 계산되었지만 일부 원/사각형이 노란색으로 보이지 않았다. 기존 OpenGL 그리기는 Layer 순서대로 진행되어, 먼저 그린 선택 Element를 뒤에 그린 다른 Layer 도형이 덮을 수 있었다. 선택된 Element를 모든 GDS Layer 그리기 이후에 다시 그리도록 변경했다. 후보 계산 결과와 Layer 필터는 변경하지 않았다.

화면 확인 순서: 수정된 실행 파일에서 같은 GDS와 같은 Input/Output, Layer 체크 상태로 `후보 경로 찾기`를 다시 실행한다. 이전에 어둡게 보이던 원/사각형이 노란색 또는 후보 외곽선으로 보이는지 확인한다. 여전히 빠진 객체는 `Input 지정` 상태에서 해당 객체를 클릭해 후보 메뉴의 Layer와 Element Key를 확인한 뒤 `선택 취소`로 복귀한다. 해당 Layer가 체크되어 있어도 빠진다면 기본 경로와 한 단계 직접 겹치는 객체인지 확인해야 한다. 현재 탐색은 그 다음 단계로 이어지는 객체까지 자동 포함하지 않는다.

### 17.20 2026-09-28 / 지도에서 Chain 후보 수동 추가와 제외

`후보 경로 찾기` 뒤 `경로 추가` 또는 `경로 제외`를 눌러 지도 객체를 클릭할 수 있게 했다. 클릭 후보는 왼쪽에서 체크한 Layer의 BOUNDARY/PATH만 사용한다. 한 위치에 여러 객체가 있으면 Layer/DataType/종류/SCENE 키가 표시된 메뉴에서 하나 또는 모두를 선택한다. 추가/제외는 GDS 원본이나 자동 추적 결과를 수정하지 않고 화면의 수동 보정 집합에 저장한다.

Input/Output 지정 객체는 일반 `경로 제외`로 제거할 수 없고 역할 지정에서 수정해야 한다. 수동 보정은 같은 도면에서 후보를 다시 찾아도 적용되고, 새 GDS를 열 때 초기화된다. Layer 체크가 바뀌어 현재 필터에서 빠진 수동 추가 객체는 화면에 표시하지 않으며 그 Layer를 다시 체크하고 탐색하면 다시 적용된다. 수동으로 연결되지 않은 객체를 추가해도 전기적 연결은 자동 확정되지 않으므로 상태에 `연결 확인 필요`를 표시한다. 이번 단계에는 DB 저장과 실행 취소/다시 실행을 포함하지 않는다. 반대 편집 모드에서 같은 객체를 클릭하면 해당 보정을 되돌릴 수 있다.

화면 확인 순서: 후보 경로를 찾고 `경로 추가`를 눌러 빠진 원/사각형을 클릭한다. 노란 강조와 `수동 추가` 개수가 늘어나는지 확인한다. `경로 제외`로 자동 후보 하나를 클릭하면 강조가 사라지고 `수동 제외`가 늘어나는지 확인한다. 겹친 위치의 메뉴에서 특정 객체만 선택했을 때 다른 객체는 그대로인지, Input/Output은 제외되지 않는지, 재탐색 후 보정이 남는지 확인한다. 같은 버튼 또는 Esc로 편집 모드를 종료한다.

### 17.21 2026-09-28 / 수동 편집 후 후보 연결 검사

최종 후보에 포함된 Element만 대상으로 첫 Input에서 도형 겹침 관계를 끝까지 탐색한다. 이전 후보 경로 찾기는 첫 Output에서 멈추지만, 이 검사는 지정된 모든 Input/Output과 수동 추가 객체까지 이어지는지 확인한다. 왼쪽 Layer 필터와 사용자가 켠 Layer 규칙을 적용하고, 수동 제외 객체는 탐색에서 뺀다.

상태 문구 앞부분에 `도형 연결 유지(전기적 확인 필요)` 또는 `연결 검증 필요: Output 도달 수/전체 수, 단절 객체 수`를 표시한다. 다른 Layer 조건에서 남은 수동 후보가 현재 조회에 없으면 성공으로 표시하지 않고 `연결 검사 불가`를 표시한다. 도형 접촉은 전기적 통전의 증거가 아니므로 이 결과만으로 Chain을 확정하거나 DB에 저장하지 않는다.

화면 확인 순서: 연결된 후보 중 중간 Element를 `경로 제외`하고 Output 도달 수 또는 단절 객체 수가 바뀌는지 확인한다. `경로 추가`로 해당 Element를 복원했을 때 연결 유지로 돌아오는지 확인한다. 떨어진 객체를 추가하면 Output에 도달해도 단절 객체가 표시되어야 한다. Layer 규칙을 켜거나 해제한 뒤 재탐색했을 때 연결 결과가 달라지는지도 확인한다.

### 17.22 2026-09-28 / 여러 Chain 정의와 관리 설계

#### 설계 목표와 현재 구조

하나의 Device/GDS Revision에는 Chain을 여러 개 만들 수 있다. 기존 4절과 6.2절의 계층 구조를 실제 화면과 저장 구조에 적용한다. 하나의 Chain을 설정하는 현재 기능은 유지하되, Input/Output, Layer 필터, 후보 결과, 수동 추가/제외가 모두 **선택 중인 Chain의 데이터**가 되어야 한다.

현재 `GdsMapTestForm_Chain.cs`는 위 값을 Form 필드 한 벌로 보관하고, 새 GDS 조회 때 전부 초기화한다. 따라서 Chain A를 설정한 뒤 Chain B로 전환하고 A를 복원할 수 없다. 여러 Chain의 첫 구현은 이 상태를 Chain별 객체로 나누는 작업이다. 화면 색상이나 복사 같은 편의 기능은 그 이후에 다룬다.

#### Chain의 범위와 식별

| 항목 | 설계 기준 |
| --- | --- |
| 소속 | 하나의 Chain은 하나의 Device와 하나의 GDS Revision에 속함 |
| Chain ID | 이름 변경과 무관한 내부 고유 ID. 신규 작성 시 부여하고 DB 저장 후에도 유지 |
| Chain Code | 동일 GDS Revision 안에서 엔지니어가 식별하는 코드. 중복 불가 |
| Chain Name | 화면 표시용 이름. 변경 가능하며 식별 키로 사용하지 않음 |
| 상태 | 작성 중 / 검증 필요 / 확정 / 사용 중지 |
| 버전 | 수정과 재조회 충돌을 판단할 Chain 버전. 확정 이력은 보존 |
| 여러 Chain | 같은 GDS Revision 아래 Chain A, B, C를 독립 생성/선택/저장 |

GDS Revision이 바뀌면 기존 Chain의 Element 참조를 자동으로 새 Revision에 붙이지 않는다. 새 Revision에서 재검증하거나 별도 이관 절차를 거친다. 같은 배치 Element를 여러 Chain이 공유할 수 있으므로 이를 DB에서 무조건 차단하지 않는다. 대신 두 Chain이 같은 Element를 포함하면 충돌 또는 공용 단자 여부를 엔지니어가 확인할 수 있게 표시한다. 향후 Defect가 공유 Element에 닿으면 관련 Chain을 모두 반환해야 한다.

#### Chain별로 보관할 설정

| 데이터 | Chain별 보관 내용 |
| --- | --- |
| Input / Output | 각각 1개 이상의 배치 Element 참조 집합과 화면 Marker 위치 |
| Layer 필터 | 후보 탐색에 사용한 체크 Layer ID 목록. Chain 전환 시 복원 |
| Layer 규칙 | 적용 여부와 당시 규칙의 복사본 또는 규칙 버전 |
| 자동 후보 | 기본 경로, 직접 겹침, 분기와 계산에 사용한 조건. 재계산 가능한 결과 |
| 수동 보정 | 추가한 Element와 제외한 Element를 별도로 보관 |
| 최종 구성 | 자동 후보에 수동 보정을 적용하고 엔지니어가 확인한 Element 집합 |
| 검증 상태 | Input/Output 연결, 단절 객체, 공유 Element 등 확인 결과 |
| 변경 정보 | 작성자, 수정자, 작성/수정 시각, 버전 |

`자동 후보`와 `최종 구성`은 구분한다. 재탐색이 자동 후보를 바꾸더라도 수동 제외를 묵시적으로 되돌리거나 이미 확정한 Chain을 자동 덮어쓰지 않는다. 전기적 연결은 도형 접촉만으로 확정하지 않으며, 엔지니어의 확인 동작이 있어야 `확정` 상태가 된다.

#### 화면 동작

1. 현재 GDS Revision을 연 뒤 왼쪽 `Chain 목록`에서 `새 Chain`을 누른다. Chain Code와 Name을 입력하고 `작성 중` 상태의 빈 Chain을 만든다.
2. 목록에서 Chain 하나를 선택하면 그 Chain이 활성화된다. 화면은 해당 Chain의 Layer 체크, Input/Output, 후보, 수동 보정과 검증 상태를 복원한다.
3. 활성 Chain에서만 `후보 경로 찾기`, `경로 추가`, `경로 제외`가 설정값을 변경한다. 다른 Chain의 데이터는 바뀌지 않는다.
4. Chain을 바꾸는 동안 아직 DB에 저장하지 않은 변경도 현재 작업 세션의 Chain별 초안에 유지한다. 목록에는 변경 여부를 표시한다.
5. 지도는 우선 활성 Chain만 강조한다. 다른 Chain의 동시 표시와 색상 비교는 후속 편의 기능으로 둔다.
6. `저장`은 활성 Chain의 초안을 DB에 기록한다. `확정`은 별도 검증과 엔지니어 확인 후 상태를 바꾼다. 확정 Chain을 수정할 때에는 버전과 이력을 남기고 재검증한다.

Chain 목록의 최소 표시값은 Code / Name / 상태 / 최종 Element 수 / 변경 여부다. GDS Revision을 바꿀 때에는 해당 Revision의 Chain 목록을 다시 조회한다. 다른 Revision의 Chain을 현재 GDS에 섞어 표시하지 않는다.

#### 영구 Element 참조와 DB 저장 경계

현재 `SCENE:번호`는 화면에 펼쳐진 순서로 정해져 조회 세션 밖에서 안정적이지 않다. Chain ID와 별도로 각 배치 Element를 재조회할 수 있는 `PlacedElementRef`가 필요하다. 참조는 GDS Revision, 원본 Structure/Element 위치, SREF/AREF 배치 경로와 배열 인덱스, 변환 정보를 구분해야 한다. 같은 Layer와 같은 좌표에 놓인 별도 Element도 서로 다른 참조여야 한다. 좌표/도형 해시는 재조회 검증에는 쓸 수 있지만 단독 식별 키로 삼지 않는다.

기존 Oracle 코드의 `GDS_ELEMENT` 저장에는 `GDS_SEQ`, `STRUCTURE_NAME`, `SEQ`가 있지만 현재 조회 쿼리는 `SEQ`를 읽지 않는다. 저장 코드에도 고정 `GDS_SEQ`와 `STRUCTURE_NAME` 값이 있다. 따라서 이 컬럼이 실제 배치 Element를 유일하게 식별하는지, SREF/AREF 배치가 DB에 어떻게 표현되는지 먼저 확인해야 한다. 이 확인 전에는 `GDS_ELEMENT.SEQ`를 영구 참조로 확정하지 않는다.

DB 논리 구조는 `GDS Revision 1:N Chain`, `Chain 1:N 구성 Element`, `Chain 1:N Input/Output 역할 Element`, `Chain 1:N Layer 필터/규칙 기록`이다. 한 배치 Element는 여러 Chain에 속할 수 있으므로 `Chain:N ↔ N:Element` 관계를 허용한다. 수동 제외는 최종 구성에서 빼되 보정 이력에는 남긴다. Chain 저장은 본체, 역할, 구성, 필터, 보정 이력을 한 트랜잭션으로 처리한다. 실제 테이블명, PK/FK와 기존 GDS 테이블 연결은 DB 구조 확인 후 확정한다.

#### 작은 단위 구현 순서와 완료 기준

1. `ChainDefinition`과 GDS Revision별 Chain 목록 모델을 만들고 현재 Form 필드를 활성 Chain의 상태로 옮긴다. 이 단계는 메모리에서만 동작한다.
2. `새 Chain` / `Chain 목록 선택`을 연결한다. A와 B에서 서로 다른 Input/Output, Layer 필터, 수동 보정을 설정한 뒤 여러 번 전환해도 각각 그대로 복원되는지 확인한다.
3. 현재 화면의 `SCENE:번호`와 영구 배치 Element 참조의 대응 방법을 검증한다. GDS 파일 조회와 DB 조회가 동일 Element를 가리키는지 대표 원/사각형과 SREF/AREF 반복 배치에서 확인한다.
4. 기존 DB 스키마를 확인하고 Chain별 저장/재조회 모델을 확정한다. 대표 Chain 두 개를 같은 GDS Revision에 저장한 뒤 프로그램을 다시 열어 각각의 최종 구성과 역할이 복원되는지 검증한다.
5. 같은 Element를 두 Chain이 공유하는 사례와 서로 다른 GDS Revision의 Chain 분리, 저장 버전 충돌을 검사한다.

1차 구현 완료 기준은 **Chain A를 편집해도 Chain B가 변하지 않고, A/B를 전환할 때 각각의 설정이 복원되는 것**이다. DB 저장은 그 다음 단위이며, AOI Defect 연관 분석은 영구 참조와 DB 재조회가 검증된 후 시작한다.

### 17.23 2026-09-28 / Chain별 수정/삭제, 표시 체크와 Element 소속 역조회 보완 설계

이 절은 17.22절의 `활성 Chain만 표시하고 다중 표시를 후속 편의 기능으로 둔다`는 제안을 대체한다. Chain별 Visible 체크와 여러 Chain 동시 표시는 기본 관리 기능으로 포함한다.

#### Chain A~Z의 독립 관리

같은 GDS Revision에 Chain A부터 Z까지 여러 개를 생성할 수 있으며, 각 Chain은 고유 `Chain ID`로 수정/삭제/버전 관리를 한다. 이름이나 목록 순서는 대상 식별에 사용하지 않는다. 선택한 Chain A를 편집하거나 삭제해도 Chain B~Z의 Input/Output, Layer 필터, 구성 Element와 표시 설정은 변경되지 않아야 한다.

| 명령 | 대상과 처리 |
| --- | --- |
| 생성 | 현재 GDS Revision에 새 Chain ID와 Chain Code를 부여하고 빈 초안을 추가 |
| 수정 | 활성 Chain ID의 이름, Input/Output, Layer 필터와 구성 Element만 변경 |
| 저장 | 활성 Chain의 변경을 해당 Chain ID/버전에만 반영 |
| 삭제 | 지정한 Chain ID 하나만 목록에서 제거. 다른 Chain의 구성과 공용 Element는 유지 |

저장하지 않은 새 초안은 현재 작업 세션에서 제거할 수 있다. DB에 저장된 Chain의 `삭제`는 물리적인 Element 삭제나 다른 Chain에 대한 연쇄 삭제가 아니다. 삭제 상태, 처리자, 시각, 사유와 유효 종료 시점을 남기는 **논리 삭제**를 기본으로 한다. 삭제된 Chain은 일반 목록과 신규 AOI 연관 분석에서 제외하지만, 과거 분석이 참조한 당시 Chain 버전과 소속 이력은 조회 가능해야 한다. 기존 `사용 중지`는 목록에 남겨 두고 분석 대상에서 빼는 운영 상태, `삭제`는 기본 목록에서도 숨기는 정리 상태로 구분한다. 복구 여부와 권한은 DB 구현 시 결정한다.

#### Layer 표시와 별개인 Chain Visible 체크

왼쪽 Chain 목록의 각 행에 `Visible` 체크박스를 둔다. 행 선택은 **편집할 활성 Chain 지정**, 체크는 **지도에 표시할 Chain 지정**으로 분리한다. 여러 행을 체크하면 해당 Chain을 함께 볼 수 있고, 한 행만 체크하면 그 Chain만 볼 수 있다. 모두 해제하면 Chain 강조만 사라지고 GDS 도형과 저장된 Chain 정보는 그대로 남는다. 활성 Chain이 체크 해제 상태라면 편집 데이터는 유지하되 지도에는 보이지 않음을 안내한다.

| 설정 | 영향 범위 |
| --- | --- |
| GDS Layer Visible | 지도에 그리는 원본 GDS Layer. 여러 Chain이 함께 보는 공통 화면 설정 |
| Chain별 Layer 필터 | 해당 Chain의 후보 경로 계산에 사용할 Layer 목록. Chain 데이터로 저장 |
| Chain Visible | 해당 Chain의 지도 강조/Marker 표시 여부. 후보 계산과 DB 소속에는 영향 없음 |
| 활성 Chain | 버튼이 수정할 Chain ID. Visible 체크와 독립 |

17.22절의 `Chain 전환 시 Layer 체크 복원`은 활성 Chain의 **탐색 필터 편집값을 불러오는 것**으로 해석한다. 다른 Chain의 저장 필터를 바꾸지 않는다. 지도에서 숨긴 GDS Layer에 속한 Chain Element는 그 Chain이 Visible이어도 보이지 않을 수 있으므로 목록에 `일부 Layer 숨김`을 표시한다. 동시에 보이는 Chain은 각기 구별되는 색과 목록 범례를 사용한다. 같은 Element가 여러 Visible Chain에 속하면 하나의 색으로 소속을 덮어쓰지 않고, 선택 시 소속 Chain 목록을 보여 준다.

Chain Visible은 분석 기준정보의 유효성이나 `확정` 상태가 아니다. 1차 구현은 현재 사용자 작업 세션의 표시 상태로 보관한다. 재접속 시 표시 상태를 복원할 필요가 확인되면 사용자/화면별 표시 선호 정보를 별도 저장하고, `CHAIN_MASTER`의 업무 상태 컬럼으로 대체하지 않는다.

#### DB에서 Element → Chain 역조회

같은 GDS 배치 Element가 여러 Chain에 포함될 수 있다. 따라서 GDS Element를 Chain마다 복사하지 않고, 영구 `PlacedElementRef`와 Chain 사이에 소속 연결 정보를 둔다. 16절의 `CHAIN_PART_ELEMENT`는 부위와 Element의 관계를 설명한 초기 모델이다. 실제 1차 저장에서는 **Chain-Element 소속 연결**을 조회의 기준으로 삼고, Ball Site/Terminal 등의 Part 묶음이 필요해질 때 소속 연결에 Part ID를 붙인다. 같은 소속을 별도 테이블 두 곳에 중복 저장하여 불일치시키지 않는다.

| 논리 데이터 | 핵심 키/내용 | 조회 방향 |
| --- | --- | --- |
| GDS_PLACED_ELEMENT | GDS Revision + 안정적인 배치 Element ID/도형 정보 | GDS에서 Element 찾기 |
| CHAIN_MASTER | Chain ID + GDS Revision + Code/Name/상태/버전 | Revision에서 Chain A~Z 찾기 |
| CHAIN_ELEMENT_MEMBER | Chain ID + Chain 버전 + 배치 Element ID, Input/Output/일반 역할, 포함 근거, 선택적 Part ID | Chain → Element / Element → Chain |
| CHAIN_ELEMENT_EDIT_HISTORY | Chain ID + 버전 + Element ID, 수동 추가/제외/복원 이력 | 편집 근거 재현 |
| CHAIN_LAYER_FILTER | Chain ID + 버전 + Layer ID와 적용 규칙 정보 | Chain별 탐색 조건 복원 |

`CHAIN_ELEMENT_MEMBER`의 중복 방지 기준은 **같은 Chain 버전 안의 같은 배치 Element를 한 번만 포함**하는 것이다. 반대로 `배치 Element ID` 단독 고유 제약은 두지 않아 Chain A/B가 같은 공용 단자를 참조할 수 있게 한다. 배치 Element ID로 소속 연결을 빠르게 찾는 역방향 인덱스를 둔다. 조회 결과는 Chain의 현재 상태/유효 버전과 결합하여, 삭제/사용 중지 Chain은 신규 분석에서 제외하고 과거 분석에서는 당시 유효 버전을 사용할 수 있게 한다. `수동 제외`된 Element는 최종 소속 연결에서 빼고 편집 이력에서만 확인한다.

예를 들어 Element E가 Chain A와 C에 속하면 `Element E → [Chain A, Chain C]`가 반환된다. E와 AOI Defect가 겹쳤을 때 두 Chain을 모두 연관 후보로 제시한다. 공유 자체를 자동 오류로 보지 않고 공용 단자 여부를 확인 대상으로 표시한다. Chain A를 삭제해도 Element E와 Chain C의 소속 연결은 남는다.

16.3절의 단일 `Input Part ID`/`Output Part ID`는 현재의 복수 Input/Output Element 지정과 맞지 않으므로 1차 물리 모델에 그대로 사용하지 않는다. 역할은 `CHAIN_ELEMENT_MEMBER`의 여러 행 또는 별도 역할 연결로 표현한다. 실제 테이블명과 컬럼 타입은 기존 GDS DB 스키마 및 SREF/AREF 배치 식별 검증 후 확정한다.

#### 확인 시나리오와 구현 순서 보완

1. 메모리에서 Chain A/B/C를 만들고 각기 다른 Input/Output과 수동 보정을 설정한다. A만 수정하거나 삭제해도 B/C가 그대로인지 확인한다.
2. A/C의 Visible을 체크하고 B를 해제해 지도에 A/C만 표시되는지 확인한다. 행 선택으로 활성 Chain을 바꾸어도 체크 상태가 바뀌지 않아야 한다. Layer 표시를 해제하면 `일부 Layer 숨김` 안내가 나와야 한다.
3. 공용 Element E를 A와 C에 포함시킨다. 소속 조회에서 A/C가 모두 나오고 A 삭제 뒤에는 현재 소속으로 C만 나오며 과거 A 이력은 남는지 확인한다.
4. 위 동작을 영구 Element 참조와 DB 저장/재조회에 연결한다. 재조회 후 Chain별 편집 결과와 Element 역조회가 동일해야 한다.

따라서 다음 구현의 첫 단위는 **Chain별 상태 객체와 Chain 목록/Visible 체크/독립 수정 및 삭제를 메모리에서 동작시키는 것**이다. DB 저장은 영구 Element 참조 검증 후 두 번째 단위로 진행한다.

### 17.24 2026-09-28 / 다중 Chain 1차 구현 기록

현재 GDS 화면에서 Chain A/B/...를 여러 개 만들고, 왼쪽 목록의 행을 눌러 편집 대상을 바꾸며 체크로 지도 표시를 켜고 끌 수 있다. 각 Chain은 Input/Output, 수동 추가/제외, Layer 필터/규칙, 마지막 후보 경로를 별도 메모리 상태로 보관한다. 이름 변경과 현재 세션에서의 개별 삭제도 지원한다. 새 GDS를 조회하면 기존 화면의 임시 Element 키와 Chain 목록을 비우고 Chain A부터 다시 시작한다.

지도에는 활성 Chain의 기존 후보 강조와 함께, 체크된 다른 Chain의 Element 외곽선을 목록에 표시된 색으로 그린다. 현재 숨긴 GDS Layer의 외곽선은 그리지 않는다. 연결 검사는 여러 Chain의 도형 캐시가 함께 존재하더라도 활성 Chain에서 체크한 Layer만 사용한다.

이번 구현은 메모리 초안 단계다. DB 저장/재조회, 영구 배치 Element ID, Element에서 소속 Chain을 찾는 역조회, 과거 버전/논리 삭제는 구현하지 않았다. 목록의 `일부 Layer 숨김` 문구와 같은 Element의 다중 Chain 소속 안내도 후속 화면 작업이다. 실행 화면에서 여러 Chain의 전환/체크/개별 삭제가 의도대로 보이는지는 엔지니어가 확인한다.

2026-09-28 화면 배치 변경: 상단 Chain 설정줄과 왼쪽 Chain 목록은 `GdsMapTestForm.Designer.cs`의 `InitializeComponent()`에서 생성/배치한다. `GdsMapTestForm_Chain.cs`와 `GdsMapTestForm_ChainList.cs`는 생성/`Controls.Add`를 하지 않고 이벤트 연결과 상태 처리만 담당한다. 이름 수정 팝업은 별도 임시 대화상자이므로 이 범위에서 제외한다.

### 17.25 2026-09-28 / Chain별 Input/Output 단자 1개 제한

현장 화면 확인에 따라 현재 운영 규칙은 **한 Chain에 Input Element 1개와 Output Element 1개만 확정**하는 것이다. 앞 절의 복수 Input/Output은 당시 탐색 기능 구현 이력이며, 현재 Chain 등록 UI의 허용 개수로 적용하지 않는다. 다른 단자 쌍을 지정해야 하면 기존 Chain에 단자를 추가하거나 재지정하지 않고 `새 Chain`을 만들어 설정한다.

Input/Output 지정 버튼을 눌렀을 때 해당 Chain의 같은 역할에 확정된 Element가 있으면 경고를 표시하고 선택 모드로 진입하지 않는다. 선택 중에도 역할별 임시 Element를 한 개로 제한하고, 겹친 위치의 `후보 모두 추가` 메뉴는 제공하지 않는다. `선택 적용`과 `후보 경로 찾기` 직전에 각각 한 개인지 다시 검사한다. 각 Chain의 기존 후보 경로와 다른 Chain의 설정은 이 검증으로 변경하지 않는다.

### 17.26 2026-09-28 / Chain 선택 시 표시 기준 일원화

실제 화면에서는 활성 Chain만 노란 GPU 선택색으로, 나머지 Chain은 목록 색 외곽선으로 표시되었다. Chain 전환 때 왼쪽 Layer 체크까지 각 Chain 값으로 바꾸어 같은 Visible 조합도 서로 다르게 보였다. 이 동작을 다음 한 가지 기준으로 정리한다.

| 조작 | 지도 표시 |
| --- | --- |
| Chain 행 선택 | 편집 대상과 해당 Input/Output Marker만 변경. 경로 표시와 Layer 체크는 유지 |
| Chain 체크 | 체크된 Chain 경로를 목록의 고유 색으로 표시. 활성 여부는 색을 바꾸지 않음 |
| Layer 체크 | 현재 지도에 표시할 GDS Layer와 Chain 외곽선 Layer를 공통으로 필터. 저장된 후보 결과는 유지 |
| 후보 경로 찾기 | 그 순간 체크된 Layer를 활성 Chain의 탐색 조건으로 저장하고 후보를 갱신 |

따라서 Chain A/B가 모두 체크되어 있고 Layer 상태가 같으면 A와 B 중 어느 행을 선택해도 두 경로의 색과 포함 Element는 같다. 단자 지정 중의 임시 선택 미리보기만 기존 선택 강조를 사용한다. 이 절은 17.23절의 `Chain 전환 시 Layer 체크 복원` 해석을 대체한다. DB 저장 시 Chain별 마지막 탐색 Layer와 사용자별 지도 Layer 표시값도 구별해야 한다.

### 17.27 2026-09-28 / Chain 미선택 시 단자 지정 차단

GDS를 조회했더라도 Chain 목록에 선택된 행이 없으면 Input/Output 지정 버튼을 비활성화한다. 버튼 호출 또는 `선택 적용`이 다른 경로로 실행되더라도 선택된 Chain이 실제 목록에 있는지 다시 확인한다. 없으면 임시 선택을 취소하고 지도 Marker를 지운 뒤 `새 Chain` 생성 또는 목록 선택을 안내한다.

목록 선택을 해제하거나 마지막 Chain을 삭제한 경우에도 같은 정리 절차를 적용한다. 기존 Chain의 확정 Input/Output은 선택 해제 전에 해당 Chain 상태에 보관하며, 다시 행을 선택하면 복원한다. 새 GDS 조회 후 목록이 비어 있는 상태에서는 Chain을 자동으로 만들지 않고 엔지니어가 `새 Chain`을 눌러 시작한다.

### 17.28 2026-09-28 / Output 없는 열린 경로 탐색

현장 테스트에서 마지막 연결부가 끊겨 Output Element를 지정할 수 없는 Chain이 확인되었다. 따라서 현재 Chain 등록 UI는 **Input Element 1개 필수 / Output Element 0개 또는 1개**로 동작한다. 17.25절의 Output 1개 필수 문구는 당시 운영 가정이고 이 절이 현재 기준이다.

Output이 있으면 기존처럼 Input에서 해당 Output에 처음 도달한 경로를 찾는다. Output이 없으면 선택한 Layer의 실제 도형 접촉만 따라 Input에서 닿을 수 있는 모든 Element를 후보로 표시한다. 떨어진 Element나 선택하지 않은 Layer의 Element는 포함하지 않는다. 분기가 있으면 연결된 분기까지 후보에 들어가므로 엔지니어가 지도를 확인해야 한다. 이 결과는 `Output 미지정` 상태의 열린 후보이며 전기적 연결 완료로 판정하지 않는다.

`후보 경로 찾기`와 수동 추가/제외는 두 모드에서 모두 사용한다. Output 없는 모드의 연결 검사는 후보 Element가 Input과 이어지는지 확인하며 Output 도달률을 표시하지 않는다. DB 저장 단계에서도 Output 역할은 선택값으로 설계하고, 열린 후보와 Input/Output 연결 검증 결과를 구분해야 한다.

### 17.29 2026-09-28 / Chain 표시를 OpenGL로 통일하는 설계

#### 현재 원인과 변경 목표

Map은 GDS 도형의 정점을 GPU 버퍼에 올려 OpenGL로 그린다. 반면 Chain 외곽선은 `OnPaint`의 OpenGL `SwapBuffers` 이후 GDI+로 그린다. 이동/줌 때마다 표시된 Chain의 모든 Element를 순회하고, 화면 안 Element의 좌표 배열과 `Pen`을 다시 만든다. 화면 밖 Element도 순회와 경계 검사를 거친다. 이동/줌이 후보 경로를 재탐색하는 것은 아니다. 이 설명은 코드 경로를 확인한 결과이며, 각 구간의 실제 지연 시간은 아직 측정하지 않았다.

목표는 Chain의 도형 외곽선도 Map과 같은 OpenGL 좌표계와 렌더링 흐름으로 그리는 것이다. Chain 소속/입출력/수동 보정은 기존 데이터가 기준이며, 렌더링 변경으로 후보 경로 계산과 DB 소속 의미를 바꾸지 않는다. GDI+는 글자처럼 별도 처리 가치가 있는 작은 표시로 한정하고, Chain Element 수에 비례하는 GDI+ 작업은 제거한다.

#### 데이터와 GPU 버퍼

| 데이터 | 보관/갱신 규칙 |
| --- | --- |
| GDS Map 정점 | 기존 `_vbo`와 Layer 도형을 그대로 사용. Chain 색 때문에 전체 Map 버퍼를 재생성하지 않음 |
| Chain 소속 | Chain ID/Element Key를 원본으로 유지. 렌더러는 현재 GDS의 `GlSceneItem`을 참조해 정점 생성. `ChainTraceElement.WorldPoints`의 반복 복사는 피함 |
| Chain 표시 정점 | Map 버퍼와 분리한 Chain 전용 VAO/VBO에 월드 좌표로 저장. Chain ID와 Layer ID별로 연속 구간을 만들고 색을 정점에 기록 |
| Chain 표시 변경 | Chain 생성/삭제, 후보 갱신, 수동 추가/제외, 색 변경, 다른 GDS 로드 때 해당 표시 데이터를 다시 구성. Chain Visible과 Layer Visible 변경은 가능한 한 그릴 구간만 바꿈 |
| 이동/줌 | Chain 정점 재생성/재업로드 없이 Map과 같은 투영 행렬만 적용해 그리기 |

첫 단계는 기존 Map과 호환되는 정점 형식/셰이더를 재사용한다. Chain 외곽선은 원본 도형의 `WorldPoints`를 선분으로 변환해 Chain별 색으로 그린다. 닫힌 Polygon/Ball 윤곽은 마지막 점과 첫 점을 연결하고, PATH는 현재 Chain 표시처럼 중심선을 그린다. 폭이 있는 PATH는 Map에서는 면으로 보이므로, 외곽선만으로 위치가 다르게 보이는 사례가 있으면 후속 단계에서 실제 폭의 양쪽 경계까지 표현한다. 중복 소속 Element는 각 Chain의 데이터에서 유지하며, 여러 색이 겹칠 때 식별 방법은 현행 목록 색과 실제 화면 비교 후 결정한다.

#### 한 프레임의 그리기 순서

1. 기존 Map Layer/Defect를 OpenGL로 그린다.
2. 현재 GDS Layer Visible과 Chain Visible을 모두 만족하는 Chain 전용 버퍼 구간을 OpenGL로 그린다. 활성 Chain 행을 바꾸어도 같은 Visible 조합과 Layer 상태라면 경로 색/구성은 바뀌지 않는다.
3. OpenGL로 옮긴 표시 요소를 그린 뒤 한 번 `SwapBuffers`한다. 1차 변경은 많은 Element를 그리는 Chain 외곽선만 OpenGL로 옮긴다. 기존 Input/Output Marker, 작업 영역, `IN/OUT` 글자와 GDS TEXT는 `SwapBuffers` 이후 GDI+로 표시한다. Marker 도형/작업 영역은 후속 단계에서 OpenGL로 이동하고, 글자는 필요할 때 별도로 검토한다.

기존 OpenGL `GL.DrawArrays`가 Element별로 호출되는 구조를 Chain에 그대로 복제하지 않는다. Chain ID/Layer ID별 정점을 묶어 그리기 호출 수를 줄인다. 표시 Chain 수가 많거나 도면이 매우 크면 화면 밖 구간을 묶음 단위로 제외하는 공간 인덱스를 추가한다. OpenGL 선 두께는 환경마다 동일하게 보장되지 않으므로, 현재 2.5픽셀 GDI+ 선을 유지해야 한다면 단순 `GL.LineWidth`에 의존하지 않고 화면 픽셀 폭을 보장하는 삼각형 선분 표현을 별도로 검토한다.

#### 구현 순서와 검증

1. 같은 GDS/화면 크기/Layer/줌/Visible Chain 조건에서 Map 그리기, 기존 Chain GDI+ 그리기, 전체 프레임 시간을 나누어 측정한다. 숫자가 없으면 병목 비율을 확정하지 않는다.
2. Chain 전용 VAO/VBO 생성/삭제와 갱신 시점을 구현한다. 빈 Chain, GDS 재조회, Chain 삭제에서 이전 버퍼가 남지 않도록 한다.
3. Chain 외곽선을 OpenGL로 전환하고 기존 GDI+ Element 순회를 제거한다. Marker와 글자 표시는 유지한 채 작은 범위로 검증한다.
4. Chain A/B 동시 표시, Layer 숨김, 활성 Chain 전환, 수동 추가/제외, 열린 경로, 줌/이동에서 기존과 같은 Element가 보이는지 엔지니어가 화면에서 확인한다. 같은 조건으로 프레임 시간을 다시 측정한다.
5. 성능이 여전히 부족하면 화면 밖 묶음 제외와 Map 자체의 Element별 `DrawArrays` 호출 감소를 각각 측정해 다음 작업을 결정한다. Marker/작업 영역의 OpenGL 전환은 외곽선 검증 뒤 진행한다.

WinForms 화면 컨트롤을 새로 추가해야 하는 경우 `Designer.cs`에 배치한다. 이 설계의 1차 구현에는 새 화면 컨트롤을 만들지 않는다.

### 17.30 2026-09-28 / Chain Visible 외곽선 OpenGL 1차 구현 기록

Visible Chain의 Element 외곽선은 Chain/Layer별 선분과 한 점 도형을 전용 OpenGL VAO/VBO에 묶어 표시한다. Chain 목록 표시가 갱신될 때 CPU 정점을 다시 구성하고 다음 화면 그리기에서 한 번 업로드한다. 화면 이동/줌은 기존 Map 투영 행렬을 공유하며 Chain 정점 배열을 다시 만들거나 업로드하지 않는다. Layer Visible은 그리는 구간만 필터하고, Chain 색과 활성 행 선택의 분리는 유지한다. 새 GDS 조회와 Map Control 종료 때 전용 버퍼 상태를 정리한다.

이 단계는 Visible Chain 외곽선만 전환했다. 후보 겹침/분기 외곽선과 Input/Output Marker, 작업 영역, GDS TEXT는 기존 GDI+ 표시를 유지한다. OpenGL 선 굵기와 한 점 도형은 그래픽 드라이버에 따라 이전 GDI+ 표시와 다를 수 있다. 실제 화면에서 색/외곽선/Layer 숨김과 이동/줌 속도를 엔지니어가 확인한 뒤, 남은 도형 Overlay와 글자의 전환 범위를 결정한다. 현재 화면의 프레임 시간 비교값은 측정되지 않았다.

`NexplantQMS.GdsMap.csproj` Debug 빌드와 기존 Chain OpenEnded/ManualConnectivity/DirectOverlap/MultiEndpoint/LayerFiltered Probe 5개는 통과했다. 이 검사는 OpenGL 화면 표시나 성능 개선을 증명하지 않는다.

### 17.31 2026-09-28 / 후보 겹침과 분기 OpenGL 전환 기록

후보 겹침의 파란 실선과 분기의 주황 점선도 Visible Chain 외곽선과 같은 Chain 전용 VAO/VBO로 그린다. 후보를 찾거나 해제할 때만 정점을 다시 구성한다. 분기 점선은 Chain 전용 셰이더에서 월드 선분 길이와 현재 확대 배율을 결합해 약 7픽셀 표시/5픽셀 공백으로 만든다. 따라서 이동/줌 때 CPU에서 점선 선분을 다시 만들지 않는다. 기존 후보 GDI+ Element 순회는 제거했다. Input/Output Marker, 작업 영역, GDS TEXT는 아직 GDI+다.

Debug 빌드는 VS 출력 창에 `GDS FRAME MapSubmit=... ChainSubmit=... Swap=... GDI=... ChainVertices=... Ranges=...`를 최대 1초 간격으로 기록한다. `MapSubmit`/`ChainSubmit`은 CPU에서 OpenGL 호출을 제출한 시간이고 실제 GPU 처리 시간은 아니다. `Swap`은 화면 버퍼 교체의 대기 시간을 포함한다. 같은 도면/Layer/줌에서 Chain 체크 전후를 비교해야 하며, 실제 측정값이 수집되기 전에는 속도 개선 폭을 확정하지 않는다.

이 변경 후 Debug 빌드와 기존 Chain Probe 5개가 통과했다. 후보 색, 점선 간격, 한 점 도형 모양, GDS Layer/Chain Visible과 이동/줌 동작은 엔지니어의 실행 화면 확인이 필요하다.

### 17.32 2026-09-28 / 반복된 불필요 도형의 일괄 제외 설계

캡처의 빨간 표시처럼 한 Chain 경로에 포함되었지만 제외해야 하는 작은 원/아치 모양이 Layer 5의 다른 유효 도형과 섞여 있다. Layer 5 전체를 해제하면 필요한 경로도 사라지므로 제외 단위는 **활성 Chain의 개별 배치 Element**다. 그림의 빨간 표시는 예시 두 곳이며, 실제 제외 대상의 수와 Element 구성은 도면 데이터로 확인해야 한다. 한 표시 영역이 하나의 Element인지 여러 Element 묶음인지도 자동으로 단정하지 않는다.

#### 엔지니어 조작 흐름

1. 활성 Chain에서 `경로 제외`를 선택하고, 빨간 표시와 같은 불필요 부분의 Element를 한 묶음으로 지정한다. 겹친 위치에서는 현재처럼 Layer/DataType/종류를 보고 대상만 고른다. Input/Output은 묶음에 넣을 수 없다.
2. `유사 반복 찾기`를 실행한다. 찾을 범위는 기본으로 **현재 활성 Chain의 최종 후보 + 현재 체크된 Layer**다. 기본은 Layer 5만 제안하되 엔지니어가 대상 Layer를 바꿀 수 있다. 다른 Chain과 전체 GDS에는 영향이 없다.
3. 화면에 반복 묶음을 미리 표시하고 `찾은 묶음 수 / 제외될 Element 수 / 보호된 단자 수 / 연결 상태 변경`을 보여 준다. 범위는 `현재 보이는 화면 / 선택한 구간 / 활성 Chain 전체` 중 선택할 수 있다. 선택한 구간은 일괄 편집 범위를 제한할 뿐 경로 탐색 조건을 바꾸지 않는다.
4. 엔지니어가 묶음별 체크를 조정하고 `미리보기 적용`으로 남은 Chain을 확인한다. 이상이 없을 때 `제외 확정`을 누르면 여러 Element Key를 한 번의 수동 제외 작업으로 기록한다. 직후 `한 번에 되돌리기`를 제공한다.

빠른 보조 조작으로 `사각형/자유형으로 후보 선택`도 둔다. 마우스로 둘러싼 **현재 Chain 후보**만 선택하며 화면에 보이는 모든 GDS 도형을 제거하지 않는다. 반복 모양을 찾기 어려운 예외 위치를 몇 군데 정리할 때 사용한다. 범위 선택은 자동 경로 찾기의 필수 입력이 아니며, 기존 Input 기반 후보 계산은 유지한다.

#### 반복 묶음 판별과 오검출 방지

한 원의 크기나 Layer ID만 같다는 이유로 제외하지 않는다. 예시 묶음 안 Element의 수, 종류, 크기, 상대 위치, 방향과 주변 주 경로에 닿는 위치를 함께 비교한다. 현재 배치 도형이 같은 GDS 원본 Element를 참조하는 경우에는 이를 유사도 근거로 사용하되, 원본 참조가 같아도 주 경로에 필요한 객체일 수 있으므로 자동 확정하지 않는다. 반복 간격이 일정하면 위치 후보 검색을 빠르게 하는 데 사용하지만, 간격만으로 소속을 결정하지 않는다. 한 예시로 모호하면 두 번째 예시를 추가해 공통 패턴을 좁힌다.

후보 묶음은 일치 정도에 따라 `높음 / 검토 필요`로 나눈다. `검토 필요`는 기본 체크 해제다. Input/Output Element와 그 역할 Marker는 항상 보호한다. 연결의 유일한 통로가 되는 Element를 제외하면 Output 연결 또는 Input 연결 성분이 달라질 수 있으므로, 미리보기에서 제외 전후의 연결 검사 결과를 비교해 `연결 끊김`을 눈에 띄게 표시한다. Output이 없는 열린 경로도 Input에서 도달 가능한 후보 수의 변화를 보여 준다. 도형 연결 검사는 전기적 연결 확정으로 취급하지 않는다.

#### 데이터와 재탐색 규칙

최종 Chain 구성은 `자동 후보 + 수동 추가 - 수동 제외 + 보호 단자`라는 기존 계산을 따른다. 일괄 제외는 수십/수백 개 Element Key를 현재 Chain의 `ManualExcluded`에 추가하되, 하나의 `BatchEditId`, 예시 묶음, 탐색 범위, 대상 Layer, 처리자/시각, 적용 전후 키를 함께 기록할 수 있게 설계한다. 실행 취소는 그 BatchEditId에서 실제로 변경한 키만 복원해야 기존 수동 제외를 지우지 않는다.

현재 `SCENE:n` 키는 GDS 조회 세션에만 유효하다. DB 저장 시에는 GDS Revision에 묶인 영구 배치 Element ID로 대상 목록을 저장하고, 규칙 설명은 재현/감사용 메타데이터로 보관한다. 자동으로 찾은 규칙을 후속 GDS나 재탐색에 무조건 다시 적용하지 않는다. 동일 Revision에서 후보를 다시 찾았을 때 기존 제외 키는 유지하고, 새로 나타난 유사 도형은 `새 후보 검토` 미리보기에서 엔지니어가 추가 확정한다. 다른 Revision에서는 배치 ID 대응 검증 없이 제외 목록을 이월하지 않는다.

#### 작은 단위 구현 순서

1. 현재 Chain 후보를 대상으로 여러 Element를 한 번에 지정하는 미리보기/확정/실행 취소를 만든다. 이 단계는 기존 수동 제외 집합을 재사용한다.
2. 선택 범위 안의 후보만 빠르게 조회하도록 공간 인덱스를 붙인다. 현재 클릭 후보 조회는 전체 Scene을 순회하므로 수백 개 편집을 반복 호출하지 않는다.
3. 예시 묶음 기반 반복 후보 찾기를 추가하고, 한 예시/두 예시의 결과와 오검출을 실제 GDS로 확인한다.
4. 영구 배치 Element ID와 DB 저장을 연결할 때 일괄 편집 이력/재탐색 미리보기 정책을 적용한다.

새 버튼/목록/미리보기 패널이 필요한 구현 단계에서는 WinForms `Designer.cs`에 배치하여 디자이너에서 바로 확인 가능하게 한다. 이번 절은 설계만 기록하며 코드와 UI는 아직 변경하지 않는다.

### 17.33 2026-09-28 / 범위 일괄 제외 1차 구현 기록

> 과거 구현 기록이다. 2026-09-29 요청으로 해당 기능을 제거했으며, 현재 설계는 17.34절을 따른다.

`범위 제외 / 제외 확정 / 미리보기 취소 / 직전 제외 되돌리기` 버튼을 WinForms 디자이너에 추가했다. 범위 모드에서 왼쪽 마우스로 드래그하면 현재 활성 Chain의 최종 후보 중 체크된 Layer에 있고 드래그 영역에 **완전히 포함된** Element만 자주색 외곽선으로 누적 미리보기한다. Input/Output은 선택에서 보호한다. 미리보기에서는 수동 제외를 아직 바꾸지 않고, 확정 후에만 현재 Chain의 수동 추가/제외 집합을 갱신한다. 한 번에 되돌리기는 직전 범위 확정에서 실제로 바뀐 키의 이전 상태만 복원한다. 단일 수동 편집, 재탐색, Layer 조건 변경은 오래된 되돌리기 기록을 해제한다.

범위 조회는 전체 GDS 도형을 순회하지 않고 현재 Chain의 Element 키만 확인한다. 같은 Layer의 필요한 도형까지 없애지 않도록 범위에 살짝 걸친 긴 선은 제외하지 않는다. 범위 선택은 일반 Map 선택과 별도 모드로 처리하여 기존 선택색/선택 목록을 건드리지 않는다. 여러 영역을 연속 드래그한 뒤 한 번에 확정할 수 있다.

이 단계에는 유사 반복 패턴 자동 찾기, 자유형 범위, DB 저장, 여러 단계 실행 취소가 없다. 화면에서 버튼 위치, 자주색 미리보기, 단자 보호, 확정/취소/되돌리기, Chain 전환 및 Layer 5만 체크한 경우를 확인해야 한다. Debug 빌드와 기존 Chain Probe 5개는 통과했으나 실제 화면은 엔지니어 확인이 필요하다.

### 17.34 2026-09-29 / 현재 Chain 내부의 유사 도형 묶음 제외 재설계

#### 목적과 적용 범위

이미 Chain 경로에 포함된 Element에서 불필요한 반복 묶음을 찾아 한 번에 제외한다. 검색 범위를 드래그하거나 화면 영역으로 지정하지 않는다. 17.32절의 범위 선택 설계와 17.33절의 범위 드래그 구현은 철회한다. 이번 변경에서는 범위 제외 버튼/드래그 이벤트/전용 미리보기/되돌리기 코드와 프로젝트 참조를 제거했다. 기존 단일 Element 경로 추가/제외, 일반 Map 선택, OpenGL Chain 표시는 유지한다. 아래 유사 묶음 기능은 재설계이며 아직 구현하지 않았다.

검색 대상은 `활성 Chain 최종 포함 Element ∩ 좌측에서 체크한 Layer`로 고정한다. 최종 포함 Element는 기존 `자동 후보 + 수동 추가 - 수동 제외 + Input/Output` 계산을 사용한다. 화면 밖에 있는 Element도 포함하고, 다른 Chain의 Element는 그 Chain이 화면에 표시되어 있어도 검색 대상으로 추가하지 않는다. 여러 Chain이 같은 Element를 공유하더라도 제외 결과는 활성 Chain의 소속 정보에만 반영한다.

#### 엔지니어 조작 흐름

1. 편집할 Chain을 선택하고 후보 경로를 준비한다. Chain 미선택/숨김/후보 무효 상태에서는 기능을 시작할 수 없다.
2. `예시 묶음 지정`을 누르고 불필요한 묶음 하나를 구성하는 Element를 차례로 클릭한다. 예를 들어 작은 원 2개와 아치 연결 도형을 고른다. 같은 Element를 다시 클릭하면 예시에서 해제한다. 실제 도형이 몇 개로 구성되는지는 데이터에 따라 달라지므로 개수를 고정하지 않는다.
3. 클릭 위치에 여러 Element가 겹치면 현재 Chain에 포함되고 체크된 Layer에 있는 후보만 Layer/DataType/종류와 함께 제시한다. 현재처럼 겹침 후보에서 필요한 Element를 선택한다. 예시 지정은 Chain 소속을 변경하지 않는다.
4. `유사 묶음 찾기`를 누르면 활성 Chain 전체에서 같은 구성과 상대 배치를 가진 묶음을 찾는다. 예시 묶음 자체도 결과에 포함한다. 한 Element만 예시로 지정한 경우에는 단일 도형 반복 검색임을 명확하게 표시한다.
5. 찾은 묶음을 OpenGL 외곽선으로 미리 표시하고 `일치 묶음 수 / 제외 예정 Element 수 / 보호 묶음 수 / 검토 필요 묶음 수`를 보여 준다. 목록에서 묶음별 체크를 바꾸고 `이전/다음`으로 해당 위치를 확인한다. 화면 이동/줌은 검색 범위를 바꾸지 않는다.
6. `제외 확정`으로 체크한 묶음만 현재 Chain에서 제외한다. `취소`는 미리보기만 해제한다. `직전 제외 되돌리기`는 그 확정 작업으로 변경한 소속 정보만 복원한다.

예시는 반복된 수십/수백 개 묶음을 모두 지정하는 작업이 아니다. 대표 묶음 한 곳의 구성만 알려 주는 작업이다. 연결된 이웃을 끝없이 따라가면 전체 Chain이 하나의 예시가 될 수 있으므로, 1차 구현에서는 예시 구성 자동 확장을 하지 않는다.

#### 유사 묶음 판별 기준

| 비교 항목 | 1차 판별 기준 |
| --- | --- |
| 구성 | 예시와 같은 수의 Element를 일대일로 대응시킨다. 일부만 일치하는 묶음은 자동 제외 대상으로 체크하지 않는다. |
| 속성 | 각 Element의 Layer/DataType/도형 종류/PATH 폭을 비교한다. Layer가 여러 개여도 체크된 Layer 안에서 대응한다. |
| 개별 형상 | 위치를 원점 기준으로 옮겨 크기와 윤곽 좌표를 비교한다. Bounding Box나 원의 반지름만 같다는 이유로 일치시키지 않는다. |
| 묶음 배치 | 기준 Element에 대한 각 구성원의 상대 위치/방향/간격을 비교한다. 같은 원이 주 경로에 있다는 이유만으로 함께 제외하지 않는다. |
| 내부 연결 | 예시 구성원 사이의 겹침/접촉 관계가 같은지 확인한다. 원본 Chain의 연결 판정 기준과 일관되게 검사한다. |
| 허용 변환 | 1차는 평행 이동만 허용한다. 회전/좌우 반전/크기 변경은 자동으로 허용하지 않고 후속 요구로 분리한다. |
| 오차 | GDS 좌표 단위와 변환 계산 오차를 고려한 고정 허용 오차를 사용한다. 확대 배율이나 화면 픽셀에 따라 결과가 바뀌지 않게 한다. 실제 도면으로 오차값을 검증한 뒤 정한다. |

동일 도형의 꼭짓점 시작 위치/윤곽 진행 방향과 PATH 점 순서가 다를 수 있으므로 비교 전에 표현을 정규화한다. 모양 자체를 회전/반전하는 것과 점 순서를 정규화하는 것은 구분한다. 원본 `Source` 참조가 같으면 후보 검색을 줄이는 보조 인덱스로 쓸 수 있지만, 같은 원본이라는 이유로 일치를 확정하지 않는다.

한 기준점에 복수 대응이 가능하거나 여러 묶음이 같은 Element를 공유하면 `검토 필요`로 분류하고 기본 체크를 해제한다. 같은 Element Key 집합으로 찾은 중복 묶음은 하나로 합친다. 제외 예정 Element 수는 체크된 묶음의 Key 합집합으로 계산한다. 임의의 유사도 퍼센트만으로 삭제 여부를 결정하지 않는다.

#### 단자 보호와 연결 검사

Input/Output Element를 포함한 일치 묶음은 전체를 보호하고 제외 체크를 허용하지 않는 것을 기본으로 한다. 예시에 단자가 포함되면 다른 예시를 선택하도록 안내한다. 단자가 예시 밖의 별도 Element이면서 주변 도형만 묶음에 들어간 경우에는 단자와 직접 겹치는 묶음을 보호 후보로 분류해 엔지니어가 확인하게 한다. 이렇게 해야 단자 Element 한 개만 남기고 주변 연결 도형을 실수로 제외하는 상황을 줄일 수 있다.

미리보기에서 제외 전후의 연결 상태를 같은 조건으로 검사한다. 원래 끊겨 있던 구간과 이번 제외로 새로 끊긴 구간을 구분해 알린다. Output이 있으면 Input/Output 연결 변화를, Output이 없으면 남은 후보 중 Input에서 도달할 수 없는 Element 수의 변화를 표시한다. 검사할 수 없는 상태를 정상 연결로 표시하지 않는다. 새 끊김이 생기면 경고를 확인한 뒤 확정하도록 하고, Input/Output 자체 제외는 허용하지 않는다. 도형 겹침 기반 결과는 실제 전기적 연결의 확정 판정이 아니다.

#### 상태 관리와 성능

검색 시작 때 Chain ID/경로 버전/체크 Layer/예시 Element Key를 고정한다. 검색 중에는 취소를 제공하고, Chain 전환/Layer 변경/경로 재탐색/수동 편집/GDS 재조회가 발생하면 기존 검색과 미리보기를 무효화한다. 늦게 끝난 검색 결과도 버전이 다르면 화면이나 Chain에 적용하지 않는다. 화면 이동/줌은 허용하며 검색 상태에 영향을 주지 않는다.

검색은 전체 Scene을 반복 순회하지 않고 활성 Chain Key로 필요한 배치 데이터만 추출해 수행한다. 형상 특성과 위치 인덱스로 기준 후보를 줄인 다음 묶음 전체를 검사한다. 형상 비교는 UI 스레드 밖에서 불변 데이터로 실행하고, OpenGL 버퍼 갱신은 UI 스레드에서 결과가 바뀔 때만 한다. 검색 인덱스와 정점을 매 이동/줌마다 다시 만들지 않는다.

현재 `ChainTraceElement`에는 DataType이 없으므로, 후속 구현에서는 유사 묶음 검색용 모델에 원본 속성과 정규화 형상을 담는다. 기존 경로 탐색 모델을 불필요하게 크게 바꾸지 않는다. 현재 `SCENE:n`은 조회 세션의 배치 Key이므로 다른 GDS 조회나 DB 영구 식별에 그대로 재사용하지 않는다.

#### 제외 기록과 DB 확장

확정 시 체크한 묶음 Key를 현재 Chain의 `ManualExcluded`에 추가하고 해당 Key를 `ManualAdded`에서 제거한다. 원본 GDS 도형과 다른 Chain의 소속은 변경하지 않는다. 한 번의 확정을 BatchEditId로 묶고 각 Key의 변경 전 수동 추가/제외 상태를 보관해 되돌린다. 이후 다른 편집이 있으면 직전 되돌리기를 무효화하여 이전 상태로 덮어쓰지 않는다.

DB 연결 단계에서는 Chain ID/GDS Revision/영구 배치 Element ID를 기준으로 제외 목록과 처리 이력을 저장한다. 예시 Key/매칭 조건/대상 Layer/실제 제외 Key/처리자/처리 시각은 재현용 정보다. 이번 검색을 도면 전체나 다른 Revision에 자동 적용하는 전역 삭제 규칙으로 저장하지 않는다.

#### 작은 단위 구현 순서

1. 현재 Chain 내부에서 클릭으로 예시 묶음을 지정/해제하는 기능을 만든다. 새로운 버튼/목록은 `Designer.cs`에 배치한다. 이 단계에서는 소속을 변경하지 않는다.
2. 평행 이동 기준 형상/배치 매칭과 결과 미리보기를 구현한다. 체인 밖 동일 도형 배제/체인 안 화면 밖 도형 포함/체크 해제 Layer 배제/단자 보호/부분 일치 배제를 검사한다.
3. 묶음별 체크/제외 확정/직전 되돌리기와 연결 변화 검사를 붙인다. 같은 Element를 공유하는 다른 Chain에 영향이 없는지 확인한다.
4. 실제 도면에서 오검출과 수백 묶음 검색 시간을 확인한 뒤 회전/반전 지원이나 예시 지정 편의성을 추가 검토한다.

#### 이번 제거 작업의 검증

범위 드래그 관련 타입/버튼/호출/프로젝트 참조가 남지 않은 것을 검색으로 확인했다. Debug 빌드와 ChainOpenEnded/ChainManualConnectivity/ChainDirectOverlap/ChainMultiEndpoint/ChainLayerFiltered Probe 5개가 통과했다. 기존 `GlDefectItem.LineVertexCount` 미할당 경고 1개는 남아 있다. 이 결과는 기존 경로 로직의 회귀 검사이며, 새 유사 묶음 설계가 구현되었거나 화면 검증을 마쳤다는 의미는 아니다. 엔지니어는 실행 화면에서 범위 제외 행이 사라지고 기존 경로 추가/제외와 이동/줌이 동작하는지 확인한다.

### 17.35 2026-09-29 / 예시 묶음 클릭 지정 1차 구현

`예시 묶음 지정` 버튼을 WinForms Designer의 Layer 설정 행에 배치했다. 후보 경로가 있는 활성 Chain에서 버튼을 누른 뒤 지도 Element를 클릭하면 예시에 넣고, 다시 클릭하면 해제한다. 클릭 지점에 도형이 겹치면 현재 Chain의 최종 포함 Key와 좌측 체크 Layer를 모두 만족하는 후보만 Layer/DataType/종류/Key로 메뉴에 표시한다. 선택한 예시는 자주색 OpenGL 외곽선으로 표시하며, 버튼을 다시 누르면 클릭 모드만 종료하고 예시는 남긴다. Esc는 예시 지정과 선택을 함께 취소한다.

예시는 현재 Chain 화면 세션의 임시 상태다. Chain 전환/선택 해제/현재 Chain 숨김/Layer 조건 변경/후보 무효화/GDS 재조회에서 자동으로 초기화한다. 선택 중에는 Input/Output 지정, 후보 찾기, 경로 추가/제외, Chain 생성/이름 수정/삭제 버튼을 비활성화한다. 다른 Chain의 소속과 GDS 원본은 바꾸지 않는다.

현재 구현 범위는 예시 선택과 시각 표시까지다. 묶음 유사도 계산/검색 결과 목록/일괄 제외/DB 기록은 17.34절의 다음 단계다. Debug 빌드와 기존 Chain Probe 5개가 통과했다. 실행 화면에서는 버튼 활성 조건, 겹침 메뉴, 자주색 외곽선, 다시 클릭 해제, Esc, Chain 전환/Layer 변경 후 초기화를 엔지니어가 확인한다.

### 17.36 2026-09-29 / 현재 Chain 내부 유사 묶음 검색과 미리보기

`유사 묶음 찾기`와 `미리보기 취소` 버튼을 Designer의 Chain 행에 추가했다. 엔지니어가 예시 지정을 끝내고 검색하면 `현재 활성 Chain 최종 포함 Element ∩ 좌측 체크 Layer`의 배치 도형만 복사하여 백그라운드에서 비교한다. 다른 Chain/화면 밖의 비소속 객체는 검색하지 않으며, 현재 Chain에 속한 화면 밖 객체는 포함한다. 한 위치의 예시 Element 전부에 대응되는 묶음만 결과에 넣고, 단일 유사 도형은 일부 구성이라는 이유로 포함하지 않는다.

비교에는 Layer/DataType/도형 종류/PATH 폭/경계 크기/꼭짓점 윤곽/묶음 내 상대 위치를 사용한다. Boundary의 시작 꼭짓점과 진행 방향, Path의 역방향 표현은 같은 도형으로 인정한다. 위치 변화는 평행 이동만 허용하고 좌표 허용 오차는 임시로 0.0001 GDS 좌표 단위다. 이 값은 실제 도면 검증 후 확정한다. X좌표 정렬 인덱스로 후보 범위를 줄이며, 검색 중 Chain/Layer/예시 변경이나 취소가 발생하면 이전 결과를 적용하지 않는다.

일반 결과는 녹색, Input/Output이 들어간 보호 묶음은 주황색, 같은 Element를 공유하거나 대응이 여럿인 묶음은 빨간색, 예시는 자주색 OpenGL 외곽선으로 미리 표시한다. 상태 문구에 전체/보호/검토 필요 묶음 수를 표시한다. 미리보기 취소는 검색과 결과 색만 해제하고 예시는 유지한다. 이 단계에서는 묶음별 결과 목록/체크/일괄 제외/연결 상태 비교/DB 저장을 아직 구현하지 않았다. 현재 색은 검색 결과 안내이며 제거 승인이 아니다.

Debug 빌드와 기존 Chain Probe 5개, 새 유사 묶음 Probe 1개가 통과했다. 새 Probe는 평행 이동된 세 도형 묶음/단일 도형 배제/DataType 차이/Boundary 시작점 변경/PATH 역방향/단자 보호를 검사한다. 실행 화면은 엔지니어가 확인해야 한다. 확인 순서는 Chain 선택과 후보 찾기, 예시의 구성 Element 클릭, 예시 지정 종료, 유사 묶음 찾기, 색과 묶음 수 확인, 미리보기 취소, Chain/Layer 변경 후 결과 초기화다.

### 17.37 2026-09-29 / 유사 묶음 목록 검토와 일괄 제외

왼쪽 `유사 묶음` 목록에 검색 결과를 묶음 단위로 표시한다. 일반 일치는 기본 체크, 검토 필요 묶음은 기본 해제, Input/Output이 포함된 보호 묶음은 체크 불가다. 목록의 행을 선택하면 해당 묶음을 흰색 외곽선으로 강조하고 지도 중심으로 이동/확대한다. 체크 여부는 검색 범위가 아니라 제외할 결과를 고르는 값이다.

`체크 묶음 제외`는 체크된 묶음의 Element Key 합집합만 사용한다. 확정 직전에 현재 Chain 소속인지, Input/Output이 섞이지 않았는지, 현재 탐색 Layer에서 연결 검사가 가능한지 다시 확인한다. 제외 전후 연결 검사는 UI 스레드 밖에서 실행한다. 이번 제외로 새 단절 객체가 생기거나 Output 도달 수가 줄면 경고를 보여 주고 엔지니어가 계속할지 선택한다. 원래 끊겨 있던 객체와 이번 작업 때문에 새로 끊긴 객체는 Key 차이로 구분한다. Output이 없는 열린 경로도 남은 객체의 단절 여부를 검사한다.

확정 후에는 현재 Chain의 `ManualAdded`/`ManualExcluded`만 바꾼다. 자동 후보는 수동 제외에 넣고, 수동 추가 객체는 수동 추가에서 뺀다. 다른 Chain과 GDS 원본은 바꾸지 않는다. `직전 제외 되돌리기`는 그 확정에서 실제로 변경된 Key의 이전 수동 상태만 복원한다. 기존 수동 제외는 유지한다. Chain별로 직전 되돌리기를 따로 보관하고, 해당 Chain의 다른 수동 편집/재탐색/Layer 조건 변경/단자 확정이 있으면 이전 되돌리기 기록을 지운다. Chain/Layer/경로가 바뀌는 동안 늦게 끝난 연결 검사는 결과를 적용하지 않는다.

현재 `SCENE:n` Key와 되돌리기 기록은 GDS 화면 세션의 메모리 상태다. DB 영구 저장은 아직 하지 않는다. 이번 단계는 빌드와 Chain Probe 7개가 통과했다. 새 Probe는 자동 후보와 수동 추가 도형의 일괄 제외 후 복원, 기존 수동 제외 보존을 검사한다. 실제 화면에서 목록 체크/단자 보호/행 이동/연결 경고/확정/되돌리기/Chain 전환은 엔지니어가 확인해야 한다.

### 17.38 2026-09-29 / Device 기준 Map 및 Chain DB 저장/재조회 설계

#### 목적과 현재 코드에서 확인한 차이

조회 입력은 Device다. Device에 속한 도면 개정본을 선택한 뒤 DB의 도형만으로 Map을 다시 그리고, 같은 개정본의 Chain을 그 위에 복원한다. 이후 AOI Defect를 동일 좌표계로 변환하여 별도 Overlay로 표시하고, Defect가 닿는 배치 Element에서 소속 Chain을 역조회한다. 이 절은 저장 구조와 구현 순서를 정하는 설계이며 DB 테이블 생성이나 저장 기능 구현 기록이 아니다.

현재 `GdsElementGridRow`는 Library/Structure의 **원본 Element**를 조회 전용으로 표시한다. `Grid No`는 행을 만들 때 증가하는 번호이며 파서가 제외한 중복 원본 행도 들어간다. 지도는 `ShowStructure`가 SREF를 펼쳐 만든 **배치 도형**(`GlSceneItem`)을 그린다. 한 원본이 여러 곳에 배치되면 Grid 행 하나와 지도 도형 여러 개가 대응할 수 있다. Chain은 배치 도형에 실행 중 순서로 `SCENE:n`을 붙인다. 따라서 Grid 행 번호, 원본 Element 객체, `SCENE:n`을 DB의 Chain 참조 키로 사용하지 않는다.

현행 DB 시험 코드는 `SaveElements`에 고정 `GDS_SEQ=2`/`STRUCTURE_NAME=TOP`을 사용하고, 조회 버튼도 `LoadElements(2, "TOP")`을 호출한다. 이 경로는 Device별 개정본 선택, 전체 도면/Chain 재조회, 배치 ID 복원을 제공하지 않는다. 또한 현재 화면 Flatten은 SREF를 처리하지만 AREF 배치는 처리하지 않으므로, AREF가 포함된 도면의 완전한 Map 저장/재현은 별도 구현과 검증이 필요하다.

#### 데이터 경계와 키

| 논리 데이터 | 키 / 주요 내용 | 역할 |
| --- | --- | --- |
| `DEVICE_MAP_REVISION` | `MapRevisionId` PK, `DeviceId` FK, Revision, TopStructure, GDS 파일 해시, 단위/원점/축 방향, 상태, 생성자/시각 | Device별 도면 개정본. 같은 Device에 여러 개정본을 허용하고 현재 사용 개정본은 별도 지정 |
| `MAP_LAYER` | (`MapRevisionId`, `LayerId`) PK, 기본색/표시 이름 | Map 그리기용 Layer 정의. 사용자 체크 상태는 별도 화면 설정 |
| `MAP_SOURCE_ELEMENT` | (`MapRevisionId`, `SourceElementId`) PK, Structure/원본 순번, 종류, Layer/DataType, 원본 도형 속성 | 기존 Grid 원본 상세값과 파일 출처 조회. 중복 제외 원본은 그 사실을 구분 |
| `MAP_PLACED_ELEMENT` | (`MapRevisionId`, `PlacedElementId`) PK, SourceElementId, 배치 경로, 월드 Bounds, 종류/Layer/DataType, 좌표/폭/TEXT 표시 데이터 | Map OpenGL 구성과 Chain/Defect 분석에 함께 쓰는 실제 화면 도형 |
| `CHAIN_MASTER` | `ChainId` PK, `MapRevisionId` FK, Code/Name/Color/상태/버전 | Device의 한 도면 개정본에 속하는 Chain A~Z 관리. 같은 개정본의 Code는 중복 금지 |
| `CHAIN_ELEMENT_MEMBER` | (`ChainId`, `PlacedElementId`) PK, 역할(`INPUT`/`OUTPUT`/`PATH`), 포함 근거, 선택적 PartId | 확정된 최종 Chain 도형. Element 하나가 Chain 여러 개에 소속 가능 |
| `CHAIN_LAYER_FILTER` / `CHAIN_LAYER_RULE` | ChainId, LayerId/규칙 순서/값, 사용 여부, 탐색 격자 크기 | 엔지니어가 당시 탐색에 사용한 Layer 및 선택 규칙 복원 |
| `CHAIN_ELEMENT_EDIT` | ChainId, PlacedElementId, 수동 추가/제외, BatchEditId, 작업자/시각, 변경 순번 | 후보 재탐색 시 수동 결정을 재적용하고 일괄 제외 근거를 조회. 최종 소속 테이블과 구분 |

여기서 영구 키는 도형마다 발급한 `PlacedElementId`다. 같은 개정본을 다시 조회할 때 이 ID를 그대로 읽는다. 파일에서 신규 생성할 때에는 TopStructure에서 시작한 참조 인스턴스 경로(SREF의 원본 순번, AREF 지원 시 행/열), 원본 Structure 안 Element 순번을 함께 기록한다. 좌표/형상 해시는 대응 결과를 검증하는 보조값이며 단독 키가 아니다. `PlacedElementId`는 **개정본 범위**에서만 유효하다. 개정본이 달라지면 Chain 참조를 자동 이월하지 않고 대응 검증을 거친다.

`CHAIN_ELEMENT_MEMBER`에는 (`ChainId`, `PlacedElementId`)만 유일하게 두고 `PlacedElementId` 단독 유일 제약은 두지 않는다. 한 Chain의 Input은 정확히 한 배치 Element가 필수이고 Output은 0개 또는 1개다. 저장 시 역할별 개수를 검사하고, DB에는 Chain별 역할 유일 제약과 같은 개정본 소속만 참조하도록 하는 FK/저장 검증을 둔다. Input/Output도 최종 소속 행에 반드시 포함한다. Output 없는 열린 경로는 정상 데이터로 저장한다. Ball Site/Terminal/Wire Segment와 여러 도형으로 된 Part는 현재 자동 판별된 사실로 저장하지 않으며, 엔지니어가 Part를 정의하는 단계에서 `CHAIN_PART`/`CHAIN_PART_ELEMENT`를 추가한다.

Chain을 논리 삭제해도 Map 도형과 다른 Chain의 소속은 삭제하지 않는다. 과거 분석 재현이 필요하므로 Chain 수정 시 버전/변경 이력을 보존하고 분석 결과에는 사용한 MapRevisionId/Chain 버전을 기록한다. Chain Visible과 지도 Layer 체크는 분석 기준정보가 아니라 표시 상태다. 재접속 복원이 필요하면 사용자별 `MAP_VIEW_PREFERENCE`에 분리하고 Chain 유효 상태와 혼합하지 않는다.

#### Grid 표시 / 실제 DB 저장값

기존 Grid의 원본 정보는 `MAP_SOURCE_ELEMENT`에서 조회할 수 있게 한다. 여기에 `SourceElementId`, 배치 인스턴스 수를 추가할 수 있다. Chain 소속은 원본 행에 단일 `ChainId` 컬럼으로 넣지 않는다. SREF 반복 배치마다 Chain이 다를 수 있고 도형 하나가 복수 Chain에 속할 수 있기 때문이다. Grid에서 Chain을 확인할 때에는 해당 원본 행의 **배치 인스턴스 목록**을 열어 `PlacedElementId / 배치 경로 / Chain 코드 목록 / 역할 / 포함 근거`를 표시한다. 배치 인스턴스 행을 주 Grid로 전환한다면 같은 열을 직접 보여 줄 수 있다. 이 열은 `CHAIN_ELEMENT_MEMBER`를 조회해 만든 표시값이고 저장 원본은 연결 테이블이다. 파서 중복 제외 행은 배치 ID가 없을 수 있으므로 Chain 없음과 구분해 표시한다.

Map을 DB만으로 그리려면 Chain에 속한 도형만 저장해서는 안 된다. 화면에 필요한 **모든 배치 도형**의 좌표, 종류, 폭, TEXT 위치/문구, Layer 색 및 좌표 단위를 복원할 수 있어야 한다. Boundary/Path의 전체 꼭짓점은 Grid의 첫/끝 좌표만으로 재구성할 수 없으므로 별도 정점 데이터 또는 버전이 명시된 이진 Geometry로 저장한다. OpenGL VAO/VBO 번호나 화면 픽셀 좌표는 저장하지 않고 DB 도형으로부터 재생성한다. 원본 GDS 파일 해시는 출처와 재현 검증에 사용하며 DB 도형의 대체물이 아니다. 큰 도면은 한 행씩 INSERT/SELECT하는 방식을 피하고 도형/정점을 배치 단위로 읽어 GPU 버퍼를 만들도록 설계한다. 공간 인덱스/영역 분할과 실제 저장 용량은 대표 Device의 배치 도형 수/정점 수를 측정해 정한다.

#### 저장과 재조회 순서

1. 엔지니어가 Device와 대상 Map 개정본을 지정한다. 파일을 등록할 때 Device/Revision/TopStructure/단위/해시를 확정하고, 원본 Element와 배치 Element의 ID 대응표를 만든다. 이미 등록된 개정본이면 같은 파일/개정본인지 검증한다.
2. Map 헤더/Layer/원본 Element/전체 배치 도형/정점을 한 도면 개정본으로 저장한다. 누락된 참조, 중복 배치 ID, 저장 건수/정점 수 불일치가 있으면 그 개정본을 사용 가능 상태로 만들지 않는다.
3. 각 Chain의 본체, Input/Output 포함 최종 소속, 탐색 Layer/규칙, 수동 보정과 버전을 같은 논리적 저장 작업으로 반영한다. 저장 직전 Chain의 모든 PlacedElementId가 그 MapRevisionId에 존재하는지 확인한다. 일부만 저장되면 전체 Chain 작업을 롤백한다.
4. Device 조회 시 사용 개정본을 정하고 Map 도형/Layer를 읽어 OpenGL 버퍼와 Grid 자료를 구성한다. 이어 해당 개정본의 Chain A~Z와 소속/역할/필터를 읽어 Overlay를 구성한다. Map 또는 Chain 참조 검증에 실패하면 `복원 불가`를 표시하고 임의의 다른 도형에 자동 연결하지 않는다.
5. 파일을 닫고 Device만으로 다시 조회해 Map/Chain 색, Input/Output, 수동 제외 후 최종 도형, 여러 Chain의 공용 도형과 Grid 역조회가 저장 전과 일치하는지 비교한다. 후보 찾기는 사용자가 명시적으로 재실행할 때만 갱신하며 재조회만으로 엔지니어의 확정 소속을 바꾸지 않는다.

#### 추후 Defect 연결을 위한 좌표 계약

AOI Defect 원본에는 Device뿐 아니라 검사 대상/발생 시각/원본 좌표계/장비 또는 이미지 좌표 변환 버전을 남긴다. Map 위에 표시할 때 Device의 **해당 MapRevisionId**를 확정한 뒤 Defect 좌표/크기/형상을 GDS 월드 좌표로 변환한다. Y축 방향, 단위, 회전/반전, 원점 및 정합 오차를 기록하고 정합되지 않은 Defect는 정확한 Chain Hit으로 처리하지 않는다. Map/Chain/Defect는 별도 Overlay로 그린다. 변환된 Defect 영역과 `MAP_PLACED_ELEMENT` 형상의 겹침으로 대상 Element를 찾고 `CHAIN_ELEMENT_MEMBER` 역조회로 관련 Chain을 모두 반환한다. 위치상 겹침은 전기적 차단 원인 확정이 아니라 분석 후보이다.

#### 1차 구현 범위와 선행 확인

1차는 대표 Device 한 개/개정본 한 개로 `영구 배치 ID 생성 → Map 전체 저장 → DB 단독 재조회 → Chain 한 개 저장/복원 → 여러 Chain과 Grid 역조회` 순서로 나눈다. 다음 단위에서 Chain 수정/논리 삭제/버전 충돌과 대량 데이터 성능을 검증한다. Defect 테이블/화면 Overlay는 Map/Chain 왕복 복원이 확인된 뒤 붙인다. 신규 화면 컨트롤이 필요해지면 WinForms Designer에 배치한다.

구현 전 확인할 항목은 실제 Device 마스터의 키/개정 규칙, 기존 GDS DB 테이블과 변경 가능 범위, GDS 좌표 단위/TopStructure 선택 기준, 실제 파일의 AREF 사용 여부, AOI 좌표 변환 기준과 대량 도형 규모다. 이 값은 아직 확정되지 않았으므로 물리 테이블명/컬럼 타입/인덱스 저장 방식을 최종 DDL로 간주하지 않는다.

### 17.39 2026-09-29 / 배치 Element ID 1차 코드 반영

`FlattenStructure`가 TopStructure/참조 Element 순번/원본 Element 순번을 연결한 `PlacedElementId`를 각 화면 도형에 부여한다. 원본 Grid에는 같은 원본 순번으로 만든 `SourceElementId`를 표시한다. 참조 구조 안의 한 원본이 여러 곳에 놓이면 `SourceElementId`는 같고 `PlacedElementId`는 배치 경로마다 다르다. Chain의 마우스 후보/강조/후보 경로는 기존 `SCENE:n` 대신 이 배치 ID를 사용한다. ID는 향후 DB에서 `MapRevisionId`와 함께 사용해야 하며, 다른 개정본 사이에 같은 문자열이 있더라도 같은 도형이라는 뜻은 아니다.

이번 변경은 ID 생성과 기존 Chain 코드의 참조 통일까지만 수행했다. DB 저장/조회, Grid의 Chain 소속 열, AREF Flatten, 파일을 다시 파싱했을 때 ID 대응 검증은 아직 구현하지 않았다. 파서의 원본 순서가 바뀌면 같은 파일도 ID가 달라질 수 있으므로 DB 등록 시 생성기 버전/원본 파일 해시를 기록하고 재파싱 대응 검사를 해야 한다. Debug 빌드는 통과했고 기존 `GlDefectItem.LineVertexCount` 경고는 남았다. 실제 화면 선택/탐색 동작은 엔지니어가 확인한다.

### 17.40 2026-09-29 / Device Map 저장 경계와 Oracle 스키마 초안

기존 `GDS_ELEMENT` 저장/조회는 Device/Revision을 구분하지 않고 Boundary/Path 일부를 고정된 번호와 Structure로 처리한다. 따라서 별도 신규 테이블 초안을 `Persistence/2026-09-29_device_map_oracle_schema.sql`에 작성했다. Device별 개정본, 원본 Grid Element, Layer, 화면의 전체 배치 Element를 분리한다. 배치 도형의 전체 월드 좌표와 TEXT 속성을 저장해야 Chain이 없는 배경 도형까지 DB만으로 복원할 수 있다. SQL은 실제 DB에 실행하지 않았고 기존 테이블도 변경하지 않았다.

`GdsMapControl.VisitPlacedElements`는 Map에 있는 Boundary/Path/TEXT를 Layer 표시 여부와 관계없이 한 건씩 DB 저장 계층에 전달한다. 좌표 배열을 한꺼번에 복사하지 않고 콜백 안에서 직렬화하도록 설계했다. 현재 ID는 `R0/R참조순번/E원본순번` 형식의 개정본 내부 키다. 원본 Grid의 `SourceElementId`와 함께 전달하며, 컬럼 길이/좌표 존재를 검사한다. 이번 단계는 저장 가능한 배치 데이터의 추출 경계까지다. 실제 Oracle INSERT/SELECT, 트랜잭션, Device 선택 화면, DB 조회 후 OpenGL 재구성, Chain 저장은 다음 구현 단위다. SQL의 Device 마스터 FK와 물리 타입은 실제 운영 스키마 확인 후 확정한다.

### 17.41 2026-09-29 / 대량 Map 데이터의 Service 전송과 DB 적재 설계

#### 현재 코드에서 조정이 필요한 부분

현행 `VisitPlacedElements`는 UI 스레드에서 동기 콜백으로 모든 화면 도형을 순회한다. 이 콜백에서 네트워크 전송이나 DB 응답을 기다리면 Map 이동/줌과 버튼 조작이 멈춘다. 향후 Service 저장 연결에서는 이 함수를 그대로 HTTP 호출에 연결하지 않는다. UI 스레드는 변경되지 않는 도형을 **한 배치만 복사/직렬화**하고 비동기 전송 작업에 넘긴 다음 다시 사용자 입력을 처리해야 한다. 도면을 다시 열거나 배치 ID 생성 조건이 바뀌면 진행 중 업로드를 중지한다.

기존 Oracle 시험 코드는 `DataTable`에 최대 5만 행을 모아 BulkCopy한다. 도형마다 꼭짓점 수가 다르므로 `5만 행`은 메모리나 요청 크기의 안전한 상한이 아니다. 전체 화면 정점 수는 DB로 보낼 원본 좌표 수와도 다르다. 실제 도면에서 Boundary/Path/TEXT 수, 원본/배치 도형 수, 좌표 수, 직렬화 바이트 수를 먼저 측정해 전송 크기와 시간을 정한다.

#### 전송 단위와 메모리 제한

```text
GDS/Map 고정 스냅샷
  → Layer / 원본 Element / 배치 Element 순서로 작은 배치 생성
  → 크기가 제한된 전송 대기열
  → Service 배치 수신 / 검증
  → Oracle 배치 INSERT / COMMIT
  → 완료 배치 번호 응답
  → 전체 검증 후 Map Revision READY
```

배치는 **도형 수와 직렬화 전 바이트 수를 동시에 제한**한다. 시작값은 예를 들어 `최대 2,000도형 또는 8MiB 중 먼저 도달한 값`으로 두고, 전송 대기열은 2~3배치만 허용한다. 이 숫자는 성능 확정값이 아니라 측정용 초기값이다. 한 도형의 좌표만으로 상한을 넘는 경우에는 해당 도형 한 건을 별도 처리하거나 좌표를 분할하는 규약이 필요하며 조용히 버리지 않는다. 대기열이 차면 생산을 잠시 멈추어 Map 전체의 좌표 복사본이 메모리에 쌓이지 않게 한다. 좌표는 화면 정점이 아니라 `MAP_PLACED_ELEMENT`의 월드 좌표를 명시된 이진 형식으로 보낸다. 압축은 실제 네트워크 시간/CPU 시간을 비교한 뒤 적용한다.

화면에 Chain을 표시하거나 지도를 이동/확대해도 업로드할 Map 데이터는 변하지 않아야 한다. 시작 시 Device/MapRevisionId/GDS 해시/ID 생성기 버전/TopStructure를 고정하고, 파일 재조회나 도면 교체는 진행 중 작업을 취소하거나 새 개정본으로 시작한다. Chain 수정은 Map 업로드 완료 후 별도 저장 작업으로 처리한다.

#### Service 계약과 실패 후 재개

| 호출 | 입력 | 반환 / 처리 |
| --- | --- | --- |
| `업로드 시작` | Device, Revision, 파일 해시, TopStructure, 생성기/좌표 형식 버전, 예상 원본/배치/좌표 수 | `JobId`, `MapRevisionId`, 이미 완료된 배치 목록. Revision은 `DRAFT` |
| `배치 업로드` | JobId, 종류(`LAYER`/`SOURCE`/`PLACED`), BatchNo, 건수/좌표 수/바이트 수, PayloadSHA256, Payload | DB Commit 후 완료 배치 번호/누적 건수 응답 |
| `상태 조회` | JobId | 완료 배치/건수, 실패 원인, 재개 가능 여부 |
| `업로드 완료` | JobId, 전체 건수/좌표 수/파일 해시 | DB 검증 성공 시 Revision을 `READY`로 전환 |
| `취소` | JobId | 신규 배치 수신 중단, Revision은 `READY`로 바꾸지 않음 |

Service는 `(JobId, 종류, BatchNo)`를 멱등 키로 사용한다. 동일 번호/동일 해시를 재전송하면 다시 INSERT하지 않고 이전 완료 응답을 돌려준다. 동일 번호/다른 해시는 충돌로 거부한다. 배치의 본문, 누적 건수, `MAP_IMPORT_BATCH` 완료 기록은 **같은 DB 트랜잭션**에서 반영하고 Commit 뒤에만 성공 응답한다. 네트워크 응답만 유실되더라도 상태 조회 후 완료되지 않은 배치부터 재개할 수 있다. 재시도는 횟수와 대기 시간을 제한하고, 인증/스키마/해시 충돌 같은 영구 오류는 자동 재시도하지 않는다. Service 계약과 전송 기술은 실제 Service 구성 확인 후 확정한다.

#### Oracle 적재와 공개 시점

`MAP_LAYER`를 먼저, `MAP_SOURCE_ELEMENT`를 다음, 참조 FK가 있는 `MAP_PLACED_ELEMENT`를 마지막에 적재한다. Service는 배치별로 크기를 제한해 적재하고 Commit하므로 큰 Map 전체를 하나의 장시간 트랜잭션으로 묶지 않는다. 기존 `OracleBulkCopy`의 BLOB 적재/트랜잭션 동작은 사용하는 Oracle 드라이버 버전과 대표 도형 배치로 확인한다. 맞지 않으면 BLOB 포함 행에 대한 배열 바인딩이나 별도 저장 방식을 측정해 선택한다. 행별 INSERT와 무제한 `DataTable` 누적은 기본 경로로 두지 않는다.

적재 중 Revision은 `DRAFT`이고 일반 Device 조회/Defect 분석 대상에서 제외한다. 완료 요청 때 배치 번호 누락, 예상 원본/배치/좌표 수 불일치, 중복 ID, 고아 Source/Layer 참조, 좌표 바이너리 길이 불일치를 확인한다. 이 검사가 모두 끝난 뒤 `MAP_IMPORT_JOB=READY`와 `DEVICE_MAP_REVISION=READY`를 한 트랜잭션에서 전환한다. 실패/취소 Revision은 조회에서 숨기고 같은 Job 재개 또는 명시적인 정리 절차로 처리한다. 정상 사용 중인 이전 READY Revision은 새 업로드 실패의 영향을 받지 않는다. Chain 저장은 READY MapRevisionId를 참조하도록 제한한다.

기존 Grid의 파서 중복 제외 행은 현재 `SourceElementId`가 비어 있다. 원본 전체를 저장하려면 이 행에도 충돌하지 않는 별도 Source ID를 부여하고, 실제 배치가 없는 원본임을 표시해야 한다. 이 처리가 끝나기 전에는 원본 건수 검증을 완료했다고 보지 않는다.

#### DB 재조회 성능과 측정 기준

Device 조회는 READY Revision의 메타데이터를 먼저 읽고, Layer/도형을 고정된 Revision에서 일정한 순서로 페이지 또는 스트림으로 가져온다. 화면에 보이는 영역을 먼저 그리는 최적화를 나중에 추가할 수 있지만, 일부만 읽은 상태를 전체 Map 로딩 완료나 Chain 분석 완료로 표시하지 않는다. 배치 수신/좌표 복원/OpenGL 버퍼 생성의 메모리 상한을 각각 측정한다. Chain/Defect 조회도 같은 MapRevisionId를 사용해 다른 개정본의 도형과 섞이지 않게 한다.

진행률은 `전송 대기`가 아니라 **Service가 Commit을 확인한 도형 수/바이트 수**로 표시한다. 측정값은 도형 추출, 직렬화, 전송 대기, 네트워크, Service 검증, DB 적재/Commit, DB 재조회, GPU 구성으로 나눠 수집한다. 대표 Device에서 전체 소요 시간/초당 도형 수/피크 메모리/배치 p50/p95 시간/재시도 후 중복 여부를 비교해 배치 크기와 동시 전송 수를 결정한다. 이 절은 설계이며 Service/DB 실행 성능은 아직 측정하지 않았다. 화면 진행률 컨트롤을 추가할 때는 WinForms Designer에 배치한다.

### 17.42 2026-09-29 / 제한된 Map 도형 배치 생성 1차 구현

`MapPlacedElementBatchBuilder`는 한 배치의 도형 수와 예상 비압축 바이트 수를 함께 제한한다. 현재 기본값은 2,000도형/8MiB다. 각 배치에 추가할 때 해당 도형의 좌표만 복사하며, 하나의 도형이 바이트 한도를 넘으면 조용히 잘라내지 않고 예외를 낸다. 바이트 값은 좌표 수와 문자열/고정 메타데이터를 이용한 **예상치**라서 실제 Service 요청 크기는 직렬화한 뒤 다시 제한해야 한다.

`GdsMapControl.VisitPlacedElementBatchesAsync`는 Map의 도형을 UI 스레드에서 한 배치씩 읽고 비동기 소비자가 끝날 때까지 다음 배치를 만들지 않는다. 각 배치 뒤에는 UI 메시지 처리를 양보한다. GDS를 새로 열면 Map 버전이 바뀌고 진행 중인 소비자가 돌아온 후 다음 배치 전에 추출을 중단한다. CancellationToken으로 사용자가 중단할 수도 있다. 비동기 소비자 안에서 네트워크/직렬화를 동기식으로 오래 실행하면 여전히 UI가 멈출 수 있으므로 실제 Service 연결 때는 직렬화 작업과 비동기 I/O를 UI 스레드 밖에서 처리한다. 현재 한 번에 보유하는 비동기 전송 배치는 1개이며, 17.41절의 2~3개 대기열은 성능 측정 후 필요한 경우 추가한다.

Debug 빌드와 새 배치 Probe가 통과했다. Probe는 도형 수/바이트 제한, 배치 뒤 다음 도형 추가, 좌표 배열 복사, 한 도형 초과 거부를 확인한다. 이 결과는 화면 반응성, 실제 요청 크기, Service 전송, Oracle 적재 시간을 검증하지 않는다. 다음 구현은 원본 Grid Element의 직렬화와 Device/Revision 업로드 계약 및 실제 전송 시간 측정이다.

### 17.43 2026-09-29 / PLACED 배치 본문 직렬화 1차 구현

`MapPlacedBatchPayload.Create`는 17.42절에서 만든 도형 배치를 Service 업로드용 **바이너리 본문**으로 바꾼다. 요청 메타데이터는 `JobId / MapRevisionId / StreamKind=PLACED / BatchNo / ElementCount / PointCount / PayloadSha256`이다. 본문 형식 v1은 `GMP1` 4바이트, 형식 버전(Int32), 배치 번호(Int32), 도형 수(Int32), 좌표 수(Int64) 뒤에 각 도형의 배치 ID/원본 ID/Layer/DataType/종류/닫힘 여부/PATH 폭/Bounds/TEXT 속성/좌표 개수/X-Y double 쌍을 기록한다. 숫자와 좌표는 little-endian, 문자열은 null을 `-1` 길이로 구분하는 UTF-8 길이+본문이다. `MAP_IMPORT_BATCH.FORMAT_VERSION`으로 수신 형식 버전을 기록한다.

SHA-256은 압축/인코딩 전의 실제 바이너리 본문 바이트를 대상으로 한다. 본문을 만든 뒤 **실제 Payload.Length**가 Service 허용값보다 크면 요청을 만들지 않고 오류로 돌린다. 17.42절의 예상 바이트 크기는 배치 분할을 위한 값이고 최종 제한값으로 쓰지 않는다. 현재 코드는 바이너리 본문을 메모리에 완성한 뒤 해시를 계산하므로 전송 배치 크기만큼 추가 메모리가 필요하다. 대표 Device에서 피크 메모리를 측정하고 필요하면 스트리밍 해시/전송으로 바꾼다. HTTP JSON/Base64에 본문을 넣으면 크기가 늘어나므로 이 형식은 별도 바이너리 본문 전송을 전제로 한다.

Debug 빌드와 `MapPlacedBatchPayloadProbe`가 통과했다. Probe는 형식 식별자/버전/건수, 한글 TEXT, 좌표, 재직렬화 해시 일치, 실제 크기 초과 거부를 검사한다. Service 수신기/DB 저장/재조회는 아직 없으므로 네트워크 왕복 복원을 증명하지 않는다. 다음 단위는 Service 수신 계약과 원본 Grid/LAYER 데이터 직렬화이며, 그 뒤 Device 기반 저장과 DB 단독 Map 재조회를 연결한다.

### 17.44 2026-09-29 / PLACED 배치 본문 복원 1차 구현

Service와 SQL 구현은 뒤로 미루고, 클라이언트 안에서 `MapPlacedBatchReader.Read`를 추가했다. 저장용 본문 한 배치를 읽어 `MapPlacedElementBatch`와 도형 데이터로 복원한다. 본문 최대 크기, SHA-256, 형식 식별자/버전, 배치 번호/도형 수/좌표 수, 문자열 길이, 좌표의 유한성, 중복 배치 Element ID, 끝까지 읽었는지를 검사한다. 본문에 들어 있던 `double` 좌표는 `GPoint` 생성자의 반올림을 거치지 않고 그대로 복원한다.

Debug 빌드와 `MapPlacedBatchPayloadProbe`가 통과했다. Probe는 직렬화 후 복원한 ID/TEXT/소수 좌표를 비교하고, 바이트 변조/미지원 버전/잘린 본문/중복 ID 거부를 확인한다. 현재 검증은 로컬 바이트 왕복에 한정된다. Service 전송, DB 재조회, 복원 데이터를 이용한 Map 화면 재구성은 아직 구현하거나 확인하지 않았다. 다음 클라이언트 작업은 복원한 PLACED 데이터와 Layer/Chain 정보를 Map 화면에 연결하는 경계를 설계하고 작은 단위로 구현하는 것이다.
