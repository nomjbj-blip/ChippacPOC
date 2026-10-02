using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public class Defect
	{
		public double X { get; set; }

		public double Y { get; set; }

		public double Width { get; set; }

		public double Height { get; set; }

		public string DefectName { get; set; }

		public string DefectCode { get; set; }
	}

	public class DefectList : List<Defect>
	{
	}
}
