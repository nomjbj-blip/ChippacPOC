using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap.Oracle
{
	public static class OracleLoadUtil
	{
		public static readonly string CONNECTION_STRING = "Data Source=ORCL;User Id=qmsmgr;Password=qmsmgr";

		public enum Col
		{
			LAYER,
			ELEMENT_TYPE,
			ELEMENT_NAME,
			POINTS,
			WIDTH,
			PATH_TYPE
		}

		public static List<GdsElement> LoadElements(int gdsSeq, string structureName)
		{
			var list = new List<GdsElement>();

			using (var conn = new OracleConnection(CONNECTION_STRING))
			using (var comm = new OracleCommand(GetSelectQuery(), conn))
			{
				comm.InitialLOBFetchSize = -1;

				comm.Parameters.Add(":GDS_SEQ", gdsSeq);
				comm.Parameters.Add(":STRUCTURE_NAME", structureName);

				conn.Open();

				using (var dr = comm.ExecuteReader())
				{
					while (dr.Read())
					{
						var e = GetGdsElement(dr);
						list.Add(e);
					}
				}
			}

			return list;
		}

		private static GdsElement GetGdsElement(IDataReader dr)
		{
			int i = 0;

			var layer = dr.GetInt32(i++);// LAYER
			var elementType = dr.GetString(i++);// ELEMENT_TYPE
			var elementName = GetString(dr, i++);// ELEMENT_NAME

			if (elementType == "BOUNDARY")
			{
				var e = new GdsBoundary();
				e.LayerID = layer;// LAYER
				e.Name = elementName;
				e.Points = GPoint.BytesToArray(GetBytes(dr, i++));
				e.SetBounds();
				return e;
			}
			else if (elementType == "PATH")
			{
				var e = new GdsPath();
				e.LayerID = layer;// LAYER
				e.Name = elementName;
				e.Points = GPoint.BytesToArray(GetBytes(dr, i++));
				e.Width = dr.GetDouble(i++);//WIDTH
				e.PathType = dr.GetByte(i++);//PATH_TYPE
				e.SetBounds();
				return e;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		private static string GetSelectQuery()
		{
			return @"
SELECT
	LAYER,
	ELEMENT_TYPE,
	ELEMENT_NAME,
	POINTS,
	WIDTH,
	PATH_TYPE
FROM GDS_ELEMENT
WHERE 1 = 1
	AND GDS_SEQ = :GDS_SEQ
	AND STRUCTURE_NAME = :STRUCTURE_NAME
";
		}

		private static byte[] BUFFER = new byte[65536];

		private static byte[] GetBytes(IDataReader dr, int index)
		{
			var bytes = new byte[dr.GetBytes(index, 0, BUFFER, 0, BUFFER.Length)];
			Buffer.BlockCopy(BUFFER, 0, bytes, 0, bytes.Length);
			return bytes;
		}

		private static string GetString(IDataReader dr, int index)
		{
			if (dr.IsDBNull(index))
				return null;

			return dr.GetString(index);
		}
	}

	public static class OracleLoadUtil_DataTable_Version
	{
		public enum Col
		{
			LAYER,
			ELEMENT_TYPE,
			ELEMENT_NAME,
			POINTS,
			WIDTH,
			PATH_TYPE
		}

		public static readonly string CONNECTION_STRING = "Data Source=ORCL;User Id=qmsmgr;Password=qmsmgr";

		public static List<GdsElement> LoadElements(int gdsSeq, string structureName)
		{
			var dt = new DataTable();
			var list = new List<GdsElement>();

			using (var conn = new OracleConnection(CONNECTION_STRING))
			using (var comm = new OracleCommand(GetSelectQuery(), conn))
			using (var da = new OracleDataAdapter(comm))
			{
				comm.Parameters.Add(":GDS_SEQ", gdsSeq);
				comm.Parameters.Add(":STRUCTURE_NAME", structureName);

				da.Fill(dt);
			}

			foreach (DataRow row in dt.Rows)
			{
				var elementType = (string)row[(int)Col.ELEMENT_TYPE];

				if (elementType == "BOUNDARY")
				{
					var e = new GdsBoundary();
					e.LayerID = (int)row[(int)Col.LAYER];
					e.Points = GPoint.BytesToArray((byte[])row[(int)Col.POINTS]);
					list.Add(e);
				}
				else if (elementType == "PATH")
				{
					var e = new GdsPath();
					e.LayerID = (int)row[(int)Col.LAYER];
					e.Points = GPoint.BytesToArray((byte[])row[(int)Col.POINTS]);
					e.Width = (double)(decimal)row[(int)Col.WIDTH];
					e.PathType = (byte)(int)row[(int)Col.PATH_TYPE];
					list.Add(e);
				}
				else
				{
					throw new NotSupportedException();
				}
			}

			return list;
		}

		private static string GetSelectQuery()
		{
			return @"
SELECT
	LAYER,
	ELEMENT_TYPE,
	ELEMENT_NAME,
	POINTS,
	WIDTH,
	PATH_TYPE
FROM GDS_ELEMENT
WHERE 1 = 1
	AND GDS_SEQ = :GDS_SEQ
	AND STRUCTURE_NAME = :STRUCTURE_NAME
";
		}
	}
}
