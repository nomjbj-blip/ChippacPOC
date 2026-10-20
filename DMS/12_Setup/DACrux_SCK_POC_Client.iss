; DACrux SCK POC 클라이언트 설치 스크립트 (Inno Setup 7)
;
; 직접 컴파일하지 않고 Build-ClientSetup.ps1 이 ISCC 에 아래 값을 넘겨 만든다.
;   /DAppVersion=2026.10.14.1   설치 버전 (yyyy.M.d.N)
;   /DStageDir=<폴더>           설치할 파일 (빌드 결과 + 설치 점검 도구 + 버전정보.txt / 파일목록.txt)
;   /DAppExe=<파일 이름>        설치 폴더의 실행 파일 이름. 빌드 결과 DACruxV5.exe 를 stage 에서 이 이름으로 바꿔 둔다
;   /DOutputDir=<폴더>          Setup 파일을 만들 폴더
;   /DDefaultServerIP=<주소>    서버 주소 입력 칸의 기본값
;   /DDotNetInstaller=<파일>    .NET Framework 4.8 오프라인 설치 파일 (넣을 때만)
;   /DIconFile=<파일>           Setup 아이콘 (선택)
;   /DTestMode                  자동 검사용. 다른 AppId / 폴더 / 이름으로 만들어 실제 설치와 섞이지 않게 한다
;
; 설치 흐름
;   1. 시작: .NET 4.8 확인 (설치 파일을 넣지 않은 Setup 이면 여기서 안내 후 중단)
;   2. 화면: 시작 -> 서버 주소 -> 설치 폴더 -> 바로 가기 -> 준비 요약
;   3. PrepareToInstall: 실행 중 확인 -> .NET 4.8 설치(없을 때만, 관리자 확인) -> 기존 버전 무음 삭제 -> 지난 파일 남음 확인
;      .NET 설치를 기존 버전 삭제보다 먼저 해서 .NET 설치가 실패해도 쓰던 프로그램이 남게 한다.
;   4. 파일 복사 -> DACruxConfig.ini 값 기록 -> 바로 가기 -> 설치 점검(SCK_EnvCheck.exe)
;
; 설치 위치는 사용자 폴더(%LOCALAPPDATA%\Programs)다. DACrux 는 로그인할 때마다 실행 폴더에 DACruxConfig.ini 를
; 다시 쓰므로 일반 사용자가 쓸 수 없는 Program Files 에는 설치하지 않는다.

#ifndef AppVersion
  #error AppVersion 이 없습니다. Build-ClientSetup.ps1 로 빌드하세요.
#endif
#ifndef StageDir
  #error StageDir 이 없습니다. Build-ClientSetup.ps1 로 빌드하세요.
#endif
#ifndef OutputDir
  #define OutputDir "Output"
#endif
#ifndef DefaultServerIP
  #define DefaultServerIP "LOCALHOST"
#endif
#ifndef AppExe
  #define AppExe "DACrux SCK POC.exe"
#endif

#ifdef TestMode
  #define AppGuid "8258B0F2-110A-49EC-AA22-52F18492815A"
  #define AppName "DACrux SCK POC (시험)"
  #define AppDirName "DACrux_SCK_POC_Test"
  #define SetupBaseName "DACrux_SCK_POC_Client_Setup_TEST_" + AppVersion
#else
  #define AppGuid "9080986A-3C09-4A8F-8F77-3987A1B92EFE"
  #define AppName "DACrux SCK POC"
  #define AppDirName "DACrux_SCK_POC"
  #define SetupBaseName "DACrux_SCK_POC_Client_Setup_" + AppVersion
#endif

