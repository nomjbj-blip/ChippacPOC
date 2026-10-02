using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap.Gds
{
	public class GdsLayer : IComparable<GdsLayer>
	{
		public GdsLayer()
		{

		}

		public GdsLayer(int layerID)
		{
			LayerID = layerID;
		}

		public int LayerID { get; set; }
		public bool Visible { get; set; } = true;
		public GdsElementList Elements { get; private set; } = new GdsElementList();

		public int CompareTo(GdsLayer other)
		{
			return LayerID.CompareTo(other.LayerID);
		}

		public override string ToString()
		{
			return $"LayerID:{LayerID}, Elements:{Elements.Count}, Visible={Visible}";
		}
	}

	public class GdsLayerList : List<GdsLayer>
	{
		public List<GdsElement> DuplicatedItems { get; set; } = new List<GdsElement>();

		public void AddElement(GdsElement el)
		{ 
			var layer = new GdsLayer(el.LayerID);
			int idx = BinarySearch(layer);

			if (idx < 0)
				Insert(~idx, layer);
			else
				layer = this[idx];

			idx = layer.Elements.BinarySearch(el);

			if (idx < 0)
				layer.Elements.Insert(~idx, el);
			else
				DuplicatedItems.Add(el);
		}

		public GdsLayer GetLayer(int layerID)
		{
			int idx = BinarySearch(new GdsLayer(layerID));

			if (idx < 0)
				throw new Exception($"Not found Layer: {layerID}");

			return this[idx];
		}
	}
}
