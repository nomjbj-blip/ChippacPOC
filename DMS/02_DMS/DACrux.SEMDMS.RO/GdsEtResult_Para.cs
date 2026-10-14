using System;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    /// <summary>
    /// E-Test Para Setup 원격 호출 (2026-10-14): Parameter 등록 / 대표 Chain / GDS 연결.
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

        public int SaveEtParameters(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            try
            {
                return m_OBJ.SaveEtParameters(aKey, aRows, strUpdateUser);
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

        #region [대표 Chain / GDS 연결]

        public DataTable GetEtTerminalList(string strFactory, string strGdsDeviceID)
        {
            try
            {
                return m_OBJ.GetEtTerminalList(strFactory, strGdsDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtRepChainList(string strFactory, string strGdsDeviceID)
        {
            try
            {
                return m_OBJ.GetEtRepChainList(strFactory, strGdsDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtGdsChainLinkList(string strFactory, string strGdsDeviceID)
        {
            try
            {
                return m_OBJ.GetEtGdsChainLinkList(strFactory, strGdsDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveEtRepChain(string[] aKey, string[] aTerminals, string strUpdateUser)
        {
            try
            {
                return m_OBJ.SaveEtRepChain(aKey, aTerminals, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RenameEtRepChain(string[] aKey, string strUpdateUser)
        {
            try
            {
                return m_OBJ.RenameEtRepChain(aKey, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemoveEtRepChainMembers(string[] aKey, string[] aTerminals, string strUpdateUser)
        {
            try
            {
                return m_OBJ.RemoveEtRepChainMembers(aKey, aTerminals, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEtRepChain(string[] aKey, string strUpdateUser)
        {
            try
            {
                return m_OBJ.DeleteEtRepChain(aKey, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable LinkEtRepChainsByName(string strFactory, string strGdsDeviceID, string strUpdateUser)
        {
            try
            {
                return m_OBJ.LinkEtRepChainsByName(strFactory, strGdsDeviceID, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveEtChainLink(string[] aKey, string strUpdateUser)
        {
            try
            {
                return m_OBJ.SaveEtChainLink(aKey, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEtChainLink(string[] aKey, string strUpdateUser)
        {
            try
            {
                return m_OBJ.DeleteEtChainLink(aKey, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtRepChainSpecInfo(string strFactory, string strGdsDeviceID, string strRepChainName)
        {
            try
            {
                return m_OBJ.GetEtRepChainSpecInfo(strFactory, strGdsDeviceID, strRepChainName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
