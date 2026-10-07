using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public struct GPoint
	{
		public double X, Y;

		public GPoint(double x, double y)
		{
			X = Math.Round(x, GdsLibrary.DECIMAL_LENGTH); 
			Y = Math.Round(y, GdsLibrary.DECIMAL_LENGTH);
		}

		public static readonly int SIZE;

		static GPoint()
		{
			SIZE = Marshal.SizeOf(typeof(GPoint));
		}

		public static byte[] ArrayToBytes(GPoint[] points)
		{
			if (points == null || points.Length == 0)
				return null;

			byte[] bytes = new byte[points.Length * SIZE];

			GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);

			try
			{
				IntPtr ptr = handle.AddrOfPinnedObject();
				Marshal.Copy(ptr, bytes, 0, bytes.Length);
			}
			finally
			{
				handle.Free();
			}

			return bytes;
		}

		public static GPoint[] BytesToArray(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return new GPoint[0];

			if (bytes.Length % SIZE != 0)
				throw new ArgumentException("잘못된 데이터 크기입니다.");

			int count = bytes.Length / SIZE;
			GPoint[] points = new GPoint[count];

			GCHandle handle = GCHandle.Alloc(points, GCHandleType.Pinned);

			try
			{
				IntPtr ptr = handle.AddrOfPinnedObject();
				Marshal.Copy(bytes, 0, ptr, bytes.Length);
			}
			finally
			{
				handle.Free();
			}

			return points;
		}

		public override string ToString()
		{
			return $"X={X}, Y={Y}";
		}
	}
}
