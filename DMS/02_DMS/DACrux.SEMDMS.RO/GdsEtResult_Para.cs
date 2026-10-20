using System;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    /// <summary>
    /// E-Test Para 설정 원격 호출 (2026-10-14): Parameter 등록 / GDS Chain + Spec 편집 / GDS Chain 참고 목록.
    /// 계약 설명은 DACrux.SEMDMS.Interface.iGdsEtResult (iGdsEtResult_Para.cs) 를 본다.
    /// </summary>
    public partial class GdsEtResult
    {
        #region [Parameter 등록 (TQP_GDS_ET_ITEM)]

        public DataTable GetEtParaProgramList(string strFactory)
        {
            try
            {
                return m_OBJ.GetEtParaProgramList(strFactory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtParameterList(string strFactory, string strDeviceID, string strTestProgram)
        {
            try
            {
                return m_OBJ.GetEtParameterList(strFactory, strDeviceID, strTestProgram);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtParameterSetupList(string strFactory, string strDeviceID, string strTestProgram)
        {
            try
            {
                return m_OBJ.GetEtParameterSetupList(strFactory, strDeviceID, strTestProgram);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable RegisterEtParameters(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            try
            {
                return m_OBJ.RegisterEtParameters(aKey, aRows, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SaveEtParameterSetup(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            try
            {
                return m_OBJ.SaveEtParameterSetup(aKey, aRows, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEtParameters(string[] aKey, string[] aParameters, string strUpdateUser)
        {
            try
            {
                return m_OBJ.DeleteEtParameters(aKey, aParameters, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [GDS Chain 참고 목록]

        public string GetEtGdsDevice(string strFactory, string strDeviceID)
        {
            try
            {
                return m_OBJ.GetEtGdsDevice(strFactory, strDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtGdsChainList(string strFactory, string strGdsDeviceID)
        {
            try
            {
                return m_OBJ.GetEtGdsChainList(strFactory, strGdsDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
