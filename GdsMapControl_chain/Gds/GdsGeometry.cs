
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public static class GdsGeometry
	{
		public static GBox ElementBounds(GdsElement e)
		{
			if (e is GdsBoundary)
			{
				return Bounds(((GdsBoundary)e).Points);
			}
			else if (e is GdsPath)
			{
				var p = (GdsPath)e; var b = Bounds(p.Points);
				double r = Math.Abs(p.Width) / 2; if (!b.IsEmpty) { b.MinX -= r; b.MaxX += r; b.MinY -= r; b.MaxY += r; }
				return b;
			}
			else if (e is GdsText)
			{ 
				var p = ((GdsText)e).Position; return new GBox(p.X, p.Y, p.X, p.Y);
			}
			else if (e is GdsSRef)
			{
				var p = ((GdsSRef)e).Origin; return new GBox(p.X, p.Y, p.X, p.Y);
			}
			else if (e is GdsARef) 
			{ 
				var a = (GdsARef)e; var b = GBox.Empty; b.Include(a.Origin); b.Include(a.ColVector); b.Include(a.RowVector); return b;
			}
			return GBox.Empty;
		}

		public static GBox Bounds(List<GPoint> p)
		{
			GBox b = GBox.Empty;
			
			foreach (var x in p) 
				b.Include(x.X, x.Y); 
			
			return b;
		}

		public static GBox Bounds(GPoint[] p)
		{
			GBox b = GBox.Empty;

			foreach (var x in p)
				b.Include(x.X, x.Y);

			return b;
		}

		public static Matrix CreateTransform(GTransform t)
		{
			Matrix m = new Matrix();

			if (t.MirrorX)
				m.Scale(1, -1, MatrixOrder.Append);

			if (t.Magnification != 0 && t.Magnification != 1)
				m.Scale((float)t.Magnification, (float)t.Magnification, MatrixOrder.Append);

			if (t.Rotation != 0) 
				m.Rotate((float)t.Rotation, MatrixOrder.Append);

			return m;
		}

		public static PointF ToPoint(GPoint p, double ox = 0, double oy = 0, double scale = 1)
		{ 
			return new PointF((float)((p.X - ox) * scale), (float)((p.Y - oy) * scale));
		}
	}
}
