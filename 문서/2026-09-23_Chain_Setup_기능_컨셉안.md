# Chain Setup 기능 컨셉안

- 작성일: 2026-09-23
- 대상: 스테츠칩팩 AOI Defect / E-Test Correlation POC
- 문서 목적: GDS 화면에서 Input부터 Output까지의 전류 연결 경로를 Chain으로 설정하고, 추후 AOI Defect와 ET Fail의 연관관계를 분석하기 위한 기능 개념을 정리한다.
- 문서 상태: 컨셉안 / 실제 GDS와 고객 기준정보 확인 필요

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
