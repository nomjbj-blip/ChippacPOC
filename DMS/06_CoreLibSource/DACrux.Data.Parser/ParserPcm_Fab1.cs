using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DACrux.Data.Parser
{
    /* 
     * PCM Parser for FAB1
     * 
     * 데이터는 순차로 읽어서 처리한다.
     * Contains 방식으로 확인하고 시작값문자, 종료값 문자를 통해 값을 가져온다. 
     * 
     * Contains = "Current Die", Start = " = ", End = ","
     * 예를들어 텍스트가 Current Die = 4,   Current Die Type = TO20 이면,
     * 얻고자 하는 값은 4
     *
     * Current Die 를 만나는 시점에 DataList 를 새로 만든 후 CurrentDataList에 값을 넣어준다.
     * 공백값을 만날때 까지 계속 CurrentDataList에 데이터 추가
     *
     * 데이터 인지 아닌지 구분 여부는 \t 문자가 있는지 여부로 판단한다.
     * 데이터는 Description, VALUE, Name 순서이다.
     * 
     * 
     *  Current Wafer = AK8363230-01

        -------------------------------------
        Current Die = 1(1, 5),   Current Die Type = KT12
        FlatZone = L (L, R, T, B)

        M09:CC_NMT5:Rc	10.848	CC_NMT5
        M09:CC_PMT5:Rc	10.0193	CC_PMT5
        M09:CC_NPY5:Rc	10.0915	CC_NPY5
        M09:CC_PPY5:Rc	10.1661	CC_PPY5
        M09:VIA_1:Rc	3.22949	V1C_NOM
        M09:VIA_5:Rc	3.18504	V5C_NOM
        M09:MT1_SER:R	1.78476	SER_M1
        M09:MT2_SER:R	1.31917	SER_M2
        M09:MT6_SER_4T:Rsh	1.7128	SER_M6T4
        M09:MT6_SER_M6_T:Rsh	1.7128	SER_M6_T
        M09:MT1_BR:Irev	-11.3372	BR_MET1
        M09:MT2_BR:Irev	-11.3546	BR_MET2
        M09:MT6_BR:Irev	-11.3768	BR_MET6
        -------------------------------------
        Current Die = 2,   Current Die Type = KT12

        M09:CC_NMT5:Rc	10.7706	CC_NMT5
        M09:CC_PMT5:Rc	10.0437	CC_PMT5
        M09:CC_NPY5:Rc	9.94817	CC_NPY5
     * 
     */
    public class ParserPcm_Fab1 : ParserPcm
    {
        public enum PcmTestItemOrder
        {
            Description,
            Value,
            Name
        }

        public ParserPcm_Fab1(string fileName)
            : base(fileName)
        {
            try
            {
                WaferDataList = new WaferDataList();
                string Name = Path.GetFileNameWithoutExtension(fileName);
                string[] sValues = Name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                LotID = sValues[0];

                //--
                bool bFlagEndTime = false;
                string text = ParsingUtil.FileToString(fileName);
                string[] arr = text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                fileName = Path.GetFileName(fileName);
                //LotID = fileName.Substring(0, fileName.IndexOf('_')).ToUpper();

                if (!text.Contains("End Time"))
                    throw new IOException(string.Format("PCM [End] 문자열이 없어 다음 주기 재시도. File : {0}", fileName));

                PcmShotData currentShot = null;

                foreach (string line in arr)
                {
                    if (String.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains('\t') && currentShot != null)
                    {
                        string[] valArr = line.Split('\t');

                        if (valArr.Length < Enum.GetNames(typeof(PcmTestItemOrder)).Length)
                            throw new Exception("데이터 길이에 문제가 있습니다.");

                        string name = valArr[(int)PcmTestItemOrder.Name].Trim();
                        string value = valArr[(int)PcmTestItemOrder.Value];
                        string desc = valArr[(int)PcmTestItemOrder.Description];

                        if (String.IsNullOrEmpty(name))
                            continue;

                        string tmp = name.ToUpper();

                        /// Item name이 대문자로 올라온 경우에 대해서만 처리
                        if (!name.Equals(tmp, StringComparison.Ordinal))
                            continue;

                        /// Item name 에 ' '이 존재하는 경우
                        if (name.Contains(' '))
                            name = name.Substring(0, name.IndexOf(' '));

                        /// Item name 에 첫 문자열이 _이 존재하는 경우
                        if (name[0] == '_' || name[0] == '%' || name[0] == '#')
                            name = name.Substring(1);

                        int idx = currentShot.FindIndex(delegate(PcmTestData data) { return String.Equals(data.Name, name); });
                        if (idx == -1)
                            currentShot.Add(new PcmTestData(name.ToUpper(), value, desc));
                        else currentShot[idx] = new PcmTestData(name.ToUpper(), value, desc);
                    }
                    else if (line.Contains("Logging Raw Data from Test")) // Logging Raw Data from Test  <KTO203CN>   Executing on: 04/08/2019 23:54:55
                    {
                        ProgramName = GetValue(line, "Logging Raw Data from Test", "<", ">").ToUpper();

                        // "."에 대한 특수문자 치환
                        if (ProgramName.Contains('.'))
                            ProgramName = ProgramName.Substring(0, ProgramName.IndexOf('.'));

                        StartTime = GetDateTime(GetValue(line, "Executing on", ": "));
                    }
                    else if (line.Contains("Operator Name is") || line.Contains("Operation Name is")) // Operator Name is : kay
                    {
                        Operator = GetValue(line, "Operator Name is", " : ");
                        if (String.IsNullOrEmpty(Operator))
                            Operator = GetValue(line, "Operation Name is", " ");
                    }
                    else if (line.Contains("Current Wafer")) // Current Wafer = AK9016600-01
                    {
                        WaferDataList.Add(new PcmWaferData(GetValue(line, "Current Wafer", " = ")));
                    }
                    else if (line.Contains("Tester"))
                    {
                        EquipID = GetValue(line, "Tester", " = ");
                    }
                    else if (line.Contains("Current Die")) // Current Die = 1 (x, y),   Current Die Type = TO20
                    {
                        string die = GetValue(line, "Current Die", " = ", "(");
                        if (string.IsNullOrEmpty(die))
                            die = GetValue(line, "Current Die", " = ", ",");

                        string x = GetValue(line, null, "(", ",");
                        string y = GetValue(line, null, ",", ")");
                        string dieType = GetValue(line, "Current Die Type", "=");

                        if (WaferDataList == null || WaferDataList.Count <= 0)
                            throw new Exception("Current Wafer 정보를 찾을 수 없습니다.");

                        if (WaferDataList[WaferDataList.Count - 1].Contains(die))
                        {
                            currentShot = WaferDataList[WaferDataList.Count - 1][die];
                        }
                        else
                        {
                            currentShot = new PcmShotData(die, x, y, dieType);
                            WaferDataList[WaferDataList.Count - 1].Add(currentShot);
                        }
                    }
                    else if (line.Contains("Flat Zone"))
                    {
                        string flatZone = GetValue(line, "Flat Zone", "=");
                        switch (flatZone)
                        {
                            case "T": Notch = "180"; break;
                            case "L": Notch = "90"; break;
                            case "R": Notch = "270"; break;
                            case "B": Notch = "0"; break;
                        }

                    }
                    else if (line.Contains("End Time"))
                    {
                        bFlagEndTime = true;
                        EndTime = GetDateTime(GetValue(line, "End Time", " : "));
                    }
                }

                foreach (PcmWaferData wafer in WaferDataList)
                {
                    foreach (PcmShotData shot in wafer)
                    {
                        shot.Sort();
                    }
                }

                if (!bFlagEndTime)
                    EndTime = System.IO.File.GetLastWriteTime(FileName);
            }
            catch (System.IO.IOException ioex)
            {
                //IO Error 의 경우 삭제 하지 않고 다음 주기에 다시 작업 한다.
                ErrorFlag = true;
                ErrorBackupFlag = false;
                ErrorMessage = ioex.Message + Environment.NewLine + ioex.StackTrace;
            }
            catch (Exception ex)
            {
                //Parsing Error 발생 시 Error Backup 에 넣는다.
                ErrorFlag = true;
                ErrorBackupFlag = true;
                ErrorMessage = ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        /// <summary>
        ///  파일의 백업 경로 설정
        ///  /BACKUP/TEST/PCM/2019/11/07/[프로그램이름]/[LOT_ID]
        ///  /BACKUP/TEST/PCM/2019/11/07/KDOA52CN/9273260
        /// </summary>
        /// <returns></returns>
        public override string GetPathForBackup()
        {
            DateTime dt = GetBackupDateTime();
            return String.Format(@"{0:0000}\{1:00}\{2:00}\{3}\{4}", dt.Year, dt.Month, dt.Day, ProgramName, LotID);
        }
    }
}
