using System;
using System.Runtime.CompilerServices;

namespace FixedMathSharp
{
        /// <summary>
    /// A static class that provides a variety of fixed-point math functions.
    /// Fixed-point numbers are represented as <see cref="PropertyFixed64"/>.
    /// </summary>
    public static partial class PropertyFixedMath
    {
        #region Fields and Constants

        public const int NUM_BITS = 64;
        public const int SHIFT_AMOUNT_I = 10;
        public const uint MAX_SHIFTED_AMOUNT_UI = (uint)((1L << SHIFT_AMOUNT_I) - 1);
        public const ulong MASK_UL = (ulong)(ulong.MaxValue << SHIFT_AMOUNT_I);

        public const long MAX_VALUE_L = long.MaxValue; // Max possible value for PropertyFixed64
        public const long MIN_VALUE_L = long.MinValue; // Min possible value for PropertyFixed64

        public const long ONE_L = 1L << SHIFT_AMOUNT_I;

        // Precomputed scale factors
        public const float SCALE_FACTOR_F = 1.0f / ONE_L;
        public const double SCALE_FACTOR_D = 1.0 / ONE_L;
        public const decimal SCALE_FACTOR_M = 1.0m / ONE_L;

        /// <summary>
        /// Represents the smallest possible value that can be represented by the PropertyFixed64 format.
        /// </summary>
        /// <remarks>
        /// Precision of this type is 2^-SHIFT_AMOUNT, 
        /// i.e. 1 / (2^SHIFT_AMOUNT) where SHIFT_AMOUNT defines the fractional bits.
        /// </remarks>
        public const long PRECISION_L = 1L;

        /// <summary>
        ///  The smallest value that a PropertyFixed64 can have different from zero.
        /// </summary>
        /// <remarks>
        /// With the following rules:
        ///      anyValue + Epsilon = anyValue
        ///      anyValue - Epsilon = anyValue
        ///      0 + Epsilon = Epsilon
        ///      0 - Epsilon = -Epsilon
        ///  A value Between any number and Epsilon will result in an arbitrary number due to truncating errors.
        /// </remarks>
        public const long EPSILON_L = 1L; //~1E-06f

        #endregion

        #region FixedMath Operations

        /// <summary>
        /// Produces a value with the magnitude of the first argument and the sign of the second argument.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 CopySign(PropertyFixed64 x, PropertyFixed64 y)
        {
            return y >= PropertyFixed64.Zero ? x.Abs() : -x.Abs();
        }

        /// <summary>
        /// Clamps value between 0 and 1 and returns value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Clamp01(PropertyFixed64 value)
        {
            return value < PropertyFixed64.Zero ? PropertyFixed64.Zero : value > PropertyFixed64.One ? PropertyFixed64.One : value;
        }

        /// <summary>
        /// Clamps a fixed-point value between the given minimum and maximum values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Clamp(PropertyFixed64 f1, PropertyFixed64 min, PropertyFixed64 max)
        {
            return f1 < min ? min : f1 > max ? max : f1;
        }

        /// <summary>
        /// Clamps a value to the inclusive range [min, max].
        /// </summary>
        /// <typeparam name="T">The type of the value, must implement IComparable&lt;T&gt;.</typeparam>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The clamped value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0) return max;
            if (value.CompareTo(min) < 0) return min;
            return value;
        }

        /// <summary>
        /// Clamps the value between -1 and 1 inclusive.
        /// </summary>
        /// <param name="f1">The PropertyFixed64 value to clamp.</param>
        /// <returns>Returns a value clamped between -1 and 1.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 ClampOne(PropertyFixed64 f1)
        {
            return f1 > PropertyFixed64.One ? PropertyFixed64.One : f1 < -PropertyFixed64.One ? -PropertyFixed64.One : f1;
        }

