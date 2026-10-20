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

		/// <summary>
		/// 정렬 기준: Layer -> 종류 -> Bounds. Bounds까지 같으면 내용(DataType / 변환 / 참조 구조 / 좌표 / 폭 / 문자열)을 비교한다.
		/// 읽기(GdsLayerList.AddElement)는 0인 요소를 중복으로 빼므로 내용까지 같은 요소만 빠진다.
		/// 같은 원점의 다른 SREF, 같은 위치의 다른 문자열 TEXT, Bounds가 같은 다른 모양이 빠지지 않게 내용 비교 추가 (2026-10-15)
		/// </summary>
		public int CompareTo(GdsElement other)
		{
			if (LayerID == other.LayerID)
			{
				if (ElementName == other.ElementName)
				{
					int bounds = Bounds.CompareTo(other.Bounds);
					return bounds != 0 ? bounds : CompareContent(other);
				}
				else
					return ElementName.CompareTo(other.ElementName);
			}
			else
			{
				return LayerID.CompareTo(other.LayerID);
			}
		}

		/// <summary>Bounds가 같은 두 요소의 내용을 비교한다. 0이면 완전히 같은 요소다.</summary>
		private int CompareContent(GdsElement other)
		{
			int c = DataType.CompareTo(other.DataType);
			if (c == 0) c = Transform.Rotation.CompareTo(other.Transform.Rotation);
			if (c == 0) c = Transform.Magnification.CompareTo(other.Transform.Magnification);
			if (c == 0) c = Transform.MirrorX.CompareTo(other.Transform.MirrorX);
			if (c != 0) return c;

			if (this is GdsSRef sref && other is GdsSRef otherSref)
			{
				c = String.CompareOrdinal(sref.StructureName, otherSref.StructureName);
				return c != 0 ? c : ComparePoint(sref.Origin, otherSref.Origin);
			}
			if (this is GdsARef aref && other is GdsARef otherAref)
			{
				c = String.CompareOrdinal(aref.StructureName, otherAref.StructureName);
				if (c == 0) c = aref.Columns.CompareTo(otherAref.Columns);
				if (c == 0) c = aref.Rows.CompareTo(otherAref.Rows);
				if (c == 0) c = ComparePoint(aref.Origin, otherAref.Origin);
				if (c == 0) c = ComparePoint(aref.ColVector, otherAref.ColVector);
				if (c == 0) c = ComparePoint(aref.RowVector, otherAref.RowVector);
				return c;
			}
			if (this is GdsPath path && other is GdsPath otherPath)
			{
				c = path.Width.CompareTo(otherPath.Width);
				if (c == 0) c = path.PathType.CompareTo(otherPath.PathType);
				return c != 0 ? c : ComparePoints(path.Points, otherPath.Points);
			}
			if (this is GdsBoundary boundary && other is GdsBoundary otherBoundary)
				return ComparePoints(boundary.Points, otherBoundary.Points);
			if (this is GdsText text && other is GdsText otherText)
			{
				c = String.CompareOrdinal(text.Text, otherText.Text);
				if (c == 0) c = text.TextType.CompareTo(otherText.TextType);
				if (c == 0) c = text.FontNumber.CompareTo(otherText.FontNumber);
				if (c == 0) c = text.HorizontalPresentation.CompareTo(otherText.HorizontalPresentation);
				if (c == 0) c = text.VerticalPresentation.CompareTo(otherText.VerticalPresentation);
				return c != 0 ? c : ComparePoint(text.Position, otherText.Position);
			}
			return 0;
		}

		private static int ComparePoint(GPoint a, GPoint b)
		{
			int c = a.X.CompareTo(b.X);
			return c != 0 ? c : a.Y.CompareTo(b.Y);
		}

		/// <summary>점 수, 그다음 점을 순서대로 비교한다. null은 빈 배열보다 앞이다.</summary>
		private static int ComparePoints(GPoint[] a, GPoint[] b)
		{
			if (a == null || b == null)
				return (a == null ? 0 : 1) - (b == null ? 0 : 1);
			int c = a.Length.CompareTo(b.Length);
			for (int i = 0; c == 0 && i < a.Length; i++)
				c = ComparePoint(a[i], b[i]);
			return c;
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
