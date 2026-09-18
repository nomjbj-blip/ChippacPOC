using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace DACrux.SEMDMS.Inspection.PreConvertingService
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        static void Main()
        {
#if DEBUG
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new DACrux.Framework.Server.DebugForm(new InspectionPreConvertingService()));
#else          
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new InspectionPreConvertingService() 
			};
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