        /// <summary>
        /// Returns the absolute value of a PropertyFixed64 number.
        /// </summary>
        public static PropertyFixed64 Abs(PropertyFixed64 value)
        {
            // For the minimum value, return the max to avoid overflow
            if (value.m_rawValue == MIN_VALUE_L)
                return PropertyFixed64.MAX_VALUE;

            // Use branchless absolute value calculation
            long mask = value.m_rawValue >> 63; // If negative, mask will be all 1s; if positive, all 0s
            return PropertyFixed64.FromRaw((value.m_rawValue + mask) ^ mask);
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified number.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Ceiling(PropertyFixed64 value)
        {
            bool hasFractionalPart = (value.m_rawValue & MAX_SHIFTED_AMOUNT_UI) != 0;
            return hasFractionalPart ? value.Floor() + PropertyFixed64.One : value;
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified number (floor function).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Floor(PropertyFixed64 value)
        {
            // Efficiently zeroes out the fractional part
            return PropertyFixed64.FromRaw((long)((ulong)value.m_rawValue & FixedMath.MASK_UL));
        }

        /// <summary>
        /// Returns the larger of two fixed-point values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Max(PropertyFixed64 f1, PropertyFixed64 f2)
        {
            return f1 >= f2 ? f1 : f2;
        }

        /// <summary>
        /// Returns the smaller of two fixed-point values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Min(PropertyFixed64 a, PropertyFixed64 b)
        {
            return (a < b) ? a : b;
        }

        /// <summary>
        /// Rounds a fixed-point number to the nearest integral value, based on the specified rounding mode.
        /// </summary>
        public static PropertyFixed64 Round(PropertyFixed64 value, MidpointRounding mode = MidpointRounding.ToEven)
        {
            long fractionalPart = value.m_rawValue & MAX_SHIFTED_AMOUNT_UI;
            PropertyFixed64 integralPart = value.Floor();
            if (fractionalPart < PropertyFixed64.Half.m_rawValue)
                return integralPart;

            if (fractionalPart > PropertyFixed64.Half.m_rawValue)
                return integralPart + PropertyFixed64.One;

            // When value is exactly PropertyFixed64.Halfway between two numbers
            return mode switch
            {
                MidpointRounding.AwayFromZero => value.m_rawValue > 0
                    ? integralPart + PropertyFixed64.One
                    : integralPart - PropertyFixed64.One, // If it's exactly PropertyFixed64.Halfway, round away from PropertyFixed64.Zero
                _ => (integralPart.m_rawValue & ONE_L) == 0 ? integralPart : integralPart + PropertyFixed64.One, // Rounds to the nearest even number (default behavior)
            };
        }

        /// <summary>
        /// Rounds a fixed-point number to a specific number of decimal places.
        /// </summary>
        public static PropertyFixed64 RoundToPrecision(PropertyFixed64 value, int decimalPlaces, MidpointRounding mode = MidpointRounding.ToEven)
        {
            if (decimalPlaces < 0 || decimalPlaces >= Pow10Lookup.Length)
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces), "Decimal places out of range.");

            int factor = Pow10Lookup[decimalPlaces];
            PropertyFixed64 scaled = value * factor;
            long rounded = Round(scaled, mode).m_rawValue;
            return new PropertyFixed64(rounded + (factor / 2)) / factor;
        }

        /// <summary>
        /// Squares the PropertyFixed64 value.
        /// </summary>
        /// <param name="value">The PropertyFixed64 value to square.</param>
        /// <returns>The squared value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Squared(PropertyFixed64 value)
        {
            return value * value;
        }

        /// <summary>
        /// Adds two fixed-point numbers without performing overflow checking.
        /// </summary>  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 FastAdd(PropertyFixed64 x, PropertyFixed64 y)
        {
            return PropertyFixed64.FromRaw(x.m_rawValue + y.m_rawValue);
        }

        /// <summary>
        /// Subtracts two fixed-point numbers without performing overflow checking.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 FastSub(PropertyFixed64 x, PropertyFixed64 y)
        {
            return PropertyFixed64.FromRaw(x.m_rawValue - y.m_rawValue);
        }

        /// <summary>
        /// Multiplies two fixed-point numbers without overflow checking for performance-critical scenarios.
        /// </summary>
        public static PropertyFixed64 FastMul(PropertyFixed64 x, PropertyFixed64 y)
        {
            long xl = x.m_rawValue;
            long yl = y.m_rawValue;

            // Split values into high and low bits for long multiplication
            ulong xlo = (ulong)(xl & MAX_SHIFTED_AMOUNT_UI);
            long xhi = xl >> SHIFT_AMOUNT_I;
            ulong ylo = (ulong)(yl & MAX_SHIFTED_AMOUNT_UI);
            long yhi = yl >> SHIFT_AMOUNT_I;

            // Perform partial products
            ulong lolo = xlo * ylo;
            long lohi = (long)xlo * yhi;
            long hilo = xhi * (long)ylo;
            long hihi = xhi * yhi;

            // Combine the results
            ulong loResult = lolo >> SHIFT_AMOUNT_I;
            long midResult1 = lohi;
            long midResult2 = hilo;
            long hiResult = hihi << SHIFT_AMOUNT_I;

            long sum = (long)loResult + midResult1 + midResult2 + hiResult;
            return PropertyFixed64.FromRaw(sum);
        }

        /// <summary>
        /// Fast modulus without the checks performed by the '%' operator.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 FastMod(PropertyFixed64 x, PropertyFixed64 y)
        {
            return PropertyFixed64.FromRaw(x.m_rawValue % y.m_rawValue);
        }

