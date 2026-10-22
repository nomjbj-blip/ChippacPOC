# 다중 파일 E-Test 결과 업로드 (참고 소스, 2026-10-15)

POC 업로드는 2026-10-15 부터 **파일 하나씩** 올린다 (사용자 지시: "Upload는 단일 파일 즉, 하나씩만 올리는 컨셉으로 한다. 멀티는 본프로젝트 할때").
이 폴더는 그 직전의 다중 파일 버전(대기 목록 + 백그라운드 분석 + 모두 저장 / 선택 저장)을 본 프로젝트에서 다시 쓰려고 남긴 사본이다. 빌드에 포함되지 않는다.

| 파일 | 원래 위치 |
| --- | --- |
| `frmEtResultUpload.cs` / `.Designer.cs` | `DMS/02_DMS/DACrux.SEMDMS.ENGUI/ETest/` |
| `EtResultUploadFormSmoke.cs` | `DMS/02_DMS/DACrux.SEMDMS.ENGUI/Verification/` (업로드 + 조회 화면 Smoke, 34 검사 PASS 상태) |

다중 파일 버전에만 있던 것:

- [파일 추가] (여러 개) / [폴더 추가] (xlsx 전부) / [목록 비우기] / Del 로 행 빼기
- 대기 목록 18열 (상태 / Device / 공정 / Lot / Slot(편집) / WAFER_ID / 헤더 / 등록 / 저장 / 누락 / Spec Fail Die / Map / 메시지 / 파일 번호)
- BackgroundWorker 순차 분석 (입력은 막지 않음), 등록 Parameter / Map 캐시
- 목록 안 중복 (같은 WAFER_ID + 공정) 오류
- [모두 저장] / [선택 저장], 저장 확인 문구 = 파일별 Parameter x Die = 값 (10줄) + 합계, 중복은 예 = 덮어쓰기 / 아니요 = 건너뛰기

공통 부분(EtResultData / EtResultDataControl / EtUploadPlan / 서버 저장 4단계)은 단일 파일 버전과 같으므로 여기 두지 않는다.
설계: [Upload / View 분리 설계](../2026-10-15_ETest_Upload_View_분리_설계.md) 12절.
