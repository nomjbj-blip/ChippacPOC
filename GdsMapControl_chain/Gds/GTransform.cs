using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public struct GTransform
	{
		public double Rotation;
		public double Magnification;
		public bool MirrorX;

		public GTransform(double rotation, double magnification, bool mirrorX)
		{
			Rotation = rotation; Magnification = magnification; MirrorX = mirrorX;
		}
		
		public static GTransform Identity { get { return new GTransform(0, 1, false); } }
	}
}
