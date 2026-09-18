using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPNet.Server.RI;
using Miracom.File;

namespace DACrux.RPNet.BSL
{
    public class TSKMapGen : Miracom.Middleware.QueryComponent, ITSKMapGen
    {
        public void TSK_Write(string FileName, object TskHeader, object TskMapData)
        {
            TSKUtil.WriteTSK(FileName, (TSKHEADER)TskHeader, (TSKMAPDATA[])TskMapData);
        }
    }
}
