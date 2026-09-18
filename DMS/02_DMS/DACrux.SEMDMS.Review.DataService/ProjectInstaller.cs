using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace DACrux.TEST.PCM.DataService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            SetCustomServiceName();
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            SetCustomServiceName();
            base.Uninstall(savedState);
        }

        private void SetCustomServiceName()
        {
#if SINGLE_EQUIP
            string assm = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string serviceName = System.IO.Path.GetFileNameWithoutExtension(assm);
            
            if (serviceInstaller1.ServiceName != serviceName)
            {
                serviceInstaller1.ServiceName = serviceName;
                serviceInstaller1.DisplayName = serviceName;
            }
#endif
        }
    }
}
