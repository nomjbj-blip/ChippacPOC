using System.ServiceProcess;

namespace DACrux.RPNet.Service
{
    class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        static void Main()
        {
#if DEBUG
            System.Windows.Forms.Application.Run(new Debug());
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new DACruxRPNet() 
			};
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
