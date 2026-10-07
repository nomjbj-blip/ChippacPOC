using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public class GdsARef : GdsElement
	{
		public string StructureName;
		public int Columns;
		public int Rows;
		public GPoint Origin;
		public GPoint ColVector;
		public GPoint RowVector;
	}
}
