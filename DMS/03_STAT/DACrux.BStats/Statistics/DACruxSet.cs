using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.BStats.Statistics
{
    public class DACruxSet
    {
        #region " MEMBER FIELD "
        private List<DACruxTable> m_Table = new List<DACruxTable>();
        string[] m_ObjVariable = null;
        string[] m_ObjVariableValue = null;
        string[] m_SeriesVariable = null;
        string[] m_SeriesVariableValue = null;
        #endregion

        #region " CREATOR "

        #endregion

        #region " PROPERTY "
        public List<DACruxTable> DACruxTable
        {
            get
            {
                return m_Table;
            }
        }
        public string[] ObjVariable
        {
            get
            {
                return m_ObjVariable;
            }
            set
            {
                m_ObjVariable = value;
            }
        }
        public string[] ObjVariableValue
        {
            get
            {
                return m_ObjVariableValue;
            }
            set
            {
                m_ObjVariableValue = value;
            }
        }
        public string[] SeriesVariable
        {
            get
            {
                return m_SeriesVariable;
            }
            set
            {
                m_SeriesVariable = value;
            }
        }
        public string[] SeriesVariableValue
        {
            get
            {
                return m_SeriesVariableValue;
            }
            set
            {
                m_SeriesVariableValue = value;
            }
        }

        #endregion

    }
}