        /// <summary>
        /// Performs a smooth step interpolation between two values.
        /// </summary>
        /// <remarks>
        /// The interpolation follows a cubic Hermite curve where the function starts at `a`,
        /// accelerates, and then decelerates towards `b`, ensuring smooth transitions.
        /// </remarks>
        /// <param name="a">The starting value.</param>
        /// <param name="b">The ending value.</param>
        /// <param name="t">A value between 0 and 1 that represents the interpolation factor.</param>
        /// <returns>The interpolated value between `a` and `b`.</returns>
        public static PropertyFixed64 SmoothStep(PropertyFixed64 a, PropertyFixed64 b, PropertyFixed64 t)
        {
            t = t * t * (PropertyFixed64.Three - PropertyFixed64.Two * t);
            return LinearInterpolate(a, b, t);
        }

        /// <summary>
        /// Performs a cubic Hermite interpolation between two points, using specified tangents.
        /// </summary>
        /// <remarks>
        /// This method interpolates smoothly between `p0` and `p1` while considering the tangents `m0` and `m1`.
        /// It is useful for animation curves and smooth motion transitions.
        /// </remarks>
        /// <param name="p0">The first point.</param>
        /// <param name="p1">The second point.</param>
        /// <param name="m0">The tangent (slope) at `p0`.</param>
        /// <param name="m1">The tangent (slope) at `p1`.</param>
        /// <param name="t">A value between 0 and 1 that represents the interpolation factor.</param>
        /// <returns>The interpolated value between `p0` and `p1`.</returns>
        public static PropertyFixed64 CubicInterpolate(PropertyFixed64 p0, PropertyFixed64 p1, PropertyFixed64 m0, PropertyFixed64 m1, PropertyFixed64 t)
        {
            PropertyFixed64 t2 = t * t;
            PropertyFixed64 t3 = t2 * t;
            return (PropertyFixed64.Two * p0 - PropertyFixed64.Two * p1 + m0 + m1) * t3
                   + (-PropertyFixed64.Three * p0 + PropertyFixed64.Three * p1 - PropertyFixed64.Two * m0 - m1) * t2
                   + m0 * t + p0;
        }

        /// <summary>
        /// Performs linear interpolation between two fixed-point values based on the interpolant t (0 greater or equal to `t` and less than or equal to 1).
        /// </summary>
        public static PropertyFixed64 LinearInterpolate(PropertyFixed64 from, PropertyFixed64 to, PropertyFixed64 t)
        {
            if (t.m_rawValue >= ONE_L)
                return to;
            if (t.m_rawValue <= 0)
                return from;

            return (to * t) + (from * (PropertyFixed64.One - t));
        }

        /// <summary>
        /// Moves a value from 'from' to 'to' by a maximum step of 'maxAmount'. 
        /// Ensures the value does not exceed 'to'.
        /// </summary>
        public static PropertyFixed64 MoveTowards(PropertyFixed64 from, PropertyFixed64 to, PropertyFixed64 maxAmount)
        {
            if (from < to)
            {
                from += maxAmount;
                if (from > to)
                    from = to;
            }
            else if (from > to)
            {
                from -= maxAmount;
                if (from < to)
                    from = to;
            }

            return PropertyFixed64.FromRaw(from.m_rawValue);
        }

        /// <summary>
        /// Adds two <see cref="long"/> values and checks for overflow.
        /// If an overflow occurs during addition, the <paramref name="overflow"/> parameter is set to true.
        /// </summary>
        /// <param name="x">The first operand to add.</param>
        /// <param name="y">The second operand to add.</param>
        /// <param name="overflow">
        /// A reference parameter that is set to true if an overflow is detected during the addition.
        /// The existing value of <paramref name="overflow"/> is preserved if already true.
        /// </param>
        /// <returns>The sum of <paramref name="x"/> and <paramref name="y"/>.</returns>
        /// <remarks>
        /// Overflow is detected by checking for a change in the sign bit that indicates a wrap-around.
        /// Additionally, a special check is performed for adding <see cref="PropertyFixed64.MIN_VALUE"/> and -1, 
        /// as this is a known edge case for overflow.
        /// </remarks>
        public static long AddOverflowHelper(long x, long y, ref bool overflow)
        {
            long sum = x + y;
            // Check for overflow using sign bit changes
            overflow |= ((x ^ y ^ sum) & MIN_VALUE_L) != 0;
            // Special check for the case when x is long.PropertyFixed64.MinValue and y is negative
            if (x == long.MinValue && y == -1)
                overflow = true;
            return sum;
        }

        #endregion
    }
}