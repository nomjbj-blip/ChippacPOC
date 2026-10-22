using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    /// <summary>기존 DMS 원격 호출 방식으로 GDS Chain 저장 / 조회 / 삭제 서비스를 화면에 제공한다.</summary>
    public class GdsChain
    {
        DACrux.SEMDMS.Interface.iGdsChain m_OBJ;

        public GdsChain()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iGdsChain),
                        strUrl + "/DACrux.SEMDMS.BSL.GdsChain.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iGdsChain;
        }

        #region [TQP_GDS_CHAIN]

        public DataTable GetChainList(string strFactory, string strDeviceID)
        {
            try
            {
                return m_OBJ.GetChainList(strFactory, strDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteChain(string strFactory, string strDeviceID, string strChainSeq, string strUpdateUser)
        {
            try
            {
                return m_OBJ.DeleteChain(strFactory, strDeviceID, strChainSeq, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_CHAIN_VER]

        public DataTable BeginChainSave(string[] ChainInfo, string[,] aLayers, string[,] aRules)
        {
            try
            {
                return m_OBJ.BeginChainSave(ChainInfo, aLayers, aRules);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateChainMemberBatch(string[] aKey, string[,] aParas, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CreateChainMemberBatch(aKey, aParas, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateChainEditBatch(string[] aKey, string[,] aParas, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CreateChainEditBatch(aKey, aParas, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CompleteChainSave(string[] aKey, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CompleteChainSave(aKey, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [Chain 상세 조회]

        public DataTable GetChainLayers(string strFactory, string strDeviceID, string strChainSeq)
        {
            try
            {
                return m_OBJ.GetChainLayers(strFactory, strDeviceID, strChainSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetChainRules(string strFactory, string strDeviceID, string strChainSeq)
        {
            try
            {
                return m_OBJ.GetChainRules(strFactory, strDeviceID, strChainSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetChainMembers(string strFactory, string strDeviceID, string strChainSeq)
        {
            try
            {
                return m_OBJ.GetChainMembers(strFactory, strDeviceID, strChainSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetChainEdits(string strFactory, string strDeviceID, string strChainSeq)
        {
            try
            {
                return m_OBJ.GetChainEdits(strFactory, strDeviceID, strChainSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [Chain 상세 범위 조회 (2026-10-21)]

        public DataTable GetChainLayersRange(string strFactory, string strDeviceID, string strFromSeq, string strToSeq)
        {
            try
            {
                return m_OBJ.GetChainLayersRange(strFactory, strDeviceID, strFromSeq, strToSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetChainRulesRange(string strFactory, string strDeviceID, string strFromSeq, string strToSeq)
        {
            try
            {
                return m_OBJ.GetChainRulesRange(strFactory, strDeviceID, strFromSeq, strToSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetChainMembersRange(string strFactory, string strDeviceID, string strFromSeq, string strToSeq)
        {
            try
            {
                return m_OBJ.GetChainMembersRange(strFactory, strDeviceID, strFromSeq, strToSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetChainEditsRange(string strFactory, string strDeviceID, string strFromSeq, string strToSeq)
        {
            try
            {
                return m_OBJ.GetChainEditsRange(strFactory, strDeviceID, strFromSeq, strToSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [Chain 일괄 저장]

        public DataTable CreateChainBulk(string[] aKey, string[,] aChains, string[,] aLayers, string[,] aMembers, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CreateChainBulk(aKey, aChains, aLayers, aMembers, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetChainCodeList(string strFactory, string strDeviceID)
        {
            try
            {
                return m_OBJ.GetChainCodeList(strFactory, strDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteChainBulk(string strFactory, string strDeviceID, string[] aChainSeqs, string strUpdateUser)
        {
            try
            {
                return m_OBJ.DeleteChainBulk(strFactory, strDeviceID, aChainSeqs, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateChainColorBulk(string strFactory, string strDeviceID, string[] aChainSeqs, string strColorArgb, string strUpdateUser)
        {
            try
            {
                return m_OBJ.UpdateChainColorBulk(strFactory, strDeviceID, aChainSeqs, strColorArgb, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
