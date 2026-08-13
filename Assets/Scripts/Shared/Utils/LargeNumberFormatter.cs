using System;
using System.Globalization;

namespace Shared.Utils
{
    /// <summary>
    ///     Компактное форматирование больших чисел для игрового UI.
    ///     Примеры:
    ///     950          -> "950"
    ///     1000         -> "1K"
    ///     1250         -> "1.25K"
    ///     12500        -> "12.5K"
    ///     125000       -> "125K"
    ///     1250000      -> "1.25M"
    ///     12500000     -> "12.5M"
    ///     125000000    -> "125M"
    ///     Для значений за пределами последнего суффикса
    ///     используется научная запись.
    /// </summary>
    public static class LargeNumberFormatter
    {
        private const double Thousand = 1000.0;

        private static readonly string[] Suffixes =
        {
            "", // 10^0
            "K", // 10^3
            "M", // 10^6
            "B", // 10^9
            "T", // 10^12
            "Qa", // 10^15
            "Qi", // 10^18
            "Sx", // 10^21
            "Sp", // 10^24
            "Oc", // 10^27
            "No", // 10^30
            "Dc" // 10^33
        };

        /// <summary>
        ///     Форматирует число в компактный игровой формат.
        /// </summary>
        public static string Format(double value)
        {
            if (double.IsNaN(value))
                return "NaN";

            if (double.IsPositiveInfinity(value))
                return "∞";

            if (double.IsNegativeInfinity(value))
                return "-∞";

            if (value == 0)
                return "0";

            var negative = value < 0;
            value = Math.Abs(value);

            var suffixIndex = 0;

            while (value >= Thousand &&
                   suffixIndex < Suffixes.Length - 1)
            {
                value /= Thousand;
                suffixIndex++;
            }

            string result;

            // Число вышло за пределы таблицы суффиксов.
            // Например, значение больше 10^36.
            if (suffixIndex == Suffixes.Length - 1 &&
                value >= Thousand)
                result = FormatScientific(value, suffixIndex);
            else
                result = FormatMantissa(value) + Suffixes[suffixIndex];

            return negative ? "-" + result : result;
        }

        /// <summary>
        ///     Форматирует мантиссу согласно правилам:
        ///     0-999       -> целое
        ///     1-9.99      -> до 2 знаков
        ///     10-99.9     -> до 1 знака
        ///     100+        -> целое
        /// </summary>
        private static string FormatMantissa(double value)
        {
            if (value < 10.0)
                return value.ToString(
                    "0.##",
                    CultureInfo.InvariantCulture
                );

            if (value < 100.0)
                return value.ToString(
                    "0.#",
                    CultureInfo.InvariantCulture
                );

            return value.ToString(
                "0",
                CultureInfo.InvariantCulture
            );
        }

        /// <summary>
        ///     Форматирование значений, для которых не хватило суффиксов.
        ///     Например:
        ///     1.23e36
        /// </summary>
        private static string FormatScientific(
            double value,
            int suffixIndex)
        {
            var exponent = suffixIndex * 3.0;

            while (value >= Thousand)
            {
                value /= Thousand;
                exponent += 3.0;
            }

            var mantissa = value.ToString(
                "0.##",
                CultureInfo.InvariantCulture
            );

            return mantissa +
                   "e" +
                   exponent.ToString(
                       "0",
                       CultureInfo.InvariantCulture
                   );
        }
    }
}