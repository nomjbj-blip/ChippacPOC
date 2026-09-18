using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NexplantQMS.GdsMap
{
	public class GdsReader
	{
		private GdsStructure str = null;
		private GdsElement el = null;
		private GdsLibrary lib = null;

		private List<int> _layerArr = new List<int>();

		public GdsReader()
		{
			LengthUnit = LengthUnit.Meter;
			IsLittleEndian = BitConverter.IsLittleEndian;
		}

		public GdsLibrary Read(string path)
		{
			lib = new GdsLibrary();

			const int READ_BUFFER = 1024 * 1024; // 1MB 버퍼
			// C# 7.3에서도 파일, 버퍼, 리더 자원을 자동 해제하기 위해 기존 using 블록 문법을 사용한다.
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, READ_BUFFER, FileOptions.SequentialScan))
			using (var bs = new BufferedStream(fs, READ_BUFFER))
			using (var br = new BinaryReader(bs))
			{

			long length = fs.Length;
			var bytes = new byte[65536];

			while (bs.Position < length)
			{
				ushort record = BE16(br);
				int dataLength = record - 4; //  헤더 4바이트 제외

				// 2. 레코드 타입 및 데이터 타입 읽기
				byte recordType = br.ReadByte();
				byte dataType = br.ReadByte();

				dataLength = br.Read(bytes, 0, dataLength);

				// 4. 레코드 종류에 따른 분기 처리
				switch (recordType)
				{
					case 0x02: lib.Name = Ascii(bytes, dataLength); break; // LIBNAME
					case 0x03: ReadUnits(bytes); break;
					case 0x05: str = new GdsStructure(); break; // BGNSTR
					case 0x06: str.Name = Ascii(bytes, dataLength); break;
					case 0x07: if (str != null) { str.Bounds = CalculateBounds(str); lib.Structures.Add(str); str = null; } break;
					case 0x08: el = new GdsBoundary(); el.ElementName = "BOUNDARY"; break;
					case 0x09: el = new GdsPath(); el.ElementName = "PATH"; break;
					case 0x0A: el = new GdsSRef(); el.ElementName = "SREF"; break;
					case 0x0B: el = new GdsARef(); el.ElementName = "AREF"; break;
					case 0x0C: el = new GdsText(); el.ElementName = "TEXT"; break;
					case 0x0D: SetLayer(bytes); break;
					case 0x0E: SetDataType(bytes); break;
					case 0x0F: SetWidth(bytes); break;
					case 0x10: SetXY(bytes, dataLength); break;
					case 0x12: SetSName(bytes, dataLength); break;
					case 0x13: SetColRow(bytes); break;
					case 0x16: if (el is GdsText) ((GdsText)el).TextType = I16(bytes); break;
					case 0x19: if (el is GdsText) ((GdsText)el).Text = Ascii(bytes, dataLength); break;
					case 0x1A: SetStrans(bytes); break;
					case 0x1B: SetMag(bytes); break;
					case 0x1C: SetAngle(bytes); break;
					case 0x11: EndElement(); break;
				}
			}

				return lib;
			}
		}

		private ushort BE16(BinaryReader br)
		{
			ushort val = br.ReadUInt16();
			return (ushort)((val << 8) | (val >> 8));
		}

		private static string Ascii(byte[] bytes, int dataLength)
		{
			return Encoding.ASCII.GetString(bytes, 0, dataLength).TrimEnd('\0');
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe short I16(byte[] arr, int index = 0)
		{
			fixed (byte* p = &arr[index])
			{
				// 2바이트를 short(Little-Endian)로 한 번에 읽음
				ushort val = *(ushort*)p;

				// 비트 이동으로 Big-Endian 변환 (JIT가 BSWAP 또는 ROL 명령어로 최적화)
				return (short)((val << 8) | (val >> 8));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe int I32(byte[] arr, int index = 0)
		{
			fixed (byte* p = &arr[index])
			{
				// Little-Endian CPU(x86/x64) 기준
				int val = *(int*)p;
				// 하드웨어 BSWAP(바이트 뒤집기) 수행
				return (val << 24) | ((val & 0xFF00) << 8) | ((val >> 8) & 0xFF00) | ((int)((uint)val >> 24));
			}
		}

		// 16^exp 사전 계산 테이블 (-64 ~ +63 범위 대응)
		private static readonly double[] Pow16Table = InitPow16Table();

		private static double[] InitPow16Table()
		{
			var tbl = new double[128];

			for (int i = 0; i < 128; i++)
				tbl[i] = Math.Pow(16, i - 64);

			return tbl;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe double Real8(byte[] bytes, int index = 0)
		{
			fixed (byte* p = &bytes[index])
			{
				byte h = p[0];
				if (h == 0) return 0.0;

				int sign = (h & 0x80) != 0 ? -1 : 1;
				int expIdx = h & 0x7F; // (h & 0x7F) - 64 대신 테이블 인덱스 사용

				// 반복문 대신 포인터 캐스팅 및 비트 반전 (Big-Endian -> Little-Endian)
				ulong raw = ((ulong)p[1] << 48) | ((ulong)p[2] << 40) |
							((ulong)p[3] << 32) | ((ulong)p[4] << 24) |
							((ulong)p[5] << 16) | ((ulong)p[6] << 8) | p[7];

				double mant = raw * (1.0 / 72057594037927936.0);

				// Math.Pow 호출 대신 배열 조회(Table Lookup)
				return sign * mant * Pow16Table[expIdx];
			}
		}

		private void ReadUnits(byte[] bytes)
		{
			lib.UserUnit = Real8(bytes, 0);
			lib.DatabaseUnit = Real8(bytes, 8);

			lib.UserUnit *= (int)LengthUnit;
			lib.DatabaseUnit *= (int)LengthUnit;
		}

		private static GBox CalculateBounds(GdsStructure str)
		{
			GBox box = GBox.Empty;

			foreach (var layer in str.Layers)
			{
				foreach (var e in layer.Elements)
					box.Include(e.Bounds);
			}

			return box;
		}

		private void EndElement()
		{
			if (el == null)
				return;

			el.SetBounds();
			str.Layers.AddElement(el);
			el = null;
		}

		private void SetLayer(byte[] bytes)
		{
			if (el != null)
				el.LayerID = I16(bytes);
		}

		private void SetDataType(byte[] bytes)
		{
			if (el != null)
				el.DataType = I16(bytes);
		}

		void SetSName(byte[] bytes, int dataLength)
		{
			if (el is GdsSRef)
				((GdsSRef)el).StructureName = Ascii(bytes, dataLength);
			else if (el is GdsARef)
				((GdsARef)el).StructureName = Ascii(bytes, dataLength);
		}

		private void SetWidth(byte[] bytes)
		{
			if (el is GdsPath)
				((GdsPath)el).Width = I32(bytes) * lib.DatabaseUnit;
		}

		void SetXY(byte[] bytes, int dataLen)
		{
			if (dataLen < 8)
				return;

			var arr = new GPoint[dataLen / 8];

			for (int i = 0; i + 7 < dataLen; i += 8)
				arr[i / 8] = new GPoint(I32(bytes, i) * lib.DatabaseUnit, I32(bytes, i + 4) * lib.DatabaseUnit);

			if (el is GdsBoundary) ((GdsBoundary)el).Points = arr;
			else if (el is GdsPath) ((GdsPath)el).Points = arr;
			else if (el is GdsText) ((GdsText)el).Position = arr[0];
			else if (el is GdsSRef) ((GdsSRef)el).Origin = arr[0];
			else if (el is GdsARef)
			{
				var a = (GdsARef)el;
				if (arr.Length > 0) a.Origin = arr[0];
				if (arr.Length > 1) a.ColVector = arr[1];
				if (arr.Length > 2) a.RowVector = arr[2];
			}
		}

		void SetColRow(byte[] bytes)
		{
			if (el is GdsARef) { var a = (GdsARef)el; a.Columns = I16(bytes, 0); a.Rows = I16(bytes, 2); }
		}

		void SetStrans(byte[] bytes)
		{
			if (el != null) el.Transform.MirrorX = (I16(bytes) & 0x8000) != 0;
		}

		void SetMag(byte[] bytes)
		{
			if (el != null) el.Transform.Magnification = Real8(bytes);
		}

		void SetAngle(byte[] bytes)
		{
			if (el != null) el.Transform.Rotation = Real8(bytes);
		}

		public bool IsLittleEndian { get; private set; }

		public LengthUnit LengthUnit { get; set; }
	}
}
