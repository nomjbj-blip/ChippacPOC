using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public abstract class GdsElement : IComparable<GdsElement>
	{
		public int LayerID;
		public int DataType;
		
		public string Name;
		public string ElementName;
		public bool Visible = true;

		public GTransform Transform = GTransform.Identity;
		public GBox Bounds = GBox.Empty;

		public int CompareTo(GdsElement other)
		{
			if (LayerID == other.LayerID)
			{
				if (ElementName == other.ElementName)
					return Bounds.CompareTo(other.Bounds);
				else
					return ElementName.CompareTo(other.ElementName);
			}
			else
			{
				return LayerID.CompareTo(other.LayerID);
			}
		}

		public void SetBounds()
		{
			Bounds = GdsGeometry.ElementBounds(this);
		}
	}

	public class GdsElementList : List<GdsElement>
	{
	}
}
