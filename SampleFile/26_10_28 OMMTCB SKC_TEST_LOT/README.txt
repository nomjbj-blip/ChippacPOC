OMMTCB 연관 분석 시험 Data (2026-10-28)

26_10_06 Sample File(XD106.02 / Defect 이미지 15장 / E-Test RH-HS2615-02)을 OMMTCB GDS Map 기준으로 바꾼 것.
값(좌표 / 크기 / 분류 / 측정값)은 원본 그대로이고 이름만 바꿨다.

1. KLARF: SKC_TEST_LOT_01
   - DeviceID OMMTCB / SetupID ROOT-CAI-OMMTCB-OMMTCB_C4Bump_Silterra / StepID OMMTCB_C4Bump_Silterra
   - LotID PFIC_SKC_TEST_LOT_C4Bump / WaferID SKC_TEST_LOT_01 / Slot 1
   - LotID 앞뒤 조각(PFIC_ / _C4Bump)은 Wafer Key 규칙(LOT_ID 의 '_' 가운데 조각 + Slot)에서
     Lot 핵심이 SKC_TEST_LOT 이 되게 붙였다 -> Wafer Key SKC_TEST_LOT-01
   - TiffFileName 15개와 Defect Images 파일 이름은 XD106.02_ 를 SKC_TEST_LOT_01_ 로 바꿨다.

2. E-Test: CAI_OMMTCB_ATV_A0_TSVET_20260609161940_SKC-TEST-LOT_SKC-TEST-LOT-01.xlsx
   - 내용은 CAI_OMM / CAI_U9 파일과 같다(Device 는 파일 이름에서 읽음).
   - 파일 이름은 '_' 8칸 규칙 때문에 Lot 을 '-' 로 썼다. 업로드 화면 정보 표에서 아래처럼 고친 뒤 저장한다.
     Lot = SKC_TEST_LOT / Lot-Wafer No = SKC_TEST_LOT_01 / Slot No = 1

3. 업로드 순서 (DB 에 넣을 때)
   - KLARF 업로드 화면: SKC_TEST_LOT_01 + Defect Images 15장 (Product 빈 값 = DeviceID OMMTCB)
   - E-Test 결과 업로드 화면: 위 xlsx, 정보 표의 Lot / Lot-Wafer No / Slot No 수정 후 저장
     (OMMTCB / TSVET 등록 Parameter 85개가 있어야 한다)

4. 로컬 DB(DMSMGR) 적재 결과 (2026-10-28)
   - KLARF STEP_SEQ 641 / WAFER_SEQ 541 / WAFER_ID PFIC_SKC_TEST_LOT_C4BUMP-01 / Defect 15 / 이미지 15
     이미지 저장소 D:\FTP\SCK\20261028\OMMTCB\PFIC_SKC_TEST_LOT_C4BUMP
   - E-Test SRC_FILE_SEQ 421 / WAFER_ID SKC_TEST_LOT-01 / Die 24 (Fail 1) / 값 2,040 / Map OMMTCB / YMS DONE
   - 연관 분석 Ready: GDS OMMTCB READY Chain 19 / E-Test 421 자동 선택 / Chain 이름 연결 17 / Map 형상 같음
   - 분석(Defect 크기 그대로): 허용오차 0 -> Chain 위 Defect 7 / 15, 허용오차 5 -> 8 / 15, Defect Die 4
