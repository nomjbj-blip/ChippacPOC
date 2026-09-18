using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.Base
{
    public static class LengthConverter
    {
        public enum Unit
        {
            None,
            nm = 1000000000,
            um = 1000000,
            mm = 1000,
            cm = 100,
            m = 1,
            meter = 1
        }

        /// <summary>
        /// 문자열을 분석하여 값과 단위를 가져옵니다.
        /// </summary>
        /// <param name="text">단위를 포함한 문자열</param>
        /// <param name="value">값</param>
        /// <param name="unit">단위</param>
        public static bool TryParse(string text, out double value, out Unit unit)
        {
            value = 0;
            unit = Unit.None;

            if (String.IsNullOrWhiteSpace(text))
                return false;

            string valueStr, unitStr;

            if (!TrySplit(text, out valueStr, out unitStr))
                return false;

            if (!Double.TryParse(valueStr, out value))
                return false;

            if (!Enum.TryParse<Unit>(unitStr, true, out unit))
                return false;

            return true;
        }

        /// <summary>
        /// 단위를 변환합니다.
        /// </summary>
        /// <param name="valueString">단위를 포함한 값 문자열</param>
        /// <param name="toUnit">변환할 단위</param>
        /// <param name="toValue">변환한 값</param>
        public static bool TryConvert(string valueString, string unitString, out double toValue)
        {
            Unit unit;

            if (!Enum.TryParse<Unit>(unitString, out unit))
                unit = Unit.None;

            return TryConvert(valueString, unit, out toValue);
        }

        /// <summary>
        /// 단위를 변환합니다.
        /// </summary>
        /// <param name="valueString">단위를 포함한 값 문자열</param>
        /// <param name="toUnit">변환할 단위</param>
        /// <param name="toValue">변환한 값</param>
        public static bool TryConvert(string valueString, Unit toUnit, out double toValue)
        {
            toValue = Double.NaN;
            Unit fromUnit;

            if (!TryParse(valueString, out toValue, out fromUnit))
                return false;

            if (fromUnit == Unit.None)
                return false;

            toValue = toValue * (int)toUnit / (int)fromUnit;
            return true;
        }

        private static bool TrySplit(string text, out string valueStr, out string unitStr)
        {
            valueStr = unitStr = null;

            if (String.IsNullOrWhiteSpace(text))
                return false;

            int index = -1;

            for (int i = text.Length - 1; i >= 0; i--)
            {
                char ch = Char.ToLower(text[i]);

                if (ch < 'a' || ch > 'z')
                {
                    index = i + 1;
                    break;
                }
            }

            if (index <= 0)
                return false;

            valueStr = text.Substring(0, index).Trim();
            unitStr = text.Substring(index).Trim();

            if (String.IsNullOrEmpty(valueStr) || String.IsNullOrEmpty(unitStr))
                return false;

            return true;
        }
    }
}
