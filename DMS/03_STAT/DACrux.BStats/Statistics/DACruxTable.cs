using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.BStats.Statistics
{
    public class DACruxTable
    {
        #region " MEMBER FIELD "
        string[] m_ObjVariable = null;
        string[] m_ObjVariableValue = null;
        string[] m_SeriesVariable = null;
        string[] m_SeriesVariableValue = null;
        string m_Variable = null;
        DataTable m_dt = null;
        #endregion

        #region " CREATOR "
        public DACruxTable()
        {
            m_dt = new DataTable();
        }
        #endregion

        #region " PROPERTY "

        public DataTable DataTable
        {
            get
            {
                return m_dt;
            }
            set
            {
                m_dt = value;
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
        public string Variable
        {
            get
            {
                return m_Variable;
            }
            set
            {
                m_Variable = value;
            }
        }
        #endregion

        #region " METHOD "
        public DACruxTable Clone()
        {
            DACruxTable dctReturn = null;

            try
            {
                dctReturn = new DACruxTable();
                dctReturn.DataTable = this.DataTable.Clone();
                dctReturn.ObjVariable = this.ObjVariable;
                dctReturn.ObjVariableValue = this.ObjVariableValue;
                dctReturn.SeriesVariable = this.SeriesVariable;
                dctReturn.SeriesVariableValue = this.SeriesVariableValue;
                dctReturn.Variable = this.Variable;
                return dctReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DACruxTable Copy()
        {
            DACruxTable dctReturn = null;
            try
            {
                dctReturn = new DACruxTable();
                this.DataTable.AcceptChanges();
                dctReturn.DataTable = this.DataTable.Copy();
                dctReturn.ObjVariable = this.ObjVariable;
                dctReturn.ObjVariableValue = this.ObjVariableValue;
                dctReturn.SeriesVariable = this.SeriesVariable;
                dctReturn.SeriesVariableValue = this.SeriesVariableValue;
                dctReturn.Variable = this.Variable;
                return dctReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CloneDataTable()
        {
            try
            {
                return this.DataTable.Clone();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CopyDataTable()
        {
            try
            {
                this.DataTable.AcceptChanges();
                return this.DataTable.Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }
}