; [Setup] 의 AppId 는 { 를 {{ 로 써야 하고, 코드에서 쓰는 등록 키 이름은 { 하나다.
#define AppIdValue "{{" + AppGuid + "}"
#define AppIdBraced "{" + AppGuid + "}"

#ifdef DotNetInstaller
  #define DotNetFileName ExtractFileName(DotNetInstaller)
#endif

[Setup]
AppId={#AppIdValue}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher=Miracom
VersionInfoVersion={#AppVersion}
VersionInfoProductName={#AppName}
VersionInfoDescription={#AppName} 클라이언트 설치
DefaultDirName={userpf}\{#AppDirName}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
DisableDirPage=no
; 예전 이름(SCK_ETest_POC) 폴더에 설치되어 있어도 새 폴더로 설치한다. 기존 버전은 PrepareToInstall 에서 삭제한다.
UsePreviousAppDir=no
PrivilegesRequired=lowest
MinVersion=10.0
OutputDir={#OutputDir}
OutputBaseFilename={#SetupBaseName}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
ShowLanguageDialog=no
CloseApplications=yes
RestartApplications=no
SetupLogging=yes
UninstallDisplayName={#AppName}
UninstallDisplayIcon={app}\{#AppExe}
#ifdef IconFile
SetupIconFile={#IconFile}
#endif

[Languages]
Name: "korean"; MessagesFile: "compiler:Languages\Korean.isl"

[Tasks]
Name: "desktopicon"; Description: "바탕 화면에 바로 가기 만들기"; GroupDescription: "바로 가기:"

[Dirs]
Name: "{app}\DEFECT_IMAGE"

[Files]
; DACruxConfig.ini 는 따로 설치하고 바로 서버 주소 등을 기록한다(AfterInstall).
; [Run] 의 설치 점검이 ssPostInstall 보다 먼저 실행되므로 그 전에 ini 가 완성되어 있어야 한다.
Source: "{#StageDir}\*"; DestDir: "{app}"; Excludes: "\DACruxConfig.ini"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#StageDir}\DACruxConfig.ini"; DestDir: "{app}"; Flags: ignoreversion; AfterInstall: WriteConfig
#ifdef DotNetInstaller
; 이미 압축된 Microsoft 설치 파일이라 다시 압축하지 않는다. 필요할 때만 임시 폴더에 꺼낸다.
Source: "{#DotNetInstaller}"; Flags: dontcopy nocompression
#endif

[Icons]
Name: "{autoprograms}\{#AppName}\{#AppName}"; Filename: "{app}\{#AppExe}"; WorkingDir: "{app}"
Name: "{autoprograms}\{#AppName}\{#AppName} 설치 점검"; Filename: "{app}\SCK_EnvCheck.exe"; WorkingDir: "{app}"
Name: "{autoprograms}\{#AppName}\{#AppName} 삭제"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExe}"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\SCK_EnvCheck.exe"; Parameters: "/setup"; WorkingDir: "{app}"; StatusMsg: "설치 환경을 점검하고 있습니다..."; Flags: waituntilterminated; Check: not WizardSilent
Filename: "{app}\SCK_EnvCheck.exe"; Parameters: "/silent"; WorkingDir: "{app}"; StatusMsg: "설치 환경을 점검하고 있습니다..."; Flags: waituntilterminated runhidden; Check: WizardSilent
Filename: "{app}\{#AppExe}"; WorkingDir: "{app}"; Description: "{#AppName} 실행"; Flags: postinstall nowait skipifsilent; Check: not DotNetRestartNeeded

[UninstallDelete]
; 실행 중 DACrux / 점검 도구가 만드는 임시 파일. 사용자가 만든 그 밖의 파일은 지우지 않는다.
Type: filesandordirs; Name: "{app}\DEFECT_IMAGE"
Type: filesandordirs; Name: "{app}\AVI"
Type: filesandordirs; Name: "{app}\Temp"
Type: files; Name: "{app}\excel_temp*.xls"
Type: files; Name: "{app}\temp_chart*.bmp"
Type: files; Name: "{app}\tmpMap_*.bmp"
Type: files; Name: "{app}\MenuOption.xml"
Type: files; Name: "{app}\설치점검_결과.txt"
Type: files; Name: "{app}\~envcheck_write.tmp"
Type: dirifempty; Name: "{app}"

[Code]
const
  UninstallKey = 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{#AppIdBraced}_is1';
  NetFxKey = 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full';
  DotNet48Release = 528040;
  ConfigFileName = 'DACruxConfig.ini';
  FileListName = '파일목록.txt';
  AppExeName = '{#AppExe}';
  // 2026-10-14 이전 설치본의 실행 파일 이름. 기존 버전이 실행 중인지 확인할 때 같이 본다.
  OldExeName = 'DACruxV5.exe';

var
  ServerPage: TInputQueryWizardPage;
  PrevInstalled: Boolean;
  PrevVersion: String;
  PrevDir: String;
  PrevUninstaller: String;
  PrevServerIP: String;
  PrevUserID: String;
  PrevLanguage: String;
  PrevFileList: TArrayOfString;
  DotNetNeeded: Boolean;
  DotNetRestartFlag: Boolean;

// ---- 공통 ------------------------------------------------------------------

// .NET Framework 4.8 이상인지 (Release 값). 32비트 Setup 이라도 64비트 Windows 는 64비트 레지스트리를 본다.
function IsDotNet48Installed: Boolean;
var
  Release: Cardinal;
  Found: Boolean;
begin
  if IsWin64 then
    Found := RegQueryDWordValue(HKLM64, NetFxKey, 'Release', Release)
  else
    Found := RegQueryDWordValue(HKLM, NetFxKey, 'Release', Release);
  Result := Found and (Release >= DotNet48Release);
end;

// DACruxConfig.ini 의 [LAST OPTION] 값을 DACrux 와 같은 규칙("키 = 값", 앞뒤 공백 제거)으로 읽는다.
function ReadLastOption(const IniPath, Key: String): String;
var
  Lines: TArrayOfString;
  I, Eq: Integer;
  Line, Section: String;
begin
  Result := '';
  if not LoadStringsFromFile(IniPath, Lines) then Exit;
  Section := '';
  for I := 0 to GetArrayLength(Lines) - 1 do
  begin
    Line := Trim(Lines[I]);
    if (Line = '') or (Copy(Line, 1, 1) = '#') then Continue;
    if Copy(Line, 1, 1) = '[' then
    begin
      Section := Uppercase(Line);
      Continue;
    end;
    Eq := Pos('=', Line);
    if (Section = '[LAST OPTION]') and (Eq > 1) and (CompareText(Trim(Copy(Line, 1, Eq - 1)), Key) = 0) then
    begin
      Result := Trim(Copy(Line, Eq + 1, Length(Line)));
      Exit;
    end;
  end;
end;

// [LAST OPTION] 의 Key 줄을 "Key = Value" 로 바꾼다. 없으면 파일 끝(= [LAST OPTION] 섹션 끝)에 붙인다.
procedure SetLastOption(var Lines: TArrayOfString; const Key, Value: String);
var
  I, Eq, Count: Integer;
  Line, Section: String;
begin
  Section := '';
  Count := GetArrayLength(Lines);
  for I := 0 to Count - 1 do
  begin
    Line := Trim(Lines[I]);
    if Copy(Line, 1, 1) = '[' then
    begin
      Section := Uppercase(Line);
      Continue;
    end;
    Eq := Pos('=', Line);
    if (Section = '[LAST OPTION]') and (Eq > 1) and (Copy(Line, 1, 1) <> '#') and (CompareText(Trim(Copy(Line, 1, Eq - 1)), Key) = 0) then
    begin
      Lines[I] := Key + ' = ' + Value;
      Exit;
    end;
  end;
  SetArrayLength(Lines, Count + 1);
  Lines[Count] := Key + ' = ' + Value;
end;

// Dir 아래에서 실행 중인 DACrux(지금 / 예전 실행 파일 이름)가 있는지 (개발 PC 의 다른 폴더 DACrux 는 무시한다).
function IsAppRunningIn(const Dir: String): Boolean;
var
  Locator, Service, Items, Item: Variant;
  I: Integer;
  ExePath, Prefix: String;
begin
  Result := False;
  if Dir = '' then Exit;
  Prefix := Lowercase(AddBackslash(Dir));
  try
    Locator := CreateOleObject('WbemScripting.SWbemLocator');
    Service := Locator.ConnectServer('.', 'root\CIMV2');
    Items := Service.ExecQuery('SELECT ExecutablePath FROM Win32_Process WHERE Name = ''' + AppExeName + ''' OR Name = ''' + OldExeName + '''');
    for I := 0 to Items.Count - 1 do
    begin
      Item := Items.ItemIndex(I);
      if not VarIsNull(Item.ExecutablePath) then
      begin
        ExePath := Lowercase(Item.ExecutablePath);
        if Copy(ExePath, 1, Length(Prefix)) = Prefix then
        begin
          Result := True;
          Exit;
        end;
      end;
    end;
  except
    Log('실행 중 확인 실패: ' + GetExceptionMessage);
  end;
end;

// 기존 설치 정보와 보관할 설정값을 읽는다. 기존 ini 는 삭제 전에 읽어야 한다.
procedure ReadPreviousInstall;
var
  IniPath: String;
begin
  PrevInstalled := RegQueryStringValue(HKCU, UninstallKey, 'UninstallString', PrevUninstaller);
  if not PrevInstalled then Exit;

  RegQueryStringValue(HKCU, UninstallKey, 'DisplayVersion', PrevVersion);
  RegQueryStringValue(HKCU, UninstallKey, 'InstallLocation', PrevDir);
  PrevDir := RemoveBackslashUnlessRoot(PrevDir);
  IniPath := AddBackslash(PrevDir) + ConfigFileName;
  PrevServerIP := ReadLastOption(IniPath, 'ServerIP');
  PrevUserID := ReadLastOption(IniPath, 'UserID');
  PrevLanguage := ReadLastOption(IniPath, 'Language');
  if not LoadStringsFromFile(AddBackslash(PrevDir) + FileListName, PrevFileList) then SetArrayLength(PrevFileList, 0);
  Log(Format('기존 설치: 버전 %s / 폴더 %s / 서버 %s', [PrevVersion, PrevDir, PrevServerIP]));
end;

// 서버 주소 기본값: 명령줄 /SERVERIP= > 기존 설치 값 > 빌드 기본값
function DefaultServer: String;
begin
  Result := Trim(ExpandConstant('{param:SERVERIP|}'));
  if Result = '' then Result := PrevServerIP;
  if Result = '' then Result := '{#DefaultServerIP}';
end;

function DotNetRestartNeeded: Boolean;
begin
  Result := DotNetRestartFlag;
end;

// ---- 설치 전 단계 ------------------------------------------------------------

#ifdef DotNetInstaller
// 포함된 .NET 4.8 설치 파일을 관리자 권한으로 실행한다. 성공하면 '', 실패하면 화면에 보일 이유.
function InstallDotNet: String;
var
  Installer, LogFile, Params: String;
  Code: Integer;
begin
  Result := '';
  ExtractTemporaryFile('{#DotNetFileName}');
  Installer := ExpandConstant('{tmp}\{#DotNetFileName}');
  LogFile := ExpandConstant('{%TEMP}\DACrux_SCK_POC_dotnet48_install.log');
  if WizardSilent then
    Params := '/q /norestart /log "' + LogFile + '"'
  else
    Params := '/passive /norestart /log "' + LogFile + '"';

  Log('.NET 4.8 설치 시작: ' + Installer + ' ' + Params);
  if not ShellExec('runas', Installer, Params, '', SW_SHOW, ewWaitUntilTerminated, Code) then
  begin
    Result := '.NET Framework 4.8 설치를 시작하지 못했습니다 (' + SysErrorMessage(Code) + ').' + #13#10 +
      '관리자 확인 창에서 허용해야 설치할 수 있습니다. 관리자 권한이 없으면 IT 담당자에게 .NET Framework 4.8 설치를 요청한 뒤 Setup 을 다시 실행하세요.' + #13#10 +
      '기존에 설치된 프로그램은 바뀌지 않았습니다.';
    Exit;
  end;

  Log('.NET 4.8 설치 종료 코드: ' + IntToStr(Code));
  case Code of
    0: ;
    1641, 3010: DotNetRestartFlag := True;
  else
    Result := '.NET Framework 4.8 설치가 실패했습니다 (코드 ' + IntToStr(Code) + ').' + #13#10 +
      '로그: ' + LogFile + #13#10 + '기존에 설치된 프로그램은 바뀌지 않았습니다.';
  end;
end;
#endif

// 기존 버전을 무음으로 삭제하고, 지난 버전 파일(파일목록.txt 기준)이 남지 않았는지 확인한다.
function UninstallPrevious: String;
var
  Uninstaller, Left, Rel: String;
  Code, I, Tab: Integer;
begin
  Result := '';
  Uninstaller := RemoveQuotes(PrevUninstaller);
  if not FileExists(Uninstaller) then
  begin
    // 설치 폴더를 손으로 지워 등록만 남은 경우. 등록을 정리하고 새로 설치한다.
    Log('기존 삭제 프로그램 없음, 등록만 정리: ' + Uninstaller);
    RegDeleteKeyIncludingSubkeys(HKCU, UninstallKey);
    Exit;
  end;

  Log('기존 버전 삭제: ' + Uninstaller);
  if not Exec(Uninstaller, '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART', '', SW_HIDE, ewWaitUntilTerminated, Code) then
  begin
    Result := '기존 버전 삭제를 시작하지 못했습니다 (' + SysErrorMessage(Code) + ').';
    Exit;
  end;

  // 삭제 프로그램은 임시 복사본으로 다시 실행되므로 등록 키와 삭제 프로그램이 사라질 때까지 기다린다 (최대 60초).
  for I := 1 to 120 do
  begin
    if (not RegKeyExists(HKCU, UninstallKey)) and (not FileExists(Uninstaller)) then Break;
    Sleep(500);
  end;
  if RegKeyExists(HKCU, UninstallKey) then
  begin
    Result := '기존 버전 ' + PrevVersion + ' 삭제가 끝나지 않았습니다.' + #13#10 +
      'Windows 설정 > 앱에서 {#AppName} 을 직접 삭제한 뒤 Setup 을 다시 실행하세요.';
    Exit;
  end;

  Left := '';
  for I := 0 to GetArrayLength(PrevFileList) - 1 do
  begin
    if (PrevFileList[I] = '') or (Copy(PrevFileList[I], 1, 1) = '#') then Continue;
    Tab := Pos(#9, PrevFileList[I]);
    if Tab <= 1 then Continue;
    Rel := Copy(PrevFileList[I], 1, Tab - 1);
    if FileExists(AddBackslash(PrevDir) + Rel) then Left := Left + #13#10 + '  ' + Rel;
  end;
  if Left <> '' then
    Result := '기존 버전 파일이 지워지지 않았습니다. 파일을 쓰는 프로그램을 모두 종료한 뒤 Setup 을 다시 실행하세요.' + Left;
end;

// ---- Inno Setup 이벤트 --------------------------------------------------------

function InitializeSetup: Boolean;
begin
  Result := True;
  ReadPreviousInstall;
  DotNetNeeded := not IsDotNet48Installed;

#ifndef DotNetInstaller
  if DotNetNeeded then
  begin
    SuppressibleMsgBox('.NET Framework 4.8 이 이 PC 에 없습니다.' + #13#10 +
      'IT 담당자에게 .NET Framework 4.8 설치를 요청하거나, .NET 이 포함된 Setup 파일로 다시 설치하세요.', mbCriticalError, MB_OK, IDOK);
    Result := False;
    Exit;
  end;
#endif

  if not IsWin64 then
    SuppressibleMsgBox('32비트 Windows 입니다. 설치는 되지만 큰 GDS 파일을 열 때 메모리가 부족할 수 있습니다.', mbInformation, MB_OK, IDOK);
end;

procedure InitializeWizard;
begin
  if PrevInstalled then
    WizardForm.WelcomeLabel2.Caption := WizardForm.WelcomeLabel2.Caption + #13#10#13#10 +
      '이 PC 에 ' + PrevVersion + ' 버전이 설치되어 있습니다. 기존 버전을 삭제한 뒤 새 버전을 설치합니다. ' +
      '서버 주소 / 사용자 ID / 언어 설정은 그대로 유지합니다.';

  ServerPage := CreateInputQueryPage(wpWelcome, '서버 주소',
    '{#AppName} 가 접속할 서버를 입력하세요.',
    '고객 서버의 IP 주소 또는 컴퓨터 이름입니다. 설치한 뒤 바꾸려면 이 Setup 을 다시 실행하세요.');
  ServerPage.Add('서버 주소:', False);
  ServerPage.Values[0] := DefaultServer;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  Value: String;
begin
  Result := True;
  if CurPageID = ServerPage.ID then
  begin
    Value := Trim(ServerPage.Values[0]);
    if (Value = '') or (Pos(' ', Value) > 0) then
    begin
      MsgBox('서버 주소를 공백 없이 입력하세요. 예) 10.10.1.20', mbError, MB_OK);
      Result := False;
    end;
  end;
end;

function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo, MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
begin
  Result := '설치 버전:' + NewLine + Space + '{#AppVersion}' + NewLine + NewLine;
  if PrevInstalled then
    Result := Result + '기존 버전:' + NewLine + Space + PrevVersion + ' (삭제한 뒤 설치)' + NewLine + NewLine;
  Result := Result + MemoDirInfo + NewLine + NewLine;
  Result := Result + '서버 주소:' + NewLine + Space + Trim(ServerPage.Values[0]) + NewLine + NewLine;
  Result := Result + '.NET Framework 4.8:' + NewLine + Space;
  if DotNetNeeded then
    Result := Result + '함께 설치 (다음 단계에서 관리자 확인 창이 뜹니다)'
  else
    Result := Result + '이미 설치됨';
  if MemoTasksInfo <> '' then Result := Result + NewLine + NewLine + MemoTasksInfo;
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  Result := '';

  while IsAppRunningIn(PrevDir) or IsAppRunningIn(WizardDirValue) do
  begin
    if SuppressibleMsgBox('{#AppName} 가 실행 중입니다. 프로그램을 종료한 뒤 [다시 시도]를 누르세요.', mbError, MB_RETRYCANCEL, IDCANCEL) = IDCANCEL then
    begin
      Result := '{#AppName} 가 실행 중이라 설치를 멈췄습니다. 프로그램을 종료한 뒤 Setup 을 다시 실행하세요.';
      Exit;
    end;
  end;

#ifdef DotNetInstaller
  if DotNetNeeded then
  begin
    Result := InstallDotNet;
    if Result <> '' then Exit;
  end;
#endif

  if PrevInstalled then
    Result := UninstallPrevious;
end;

// DACruxConfig.ini 가 설치된 직후 서버 주소 / Factory / 보관한 사용자 설정을 기록한다 ([Files] AfterInstall).
procedure WriteConfig;
var
  IniPath, Server: String;
  Lines: TArrayOfString;
begin
  IniPath := ExpandConstant('{app}\') + ConfigFileName;
  if not LoadStringsFromFile(IniPath, Lines) then
  begin
    Log('ini 읽기 실패: ' + IniPath);
    Exit;
  end;

  Server := Trim(ServerPage.Values[0]);
  SetLastOption(Lines, 'ServerIP', Server);
  SetLastOption(Lines, 'EAIServerIP', Server);
  SetLastOption(Lines, 'Factory', 'SCK');
  SetLastOption(Lines, 'FactoryList', 'SCK');
  if PrevUserID <> '' then SetLastOption(Lines, 'UserID', PrevUserID);
  if PrevLanguage <> '' then SetLastOption(Lines, 'Language', PrevLanguage);

  if SaveStringsToFile(IniPath, Lines, False) then
    Log('ini 기록: ServerIP = ' + Server)
  else
    Log('ini 저장 실패: ' + IniPath);
end;

function NeedRestart: Boolean;
begin
  Result := DotNetRestartFlag;
end;
