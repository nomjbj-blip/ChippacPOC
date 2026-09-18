using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DACrux.ProjectManager.UI;

namespace DACrux.BStats.StatisticsInput
{
    public class inputDefault : iStatInformation
    {
        #region " MEMBER FIELD "

        string m_Title = string.Empty;
        StatType m_Type = StatType.None;
        int m_iDecimalInner = 15;
        int m_iDecimalOuter = 4;
        DataTable m_dt = null;
        string m_ResultFilePath = string.Empty;        
        StatInformation m_Info = null;
        string m_Project = string.Empty;
        string m_WorkSheet = string.Empty;
        string m_User = string.Empty;
        GraphInformation[] m_Graphinfomations = null;
        
        #endregion

        #region " PROPERTY "

        /// <summary>
        /// 
        /// </summary>
        public GraphInformation[] GraphInformations
        {
            get
            {
                return m_Graphinfomations;
            }
            set
            {
                m_Graphinfomations = value;
            }
        }

        /// <summary>
        /// Stat Information
        /// </summary>
        public StatInformation StatInfo
        {
            get
            {
                if (m_Info == null)
                {
                    m_Info = new StatInformation();
                }
                m_Info.Title = m_Title;
                m_Info.Type = m_Type;
                m_Info.ResultFilePath = m_ResultFilePath;
                m_Info.GraphInformations = m_Graphinfomations;
                return m_Info;
            }
        }

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        /// <summary>
        /// Project Name
        /// </summary>
        public string Project
        {
            get { return m_Project; }
            set { m_Project = value; }
        }
        /// <summary>
        /// WorkSheet Name
        /// </summary>
        public string WorkSheet
        {
            get { return m_WorkSheet; }
            set { m_WorkSheet = value; }
        }

        /// <summary>
        /// User Name
        /// </summary>
        public string User
        {
            get { return m_User; }
            set { m_User = value; }
        }

        /// <summary>
        /// Analysis Type
        /// </summary>
        public StatType Type
        {
            get { return m_Type; }
            set { m_Type = value;  }
        }

        /// <summary>
        /// Inner Decimal
        /// </summary>
        public int DecimalInner
        {
            get { return m_iDecimalInner; }
            set { m_iDecimalInner = value; }
        }

        /// <summary>
        /// Outer Decimal
        /// </summary>
        public int DecimalOuter
        {
            get { return m_iDecimalOuter; }
            set { m_iDecimalOuter = value; }
        }

        /// <summary>
        /// Source
        /// </summary>
        public DataTable DataSource
        {
            get { return m_dt; }
            set { m_dt = value; }
        }

        /// <summary>
        /// Result File Full Path
        /// </summary>
        public string ResultFilePath
        {
            get { return m_ResultFilePath; }
            set { m_ResultFilePath = value; }
        }

        #endregion             

    }
}
