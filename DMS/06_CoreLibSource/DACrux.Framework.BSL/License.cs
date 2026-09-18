using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.Framework.BSL
{
    public class License : Miracom.Middleware.BaseComponent
    {
        public License()
        {
        }

        public bool LicenseCheck(string IPAddress)
        {
            return this.ChkLicense(IPAddress);
        }
    }
}
