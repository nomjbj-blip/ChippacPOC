using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	[Serializable]
	public struct GBox : IComparable<GBox>
	{
		public double MinX, MinY, MaxX, MaxY;

		public GBox(double minX, double minY, double maxX, double maxY)
		{
			MinX = minX;
			MinY = minY;
			MaxX = maxX;
			MaxY = maxY;
		}

		public bool IsEmpty { get { return MaxX < MinX || MaxY < MinY; } }
		
		public double Width { get { return IsEmpty ? 0 : Math.Round(MaxX - MinX, GdsLibrary.DECIMAL_LENGTH); } }
		
		public double Height { get { return IsEmpty ? 0 : Math.Round(MaxY - MinY, GdsLibrary.DECIMAL_LENGTH); } }
		
		public void Include(double x, double y)
		{
			if (IsEmpty) { MinX = MaxX = x; MinY = MaxY = y; return; }
			if (x < MinX) MinX = x; if (x > MaxX) MaxX = x;
			if (y < MinY) MinY = y; if (y > MaxY) MaxY = y;
		}
		
		public void Include(GBox b)
		{
			if (b.IsEmpty) return;
			Include(b.MinX, b.MinY); Include(b.MaxX, b.MaxY);
		}

		public void Include(GPoint p)
		{
			Include(p.X, p.Y);
		}

		public bool IntersectsWith(GBox other)
		{
			return this.MinX <= other.MaxX &&
			   this.MaxX >= other.MinX &&
			   this.MinY <= other.MaxY &&
			   this.MaxY >= other.MinY;
		}

		public static GBox Empty { get { return new GBox(1, 1, 0, 0); } }

		public override string ToString()
		{
			return $"MinX={MinX}, MinY={MinY}, MaxX={MaxX}, MaxY={MaxY}, Width={Width}, Height={Height}";
		}

		public int CompareTo(GBox other)
		{
			// 정렬 기준 : 좌상단 -> 우하단

			if (MinX == other.MinX)
				if (MinY == other.MinY)
					if (MaxX == other.MaxX)
						return MaxY.CompareTo(other.MaxY);
					else
						return MaxX.CompareTo(other.MaxX);
				else
					return MinY.CompareTo(other.MinY);
			else
				return MinX.CompareTo(other.MinX);
		}
	}
}