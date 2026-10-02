# F06 / KLARF 파일 조회

확인일: 2026-10-02 / 상태: 소스 확인 / 실제 고객 샘플 화면은 미검증

## 목적과 파일

AOI 데이터를 GDS와 비교하기 전에 KLARF 자체의 Wafer / 검사 Die / Defect 좌표를 DB 없이 확인한다.

- [frmKlarfFileViewer.cs](../../02_DMS/DACrux.SEMDMS.ENGUI/DefectMapAnalysis/frmKlarfFileViewer.cs): 파일 열기 / Wafer 선택 / 색상 / Grid / 마우스 정보
- [frmKlarfFileViewer.Designer.cs](../../02_DMS/DACrux.SEMDMS.ENGUI/DefectMapAnalysis/frmKlarfFileViewer.Designer.cs): 화면 배치
- [KlarfDefectMap.cs](../../02_DMS/DACrux.SEMDMS.ENGUI/DefectMapAnalysis/KlarfDefectMap.cs): 기존 DefectMap 상속 / Die 번호 / 좌표 이벤트
- [Verify-KlarfFileViewer.ps1](../../02_DMS/DACrux.SEMDMS.ENGUI/Verification/Verify-KlarfFileViewer.ps1) / `KlarfViewerFormSmoke.cs` / 합성 샘플 `XD074_synthetic.000`: TEMP 별도 빌드 + 화면 밖 Form 검사
- 메뉴: `MNU_KLARF_FILE_VIEW` / KLARF 파일 조회 / `DACrux.SEMDMS.ENGUI.frmKlarfFileViewer`
- 이 화면 때문에 SEMDMS.ENGUI csproj에 `06_CoreLibSource/DACrux.Data.Parser` 프로젝트 참조가 추가되었다(미커밋). 공통 `ParserBase.GetDictionaryWithLength` 변경도 함께 있으며 다른 Parser에 영향을 줄 수 있다. [작업 현황](../작업_현황.md)의 확인 필요 표 참고.

예전 기록(Claude 메모)의 `09_TEST/.../Analysis/frmKlarfFileViewer.cs` 경로는 현재 없다. 현재 위치는 위의 SEMDMS.ENGUI다.

## 현재 로직

1. `FileAnalyzer`로 버전을 판단해 `ParserKlarf` 또는 `ParserKlarf_18`을 사용한다.
2. Parser의 `ErrorFlag` / `ErrorMessage`, Wafer 개수, DiePitch와 SampleSize를 검사한다.
3. 기존 DefectMap 흐름으로 Wafer와 검사 Die / Defect를 표시한다. 색상 기준 선택과 Grid 선택 연계 코드가 있다.
4. 마우스 위치에 Wafer 좌표 / KLARF 좌표 / Die Index / Die 상대 위치 / 가까운 Defect 정보를 표시한다.
5. 기존 Parser가 WaferID를 DMS 규칙으로 변경하기 때문에 파일 원본 WaferID를 별도로 읽는다. `ReadRawWaferIds`는 KLARF 1.2의 WaferID 다음 Slot 형식에 의존한다. 다른 형식의 원본 ID 표시를 보장하지 않는다.

이 화면은 DB 저장이나 Service 호출을 하지 않는다. 원본 파일 표시를 GDS 좌표 정합 또는 Correlation 완료로 취급하지 않는다.

## 정상 동작 확인과 다음 단계

- 샘플의 버전 / WaferID / Slot / DiePitch / SampleSize와 화면 값을 대조한다.
- 검사 Die 수 / Defect 수 / 양수와 음수 좌표 / Grid 선택과 Map 위치를 확인한다.
- 1.2 / 1.8의 원본 WaferID 처리 차이를 확인한다.
- 좌표 단위와 DieOrigin / 방향을 확인한 뒤 [F07](F07_Correlation.md)의 변환 계약으로 이어간다.

이번에는 화면을 실행하지 않았다. 기존 공통 Parser 파일에 미커밋 변경이 있으므로 후속 작업자는 해당 차이를 확인하되 이 화면 작업이라는 이유로 되돌리지 않는다.
