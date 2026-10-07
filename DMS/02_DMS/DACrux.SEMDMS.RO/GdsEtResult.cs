using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    /// <summary>기존 DMS 원격 호출 방식으로 E-Test 결과 파일 저장 / 조회 / 삭제 서비스를 화면에 제공한다. (CLAUDE-029)</summary>
    public class GdsEtResult
    {
        DACrux.SEMDMS.Interface.iGdsEtResult m_OBJ;

        public GdsEtResult()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iGdsEtResult),
                        strUrl + "/DACrux.SEMDMS.BSL.GdsEtResult.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iGdsEtResult;
        }

        /// <summary>원격 호출 대신 지정한 구현을 사용한다. 화면 Smoke 검사에서 BSL을 같은 프로세스에 붙일 때 쓴다(RO.KlarfUpload와 같은 방식).</summary>
        public GdsEtResult(DACrux.SEMDMS.Interface.iGdsEtResult service)
        {
            m_OBJ = service;
        }

        #region [TQP_GDS_SRC_FILE]

        public DataTable GetEtFileByKey(string[] aKey)
        {
            try
            {
                return m_OBJ.GetEtFileByKey(aKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtFile(string strSrcFileSeq)
        {
            try
            {
                return m_OBJ.GetEtFile(strSrcFileSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtFileList(string strFactory, string strDeviceID)
        {
            try
            {
                return m_OBJ.GetEtFileList(strFactory, strDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEtFile(string strSrcFileSeq, string strUpdateUser)
        {
            try
            {
                return m_OBJ.DeleteEtFile(strSrcFileSeq, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BeginEtFile(string[] FileInfo)
        {
            try
            {
                return m_OBJ.BeginEtFile(FileInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CompleteEtFile(string strSrcFileSeq, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CompleteEtFile(strSrcFileSeq, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_ET_ITEM]

        public DataTable MergeEtItem(string[] aKey, string[,] aItems, string strUpdateUser)
        {
            try
            {
                return m_OBJ.MergeEtItem(aKey, aItems, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtItemList(string strFactory, string strDeviceID)
        {
            try
            {
                return m_OBJ.GetEtItemList(strFactory, strDeviceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_ET_DIE]

        public int CreateEtDieBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CreateEtDieBatch(strSrcFileSeq, aParas, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtDieList(string strSrcFileSeq)
        {
            try
            {
                return m_OBJ.GetEtDieList(strSrcFileSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [TQP_GDS_ET_VALUE]

        public int CreateEtValueBatch(string strSrcFileSeq, string[,] aParas, string strUpdateUser)
        {
            try
            {
                return m_OBJ.CreateEtValueBatch(strSrcFileSeq, aParas, strUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEtValueList(string strSrcFileSeq)
        {
            try
            {
                return m_OBJ.GetEtValueList(strSrcFileSeq);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
