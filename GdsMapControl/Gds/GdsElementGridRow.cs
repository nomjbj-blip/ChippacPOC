using System;
using System.ComponentModel;

namespace NexplantQMS.GdsMap
{
	/// <summary>
	/// GDS Element 원본을 복사하지 않고 DataGridView의 상세 헤더로 노출하는 조회 전용 행이다.
	/// 좌표 전체 문자열은 대용량 메모리를 사용하므로 좌표 수와 시작/끝 좌표만 제공한다.
	/// </summary>
	public sealed class GdsElementGridRow
	{
		private readonly GdsLibrary _library;
		private readonly GdsStructure _structure;
		private readonly GdsElement _element;
		private readonly GPoint _firstPoint;
		private readonly GPoint _lastPoint;
		private readonly bool _hasPoint;

		public GdsElementGridRow(GdsLibrary library, GdsStructure structure, int gridNumber, GdsElement element, bool parserDuplicate)
		{
			_library = library;
			_structure = structure;
			GridNumber = gridNumber;
			_element = element;
			ParserDuplicate = parserDuplicate;

			GPoint[] points = element is GdsBoundary boundary ? boundary.Points : element is GdsPath path ? path.Points : null;
			if (points != null && points.Length > 0)
			{
				PointCount = points.Length;
				_firstPoint = points[0];
				_lastPoint = points[points.Length - 1];
				_hasPoint = true;
			}
			else if (element is GdsText text)
			{
				PointCount = 1;
				_firstPoint = _lastPoint = text.Position;
				_hasPoint = true;
			}
			else if (element is GdsSRef sref)
			{
				PointCount = 1;
				_firstPoint = _lastPoint = sref.Origin;
				_hasPoint = true;
			}
			else if (element is GdsARef aref)
			{
				PointCount = 3;
				_firstPoint = aref.Origin;
				_lastPoint = aref.RowVector;
				_hasPoint = true;
			}
		}

		[DisplayName("Library")]
		public string LibraryName { get { return _library.Name; } }
		[DisplayName("User Unit")]
		public double UserUnit { get { return _library.UserUnit; } }
		[DisplayName("Database Unit")]
		public double DatabaseUnit { get { return _library.DatabaseUnit; } }
		[DisplayName("Structure")]
		public string StructureName { get { return _structure.Name; } }
		[DisplayName("Structure MinX")]
		public double? StructureMinX { get { return _structure.Bounds.IsEmpty ? (double?)null : _structure.Bounds.MinX; } }
		[DisplayName("Structure MinY")]
		public double? StructureMinY { get { return _structure.Bounds.IsEmpty ? (double?)null : _structure.Bounds.MinY; } }
		[DisplayName("Structure MaxX")]
		public double? StructureMaxX { get { return _structure.Bounds.IsEmpty ? (double?)null : _structure.Bounds.MaxX; } }
		[DisplayName("Structure MaxY")]
		public double? StructureMaxY { get { return _structure.Bounds.IsEmpty ? (double?)null : _structure.Bounds.MaxY; } }
		[DisplayName("Grid No")]
		public int GridNumber { get; private set; }
		[DisplayName("Parser 중복 제외")]
		public bool ParserDuplicate { get; private set; }
		[DisplayName("Layer")]
		public int LayerId { get { return _element.LayerID; } }
		[DisplayName("Element")]
		public string ElementType { get { return _element.ElementName; } }
		[DisplayName("DataType")]
		public int DataType { get { return _element.DataType; } }
		[DisplayName("Name")]
		public string Name { get { return _element.Name; } }
		[DisplayName("Visible")]
		public bool Visible { get { return _element.Visible; } }

		[DisplayName("Bounds MinX")]
		public double? BoundsMinX { get { return _element.Bounds.IsEmpty ? (double?)null : _element.Bounds.MinX; } }
		[DisplayName("Bounds MinY")]
		public double? BoundsMinY { get { return _element.Bounds.IsEmpty ? (double?)null : _element.Bounds.MinY; } }
		[DisplayName("Bounds MaxX")]
		public double? BoundsMaxX { get { return _element.Bounds.IsEmpty ? (double?)null : _element.Bounds.MaxX; } }
		[DisplayName("Bounds MaxY")]
		public double? BoundsMaxY { get { return _element.Bounds.IsEmpty ? (double?)null : _element.Bounds.MaxY; } }
		[DisplayName("Bounds Width")]
		public double? BoundsWidth { get { return _element.Bounds.IsEmpty ? (double?)null : _element.Bounds.Width; } }
		[DisplayName("Bounds Height")]
		public double? BoundsHeight { get { return _element.Bounds.IsEmpty ? (double?)null : _element.Bounds.Height; } }

