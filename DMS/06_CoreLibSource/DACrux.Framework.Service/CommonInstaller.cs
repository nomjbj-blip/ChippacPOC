using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace DACrux.Framework.Service
{
    [RunInstaller(true)]
    public partial class CommonInstaller : Installer
    {
        #region Creator

        public CommonInstaller()
        {
            InitializeComponent();
        }

        #endregion
    }
}