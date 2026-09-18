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
    internal sealed class GraphAnalysis : Analysis
    {
        #region " MEMBER FIELD "

        private GraphInformation graphInfo = null;
        private GraphType graphAnalysisType = GraphType.None;
        private static Hashtable htNameCount = new Hashtable();

        #endregion

        #region " PROPERTY "

        public GraphType Type
        {
            get { return graphAnalysisType; }
            set { graphAnalysisType = value; }
        }

        public GraphInformation GraphInfo
        {
            get { return graphInfo; }
            set { graphInfo = value; }
        }

        public override object AnalysisData
        {
            get { return graphInfo; }
        }

        #endregion

        #region " CREATOR "

        public GraphAnalysis(WorkSheet workSheet, GraphInformation graphInfo) 
        {
            oProject = Project.GetInstance();
            oWorkSheet = workSheet;

            if (graphInfo.Name == string.Empty)
            {
                strName = GetNewInstanceName(graphInfo.Type);
                graphInfo.Name = strName;
            }
            else
            {
                SetNameCount(graphInfo.Type);
                strName = graphInfo.Name;
            }

            GraphInfo = graphInfo;
            Type = graphInfo.Type;
            modelType = ModelType.GraphAnalysis;
        }

        #endregion

        #region " METHOD "

        public static void SetNameCount(GraphType graphAnalysisType)
        {
            if (htNameCount[graphAnalysisType] == null)
                htNameCount.Add(graphAnalysisType, 1);
            else
                htNameCount[graphAnalysisType] = (int)htNameCount[graphAnalysisType] + 1;                
        }

        public static string GetNewInstanceName(GraphType graphAnalysisType)
        {
            string strNewInstanceName = string.Empty;

            try
            {
                SetNameCount(graphAnalysisType);
                strNewInstanceName = graphAnalysisType.ToString() + (int)htNameCount[graphAnalysisType];
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: Project.GetNewInstanceName()"));
            }

            return strNewInstanceName;
        }

        override internal bool Saving()
        {
            return true;
        }

        override internal void Saved()
        {
        }

        override internal void Close()
        {
            htNameCount.Clear();
        }

        #endregion
    }
}