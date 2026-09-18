using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DACrux.Data.Parser.Klarf;
using System.Collections.Specialized;
using DACrux.Base;

namespace DACrux.Data.Parser.Klarf
{
    /*
     * trf 파일 내용 
FileVersion 1 2;
FileTimestamp 10-30-19 10:35:07;
StepID "RD_NS";
InspectionStationID "KLA-TENCOR" "2360" "ips204";
SampleType WAFER;
ResultTimestamp 10-30-19 10:24:35;
LotID "199F44";
TiffFileName _071030103507.tif;
Slot 7;
DefectRecordSpec 4 DEFECTID CLASSNUMBER IMAGECOUNT IMAGELIST ;
DefectList
 6799 0 1 1 1 0
 6971 0 1 1 2 0
 10762 0 1 1 3 0
 18150 0 1 1 4 0
 21338 0 1 1 5 0;
EndOfFile;
     * 
     */
    /// <summary>
    /// FAB2의 trf 파일을 처리하기 위한 Klarf Parser
    /// </summary>
    public class ParserKlarf_Trf : ParserKlarf
    {
        /// <summary>
        /// Klarf 파일을 이용한 인스턴스 생성
        /// </summary>
        public ParserKlarf_Trf(string fileName)
            : base(fileName, false)
        {
            try
            {
                Wafers = new WaferList();

                // 파일을 텍스트로 읽기
                string text = ParsingUtil.FileToString(fileName);

                NameValueCollection mainCol;
                List<NameValueCollection> waferColList;
                ParserKlarf.GetNameValueCollection(text, "Slot", out mainCol, out waferColList);
                
                FileTimestamp = GetDateTime(GetString(mainCol, "FileTimestamp"));
                SampleType = GetString(mainCol, "SampleType");
                ResultTimestamp = GetDateTime(GetString(mainCol, "ResultTimestamp"));
                LotID = GetString(mainCol, "LotID");
                StepID = GetString(mainCol, "StepID");

                // Wafer 단위 처리
                foreach (NameValueCollection waferCol in waferColList)
                {
                    Wafer wafer = new Wafer();
                    wafer.Parser = this;
                    wafer.Slot = GetInt(GetString(waferCol, "Slot"));

                    // Defect 처리
                    wafer.DefectHeaders = GetStringWithLength(waferCol["DefectRecordSpec"]);
                    string[] defectArr = waferCol.GetValues("DefectList");

                    if (defectArr != null && defectArr.Length > 0)
                    {
                        wafer.DefectList = DefectDataHelper.GetDefectList(wafer.DefectHeaders, defectArr);

                        foreach (Defect d in wafer.DefectList)
                        {
                            if (d.IMAGECOUNT > 0)
                            {
                                wafer.ImageDefectList.Add(d);
                            }

                            if (d.CLASSNUMBER != 0)
                            {
                                wafer.ClassifedDefectList.Add(d);
                            }
                        }
                    }

                    Wafers.Add(wafer);
                }

                // 이미지 매핑
                ImageManagerList = ImageMapper.Mapping(this, text);
            }
            catch (Exception ex)
            {
                ErrorFlag = true;
                ErrorMessage = ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }
    }
}
