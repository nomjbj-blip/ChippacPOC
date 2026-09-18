using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using DACrux.Common.DSL;
using DACrux.Common.Interface;

namespace DACrux.Common.BSL
{
    public class ComConfiguration : Miracom.Middleware.BaseComponent, iComConfiguration
    {
        #region [ TQC_CONFIG ]
        public int AddConfiguration(
            String[,] parameters
            )
        {
            int iResult = -1;
            TQC_CONFIG oConfig = null;
            DataTable dt = null;
            DataRow[] dr = null;

            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    oConfig = new TQC_CONFIG();

                    for (int idx = 0; idx < parameters.GetLength(1); idx++)
                    {
                        dt = oConfig.GetCategory(parameters[idx, 0]); // checked category 
                        dr = dt.Select(string.Format("NAME='{0}'", parameters[idx, 1]));

                        if (dr != null && dr.Length > 0)
                        {
                            iResult += oConfig.UpdateValue(
                                parameters[idx, 0],
                                parameters[idx, 1],
                                parameters[idx, 2],
                                parameters[idx, 3],
                                parameters[idx, 4]
                                );
                        }
                        else
                        {

                            iResult += oConfig.SetValue(
                                parameters[idx, 0],
                                parameters[idx, 1],
                                parameters[idx, 2],
                                parameters[idx, 3],
                                parameters[idx, 4]
                                );
                        }
                    }
                    ts.Complete();
                }
                return iResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddConfiguration(
            string ValueCategory,
            string ValueName,
            string Value,
            string ValueType,
            string Description
            )
        {
            TQC_CONFIG oConfig = null;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    oConfig = new TQC_CONFIG();
                    DataTable dt = oConfig.GetCategory(ValueCategory);

                    DataRow[] dr = dt.Select(string.Format("NAME='{0}'", ValueName));

                    if (dr != null && dr.Length > 0)
                    {
                        oConfig.UpdateValue(ValueCategory, ValueName, Value, ValueType, Description);
                    }
                    else
                    {
                        oConfig.SetValue(ValueCategory, ValueName, Value, ValueType, Description);
                    }

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateConfiguration(
            string ValueCategory,
            string ValueName,
            string Value,
            string ValueType,
            string Description
            )
        {
            TQC_CONFIG oConfig = new TQC_CONFIG();
            oConfig.UpdateValue(ValueCategory, ValueName, Value, ValueType, Description);
        }

        public DataTable GetSEMDASConfigure(
            )
        {

            TQC_CONFIG oConfig = new TQC_CONFIG();
            return oConfig.GetValue();
        }

        public DataTable GetConfigCategory(
            string strCategory
            )
        {
            TQC_CONFIG obj = new TQC_CONFIG();
            return obj.GetCategory(strCategory);
        }

        public DataTable GetDefectFTP(
            )
        {
            TQC_CONFIG obj = new TQC_CONFIG();
            return obj.GetDefectFtp();
        }

        public DataTable GetAVIImageFTPInfo()
        {
            TQC_CONFIG oConfig = new TQC_CONFIG();
            return oConfig.GetAVIImageFTPInfo();

        }

        public DataTable GetAVIMapFTPInfo()
        {
            TQC_CONFIG oConfig = new TQC_CONFIG();
            return oConfig.GetAVIMapFTPInfo();
        }

        public Dictionary<string, string> GetData(string category)
        {
            TQC_CONFIG obj = new TQC_CONFIG();
            return obj.GetData(category);
        }

        #endregion [ TQC_CONFIG ]

        #region [ TQC_CONFIG_USER ]

        public DataTable GetConfigUser(
            string factory,
            string category,
            string userid
            )
        {
            TQC_CONFIG_USER oConfigUser = new TQC_CONFIG_USER();
            return oConfigUser.SelectTqcConfigUser01(factory, category, userid);
        }

        public DataTable GetConfigurationUser(
            string factory,
            string category,
            string userid
            )
        {
            TQC_CONFIG_USER oConfigUser = new TQC_CONFIG_USER();
            return oConfigUser.SelectTqcConfigUser03(factory, category, userid);
        }

        public Color GetConfigUserColor(
            string factory,
            string category,
            string name,
            string userid
            )
        {
            TQC_CONFIG_USER oConfigUser = new TQC_CONFIG_USER();
            string colorStr = oConfigUser.SelectTqcConfigUser(factory, category, name, userid);
            if (String.IsNullOrEmpty(colorStr))
                return Color.Empty;

            return ColorTranslator.FromHtml(colorStr);
        }

        public object GetConfigUserValue(
            string factory,
            string category,
            string name,
            string userid
            )
        {
            TQC_CONFIG_USER oConfigUser = new TQC_CONFIG_USER();
            return oConfigUser.SelectTqcConfigUser(factory, category, name, userid);
        }

        public int SetConfigUser(
            string factory,
            string[] category,
            string userid,
            string[,] parameters
            )
        {
            TQC_CONFIG_USER oConfigUser = new TQC_CONFIG_USER();
            int result = -1;
            result = oConfigUser.DelectTqcConfigUser01(factory, category, userid);
            result = oConfigUser.InsertTqcConfigUser01(parameters);
            return result;
        }

        public string GetConfigValue(string factory, string category, string name, string userId)
        {
            TQC_CONFIG_USER obj = new TQC_CONFIG_USER();
            return obj.SelectTqcConfigUser(factory, category, name, userId);
        }

        //--

        public Color GetDieBackColor(
            string factory,
            string userid
            )
        {
            TQC_CONFIG_USER obj = new TQC_CONFIG_USER();
            string colorVal = obj.SelectTqcConfigUser(factory, "WAFER_DIE_COLOR", "WAFER_MAP_BG", userid);

            if(string.IsNullOrEmpty(colorVal))
                return Color.Empty;

            return ColorTranslator.FromHtml(colorVal);
        }

        public DataTable SelectDefectMapConfig(
            string factory,
            string userid
            )
        {
            TQC_CONFIG_USER obj = new TQC_CONFIG_USER();
            return obj.SelectTqcConfigUser01(factory, "WAFER_DIE_COLOR", userid);
        }

        #endregion [ TQC_CONFIG_USER]

        #region Global Config

        public DataTable GetConfigListAll()
        {
            TQC_CONFIG obj = new TQC_CONFIG();
            return obj.GetConfigListAll();
        }

        public DataTable GetConfigListDuple(string strCategory, string strName)
        {
            TQC_CONFIG obj = new TQC_CONFIG();
            return obj.GetConfigListDuple(strCategory, strName);
        }

        public void UpdateConfiagData(string strCategory, string strName, string strValue, string strType, string strComment)
        {
            TQC_CONFIG obj = new TQC_CONFIG();
            obj.UpdateConfiagData(strCategory, strName, strValue, strType, strComment);
        }

        public void DeleteConfiagData(string strCategory, string strName)
        {
            TQC_CONFIG obj = new TQC_CONFIG();
            obj.DeleteConfiagData(strCategory, strName);
        }

        public void InsertConfiagData(string strCategory, string strName, string strValue, string strType, string strComment)
        {
            TQC_CONFIG obj = new TQC_CONFIG();
            obj.InsertConfiagData(strCategory, strName, strValue, strType, strComment);
        }


        #endregion Global Config

        #region TQC_LOT_HIS

        public DataTable GetLotHisData(string strStartTime, string strEndTime)
        {
            TQC_LOT_HIS obj = new TQC_LOT_HIS();
            return obj.GetLotHisData(strStartTime, strEndTime);
        }

        public DataTable GetLotHisDataGroup(string strItem, string strStartTime, string strEndTime)
        {
            TQC_LOT_HIS obj = new TQC_LOT_HIS();
            return obj.GetLotHisDataGroup(strItem, strStartTime, strEndTime);
        }


        public DataSet GetCommonality(string[] strGoodLot, string[] strBadLot, string[] strItem, bool bTestArea)
        {
            TQC_LOT_HIS obj = new TQC_LOT_HIS();

            DataSet ds = new DataSet();
            DataTable dtTemp = null;
            string strTestArea = string.Empty;

            if (bTestArea == true)
                strTestArea = " AND RES_AREA NOT IN ('METRO', 'DM', 'TEST') ";

            if(strGoodLot == null || strGoodLot.Length <= 0 || strBadLot == null || strBadLot.Length <= 0 || strItem == null || strItem.Length<= 0)
                throw new Exception("조회 조건이 충분치 않습니다.");

            string strGoodList = string.Format("'{0}'", string.Join("','", strGoodLot));
            string strBadList =string.Format("'{0}'", string.Join("','", strBadLot));
            string strItemList = string.Join(",", strItem);


            dtTemp = obj.GetCommonalityLotGrp(strGoodList, strBadList, strItemList, strTestArea);
            if (dtTemp == null || dtTemp.Rows.Count <= 0)
                throw new Exception("Not Found Data");

            dtTemp.TableName = "GRP";
            ds.Tables.Add(dtTemp.Copy());

            dtTemp = obj.GetCommonalityLot(strGoodList, strBadList, strTestArea);
            if (dtTemp == null || dtTemp.Rows.Count <= 0)
                throw new Exception("Not Found Data");

            dtTemp.TableName = "LOT";
            ds.Tables.Add(dtTemp.Copy());

            return ds;
        }

        public DataSet GetCommonalityWafer(string[] strGoodLot, string[] strBadLot, string[] strItem, bool bTestArea)
        {
            TQC_LOT_HIS obj = new TQC_LOT_HIS();

            DataSet ds = new DataSet();
            DataTable dtTemp = null;

            string strTestArea = string.Empty;

            if (bTestArea == true)
                strTestArea = " AND RES_AREA NOT IN ('METRO', 'DM', 'TEST') ";

            if (strGoodLot == null || strGoodLot.Length <= 0 || strBadLot == null || strBadLot.Length <= 0 || strItem == null || strItem.Length <= 0)
                throw new Exception("조회 조건이 충분치 않습니다.");

            string goodWaferList = string.Format("'{0}'", string.Join("','", strGoodLot));
            string badWaferList = string.Format("'{0}'", string.Join("','", strBadLot));
            string strItemList = string.Join(",", strItem);
            //string strWaferNoList = string.Format("{0}", string.Join(",", strWafer));

            string[] goodLots = GetLots(strGoodLot);
            string[] badLots = GetLots(strBadLot);

            string goodLot = string.Format("'{0}'", string.Join("','", goodLots));
            string badLot = string.Format("'{0}'", string.Join("','", badLots));
            dtTemp = obj.GetCommonalityWaferGrp(goodLot, goodWaferList, badLot, badWaferList, strItemList, strTestArea);
            if (dtTemp == null || dtTemp.Rows.Count <= 0)
                throw new Exception("Not Found Data");

            dtTemp.TableName = "GRP";
            ds.Tables.Add(dtTemp.Copy());

            dtTemp = obj.GetCommonalityWafer(goodLot, goodWaferList, badLot, badWaferList, strTestArea);
            if (dtTemp == null || dtTemp.Rows.Count <= 0)
                throw new Exception("Not Found Data");

            dtTemp.TableName = "WAFER";
            ds.Tables.Add(dtTemp.Copy());

            return ds;
        }

        private string[] GetLots(
            string[] sWafers
            )
        {
            List<string> lst = new List<string>();
            for (int i = 0; i < sWafers.Length; i++)
            {
                string lotid = sWafers[i].Substring(0, sWafers[i].IndexOf('-'));
                int idx = lst.BinarySearch(lotid);
                if (idx < 0)
                    lst.Insert(~idx, lotid);
            }
            return lst.ToArray();
        }
        #endregion TQC_LOT_HIS
    }
}
