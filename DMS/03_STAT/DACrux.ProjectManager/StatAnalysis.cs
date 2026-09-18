using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using DACrux.ProjectManager.UI;
using DACrux.BStats;


namespace DACrux.ProjectManager
{
    [Serializable]
    internal sealed class StatAnalysis : Analysis
    {
        #region " MEMBER FIELD "
        private StatInformation statInfo = null;
        private StatType statAnalysisType = StatType.None;
        private static Hashtable htNameCount = new Hashtable();
        #endregion

        #region " PROPERTY "
        public StatType Type
        {
            get { return statAnalysisType; }
            set { statAnalysisType = value; }
        }

        public StatInformation StatInfo
        {
            get { return statInfo; }
            set { statInfo = value; }
        }

        public override object AnalysisData
        {
            get
            {
                return statInfo;
            }
        }

        internal bool PrepareStatResult(StatInformation statInfo)
        {
            Byte[] arrByte;

            foreach(string filePath in statInfo.SerializationData.Keys)
            {
                arrByte = (byte[])statInfo.SerializationData[filePath];
                SaveFile(System.IO.Path.Combine(DACrux.ProjectManager.UI.Common.TempPath, System.IO.Path.GetFileName(filePath)), arrByte);
            }

            return true;
        }

        private byte [] LoadFile(string path)
        {
            Byte[] arrByte;
            FileStream fs = null;

            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                arrByte = new byte[fs.Length];

                fs.Read(arrByte, 0, arrByte.Length);

                return arrByte;
            }
            catch
            {
                return null;
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
        }

        private void SaveFile(string path, byte [] arrByte)
        {
            FileStream fs = null;

            System.IO.DirectoryInfo di = new DirectoryInfo(DACrux.ProjectManager.UI.Common.TempPath);

            try
            {
                if (!di.Exists)
                    di.Create();

                fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                fs.Write(arrByte, 0, arrByte.Length);
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
        }


        #endregion

        #region " CREATOR "

        public StatAnalysis(WorkSheet workSheet, StatInformation statInfo) 
        {
            oProject = Project.GetInstance();
            oWorkSheet = workSheet;

            if (statInfo.Title == string.Empty)
            {
                strName = GetNewInstanceName(statInfo.Type);
                statInfo.Title = strName;
            }
            else
                strName = statInfo.Title;

            PrepareStatResult(statInfo);

            StatInfo = statInfo;
            Type = statInfo.Type;
            modelType = ModelType.StatAnalysis;
        }

        #endregion

        #region " METHOD "

        public static string GetNewInstanceName(StatType statAnalysisType)
        {
            string strNewInstanceName = string.Empty;

            try
            {
                if (htNameCount[statAnalysisType] == null)
                    htNameCount.Add(statAnalysisType, 1);
                else
                    htNameCount[statAnalysisType] = (int)htNameCount[statAnalysisType] + 1;

                strNewInstanceName = statAnalysisType.ToString() + (int)htNameCount[statAnalysisType];
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: Project.GetNewInstanceName()"));
            }

            return strNewInstanceName;
        }

        private void ClearResultFiles(StatInformation statInfo)
        {
            FileInfo fi = null;

            try
            {
                fi = new FileInfo(DACrux.ProjectManager.UI.Common.TempPath + statInfo.ResultFilePath);
                fi.Delete();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        override internal void Close()
        {
            htNameCount.Clear();

            ClearResultFiles(statInfo);
        }

        override internal void Saved()
        {
            statInfo.SerializationData.Clear();
            statInfo.SerializationData = null;
        }

        override internal bool Saving()
        {
            Byte[] arrByte;
            Hashtable htFiles = new Hashtable();

            try
            {
                arrByte = LoadFile(DACrux.ProjectManager.UI.Common.TempPath + statInfo.ResultFilePath);
                if (arrByte != null)
                {
                    htFiles.Add(statInfo.ResultFilePath, arrByte);
                    statInfo.SerializationData = htFiles;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
