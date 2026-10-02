using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	/// <summary>GDS PRESENTATION 레코드의 문자 가로 기준점을 표현한다.</summary>
	internal enum GdsTextHorizontalPresentation
	{
		Left,
		Center,
		Right
	}

	/// <summary>GDS PRESENTATION 레코드의 문자 세로 기준점을 표현한다.</summary>
	internal enum GdsTextVerticalPresentation
	{
		Top,
		Middle,
		Bottom
	}

	/// <summary>GDS TEXT 요소의 문자열, 삽입 좌표, 기준점 정보를 보관한다.</summary>
	internal class GdsText : GdsElement
	{
		public string Text;
		public GPoint Position;
		public int TextType;
		public int FontNumber;
		public GdsTextHorizontalPresentation HorizontalPresentation = GdsTextHorizontalPresentation.Left;
		public GdsTextVerticalPresentation VerticalPresentation = GdsTextVerticalPresentation.Top;
	}
}
