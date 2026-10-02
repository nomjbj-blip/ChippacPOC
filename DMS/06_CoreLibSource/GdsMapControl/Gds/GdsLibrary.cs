using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public sealed class GdsLibrary
	{
		public static int DECIMAL_LENGTH = 5;

		public string Name { get; set; }
		public double UserUnit { get; set; }       // meters per user unit
		public double DatabaseUnit { get; set; }   // meters per database unit

		public GdsStructureList Structures { get; private set; } = new GdsStructureList();
	}
}
