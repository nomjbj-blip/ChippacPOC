using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;

namespace DACrux.Data.Parser
{
    /// <summary>
    /// CP Parser
    /// </summary>
    public class ParserPst : ParserCp, ISaveFile
    {
        public ParserPst(string fileName)
            : base(fileName)
        {
            try
            {
                string[] strValue;
                List<string> lsChipData = new List<string>();
                DieDataList = new List<CpDieData>();

                if(Path.GetExtension(fileName).ToUpper() == ".MAP")
                    throw new Exception(".map File 은 Parsing 대상이 아닙니다.");

                string text = ParsingUtil.FileToString(fileName);
                string[] arr = text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.None);
                string strDateEndTime = string.Empty;


                foreach (string line in arr)
                {
                    if (String.IsNullOrEmpty(line))
                        continue;

                    strValue = line.Split(new string[] { "=", }, StringSplitOptions.RemoveEmptyEntries);

                    if (strValue.Length < 2)
                        continue;

                    for (int ir = 0; ir < strValue.Length; ir++)
                        strValue[ir] = strValue[ir].Trim();

                    if (strValue[0] == "X")
                    {
                        lsChipData.Add(line);
                    }
                    else if (strValue[0] == "CUS_DATE")
                    {
                        //Start, End Time 을 동일하게 사용.
                        StartTime = GetDateTime(strValue[1]);
                        EndTime = GetDateTime(strValue[1]);
                    }
                    else if (strValue[0] == "CUS_TIME")
                    {
                        //Start, End Time 을 동일하게 사용.
                        StartTime = GetDateTime(strValue[1]);
                        EndTime = GetDateTime(strValue[1]);
                    }
                    else if (strValue[0] == "CUS_LOT")
                    {
                        LotID = strValue[1];
                    }
                    else if (strValue[0] == "CUS_PGM")
                    {
                        ProgramName = strValue[1];
                    }
                    else if (strValue[0] == "CUS_TESTER")
                    {
                        Tester = strValue[1];
                    }
                    else if (strValue[0] == "CUS_CARD")
                    {
                        ProbeCard = strValue[1];
                    }
                    else if (strValue[0] == "CUS_OPER")
                    {
                        Operator = strValue[1];
                    }
                    else if (strValue[0] == "CUS_WF")
                    {
                        //Wafer ID 가 LotID-1, LotID-2, LotID-3 형태로 나와도 LotID-01, LotID-02, LotID-03 형태로 변경 해준다.
                        WaferID = string.Format("{0}-{1:00}", LotID, Convert.ToInt32(strValue[1]));
                    }

                }

                if (string.IsNullOrEmpty(ProgramName))
                    throw new Exception("Program 정의가 되어 있지 않습니다.");

                foreach (string data in lsChipData.ToArray())
                {
                    CpDieData dieData = new CpDieData();
                    string[] dataArr = data.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    ChipDataDef = new string[dataArr.Length];
                    for (int i = 0; i < dataArr.Length; i++)
                    {
                        strValue = dataArr[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strValue.Length < 2)
                            continue;

                        string strHeader = strValue[0].Trim() == "B" ? "BIN" : strValue[0].Trim();
                        string strValues = strValue[1].Trim();

                        ChipDataDef[i] = strHeader;
                        dieData.Add(new CpTestData(strHeader, strValues));
                    }

                    // 값 정렬
                    dieData.Sort();

                    DieDataList.Add(dieData);
                }

                switch (Path.GetExtension(fileName).ToUpper())
                {
                    case ".PST":
                        TestArea = "POSTLASER";
                        break;
                    case ".CP4":
                        TestArea = "LASER";
                        break;
                    case ".CP3":
                        TestArea = "MULTIPROBE";
                        break;
                    case ".MAP":
                        throw new Exception(".map File 은 Parsing 대상이 아닙니다.");
                    default:
                        throw new Exception(string.Format("정의된 확장자가 아닙니다. {0}", Path.GetExtension(fileName).ToUpper()));
                }
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

        protected override string GetValue(List<string> list, string startVal)
        {
            foreach (string line in list)
            {
                if (line.StartsWith(startVal))
                {
                    string[] arr = line.Split('=');

                    if (arr.Length >= Enum.GetNames(typeof(CpDataOrder)).Length)
                        return arr[(int)CpDataOrder.Value];
                }
            }

            return null;
        }
    }
}
