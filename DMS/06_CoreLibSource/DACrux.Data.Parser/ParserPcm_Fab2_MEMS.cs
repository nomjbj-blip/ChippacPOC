using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DACrux.Data.Parser
{
    /*
     * PCM Parser for FAB2 MEMS (PRELASER) : CP 데이터와 비슷
     * 
     * 검사항목이 column으로, 테스트 DIE 단위가 row로 표현
     * 같은 데이터가 여러번 반복되어 올라올 수 있으며 마지막 데이터가 저장할 데이터가 되도록 한다.
     * 
     * No X Y Vp Bin Tp PARA_10_0 PARA_10_5 PARA_11_0 PARA_11_5 PARA_12_0 PARA_12_5 PARA_13_0 PARA_13_5 PARA_14_0 PARA_14_5 PARA_15_0 PARA_15_5 PARA_16_0 PARA_16_5 PARA_17_0 PARA_17_5 PARA_18_0
        1 76 169 11.0 3 1 +1.081E-12 +1.132E-12 +1.196E-12 +1.746E-12 +1.942E-12 +2.015E-12 +2.098E-12 +2.143E-12 +2.191E-12 +2.263E-12 +2.404E-12 +2.460E-12 +2.482E-12 +2.510E-12 +2.546E-12 +2.593E-12 +2.615E-12 
        2 77 169 11.0 3 2 +1.115E-12 +1.197E-12 +1.358E-12 +1.872E-12 +2.001E-12 +2.048E-12 +2.144E-12 +2.176E-12 +2.233E-12 +2.345E-12 +2.467E-12 +2.490E-12 +2.519E-12 +2.541E-12 +2.586E-12 +2.627E-12 +2.649E-12 
        3 78 169 11.0 3 3 +1.120E-12 +1.195E-12 +1.349E-12 +1.870E-12 +2.010E-12 +2.066E-12 +2.136E-12 +2.188E-12 +2.239E-12 +2.327E-12 +2.467E-12 +2.492E-12 +2.519E-12 +2.548E-12 +2.586E-12 +2.629E-12 +2.653E-12 
        4 79 169 11.5 1 4 +1.056E-12 +1.103E-12 +1.156E-12 +1.253E-12 +1.857E-12 +1.993E-12 +2.065E-12 +2.144E-12 +2.187E-12 +2.239E-12 +2.388E-12 +2.440E-12 +2.478E-12 +2.508E-12 +2.532E-12 +2.569E-12 +2.609E-12 
        1 76 169 11.5 1 1 +1.061E-12 +1.098E-12 +1.149E-12 +1.213E-12 +1.846E-12 +1.999E-12 +2.071E-12 +2.138E-12 +2.172E-12 +2.229E-12 +2.353E-12 +2.459E-12 +2.480E-12 +2.505E-12 +2.537E-12 +2.584E-12 +2.612E-12 
        2 77 169 11.5 1 2 +1.091E-12 +1.130E-12 +1.202E-12 +1.334E-12 +1.945E-12 +2.026E-12 +2.091E-12 +
     */
    public class ParserPcm_Fab2_MEMS : ParserPcm
    {
        public enum Col
        {
            No,
            X,
            Y,
            Vp,
            Bin,
            Tp
        }

        public static readonly string DATETIME_FORMAT = "MM/dd/yyyy/HH:mm:ss";

        public ParserPcm_Fab2_MEMS(string fileName)
            : base(fileName)
        {
            try
            {
                WaferDataList = new WaferDataList();

                string text = ParsingUtil.FileToString(fileName);
                string[] arr = text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                PcmWaferData currentWafer = null;
                Dictionary<Point, string[]> dataDictionary = new Dictionary<Point, string[]>();

                string[] headerArr = null;
                this.Notch = "270"; // MEMS에 대한 Default Value

                foreach (string line in arr)
                {
                    if (String.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    else if (currentWafer != null && headerArr != null && !String.IsNullOrWhiteSpace(line))
                    {
                        string[] dataArr = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        int x = Int32.Parse(dataArr[(int)Col.X]);
                        int y = Int32.Parse(dataArr[(int)Col.Y]);
                        Point point = new Point(x, y);

                        if (!dataDictionary.ContainsKey(point))
                        {
                            dataDictionary[point] = dataArr;
                            continue;
                        }

                        /// 중복 좌표 발생시
                        string[] prevArr = dataDictionary[point];
                        int prevBin = -1;
                        int currBin = -1;
                        double currVp = double.NaN;
                        double prevVp = double.NaN;

                        /// 동일한 좌표의 결과가 있을 경우 Bin 기준으로 아래 순서대로 Update가 되도록.
                        /// Bin 1 – 3 – 4 – 2 – 15 – 17 – 10 – 11 – 21 – 31
                        /// 예를 들어 Bin 15와 21값 이 있으면 Bin 15의 동일 좌표 값 전체 Update 
                        if (CheckedBin(dataArr, prevArr, out currBin, out prevBin))
                        {
                            if (currBin != prevBin)
                            {
                                dataDictionary[point] = dataArr;
                                continue;
                            }

                            /// 초기 측정 결과와 재측정 결과의 Bin이 모두 Bin 1 이면 Vp 가 높은 값으로 Update
                            /// 예를 들어 Vp가 13.0V,  14.0V 2개 있을 경우 14.0V의 값으로 동일 좌표의 값 전체 Update
                            if (currBin == 1 && prevBin == 1)
                            {
                                prevVp = Base.Convert.doubleParse(prevArr[(int)Col.Vp]);
                                currVp = Base.Convert.doubleParse(dataArr[(int)Col.Vp]);
                                if (prevVp < currVp)
                                    dataDictionary[point] = dataArr;
                            }
                        }
                    }
                    else if (line.Contains("LOT ID"))
                    {
                        LotID = GetValue(line, "LOT ID", " : ").ToUpper();
                    }
                    else if (line.Contains("Program Name"))
                    {
                        // FAB2는 프로그램명이 CP 이름과 중복되므로 P_ 를 붙인다.
                        ProgramName = GetValue(line, "Program Name", " : ").ToUpper();
                        ProgramName = string.Format("P_{0}", ProgramName);
                    }
                    else if (line.Contains("Notch"))
                    {
                        Notch = GetValue(line, "Notch", " : ");
                    }
                    else if (line.Contains("Probe Card Name"))
                    {
                        ProbeCard = GetValue(line, "Probe Card Name", " : ");
                    }
                    else if (line.Contains("Start Time"))
                    {
                        string start = GetValue(line, "Start Time", " : ");
                        StartTime = GetDateTime(start, DATETIME_FORMAT);
                    }
                    else if (line.Contains("Epuip"))
                    {
                        EquipID = GetValue(line, "Epuip", " : ");
                    }
                    else if (line.Contains("Wafer ID"))
                    {
                        int slotID = int.Parse(GetValue(line, "Wafer ID", " : ").Trim());
                        // 새로운 WaferData 생성
                        currentWafer = new PcmWaferData(
                            string.Format("{0}-{1,2:D2}", LotID, slotID)
                            );
                        WaferDataList.Add(currentWafer);
                    }
                    else if (line.Contains("No X Y")) // Header
                    {
                        headerArr = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }

                // Wafer 단위 End 인 경우 빈줄 올라옴
                if (dataDictionary.Count > 0)
                {
                    AddShotData(currentWafer, dataDictionary, headerArr);

                    // 데이터 Clear
                    dataDictionary.Clear();
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

        /// 동일한 좌표의 결과가 있을 경우 Bin 기준으로 아래 순서대로 Update가 되도록.
        /// Bin 1 – 3 – 4 – 2 – 15 – 17 – 10 – 11 – 21 – 31
        /// 예를 들어 Bin 15와 21값 이 있으면 Bin 15의 동일 좌표 값 전체 Update 
        private bool CheckedBin(string[] dataArr, string[] prevArr, out int currBin, out int prevBin)
        {
            bool bResult = false;
            currBin = Base.Convert.intParse(dataArr[(int)Col.Bin]);
            prevBin = Base.Convert.intParse(prevArr[(int)Col.Bin]);

            if (currBin == 31)
            {
                if (prevBin == 1 || prevBin == 3 || prevBin == 4 || prevBin == 2 || prevBin == 15 || prevBin == 17 || prevBin == 10 || prevBin == 11 || prevBin == 21)
                    bResult = false;
                else bResult = true;
            }
            else if (currBin == 21)
            {
                if (prevBin == 1 || prevBin == 3 || prevBin == 4 || prevBin == 2 || prevBin == 15 || prevBin == 17 || prevBin == 10 || prevBin == 11)
                    bResult = false;
                else bResult = true;
            }
            else if (currBin == 11)
            {
                if (prevBin == 1 || prevBin == 3 || prevBin == 4 || prevBin == 2 || prevBin == 15 || prevBin == 17 || prevBin == 10)
                    bResult = false;
                else bResult = true;
            }
            else if (currBin == 10)
            {
                if (prevBin == 1 || prevBin == 3 || prevBin == 4 || prevBin == 2 || prevBin == 15 || prevBin == 17)
                    bResult = false;
                else bResult = true;
            }
            else if (currBin == 17)
            {
                if (prevBin == 1 || prevBin == 3 || prevBin == 4 || prevBin == 2 || prevBin == 15)
                    bResult = false;
                else bResult = true;
            }
            else if (currBin == 15)
            {
                if (prevBin == 1 || prevBin == 3 || prevBin == 4 || prevBin == 2)
                    bResult = false;
                else bResult = true;
            }
            else if (currBin == 2)
            {
                if (prevBin == 1 || prevBin == 3 || prevBin == 4)
                    bResult = false;
                else bResult = true;
            }
            else if (currBin == 4)
            {
                if (prevBin == 1 || prevBin == 3)
                    bResult = false;
                else bResult = true;
            }
            else if (currBin == 3)
            {
                if (prevBin == 1)
                    bResult = false;
                else bResult = true;
            }
            else if (currBin == 1)
            {
                bResult = true;
            }
            return bResult;
        }

        private void AddShotData(PcmWaferData wafer, Dictionary<Point, string[]> dataDictionary, string[] headerArr)
        {
            foreach (var item in dataDictionary)
            {
                PcmShotData shotData = new PcmShotData(item.Key.X, item.Key.Y);

                // Y 이후의 데이터만 저장
                for (int i = (int)Col.Y + 1; i < headerArr.Length; i++)
                    shotData.Add(new PcmTestData(headerArr[i], item.Value[i]));

                wafer.Add(shotData);
            }
        }
    }
}
