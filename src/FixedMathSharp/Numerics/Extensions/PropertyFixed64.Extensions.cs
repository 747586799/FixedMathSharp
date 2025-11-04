using System;
using System.Runtime.CompilerServices;

namespace FixedMathSharp
{
        public static class PropertyFixed64Extensions
    {
        #region PropertyFixed64 Operations

        /// <inheritdoc cref="PropertyFixed64.Sign(PropertyFixed64)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(this PropertyFixed64 value)
        {
            return PropertyFixed64.Sign(value);
        }

        /// <inheritdoc cref="PropertyFixed64.IsInteger(PropertyFixed64)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInteger(this PropertyFixed64 value)
        {
            return PropertyFixed64.IsInteger(value);
        }

        /// <inheritdoc cref="PropertyFixedMath.Squared(PropertyFixed64)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Squared(this PropertyFixed64 value)
        {
            return PropertyFixedMath.Squared(value);
        }

        /// <inheritdoc cref="PropertyFixedMath.Round(PropertyFixed64, MidpointRounding)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Round(this PropertyFixed64 value, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return PropertyFixedMath.Round(value, mode);
        }

        /// <inheritdoc cref="PropertyFixedMath.RoundToPrecision(PropertyFixed64, int, MidpointRounding)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 RoundToPrecision(this PropertyFixed64 value, int places, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return PropertyFixedMath.RoundToPrecision(value, places, mode);
        }

        /// <inheritdoc cref="PropertyFixedMath.ClampOne(PropertyFixed64)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 ClampOne(this PropertyFixed64 f1)
        {
            return PropertyFixedMath.ClampOne(f1);
        }

        /// <inheritdoc cref="PropertyFixedMath.Clamp01(PropertyFixed64)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Clamp01(this PropertyFixed64 f1)
        {
            return PropertyFixedMath.Clamp01(f1);
        }

        /// <inheritdoc cref="PropertyFixedMath.Abs(PropertyFixed64)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Abs(this PropertyFixed64 value)
        {
            return PropertyFixedMath.Abs(value);
        }

        /// <summary>
        /// Checks if the absolute value of x is less than y.
        /// </summary>
        /// <param name="x">The value to compare.</param>
        /// <param name="y">The comparison threshold.</param>
        /// <returns>True if |x| &lt; y; otherwise false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AbsLessThan(this PropertyFixed64 x, PropertyFixed64 y)
        {
            return Abs(x) < y;
        }

        /// <inheritdoc cref="PropertyFixedMath.FastAdd(PropertyFixed64, PropertyFixed64)" />
        public static PropertyFixed64 FastAdd(this PropertyFixed64 a, PropertyFixed64 b)
        {
            return PropertyFixedMath.FastAdd(a, b);
        }

        /// <inheritdoc cref="PropertyFixedMath.FastSub(PropertyFixed64, PropertyFixed64)" />
        public static PropertyFixed64 FastSub(this PropertyFixed64 a, PropertyFixed64 b)
        {
            return PropertyFixedMath.FastSub(a, b);
        }

        /// <inheritdoc cref="PropertyFixedMath.FastMul(PropertyFixed64, PropertyFixed64)" />
        public static PropertyFixed64 FastMul(this PropertyFixed64 a, PropertyFixed64 b)
        {
            return PropertyFixedMath.FastMul(a, b);
        }

        /// <inheritdoc cref="PropertyFixedMath.FastMod(PropertyFixed64, PropertyFixed64)" />
        public static PropertyFixed64 FastMod(this PropertyFixed64 a, PropertyFixed64 b)
        {
            return PropertyFixedMath.FastMod(a, b);
        }

        /// <inheritdoc cref="PropertyFixedMath.Floor(PropertyFixed64)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Floor(this PropertyFixed64 value)
        {
            return PropertyFixedMath.Floor(value);
        }

        /// <inheritdoc cref="PropertyFixedMath.Ceiling(PropertyFixed64)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Ceiling(this PropertyFixed64 value)
        {
            return PropertyFixedMath.Ceiling(value);
        }

        /// <summary>
        /// Rounds the PropertyFixed64 value to the nearest integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(this PropertyFixed64 x)
        {
            return (int)PropertyFixedMath.Round(x);
        }

        /// <summary>
        /// Rounds up the PropertyFixed64 value to the nearest integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilToInt(this PropertyFixed64 x)
        {
            return (int)PropertyFixedMath.Ceiling(x);
        }

        /// <summary>
        /// Rounds down the PropertyFixed64 value to the nearest integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(this PropertyFixed64 x)
        {
            return (int)Floor(x);
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Converts the PropertyFixed64 value to a string formatted to 2 decimal places.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToFormattedString(this PropertyFixed64 f1)
        {
            return f1.ToPreciseFloat().ToString("0.##");
        }

        /// <summary>
        /// Converts the PropertyFixed64 value to a double with specified decimal precision.
        /// </summary>
        /// <param name="f1">The PropertyFixed64 value to convert.</param>
        /// <param name="precision">The number of decimal places to round to.</param>
        /// <returns>The formatted double value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToFormattedDouble(this PropertyFixed64 f1, int precision = 2)
        {
            return Math.Round((double)f1, precision, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Converts the PropertyFixed64 value to a float with 2 decimal points of precision.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFormattedFloat(this PropertyFixed64 f1)
        {
            return (float)ToFormattedDouble(f1);
        }

        /// <summary>
        /// Converts the PropertyFixed64 value to a precise float representation (without rounding).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToPreciseFloat(this PropertyFixed64 f1)
        {
            return (float)(double)f1;
        }

        /// <summary>
        /// Converts the angle in degrees to radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 ToRadians(this PropertyFixed64 angleInDegrees)
        {
            return PropertyFixedMath.DegToRad(angleInDegrees);
        }

        /// <summary>
        /// Converts the angle in radians to degree.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 ToDegree(this PropertyFixed64 angleInRadians)
        {
            return PropertyFixedMath.RadToDeg(angleInRadians);
        }

        #endregion

        #region Equality

        /// <summary>
        /// Checks if the value is greater than epsilon (positive or negative).
        /// Useful for determining if a value is effectively non-zero with a given precision.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MoreThanEpsilon(this PropertyFixed64 d)
        {
            return d.Abs() > PropertyFixed64.Epsilon;
        }

        /// <summary>
        /// Checks if the value is less than epsilon (i.e., effectively zero).
        /// Useful for determining if a value is close enough to zero with a given precision.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanEpsilon(this PropertyFixed64 d)
        {
            return d.Abs() < PropertyFixed64.Epsilon;
        }

        /// <summary>
        /// Helper method to compare individual vector components for approximate equality, allowing a fractional difference.
        /// Handles zero components by only using the allowed percentage difference.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FuzzyComponentEqual(this PropertyFixed64 a, PropertyFixed64 b, PropertyFixed64 percentage)
        {
            var diff = (a - b).Abs();
            var allowedErr = a.Abs() * percentage;
            // Compare directly to percentage if a is zero
            // Otherwise, use percentage of a's magnitude
            return a == PropertyFixed64.Zero ? diff <= percentage : diff <= allowedErr;
        }

        #endregion
    }
}