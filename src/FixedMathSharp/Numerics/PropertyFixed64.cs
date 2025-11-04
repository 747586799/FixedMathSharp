using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace FixedMathSharp
{
    [Serializable]
    public partial struct PropertyFixed64 : IEquatable<PropertyFixed64>, IComparable<PropertyFixed64>, IEqualityComparer<PropertyFixed64>
    {
        #region Fields and Constants

        /// <summary>
        /// The underlying raw long value representing the fixed-point number.
        /// </summary>
        public long m_rawValue;

        public static readonly PropertyFixed64 MAX_VALUE = new PropertyFixed64(PropertyFixedMath.MAX_VALUE_L);
        public static readonly PropertyFixed64 MIN_VALUE = new PropertyFixed64(PropertyFixedMath.MIN_VALUE_L);

        public static readonly PropertyFixed64 One = new PropertyFixed64(PropertyFixedMath.ONE_L);
        public static readonly PropertyFixed64 Two = One * 2;
        public static readonly PropertyFixed64 Three = One * 3;
        public static readonly PropertyFixed64 Half = One / 2;
        public static readonly PropertyFixed64 Quarter = One / 4;
        public static readonly PropertyFixed64 Eighth = One / 8;
        public static readonly PropertyFixed64 Zero = new PropertyFixed64(0);

        /// <inheritdoc cref="PropertyFixedMath.EPSILON_L" />
        public static readonly PropertyFixed64 Epsilon = new PropertyFixed64(PropertyFixedMath.EPSILON_L);
        /// <inheritdoc cref="PropertyFixedMath.PRECISION_L" />
        public static readonly PropertyFixed64 Precision = new PropertyFixed64(PropertyFixedMath.PRECISION_L);

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor for a PropertyFixed64 from a raw long value.
        /// </summary>
        /// <param name="rawValue">Raw long value representing the fixed-point number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal PropertyFixed64(long rawValue)
        {
            m_rawValue = rawValue;
        }

        /// <summary>
        /// Constructs a PropertyFixed64 from an integer, with the fractional part set to zero.
        /// </summary>
        /// <param name="value">Integer value to convert to </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PropertyFixed64(int value) : this((long)value << PropertyFixedMath.SHIFT_AMOUNT_I)
        {
        }

        /// <summary>
        /// Constructs a PropertyFixed64 from a double-precision floating-point value.
        /// </summary>
        /// <param name="value">Double value to convert to </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PropertyFixed64(double value) : this((long)Math.Round((double)value * PropertyFixedMath.ONE_L))
        {
        }

        #endregion

        #region Properties and Methods (Instance)

        /// <summary>
        /// Offsets the current PropertyFixed64 by an integer value.
        /// </summary>
        /// <param name="x">The integer value to add.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Offset(int x)
        {
            m_rawValue += (long)x << PropertyFixedMath.SHIFT_AMOUNT_I;
        }

        /// <summary>
        /// Returns the raw value as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string RawToString()
        {
            return m_rawValue.ToString();
        }

        #endregion

        #region PropertyFixed64 Operations

        /// <summary>
        /// Creates a PropertyFixed64 from a fractional number.
        /// </summary>
        /// <param name="numerator">The numerator of the fraction.</param>
        /// <param name="denominator">The denominator of the fraction.</param>
        /// <returns>A PropertyFixed64 representing the fraction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 Fraction(double numerator, double denominator)
        {
            return new PropertyFixed64(numerator / denominator);
        }

        /// <summary>
        /// x++ (post-increment)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 PostIncrement(ref PropertyFixed64 a)
        {
            PropertyFixed64 originalValue = a;
            a.m_rawValue += One.m_rawValue;
            return originalValue;
        }

        /// <summary>
        /// x-- (post-decrement)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 PostDecrement(ref PropertyFixed64 a)
        {
            PropertyFixed64 originalValue = a;
            a.m_rawValue -= One.m_rawValue;
            return originalValue;
        }

        /// <summary>
        /// Counts the leading zeros in a 64-bit unsigned integer.
        /// </summary>
        /// <param name="x">The number to count leading zeros for.</param>
        /// <returns>The number of leading zeros.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CountLeadingZeroes(ulong x)
        {
            int result = 0;
            while ((x & 0xF000000000000000) == 0)
            {
                result += 4;
                x <<= 4;
            }

            while ((x & 0x8000000000000000) == 0)
            {
                result += 1;
                x <<= 1;
            }

            return result;
        }

        /// <summary>
        /// Returns a number indicating the sign of a Fix64 number.
        /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(PropertyFixed64 value)
        {
            // Return the sign of the value, optimizing for branchless comparison
            return value.m_rawValue < 0 ? -1 : (value.m_rawValue > 0 ? 1 : 0);
        }

        /// <summary>
        /// Returns true if the number has no decimal part (i.e., if the number is equivalent to an integer) and False otherwise. 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInteger(PropertyFixed64 value)
        {
            return ((ulong)value.m_rawValue & PropertyFixedMath.MAX_SHIFTED_AMOUNT_UI) == 0;
        }

        #endregion

        #region Explicit and Implicit Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PropertyFixed64(long value)
        {
            return FromRaw(value << PropertyFixedMath.SHIFT_AMOUNT_I);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(PropertyFixed64 value)
        {
            return value.m_rawValue >> PropertyFixedMath.SHIFT_AMOUNT_I;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PropertyFixed64(int value)
        {
            return new PropertyFixed64(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(PropertyFixed64 value)
        {
            return (int)(value.m_rawValue >> PropertyFixedMath.SHIFT_AMOUNT_I);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PropertyFixed64(float value)
        {
            return new PropertyFixed64((double)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(PropertyFixed64 value)
        {
            return value.m_rawValue * PropertyFixedMath.SCALE_FACTOR_F;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PropertyFixed64(double value)
        {
            return new PropertyFixed64(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(PropertyFixed64 value)
        {
            return value.m_rawValue * PropertyFixedMath.SCALE_FACTOR_D;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator PropertyFixed64(decimal value)
        {
            return new PropertyFixed64((double)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator decimal(PropertyFixed64 value)
        {
            return value.m_rawValue * PropertyFixedMath.SCALE_FACTOR_M;
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Adds two PropertyFixed64 numbers, with saturating behavior in case of overflow.
        /// </summary>
        public static PropertyFixed64 operator +(PropertyFixed64 x, PropertyFixed64 y)
        {
            long xl = x.m_rawValue;
            long yl = y.m_rawValue;
            long sum = xl + yl;
            // Check for overflow, if signs of operands are equal and signs of sum and x are different
            if (((~(xl ^ yl) & (xl ^ sum)) & PropertyFixedMath.MIN_VALUE_L) != 0)
                sum = xl > 0 ? PropertyFixedMath.MAX_VALUE_L : PropertyFixedMath.MIN_VALUE_L;
            return new PropertyFixed64(sum);
        }

        /// <summary>
        /// Adds an int to x 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator +(PropertyFixed64 x, int y)
        {
            return new PropertyFixed64((x.m_rawValue * PropertyFixedMath.SCALE_FACTOR_D) + y);
        }

        /// <summary>
        /// Adds an PropertyFixed64 to x 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator +(int x, PropertyFixed64 y)
        {
            return y + x;
        }

        /// <summary>
        /// Adds a float to x 
        /// </summary>
        public static PropertyFixed64 operator +(PropertyFixed64 x, float y)
        {
            return new PropertyFixed64((x.m_rawValue * PropertyFixedMath.SCALE_FACTOR_D) + y);
        }

        /// <summary>
        /// Adds a PropertyFixed64 to x 
        /// </summary>
        public static PropertyFixed64 operator +(float x, PropertyFixed64 y)
        {
            return y + x;
        }

        /// <summary>
        /// Subtracts one PropertyFixed64 number from another, with saturating behavior in case of overflow.
        /// </summary>
        public static PropertyFixed64 operator -(PropertyFixed64 x, PropertyFixed64 y)
        {
            long xl = x.m_rawValue;
            long yl = y.m_rawValue;
            long diff = xl - yl;
            // Check for overflow, if signs of operands are different and signs of sum and x are different
            if ((((xl ^ yl) & (xl ^ diff)) & PropertyFixedMath.MIN_VALUE_L) != 0)
                diff = xl < 0 ? PropertyFixedMath.MIN_VALUE_L : PropertyFixedMath.MAX_VALUE_L;
            return new PropertyFixed64(diff);
        }

        /// <summary>
        /// Subtracts an int from x 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator -(PropertyFixed64 x, int y)
        {
            return new PropertyFixed64((x.m_rawValue * PropertyFixedMath.SCALE_FACTOR_D) - y);
        }

        /// <summary>
        /// Subtracts a PropertyFixed64 from x 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator -(int x, PropertyFixed64 y)
        {
            return new PropertyFixed64(x - (y.m_rawValue * PropertyFixedMath.SCALE_FACTOR_D));
        }

        /// <summary>
        /// Subtracts a float from x 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator -(PropertyFixed64 x, float y)
        {
            return new PropertyFixed64((x.m_rawValue * PropertyFixedMath.SCALE_FACTOR_D) - y);
        }

        /// <summary>
        /// Subtracts a PropertyFixed64 from x 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator -(float x, PropertyFixed64 y)
        {
            return new PropertyFixed64(x - (y.m_rawValue * PropertyFixedMath.SCALE_FACTOR_D));
        }

        /// <summary>
        /// Multiplies two PropertyFixed64 numbers, handling overflow and rounding.
        /// </summary>
        public static PropertyFixed64 operator *(PropertyFixed64 x, PropertyFixed64 y)
        {
            long xl = x.m_rawValue;
            long yl = y.m_rawValue;

            // Split both numbers into high and low parts
            ulong xlo = (ulong)(xl & PropertyFixedMath.MAX_SHIFTED_AMOUNT_UI);
            long xhi = xl >> PropertyFixedMath.SHIFT_AMOUNT_I;
            ulong ylo = (ulong)(yl & PropertyFixedMath.MAX_SHIFTED_AMOUNT_UI);
            long yhi = yl >> PropertyFixedMath.SHIFT_AMOUNT_I;

            // Perform partial products
            ulong lolo = xlo * ylo; // low bits * low bits
            long lohi = (long)xlo * yhi; // low bits * high bits
            long hilo = xhi * (long)ylo; // high bits * low bits
            long hihi = xhi * yhi; // high bits * high bits

            // Combine results, starting with the low part
            ulong loResult = lolo >> PropertyFixedMath.SHIFT_AMOUNT_I;
            long hiResult = hihi << PropertyFixedMath.SHIFT_AMOUNT_I;

            // Adjust rounding for the fractional part of the lolo term
            if ((lolo & (1UL << (PropertyFixedMath.SHIFT_AMOUNT_I - 1))) != 0)
                loResult++; // Apply rounding up if the dropped bit is 1 (round half-up)

            bool overflow = false;
            long sum = PropertyFixedMath.AddOverflowHelper((long)loResult, lohi, ref overflow);
            sum = PropertyFixedMath.AddOverflowHelper(sum, hilo, ref overflow);
            sum = PropertyFixedMath.AddOverflowHelper(sum, hiResult, ref overflow);

            // Overflow handling
            bool opSignsEqual = ((xl ^ yl) & PropertyFixedMath.MIN_VALUE_L) == 0;

            // Positive overflow check
            if (opSignsEqual)
            {
                if (sum < 0 || (overflow && xl > 0))
                    return MAX_VALUE;
            }
            else
            {
                if (sum > 0)
                    return MIN_VALUE;
            }

            // Final overflow check: if the high 32 bits are non-zero or non-sign-extended, it's an overflow
            long topCarry = hihi >> PropertyFixedMath.SHIFT_AMOUNT_I;
            if (topCarry != 0 && topCarry != -1)
                return opSignsEqual ? MAX_VALUE : MIN_VALUE;

            // Negative overflow check
            if (!opSignsEqual)
            {
                long posOp = xl > yl ? xl : yl;
                long negOp = xl < yl ? xl : yl;

                if (sum > negOp && negOp < -PropertyFixedMath.ONE_L && posOp > PropertyFixedMath.ONE_L)
                    return MIN_VALUE;
            }

            return new PropertyFixed64(sum);
        }

        /// <summary>
        /// Multiplies a PropertyFixed64 by an integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator *(PropertyFixed64 x, int y)
        {
            return new PropertyFixed64((x.m_rawValue * PropertyFixedMath.SCALE_FACTOR_D) * y);
        }

        /// <summary>
        /// Multiplies an integer by a 
        /// </summary>
        public static PropertyFixed64 operator *(int x, PropertyFixed64 y)
        {
            return y * x;
        }

        /// <summary>
        /// Divides one PropertyFixed64 number by another, handling division by zero and overflow.
        /// </summary>
        public static PropertyFixed64 operator /(PropertyFixed64 x, PropertyFixed64 y)
        {
            long xl = x.m_rawValue;
            long yl = y.m_rawValue;

            if (yl == 0)
                throw new DivideByZeroException($"Attempted to divide {x} by zero.");

            ulong remainder = (ulong)(xl < 0 ? -xl : xl);
            ulong divider = (ulong)(yl < 0 ? -yl : yl);
            ulong quotient = 0UL;
            int bitPos = PropertyFixedMath.SHIFT_AMOUNT_I + 1;

            // If the divider is divisible by 2^n, take advantage of it.
            while ((divider & 0xF) == 0 && bitPos >= 4)
            {
                divider >>= 4;
                bitPos -= 4;
            }

            while (remainder != 0 && bitPos >= 0)
            {
                int shift = CountLeadingZeroes(remainder);
                if (shift > bitPos)
                    shift = bitPos;

                remainder <<= shift;
                bitPos -= shift;

                ulong div = remainder / divider;
                remainder %= divider;
                quotient += div << bitPos;

                // Detect overflow
                if ((div & ~(0xFFFFFFFFFFFFFFFF >> bitPos)) != 0)
                    return ((xl ^ yl) & PropertyFixedMath.MIN_VALUE_L) == 0 ? MAX_VALUE : MIN_VALUE;

                remainder <<= 1;
                --bitPos;
            }

            // Rounding logic: "Round half to even" or "Banker's rounding"
            if ((quotient & 0x1) != 0)
                quotient += 1;

            long result = (long)(quotient >> 1);
            if (((xl ^ yl) & PropertyFixedMath.MIN_VALUE_L) != 0)
                result = -result;

            return new PropertyFixed64(result);
        }

        /// <summary>
        /// Divides a PropertyFixed64 by an integer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator /(PropertyFixed64 x, int y)
        {
            return new PropertyFixed64((x.m_rawValue * PropertyFixedMath.SCALE_FACTOR_D) / y);
        }

        /// <summary>
        /// Computes the remainder of division of one PropertyFixed64 number by another.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator %(PropertyFixed64 x, PropertyFixed64 y)
        {
            if (x.m_rawValue == PropertyFixedMath.MIN_VALUE_L && y.m_rawValue == -1)
                return Zero;
            return new PropertyFixed64(x.m_rawValue % y.m_rawValue);
        }

        /// <summary>
        /// Unary negation operator.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator -(PropertyFixed64 x)
        {
            return x.m_rawValue == PropertyFixedMath.MIN_VALUE_L ? MAX_VALUE : new PropertyFixed64(-x.m_rawValue);
        }

        /// <summary>
        /// Pre-increment operator (++x).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator ++(PropertyFixed64 a)
        {
            a.m_rawValue += One.m_rawValue;
            return a;
        }

        /// <summary>
        /// Pre-decrement operator (--x).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator --(PropertyFixed64 a)
        {
            a.m_rawValue -= One.m_rawValue;
            return a;
        }

        /// <summary>
        /// Bitwise left shift operator.
        /// </summary>
        /// <param name="a">Operand to shift.</param>
        /// <param name="shift">Number of bits to shift.</param>
        /// <returns>The shifted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator <<(PropertyFixed64 a, int shift)
        {
            return new PropertyFixed64(a.m_rawValue << shift);
        }

        /// <summary>
        /// Bitwise right shift operator.
        /// </summary>
        /// <param name="a">Operand to shift.</param>
        /// <param name="shift">Number of bits to shift.</param>
        /// <returns>The shifted value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 operator >> (PropertyFixed64 a, int shift)
        {
            return new PropertyFixed64(a.m_rawValue >> shift);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Determines whether one PropertyFixed64 is greater than another.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(PropertyFixed64 x, PropertyFixed64 y)
        {
            return x.m_rawValue > y.m_rawValue;
        }

        /// <summary>
        /// Determines whether one PropertyFixed64 is less than another.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(PropertyFixed64 x, PropertyFixed64 y)
        {
            return x.m_rawValue < y.m_rawValue;
        }

        /// <summary>
        /// Determines whether one PropertyFixed64 is greater than or equal to another.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(PropertyFixed64 x, PropertyFixed64 y)
        {
            return x.m_rawValue >= y.m_rawValue;
        }

        /// <summary>
        /// Determines whether one PropertyFixed64 is less than or equal to another.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(PropertyFixed64 x, PropertyFixed64 y)
        {
            return x.m_rawValue <= y.m_rawValue;
        }

        /// <summary>
        /// Determines whether two PropertyFixed64 instances are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PropertyFixed64 left, PropertyFixed64 right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two PropertyFixed64 instances are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PropertyFixed64 left, PropertyFixed64 right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the string representation of this PropertyFixed64 instance.
        /// </summary>
        /// <remarks>
        /// Up to 10 decimal places.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return ((double)this).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the numeric value of the current PropertyFixed64 object to its equivalent string representation.
        /// </summary>
        /// <param name="format">A format specification that governs how the current PropertyFixed64 object is converted.</param>
        /// <returns>The string representation of the value of the current PropertyFixed64 object.</returns>  
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format)
        {
            return ((double)this).ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses a string to create a PropertyFixed64 instance.
        /// </summary>
        /// <param name="s">The string representation of the </param>
        /// <returns>The parsed PropertyFixed64 value.</returns>
        public static PropertyFixed64 Parse(string s)
        {
            if (string.IsNullOrEmpty(s)) throw new ArgumentNullException(nameof(s));

            // Check if the value is negative
            bool isNegative = false;
            if (s[0] == '-')
            {
                isNegative = true;
                s = s.Substring(1);
            }

            if (!long.TryParse(s, out long rawValue))
                throw new FormatException($"Invalid format: {s}");

            // If the value was negative, negate the result
            if (isNegative)
                rawValue = -rawValue;

            return FromRaw(rawValue);
        }

        /// <summary>
        /// Tries to parse a string to create a PropertyFixed64 instance.
        /// </summary>
        /// <param name="s">The string representation of the </param>
        /// <param name="result">The parsed PropertyFixed64 value.</param>
        /// <returns>True if parsing succeeded; otherwise, false.</returns>
        public static bool TryParse(string s, out PropertyFixed64 result)
        {
            result = Zero;
            if (string.IsNullOrEmpty(s)) return false;

            // Check if the value is negative
            bool isNegative = false;
            if (s[0] == '-')
            {
                isNegative = true;
                s = s.Substring(1);
            }

            if (!long.TryParse(s, out long rawValue)) return false;

            // If the value was negative, negate the result
            if (isNegative)
                rawValue = -rawValue;

            result = FromRaw(rawValue);
            return true;
        }

        /// <summary>
        /// Creates a PropertyFixed64 from a raw long value.
        /// </summary>
        /// <param name="rawValue">The raw long value.</param>
        /// <returns>A PropertyFixed64 representing the raw value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyFixed64 FromRaw(long rawValue)
        {
            return new PropertyFixed64(rawValue);
        }

        /// <summary>
        /// Converts a Fixed64s RawValue (Int64) into a double
        /// </summary>
        /// <param name="f1"></param>
        /// <returns></returns>
        public static double ToDouble(long f1)
        {
            return f1 * PropertyFixedMath.SCALE_FACTOR_D;
        }

        /// <summary>
        /// Converts a Fixed64s RawValue (Int64) into a float
        /// </summary>
        /// <param name="f1"></param>
        /// <returns></returns>
        public static float ToFloat(long f1)
        {
            return f1 * PropertyFixedMath.SCALE_FACTOR_F;
        }

        /// <summary>
        /// Converts a Fixed64s RawValue (Int64) into a decimal
        /// </summary>
        /// <param name="f1"></param>
        /// <returns></returns>
        public static decimal ToDecimal(long f1)
        {
            return f1 * PropertyFixedMath.SCALE_FACTOR_M;
        }

        #endregion

        #region Equality, HashCode, Comparable Overrides

        /// <summary>
        /// Determines whether this instance equals another object.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is PropertyFixed64 other && Equals(other);
        }

        /// <summary>
        /// Determines whether this instance equals another 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(PropertyFixed64 other)
        {
            return m_rawValue == other.m_rawValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(PropertyFixed64 x, PropertyFixed64 y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Returns the hash code for this PropertyFixed64 instance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return m_rawValue.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(PropertyFixed64 obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Compares this instance to another 
        /// </summary>
        /// <param name="other">The PropertyFixed64 to compare with.</param>
        /// <returns>-1 if less than, 0 if equal, 1 if greater than other.</returns>
        public int CompareTo(PropertyFixed64 other)
        {
            return m_rawValue.CompareTo(other.m_rawValue);
        }

        #endregion
    }
}