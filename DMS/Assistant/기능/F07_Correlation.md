# F07 / AOI / E-Test / GDS Correlation

확인일: 2026-10-02 / 상태: 기존 요구 검토 문서와 선행 기능 기준 / 통합 실행 미확인

## 목적과 근거

AOI KLARF의 Defect 위치가 GDS Chain을 구성하는 Bump / RDL 영역과 관련되는지 확인하고 E-Test Fail Chain과 연결한다.

근거는 [9/30 문서 검토 종합](../../../문서/2026-09-30_문서검토_종합.md)과 [Chain Setup 컨셉안](../../../문서/2026-09-23_Chain_Setup_기능_컨셉안.md)이다. 이번 정리에서는 고객 원본 PDF / XLSX를 새로 분석하지 않았다. 확정 고객 요구로 인용할 때 원본과 최신 협의 내용을 다시 확인한다.

## 현재 구분

| 항목 | 현재 확인 범위 |
| --- | --- |
| GDS 도면 / Chain | Viewer와 메모리 기반 Chain 편집 코드 존재 |
| AOI 파일 확인 | KLARF 파일 조회 화면 코드 존재 |
| 영구 Map / Chain | 배치 기반과 스키마 존재 / 전체 왕복 저장 미완료 |
| AOI -> GDS 좌표 변환 | 원점 / 방향 / 미러 / 회전 / 단위 / 허용오차 계약 확인 필요 |
| E-Test -> Chain | 고객 ET 항목명과 Chain Code 연결 키 확인 필요 |
| 최종 판정 / 통합 화면 | 이번 조사 범위에서 통합 완료 근거 없음 |

## 이어갈 때 지켜야 할 기준

- 고객 검토 문서의 판정 사례 단위는 `(Die, Fail Chain)`이다. Die 전체 하나의 Yes/No로 단순화하지 않는다.
- GDS Layer와 Chain은 같은 개념이 아니다. 하나의 Defect와 여러 Chain의 관계가 생길 수 있다.
- 사각 영역 / Bounding Box 비교와 정밀 Polygon 교차는 서로 다르다. 사용할 판정 방식과 Tolerance를 고객 샘플로 확정한다.
- KLARF의 절대 좌표 / Die 상대 좌표 / Die Index를 구분한다. 화면에서 맞아 보인다는 이유로 미러 / 회전을 자동 적용하지 않는다.
- 비교 결과에는 어떤 Map Revision / Chain Version과 좌표 변환 기준을 사용했는지 남겨야 한다.

## 다음 단계

동일 Device / Wafer / Die의 KLARF, GDS, E-Test 샘플을 기준으로 좌표와 Chain 매핑 표부터 만든다. 확정되지 않은 부분은 추정 / 확인 필요로 표시한다. 그 뒤 한 개 Die / Chain 사례를 왕복 검증하고 범위를 넓힌다.


## 2026-10-02 사용자 결정

KLARF Defect와의 연관분석은 Chain 영역(현재 Chain Version의 Member 도형)만 대상으로 한다. 배경 도면 도형은 비교하지 않는다. 1차 후보는 Defect 좌표 +/- Tolerance와 Member 도형 Bounds 겹침. 상세는 [GDS Map DB 저장 설계 13.3절](../../../문서/2026-10-02_GDS_Map_DB저장_설계.md).


## 2026-10-07 E-Test 결과 파일 설계 (CLAUDE-029)

샘플 `SampleFile/26_10_06 Sample File/CAI_OMM_ATV_A0_TSVET_..._RH-HS2615-02.xlsx` 분석 결과 헤더 형식은 `{Para}[_{조건}={값}]@{Chain}[:{Chain2}]` (Leakage_Serial 은 Chain 2개). 파일 Limit 은 쓰지 않고 별도 Limit 테이블 / 화면으로 판정한다 (Connectivity / Resistance 는 파일 Limit 이 N/A 라 Open 판정에 Limit 화면 필수). E-Test Chain 이름(최대 19자)과 GDS `CHAIN_CODE` 는 FK 없이 이름으로 대응. 상세와 결정 대기 항목 D1~D8은 [E-Test Parsing / DB 설계](../../../문서/2026-10-07_ETest_결과파일_Parsing_DB설계.md). 설계만 됨 / 구현 없음.

KLARF `XD106.02` 연결 분석 (같은 날): Wafer 는 (Lot 핵심 문자열, Slot) 으로 대응, Die 는 시계 90도 + 이동 (4, 5) 잠정 (Defect Die 4 중 3 이 E-Test Fail Die). 설계 문서 8절, 연구소 좌표 대응표로 확정 필요.

범위 조정 (같은 날 사용자 지시): 먼저 파일 간 기준 정보 관계만 설계 -> [파일 간 기준 정보 관계 설계](../../../문서/2026-10-07_파일간_기준정보_관계_설계.md). 연결 규칙 / Die 좌표 / 결과 Data 는 나중.

E-Test 결과 저장 구현 (CLAUDE-029, 2026-10-07): 테이블 `TQP_GDS_SRC_FILE` / `TQP_GDS_ET_ITEM` / `TQP_GDS_ET_DIE` / `TQP_GDS_ET_VALUE` (로컬 DB 적용), 서비스 `GdsEtResult`, 화면 DMS > E-Test 결과 업로드 (`ENGUI/ETest/frmEtResultUpload`). 13:11 배포, Probe 16/16 / Smoke 17 PASS, DMS 화면 확인 대기. Limit 판정 / KLARF 연결은 나중.

E-Test 중복 기준 변경 (같은 날 사용자 지시): Device 중복 허용, UNIQUE = FACTORY + WAFER_ID, Slot No 는 업로드 화면에서 입력 (설계 12절). 로컬 DB 적용, 배포 승인 대기.
