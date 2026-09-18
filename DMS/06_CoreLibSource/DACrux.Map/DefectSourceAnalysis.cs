using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DACrux.Base;

namespace DACrux.Map
{
    public class DefectSourceAnalysis
    {
        public void Calculate(CalcBy calcBy, float tolerance, DefectList defectList, List<KeyValuePair<string, long>> stepList)
        {
            // Stpe별 Defect 분류
            List<KeyValuePair<string, DefectList>> list = new List<KeyValuePair<string, DefectList>>();
            Dictionary<string, List<long>> stepDic = new Dictionary<string, List<long>>();

            // Step Name으로 Distinct
            foreach (var step in stepList)
            {
                if (!stepDic.ContainsKey(step.Key))
                    stepDic.Add(step.Key, new List<long>());

                stepDic[step.Key].Add(step.Value);
            }
            
            // Step Name 별 리스트에 추가
            foreach (var item in stepDic)
            {
                DefectList defecList = new DefectList();

                foreach (var stepSeq in item.Value)
                    defecList.AddRange(defectList.GetDefectArray(stepSeq));

                defectList.Sort();
                list.Add(new KeyValuePair<string, DefectList>(item.Key, defecList));
            }

            Results = new List<DSAResult>();
            Missings = new List<DSAResult>();

            // 인스턴스 생성
            for (int i = 0; i < list.Count; i++)
            {
                DSAResult result = new DSAResult(list[i].Key);
                DSAResult missing = new DSAResult(list[i].Key);

                for (int j = 0; j < list.Count; j++)
                {
                    result.Add(new DSAItem(list[j].Key));
                    missing.Add(new DSAItem(list[j].Key));
                }

                Results.Add(result);
                Missings.Add(missing);
            }

            // DefectList
            for (int i = 0; i < list.Count; i++)
            {
                DSAResult result = Results[i];
                DSAResult missing = Missings[i];

                // 자기 자신의 데이터 업데이트
                result[result.StepID].DefectList.AddRange(list[i].Value);

                DefectList currCmp = list[i].Value;
                DefectList nextCmp = new DefectList();

                for (int j = i + 1; j < list.Count; j++)
                {
                    DefectList matchList = new DefectList();
                    string stepID = list[j].Key;
                    
                    foreach (Defect d1 in currCmp)
                    {
                        RectangleF rect;

                        if (calcBy == CalcBy.Size)
                        {
                            rect = new RectangleF(
                                (float)(d1.X - d1.XSIZE * 0.5 - tolerance),
                                (float)(d1.Y - d1.YSIZE * 0.5 - tolerance),
                                (float)(d1.XSIZE + 2 * tolerance),
                                (float)(d1.YSIZE + 2 * tolerance));
                        }
                        else
                        {
                            rect = new RectangleF(
                                (float)(d1.X - tolerance),
                                (float)(d1.Y - tolerance),
                                (float)(2 * tolerance),
                                (float)(2 * tolerance));
                        }

                        bool matched = false;

                        for (int k = list[j].Value.Count - 1; k >= 0; k--)
                        {
                            Defect d2 = list[j].Value[k];

                            if (rect.Contains((float)d2.X, (float)d2.Y))
                            {
                                matched = true;

                                matchList.Add(d2);
                                list[j].Value.RemoveAt(k);

                                int idx = nextCmp.BinarySearch(d1);

                                if (idx < 0)
                                    nextCmp.Insert(~idx, d1);
                            }

                            if (matched)
                                break;
                        }
                    }

                    // missing defect 처리
                    foreach (var defect in currCmp)
                    {
                        if (nextCmp.BinarySearch(defect) < 0)
                            missing[stepID].DefectList.Add(defect);
                    }

                    result[stepID].DefectList.AddRange(matchList.ToArray());

                    // match된 Defect만 다음 Step 비교 시 사용한다.
                    currCmp.Clear();
                    currCmp.AddRange(nextCmp);
                    nextCmp.Clear();
                }
            }
        }

        public static List<DSAResult> GetMissingResults(List<DSAResult> results)
        {
            if (results == null || results.Count == 0)
                return null;

            List<DSAResult> missings = new List<DSAResult>();

            foreach (var result in results)
            {
                DSAResult missing = new DSAResult(result.StepID);

                for (int i = 0; i < result.Count; i++)
                {
                    DSAItem newItem = new DSAItem(result[i].StepID);
                    missing.Add(newItem);

                    if (i == 0)
                        continue;

                    DSAItem prev = result[i - 1];
                    DSAItem curr = result[i];

                    if (curr.DefectList.Count == 0 && prev.DefectList.Count > 0)
                    {
                        newItem.DefectList.AddRange(prev.DefectList);
                    }
                    else if (curr.DefectList.Count > 0 && prev.DefectList.Count == 0)
                    {
                        newItem.DefectList.AddRange(curr.DefectList);
                    }
                    else if (curr.DefectList.Count > 0 && prev.DefectList.Count > 0)
                    {
                    }
                }
            }

            return null;
        }

        public List<DSAResult> Results
        {
            get;
            private set;
        }

        public List<DSAResult> Missings
        {
            get;
            private set;
        }
    }

    public class DSAResult : List<DSAItem>
    {
        public DSAResult(string stepID)
        {
            StepID = stepID;
        }

        public override string ToString()
        {
            return String.Format("{0} : List={1}", StepID, Count);
        }

        public string StepID
        {
            get;
            private set;
        }

        public DSAItem this[string stepID]
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.StepID == stepID)
                        return item;
                }

                return null;
            }
        }
    }

    public class DSAItem
    {
        public DSAItem(string stepID)
        {
            StepID = stepID;
            DefectList = new DefectList();
        }

        public string StepID
        {
            get;
            private set;
        }

        public DefectList DefectList
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return String.Format("{0} : {1:N0}", StepID, DefectList.Count);
        }
    }
}
