using System;
using System.Data;

namespace DACrux.SEMDMS.RO
{
    /// <summary>기존 DMS 원격 호출 방식으로 GDS × KLARF 연관 분석 서비스를 화면에 제공한다.</summary>
    public class GdsKlarfAnalysis
    {
        DACrux.SEMDMS.Interface.iGdsKlarfAnalysis m_OBJ;

        public GdsKlarfAnalysis()
        {
            string strUrl = DACrux.Base.RemoteConfig.url(DACrux.Base.ApplicationUnit.MIRACOM_DACRUX_DMS);
            object obj = Activator.GetObject(typeof(DACrux.SEMDMS.Interface.iGdsKlarfAnalysis),
                        strUrl + "/DACrux.SEMDMS.BSL.GdsKlarfAnalysis.bin");
            m_OBJ = obj as DACrux.SEMDMS.Interface.iGdsKlarfAnalysis;
        }

        /// <summary>원격 호출 대신 지정한 구현을 사용한다. 화면 Smoke 검사에서 BSL을 같은 프로세스에 붙일 때 쓴다(RO.GdsEtResult와 같은 방식).</summary>
        public GdsKlarfAnalysis(DACrux.SEMDMS.Interface.iGdsKlarfAnalysis service)
        {
            m_OBJ = service;
        }

        public DataSet AnalyzeWafer(string strFactory, string strDeviceID, long lStepSeq, string strViewSide,
            double dToleranceUm, bool bUseDefectSize, double dOffsetXUm, double dOffsetYUm,
            string[] aChainSeqs, int[] aClassNumbers)
        {
            try
            {
                return m_OBJ.AnalyzeWafer(strFactory, strDeviceID, lStepSeq, strViewSide, dToleranceUm, bUseDefectSize,
                    dOffsetXUm, dOffsetYUm, aChainSeqs, aClassNumbers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