		[DisplayName("Point Count")]
		public int PointCount { get; private set; }
		[DisplayName("First X")]
		public double? FirstX { get { return _hasPoint ? (double?)_firstPoint.X : null; } }
		[DisplayName("First Y")]
		public double? FirstY { get { return _hasPoint ? (double?)_firstPoint.Y : null; } }
		[DisplayName("Last X")]
		public double? LastX { get { return _hasPoint ? (double?)_lastPoint.X : null; } }
		[DisplayName("Last Y")]
		public double? LastY { get { return _hasPoint ? (double?)_lastPoint.Y : null; } }
		[DisplayName("Closed")]
		public bool? Closed { get { return _element is GdsBoundary ? (bool?)true : _element is GdsPath ? false : (bool?)null; } }

		[DisplayName("PATH Type")]
		public int? PathType { get { return _element is GdsPath path ? (int?)path.PathType : null; } }
		[DisplayName("PATH Type Name")]
		public string PathTypeName
		{
			get
			{
				var path = _element as GdsPath;
				if (path == null) return null;
				return path.PathType == 0 ? "Flush" : path.PathType == 1 ? "Round" : path.PathType == 2 ? "Extended" : "Unknown";
			}
		}
		[DisplayName("PATH Width")]
		public double? PathWidth { get { return _element is GdsPath path ? (double?)path.Width : null; } }

		[DisplayName("TEXT")]
		public string Text { get { return (_element as GdsText)?.Text; } }
		[DisplayName("TEXT Type")]
		public int? TextType { get { return _element is GdsText text ? (int?)text.TextType : null; } }
		[DisplayName("TEXT Font")]
		public int? TextFont { get { return _element is GdsText text ? (int?)text.FontNumber : null; } }
		[DisplayName("TEXT Horizontal")]
		public string TextHorizontal { get { return (_element as GdsText)?.HorizontalPresentation.ToString(); } }
		[DisplayName("TEXT Vertical")]
		public string TextVertical { get { return (_element as GdsText)?.VerticalPresentation.ToString(); } }
		[DisplayName("TEXT X")]
		public double? TextX { get { return _element is GdsText text ? (double?)text.Position.X : null; } }
		[DisplayName("TEXT Y")]
		public double? TextY { get { return _element is GdsText text ? (double?)text.Position.Y : null; } }

		[DisplayName("Reference Structure")]
		public string ReferenceStructure { get { return _element is GdsSRef sref ? sref.StructureName : _element is GdsARef aref ? aref.StructureName : null; } }
		[DisplayName("Reference Origin X")]
		public double? ReferenceOriginX { get { return _element is GdsSRef sref ? (double?)sref.Origin.X : _element is GdsARef aref ? (double?)aref.Origin.X : null; } }
		[DisplayName("Reference Origin Y")]
		public double? ReferenceOriginY { get { return _element is GdsSRef sref ? (double?)sref.Origin.Y : _element is GdsARef aref ? (double?)aref.Origin.Y : null; } }
		[DisplayName("AREF Columns")]
		public int? ArrayColumns { get { return _element is GdsARef aref ? (int?)aref.Columns : null; } }
		[DisplayName("AREF Rows")]
		public int? ArrayRows { get { return _element is GdsARef aref ? (int?)aref.Rows : null; } }
		[DisplayName("AREF Column X")]
		public double? ArrayColumnX { get { return _element is GdsARef aref ? (double?)aref.ColVector.X : null; } }
		[DisplayName("AREF Column Y")]
		public double? ArrayColumnY { get { return _element is GdsARef aref ? (double?)aref.ColVector.Y : null; } }
		[DisplayName("AREF Row X")]
		public double? ArrayRowX { get { return _element is GdsARef aref ? (double?)aref.RowVector.X : null; } }
		[DisplayName("AREF Row Y")]
		public double? ArrayRowY { get { return _element is GdsARef aref ? (double?)aref.RowVector.Y : null; } }

		[DisplayName("Rotation")]
		public double Rotation { get { return _element.Transform.Rotation; } }
		[DisplayName("Magnification")]
		public double Magnification { get { return _element.Transform.Magnification; } }
		[DisplayName("Mirror X")]
		public bool MirrorX { get { return _element.Transform.MirrorX; } }
	}
}
