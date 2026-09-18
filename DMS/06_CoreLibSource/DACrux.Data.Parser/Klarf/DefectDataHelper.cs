using System;
using System.Collections.Generic;
using System.Reflection;
using DACrux.Base;

namespace DACrux.Data.Parser.Klarf
{
    #region Defect 데이터 관련

    public static class DefectDataHelper
    {
        /// <summary>
        /// 줄바꿈 문자 단위로 Row를 나누고, 헤더 갯수를 넘어가는 데이터는 저장하지 않는다.
        /// </summary>
        public static DefectList GetDefectList(List<string> headers, string[] textList)
        {
            DefectList list = new DefectList();

            if (headers == null || headers.Count == 0 || textList == null || textList.Length == 0)
                return list;

            List<string> rowList = new List<string>();

            foreach (string text in textList)
            {
                rowList.AddRange(text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            FieldInfo[] fieldArr = new FieldInfo[headers.Count];

            for (int i = 0; i < fieldArr.Length; i++)
                fieldArr[i] = typeof(Defect).GetField(headers[i]);

            Defect prevDefect = null;

            foreach (string row in rowList)
            {
                Defect defect = new Defect();

                double[] rowDataArr = ToDoubleList(row.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));

                if (fieldArr.Length <= rowDataArr.Length)
                {
                    for (int i = 0; i < fieldArr.Length; i++)
                    {
                        if (fieldArr[i] != null)
                            fieldArr[i].SetValue(defect, System.Convert.ChangeType(rowDataArr[i], fieldArr[i].FieldType));
                    }

                    int idx = list.BinarySearch(defect);

                    if (idx < 0)
                    {
                        list.Insert(~idx, defect);
                        prevDefect = defect;
                    }

                    // 헤더 갯수보다 데이터 갯수가 많은 경우는 Review 데이터에 이미지 정보가 붙어오는 경우이다. 2019.09.08
                    // 이미지 1개 당 2개의 데이터가 붙어오는것으로 확인됨
                    if (fieldArr.Length < rowDataArr.Length)
                    {
                        for (int i = fieldArr.Length; i < rowDataArr.Length; i += 2)
                        {
                            defect.Images.Add((int)rowDataArr[i]);
                        }
                    }
                }
                // 헤더 갯수보다 데이터 갯수가 적은 경우
                else if (rowDataArr.Length > 0 && list.Count > 0)
                {
                    /* Defect 이미지가 있을 경우 그 다음 줄 첫째값이 이미지 인덱스 값이다. 따라서 짧은 데이터가 오는 경우 윗쪽 데이터의 imageseq 에 연결
                       이미지가 여러개 오는 경우 여러줄이 생길 수 있다. 2019.09.03 Taihi,Kim.
                     * 
                       예제) 헤더는 DEFECTID XREL YREL XINDEX YINDEX XSIZE YSIZE DEFECTAREA DSIZE CLASSNUMBER TEST IMAGECOUNT IMAGELIST;
                     * 
                       22 13437.400 422.900 2 9 16.000 2.700 43.200001 107.000 0 1 1 1
	                   10 0
                     * 
                       22 13437.400 422.900 2 9 16.000 2.700 43.200001 107.000 0 1 3 3
	                   10 0
                        9 0
                        8 0
                     */
                    //Defect prevDefect = list[list.Count - 1];

                    if (prevDefect != null)
                    {
                        // 이미지 1개 당 2개의 데이터가 붙어오는것으로 확인됨
                        for (int i = 0; i < rowDataArr.Length; i += 2)
                        {
                            prevDefect.Images.Add((int)rowDataArr[i]);
                            prevDefect.IMAGECOUNT = prevDefect.Images.Count;
                        }
                    }
                }

            }

            return list;
        }

        private static double[] ToDoubleList(string[] arr)
        {
            if (arr == null || arr.Length == 0)
                return null;

            double[] doubleArr = new double[arr.Length];

            for (int i = 0; i < arr.Length; i++)
                doubleArr[i] = DACrux.Base.Convert.doubleParse(arr[i]);

            return doubleArr;
        }

        public static DefectList GetDefectList(List<string> headers, List<double> dataArr)
        {
            if (headers == null || headers.Count == 0 || dataArr == null || dataArr.Count == 0)
                return null;

            DefectList list = new DefectList();

            int length = dataArr.Count / headers.Count;
            Type t = typeof(Defect);

            for (int i = 0; i < length; i++)
            {
                Defect defect = new Defect();
                List<double> rowData = dataArr.GetRange(i * headers.Count, headers.Count);

                for (int j = 0; j < headers.Count; j++)
                {
                    FieldInfo fi = t.GetField(headers[j]);

                    if (fi != null)
                        fi.SetValue(defect, System.Convert.ChangeType(rowData[j], fi.FieldType));
                }

                list.Add(defect);
            }

            return list;
        }
    }

    #endregion
}
