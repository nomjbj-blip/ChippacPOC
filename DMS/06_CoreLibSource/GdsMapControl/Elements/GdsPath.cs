using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public class GdsPath : GdsElement
	{
		public GPoint[] Points;
		public byte PathType; // 0 flush, 1 round, 2 extended
		public double Width;
	}
}
