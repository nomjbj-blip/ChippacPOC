using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public sealed class GdsLibrary
	{
		public static int DECIMAL_LENGTH = 5;

		public string Name { get; set; }
		// 아래 두 값은 GdsReader.LengthUnit 을 곱한 값이다. DatabaseUnit = DB 단위 1개의 길이(Micrometer 로 읽으면 µm, 좌표 계산에 사용) (2026-10-16 주석 정정)
		public double UserUnit { get; set; }       // 파일 UNITS 첫째 값 x LengthUnit (좌표 계산에 쓰지 않음)
		public double DatabaseUnit { get; set; }   // 파일 UNITS 둘째 값 x LengthUnit

		// 파일 UNITS 원값. TQP_GDS_MAP.USER_UNIT / DATABASE_UNIT 에 이 값을 저장한다 (2026-10-16)
		public double FileUserUnit { get; set; }       // DB 단위 1개의 크기(사용자 단위), 보통 0.001
		public double FileDatabaseUnit { get; set; }   // DB 단위 1개의 크기(m), 보통 1E-9

		public GdsStructureList Structures { get; private set; } = new GdsStructureList();

		/// <summary>
		/// 최상위 Cell 목록: SREF / AREF 로 한 번도 참조되지 않은 Structure 를 펼친 도형 수가 많은 순서로 돌려준다 (2026-10-20).
		/// GDS 는 Structure 기록 순서가 자유라 첫 Structure 가 하위 Cell 일 수 있다(U8: 첫 Structure = U8_PIC_C4).
		/// 펼친 도형 수는 화면 펼치기(FlattenStructure)와 같은 기준이다: BOUNDARY / PATH / TEXT 를 세고 SREF 는 자식 수를 더하며 AREF 는 펼치지 않는다.
		/// </summary>
		public List<GdsTopStructureInfo> GetTopStructures()
		{
			var referenced = new HashSet<string>(StringComparer.Ordinal);
			foreach (var structure in Structures)
				foreach (var layer in structure.Layers)
					foreach (var element in layer.Elements)
					{
						var sref = element as GdsSRef;
						if (sref != null) { referenced.Add(sref.StructureName); continue; }
						var aref = element as GdsARef;
						if (aref != null) referenced.Add(aref.StructureName);
					}

			var counts = new Dictionary<string, long>(StringComparer.Ordinal);
			var result = new List<GdsTopStructureInfo>();
			var seen = new HashSet<string>(StringComparer.Ordinal);
			for (int i = 0; i < Structures.Count; i++)
			{
				var structure = Structures[i];
				if (referenced.Contains(structure.Name) || !seen.Add(structure.Name))
					continue;
				result.Add(new GdsTopStructureInfo(structure.Name, CountFlatElements(structure, counts, new HashSet<string>(StringComparer.Ordinal)), i));
			}
			// 도형이 많은 Cell 을 앞에 둔다. 같으면 파일 순서.
			result.Sort((left, right) => left.ElementCount != right.ElementCount
				? right.ElementCount.CompareTo(left.ElementCount) : left.FileOrder.CompareTo(right.FileOrder));
			return result;
		}

		/// <summary>
		/// 처음 그릴 Cell: 펼친 도형이 가장 많은 최상위 Cell (KLayout 이 여는 Cell 과 같음, U8 확인 2026-10-20).
		/// 최상위 Cell 이 하나뿐인 파일(U9 / OMM_TCB)은 지금까지와 같다. 계산할 수 없으면 첫 Structure.
		/// </summary>
		public string GetDefaultTopStructureName()
		{
			List<GdsTopStructureInfo> tops = GetTopStructures();
			if (tops.Count > 0) return tops[0].Name;
			return Structures.Count > 0 ? Structures[0].Name : null;
		}

		/// <summary>Structure 하나를 펼쳤을 때 생기는 도형 수. 같은 Cell 은 한 번만 세고 재귀 참조는 0으로 본다.</summary>
		private long CountFlatElements(GdsStructure structure, Dictionary<string, long> counts, HashSet<string> visiting)
		{
			long cached;
			if (counts.TryGetValue(structure.Name, out cached)) return cached;
			if (!visiting.Add(structure.Name)) return 0;

			long total = 0;
			foreach (var layer in structure.Layers)
				foreach (var element in layer.Elements)
				{
					var sref = element as GdsSRef;
					if (sref != null)
					{
						GdsStructure child;
						if (Structures.TryGetValue(sref.StructureName, out child))
							total += CountFlatElements(child, counts, visiting);
					}
					else if (!(element is GdsARef))
						total++;
				}

			visiting.Remove(structure.Name);
			counts[structure.Name] = total;
			return total;
		}
	}

	/// <summary>최상위 Cell 이름과 펼친 도형 수 (2026-10-20)</summary>
	public sealed class GdsTopStructureInfo
	{
		public GdsTopStructureInfo(string name, long elementCount, int fileOrder)
		{
			Name = name;
			ElementCount = elementCount;
			FileOrder = fileOrder;
		}

		public string Name { get; private set; }
		public long ElementCount { get; private set; }
		public int FileOrder { get; private set; }
	}
}
