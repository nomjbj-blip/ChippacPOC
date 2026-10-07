using NexplantQMS.GdsMap.Gds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public class GdsStructure
	{
		public string Name;
		public GdsLayerList Layers { get; private set; } = new GdsLayerList();
		public DefectList DefectList { get; private set; } = new DefectList();
		public GBox Bounds = GBox.Empty;
	}

	public class GdsStructureList : List<GdsStructure>
	{
		public bool TryGetValue(string name, out GdsStructure value)
		{
			value = null;

			foreach (var str in this)
			{
				if (str.Name == name)
				{
					value = str;
					return true;
				}
			}

			return false;
		}
	}
}
