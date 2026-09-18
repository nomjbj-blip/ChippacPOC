using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DACrux.SEMDMS.Interface
{
    public interface iSEMControl
    {
        #region [ DM Search Control ]

        void DrawWafer(DACrux.Base.DPWafer[] wafer);

        #endregion
    }

    public interface iFileControl
    {
        void DrawWafer(DACrux.Base.DefectList defectList);
    }
}
