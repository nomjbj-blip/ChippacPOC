using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap.Oracle
{
	public static class OracleSaveUtil
	{
		public static readonly int BATCH_SIZE = 50000;
		public static readonly string CONNECTION_STRING = "Data Source=ORCL;User Id=qmsmgr;Password=qmsmgr";

		public enum Col
		{
			GDS_SEQ,
			STRUCTURE_NAME,
			LAYER,
			SEQ,
			ELEMENT_TYPE,
			ELEMENT_NAME,
			POINTS,
			WIDTH,
			PATH_TYPE
		}

		public static void SaveElements(List<GdsElement> list)
		{
			var dt = GetEmptyTable();

			using (var bulk = new OracleBulkCopy(CONNECTION_STRING, OracleBulkCopyOptions.Default))
			{
				bulk.DestinationTableName = "GDS_ELEMENT";
				bulk.BatchSize = BATCH_SIZE;
				bulk.BulkCopyTimeout = 60;

				foreach (DataColumn col in dt.Columns)
				{
					bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);
				}

				int seq = 0;
				int total = list.Count;

				for (int i = 0; i < total; i++)
				{
					var e = list[i];

					GPoint[] points = null;
					string elementType = null;
					object width = null;
					object pathType = null;

					if (e is GdsBoundary objBdry)
					{
						elementType = "BOUNDARY";
						points = objBdry.Points;
						width = DBNull.Value;
						pathType = DBNull.Value;
					}
					else if (e is GdsPath objPath)
					{
						elementType = "PATH";
						points = objPath.Points;
						width = objPath.Width;
						pathType = objPath.PathType;
					}
					else
					{
						continue;
					}

					var row = dt.NewRow();
					dt.Rows.Add(row);
					seq++;

					row[(int)Col.GDS_SEQ] = 2;
					row[(int)Col.STRUCTURE_NAME] = "TOP";
					row[(int)Col.LAYER] = e.LayerID;
					row[(int)Col.SEQ] = seq;
					row[(int)Col.ELEMENT_TYPE] = elementType;
					row[(int)Col.ELEMENT_NAME] = DBNull.Value;
					row[(int)Col.POINTS] = GPoint.ArrayToBytes(points);
					row[(int)Col.WIDTH] = width;
					row[(int)Col.PATH_TYPE] = pathType;

					if (seq % BATCH_SIZE == 0 || i == total - 1)
					{
						bulk.WriteToServer(dt);
						dt.Clear();
					}
				}
			}
		}

		private static DataTable GetEmptyTable()
		{
			var dt = new DataTable();
			dt.Columns.Add(Col.GDS_SEQ.ToString(), typeof(int));
			dt.Columns.Add(Col.STRUCTURE_NAME.ToString(), typeof(string));
			dt.Columns.Add(Col.LAYER.ToString(), typeof(int));
			dt.Columns.Add(Col.SEQ.ToString(), typeof(int));
			dt.Columns.Add(Col.ELEMENT_TYPE.ToString(), typeof(string));
			dt.Columns.Add(Col.ELEMENT_NAME.ToString(), typeof(string));
			dt.Columns.Add(Col.POINTS.ToString(), typeof(byte[]));
			dt.Columns.Add(Col.WIDTH.ToString(), typeof(double));
			dt.Columns.Add(Col.PATH_TYPE.ToString(), typeof(int));
			return dt;
		}
	}
}
