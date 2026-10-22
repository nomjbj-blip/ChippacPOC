using System;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    /// <summary>
    /// E-Test Bin 설정 원격 호출 (2026-10-22): YMS Program Bin 정의 조회 / 저장 / 삭제.
    /// 계약 설명은 DACrux.SEMDMS.Interface.iGdsEtResult (iGdsEtResult_Bin.cs) 를 본다.
    /// </summary>
    public partial class GdsEtResult
    {
        #region [Bin 설정 (TQP_BINDESC)]

        public DataTable GetEtBinProgramList(string strFactory)
        {
            try
            {
                return m_OBJ.GetEtBinProgramList(strFactory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtBinList(string[] aKey)
        {
            try
            {
                return m_OBJ.GetEtBinList(aKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SaveEtBins(string[] aKey, string[,] aRows, string strUpdateUser)
        {
            try
            {
                return m_OBJ.SaveEtBins(aKey, aRows, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEtBins(string[] aKey, string[] aBins, string strUpdateUser)
        {
            try
            {
                return m_OBJ.DeleteEtBins(aKey, aBins, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
