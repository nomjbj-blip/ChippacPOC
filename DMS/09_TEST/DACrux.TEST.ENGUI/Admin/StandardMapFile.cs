using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace DACrux.TEST.ENGUI
{
    /// <summary>
    /// 칩팩 텍스트 Map을 표준 Map 형상으로 변환한다.
    /// Bin 판정은 하지 않고 빈 위치와 칩 좌표만 보존하며, 기존 AVI Parser에는 영향을 주지 않는다.
    /// </summary>
    internal sealed class StandardMapFile
    {
        public string Device { get; private set; }
        public string Lot { get; private set; }
        public string Wafer { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public int Angle { get; private set; }
        public int ReferenceX { get; private set; }
        public int ReferenceY { get; private set; }
        /// <summary>기존 Map Define의 중앙 인덱스를 우선하고, 빈 칸이면 가장 가까운 사용 다이를 선택한다.</summary>
        public Point CenterDie { get; private set; }
        public double PitchX { get; private set; }
        public double PitchY { get; private set; }
        public IList<Point> Dies { get; private set; }

        /// <summary>파일을 한 번 읽고 검증한다. 실패 시 완성되지 않은 Map을 화면에 반환하지 않는다.</summary>
        public static StandardMapFile Load(string path)
        {
            return Parse(File.ReadAllText(path));
        }

        /// <summary>
        /// 헤더와 RowData를 분리하고 행/열 개수를 확인한다.
        /// 파일의 첫 행은 위쪽이므로 X/Y 모두 왼쪽 위부터 1로 기록한다.
        /// FNLOC는 방향 정보로 보존하되 원본 배치를 임의로 회전하지 않는다.
        /// </summary>
        public static StandardMapFile Parse(string text)
        {
            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var rows = new List<string>();
            using (var reader = new StringReader(text))
            {
                string line;
                int lineNumber = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    line = line.Trim().TrimStart('\uFEFF');
                    if (line.Length == 0) continue;
                    int separator = line.IndexOf(':');
                    if (separator <= 0)
                        throw new FormatException(lineNumber + "행: '항목:값' 형식이 아닙니다.");
                    string key = line.Substring(0, separator).Trim();
                    string value = line.Substring(separator + 1).Trim();
                    if (key.Equals("RowData", StringComparison.OrdinalIgnoreCase))
                        rows.Add(value);
                    else
                    {
                        if (headers.ContainsKey(key)) throw new FormatException(key + " 항목이 중복되었습니다.");
                        headers.Add(key, value);
                    }
                }
            }

            var map = new StandardMapFile();
            map.Device = Required(headers, "DEVICE");
            map.Lot = Optional(headers, "LOT", "");
            map.Wafer = Optional(headers, "WAFER", "");
            map.Rows = Integer(headers, "ROWCT");
            map.Columns = Integer(headers, "COLCT");
            if (map.Rows < 1 || map.Columns < 1 || map.Rows > 2048 || map.Columns > 2048
                || (long)map.Rows * map.Columns > 1000000)
                throw new FormatException("행/열은 1~2048, 전체 위치는 1,000,000개 이하여야 합니다.");
            map.Angle = Integer(headers, "FNLOC");
            if (map.Angle != 0 && map.Angle != 90 && map.Angle != 180 && map.Angle != 270)
                throw new FormatException("FNLOC는 0 / 90 / 180 / 270 중 하나여야 합니다.");
            // 기존 Map Setup의 원점 인덱스 범위에 맞춘다. 생략된 기준 좌표는 (1,1)을 사용한다.
            map.ReferenceX = int.Parse(Optional(headers, "REFPX", "1"), CultureInfo.InvariantCulture);
            map.ReferenceY = int.Parse(Optional(headers, "REFPY", "1"), CultureInfo.InvariantCulture);
            if (map.ReferenceX < 1 || map.ReferenceX > map.Columns || map.ReferenceY < 1 || map.ReferenceY > map.Rows)
                throw new FormatException("REFPX / REFPY는 1부터 시작하는 Map 범위 안에 있어야 합니다.");

            string unit = Required(headers, "DUTMS").ToLowerInvariant();
            if (unit != "mm" && unit != "mil") throw new FormatException("DUTMS는 mm 또는 mil이어야 합니다.");
            double factor = unit == "mil" ? 0.0254 : 1.0;
            map.PitchX = PositiveNumber(headers, "XDIES") * factor;
            map.PitchY = PositiveNumber(headers, "YDIES") * factor;
            if (map.PitchX > 10000 || map.PitchY > 10000)
                throw new FormatException("칩 간격이 10,000 mm를 초과합니다. DUTMS / XDIES / YDIES를 확인하세요.");
            if (rows.Count != map.Rows)
                throw new FormatException(string.Format("ROWCT={0}, 실제 RowData={1}행입니다.", map.Rows, rows.Count));

            var dies = new List<Point>();
            for (int row = 0; row < rows.Count; row++)
            {
                string[] cells = rows[row].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                if (cells.Length != map.Columns)
                    throw new FormatException(string.Format("RowData {0}행: COLCT={1}, 실제={2}열입니다.", row + 1, map.Columns, cells.Length));
                for (int column = 0; column < cells.Length; column++)
                {
                    string cell = cells[column];
                    if (cell == "___" || cell == "__") continue;
                    // 숫자 Bin은 값과 관계없이 동일한 사용 칩이다. 000도 반드시 포함한다.
                    int bin;
                    if (!int.TryParse(cell, NumberStyles.None, CultureInfo.InvariantCulture, out bin))
                        throw new FormatException(string.Format("RowData {0}행 {1}열: 지원하지 않는 값 '{2}'입니다.", row + 1, column + 1, cell));
                    // RowData는 화면 위에서 아래로 기록된다. LeftTop 인덱스와 맞춰 첫 행을 Y=1로 둔다.
                    dies.Add(new Point(column + 1, row + 1));
                }
            }
            if (dies.Count == 0) throw new FormatException("사용할 칩이 없습니다.");
            map.Dies = dies.AsReadOnly();
            // 기존 Map Define은 범위의 중앙 인덱스를 선택한다. 해당 칸이 비었으면 실제 사용 다이 중 가장 가까운 것을 사용한다.
            int centerX = 1 + (map.Columns - 1) / 2;
            int centerY = 1 + (map.Rows - 1) / 2;
            long nearest = long.MaxValue;
            foreach (Point die in dies)
            {
                long distance = (long)(die.X - centerX) * (die.X - centerX)
                    + (long)(die.Y - centerY) * (die.Y - centerY);
                if (distance < nearest)
                {
                    nearest = distance;
                    map.CenterDie = die;
                }
            }
            return map;
        }

        /// <summary>파일 기준 좌표를 유지하며 전체 배치를 Wafer 가운데 놓는 X 보정값이다.</summary>
        public double CenterX { get { return (Columns / 2.0 - ReferenceX) * PitchX; } }
        /// <summary>파일 기준 좌표를 유지하며 전체 배치를 Wafer 가운데 놓는 Y 보정값이다.</summary>
        public double CenterY { get { return (ReferenceY - Rows / 2.0) * PitchY; } }

        /// <summary>필수 헤더의 누락을 항목 이름과 함께 알린다.</summary>
        private static string Required(Dictionary<string, string> headers, string key)
        {
            string value = Optional(headers, key, "");
            if (value.Length == 0) throw new FormatException(key + " 항목이 없습니다.");
            return value;
        }

        /// <summary>선택 헤더가 없으면 명시한 기본값을 사용한다.</summary>
        private static string Optional(Dictionary<string, string> headers, string key, string fallback)
        {
            string value;
            return headers.TryGetValue(key, out value) ? value : fallback;
        }

        /// <summary>행/열/각도 헤더를 지역 설정과 무관한 정수로 읽는다.</summary>
        private static int Integer(Dictionary<string, string> headers, string key)
        {
            int value;
            if (!int.TryParse(Required(headers, key), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                throw new FormatException(key + " 값이 정수가 아닙니다.");
            return value;
        }

        /// <summary>파일의 소수점은 '.'으로 해석하고 0, 음수, NaN, 무한대를 거부한다.</summary>
        private static double PositiveNumber(Dictionary<string, string> headers, string key)
        {
            double value;
            if (!double.TryParse(Required(headers, key), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                || double.IsNaN(value) || double.IsInfinity(value) || value <= 0)
                throw new FormatException(key + " 값은 0보다 큰 유한한 숫자여야 합니다.");
            return value;
        }
    }
}
