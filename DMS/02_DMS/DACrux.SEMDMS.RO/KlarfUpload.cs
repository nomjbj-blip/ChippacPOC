using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    /// <summary>기존 DMS 원격 호출 방식으로 KLARF 업로드 / DB 적재 서비스를 화면에 제공한다. (F08)</summary>
    public class KlarfUpload
    {
        DACrux.SEMDMS.Interface.iKlarfUpload m_OBJ;

        public KlarfUpload()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iKlarfUpload),
                        strUrl + "/DACrux.SEMDMS.BSL.KlarfUpload.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iKlarfUpload;
        }

        /// <summary>
        /// 원격 호출 대신 주어진 구현을 쓴다. 화면 검사(Verification/KlarfUploadFormSmoke)가 Service 없이 BSL 을 같은 프로세스에서
        /// 붙여 화면 -> RO -> BSL 전체 흐름을 확인할 때만 사용한다. 화면은 기본 생성자(Remoting)를 쓴다.
        /// </summary>
        public KlarfUpload(DACrux.SEMDMS.Interface.iKlarfUpload service)
        {
            if (service == null) throw new ArgumentNullException("service");
            m_OBJ = service;
        }

        #region [KLARF 업로드]

        public DataTable BeginUpload(string[] UploadInfo, byte[] KlarfFile)
        {
            try
            {
                return m_OBJ.BeginUpload(UploadInfo, KlarfFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddImage(string strUploadKey, string strFileName, byte[] ImageFile)
        {
            try
            {
                m_OBJ.AddImage(strUploadKey, strFileName, ImageFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CommitUpload(string strUploadKey, string[] Options)
        {
            try
            {
                return m_OBJ.CommitUpload(strUploadKey, Options);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CancelUpload(string strUploadKey)
        {
            try
            {
                m_OBJ.CancelUpload(strUploadKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteUploadedStep(string strStepSeq, string strWaferSeq, string strUser)
        {
            try
            {
                return m_OBJ.DeleteUploadedStep(strStepSeq, strWaferSeq, strUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
