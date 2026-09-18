# ChipPac POC

스테츠칩팩 POC의 DMS와 GDS Map Control 소스를 관리하는 저장소이다.

## 포함 대상

- `DMS`: DMS 관련 C# 소스, 솔루션, 프로젝트 파일, DB Query XML
- `GdsMapControl`: GDSII 파일 파싱과 OpenGL 기반 도면 표시 WinForms 프로젝트

## 제외 대상

- Visual Studio 사용자 설정과 빌드 산출물
- 실행 파일, DLL, PDB, NuGet 복원 파일
- 로그와 검사 백업 데이터
- 압축 파일과 임시 파일

필요한 외부 라이브러리는 각 프로젝트의 프로젝트 참조 및 NuGet 설정을 기준으로 별도 복원한다.
