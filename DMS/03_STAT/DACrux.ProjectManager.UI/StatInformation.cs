using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.ProjectManager.UI
{
    [Serializable]
    public sealed class StatInformation
    {
        #region " Class Member "
        
        StatType m_Type = StatType.None;
        string m_Title = string.Empty;        
        string m_ResultFilePath = string.Empty;
        //List<string> lstImagePath = new List<string>();
        GraphInformation[] m_Graphinfomations = null;

        Hashtable htSerializationData = new Hashtable();

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

        public StatType Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }        

        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                m_Title = value;
            }
        }        

        public string ResultFilePath
        {
            get
            {
                return m_ResultFilePath;
            }
            set
            {
                m_ResultFilePath = value;
            }
        }

        //public List<string> ImagePathCollection
        //{
        //    get { return lstImagePath; }
        //    set { lstImagePath = value; }
        //}

        public Hashtable SerializationData
        {
            get { return htSerializationData; }
            set { htSerializationData = value; }
        }

        #endregion
    }
}
