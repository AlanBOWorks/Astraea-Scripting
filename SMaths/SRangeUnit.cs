using UnityEngine;

namespace SMaths
{
    /// <summary>
    /// Utility class for special function related to linear effects in float ranged within [0,1]
    /// </summary>
    public static class SRangeUnit
    {

        public static SRange UnitRange = new SRange(0,1);

        /// <summary>
        /// Generic returning value in lineal effect (doesn't change anything)
        /// </summary>
        /// <param name="value">The value for changes</param>
        /// <returns>The actual <paramref name="value"/></returns>
        public static float Lineal(float value)
        {
            return value;
        }

        /// <summary>
        /// Inverse value of the <see cref="Lineal(float)"/> function
        /// </summary>
        /// <param name="value">What value for inverting its lineal function</param>
        /// <returns>1- <paramref name="value"/></returns>
        public static float InverseLineal(float value)
        {
            return 1f - Lineal(value);
        }


        /// <summary>
        /// Returns the Pow EaseIn function for smooth effect and better velocity of calculations
        /// <para>E.g: 1.3 -> <paramref name="decimalPow"/> = 3</para>
        /// <para>E.g: 1.7 -> <paramref name="decimalPow"/> = 7</para>
        /// </summary>
        /// <param name="value">The value which it calcules the function's values</param>
        /// <param name="decimalPow">The the decimal in ease^1.<paramref name="decimalPow"/></param>
        /// <returns></returns>
        public static float EaseIn1_pow(float value, float decimalPow)
        {
            return Mathf.Lerp(Lineal(value), EaseIn2(value), decimalPow);
        }

        /// <summary>
        /// The square value of the parameter in EaseIn function for a smooth effect
        /// </summary>
        /// <param name="value">Which value to calcule the smooth</param>
        /// <returns><paramref name="value"/>*2</returns>
        public static float EaseIn2(float value)
        {
            return Lineal(value) * Lineal(value);
        }

        /// <summary>
        /// Returns the Pow EaseIn function for smooth effect and better velocity of calculations
        /// <para>E.g: 2.3 -> <paramref name="decimalPow"/> = 3</para>
        /// <para>E.g: 2.7 -> <paramref name="decimalPow"/> = 7</para>
        /// </summary>
        /// <param name="value">The value which it calcules the function's values</param>
        /// <param name="decimalPow">The the decimal in ease^2.<paramref name="decimalPow"/></param>
        /// <returns></returns>
        public static float EaseIn2_pow(float value, float decimalPow)
        {
            return Mathf.Lerp(EaseIn2(value), EaseIn3(value), decimalPow);
        }

        /// <summary>
        /// The cubic value of the parameter in EaseIn function for a smooth effect
        /// </summary>
        /// <param name="value">Which value to calcule the smooth</param>
        /// <returns><paramref name="value"/>*3</returns>
        public static float EaseIn3(float value)
        {
            return EaseIn2(value) * Lineal(value);
        }

        /// <summary>
        /// Returns the Pow EaseIn function for smooth effect and better velocity of calculations
        /// <para>E.g: 3.3 -> <paramref name="decimalPow"/> = 3</para>
        /// <para>E.g: 3.7 -> <paramref name="decimalPow"/> = 7</para>
        /// </summary>
        /// <param name="value">The value which it calcules the function's values</param>
        /// <param name="decimalPow">The the decimal in ease^3.<paramref name="decimalPow"/></param>
        /// <returns></returns>
        public static float EaseIn3_pow(float value, float decimalPow)
        {
            return Mathf.Lerp(EaseIn3(value), EaseIn4(value), decimalPow);
        }

        /// <summary>
        /// The Quartic value of the parameter in EaseIn function for a smooth effect
        /// </summary>
        /// <param name="value">Which value to calcule the smooth</param>
        /// <returns><paramref name="value"/>*2</returns>
        public static float EaseIn4(float value)
        {
            return EaseIn3(value) * Lineal(value);
        }

        /// <summary>
        /// The square inverted function for an EaseOut output in smooth effect
        /// </summary>
        /// <param name="value">Which value to calcule the smooth</param>
        /// <returns>1 - <see cref="InverseLineal(float)"/>^2</returns>
        public static float EaseOut2(float value)
        {
            return 1f - InverseLineal(value) * InverseLineal(value);
        }

        /// <summary>
        /// The cubic inverted function for an EaseOut output in smooth effect
        /// </summary>
        /// <param name="value">Which value to calcule the smooth</param>
        /// <returns>1 - <see cref="InverseLineal(float)"/>^3</returns>
        public static float EaseOut3(float value)
        {
            return 1f - InverseLineal(value) * InverseLineal(value) * InverseLineal(value);
        }

        /// <summary>
        /// The cubic inverted function for an EaseOut output in smooth effect
        /// </summary>
        /// <param name="value">Which value to calcule the smooth</param>
        /// <returns>1 - <see cref="InverseLineal(float)"/>^4</returns>
        public static float EaseOut4(float value)
        {
            return 1f - InverseLineal(value) * InverseLineal(value) * InverseLineal(value) * InverseLineal(value);
        }
    

        /// <summary>
        /// Interpolation within EaseIn and EaseOut function dependin in <paramref name="blend"/>'s value in Pow2
        /// </summary>
        /// <param name="value">The value which generates the function</param>
        /// <param name="blend">the amount of blend within EaseIn and EaseOut</param>
        /// <returns>The correspondent value of the blend</returns>
        public static float MixEase2(float value, float blend)
        {
            return Mathf.Lerp(EaseIn2(value), EaseOut2(value), blend);
        }

        /// <summary>
        /// Interpolation within EaseIn and EaseOut function dependin in <paramref name="blend"/>'s value in Pow3
        /// </summary>
        /// <param name="value">The value which generates the function</param>
        /// <param name="blend">the amount of blend within EaseIn and EaseOut</param>
        /// <returns>The correspondent value of the blend</returns>
        public static float MixEase3(float value, float blend)
        {
            return Mathf.Lerp(EaseIn3(value), EaseOut3(value), blend);
        }

        /// <summary>
        /// Interpolation within EaseIn and EaseOut function dependin in <paramref name="blend"/>'s value in Pow4
        /// </summary>
        /// <param name="value">The value which generates the function</param>
        /// <param name="blend">the amount of blend within EaseIn and EaseOut</param>
        /// <returns>The correspondent value of the blend</returns>
        public static float MixEase4(float value, float blend)
        {
            return Mathf.Lerp(EaseIn4(value), EaseOut4(value), blend);
        }

        /// <summary>
        /// Scales the value based in <paramref name="scale"/>'s value
        /// </summary>
        /// <param name="value">The value which we want to scale things</param>
        /// <param name="scale">In which scale parameter we want to scale (e.g: time)</param>
        /// <returns><paramref name="scale"/> * <paramref name="value"/></returns>
        public static float Scale(float value, float scale)
        {
            return value * scale;
        }

        /// <summary>
        /// Returns the inversed scale function based in <paramref name="scale"/>'s value
        /// </summary>
        /// <param name="value">The value which we want to scale things</param>
        /// <param name="scale">In which scale parameter we want to scale (e.g: time)</param>
        /// <returns>(1 - <paramref name="scale"/>) * <paramref name="value"/></returns>
        public static float InverseScale(float value, float scale)
        {
            return (1f - scale) * value;
        }

        /// <summary>
        /// Generates a parabolic function
        /// </summary>
        /// <param name="value">The value which generate its parabolic function</param>
        /// <returns><paramref name="value"/> * (1 - <paramref name="value"/>)</returns>
        public static float Arch2(float value)
        {
            return value * (1-value);
        }

        /// <summary>
        /// Generates a parabolic function with a smooth start
        /// </summary>
        /// <param name="value">The value which generate its parabolic function</param>
        /// <returns><paramref name="value"/>^2 * (1 - <paramref name="value"/>)</returns>
        public static float EaseInArch3(float value)
        {
            return value * Arch2(value);
        }

        /// <summary>
        /// Generates a parabolic funcion with a smooth stop
        /// </summary>
        /// <param name="value">The value which generate its parabolic function</param>
        /// <returns><paramref name="value"/> * (1 - <paramref name="value"/>)^2</returns>
        public static float EaseOutArch3(float value)
        {
            return value * InverseLineal(value) * InverseLineal(value);
        }

        /// <summary>
        /// Generates a bell curve with an smooth easeIn and easeOut
        /// </summary>
        /// <param name="value">The value which generate its parabolic function</param>
        /// <returns><see cref="EaseIn2(float)"/> * <see cref="EaseOut2(float)"/></returns>
        public static float BellCurve4(float value)
        {
            return EaseOut2(value) * EaseIn2(value);
        }

        /// <summary>
        /// Generates a bell curve with an smooth easeIn and easeOut
        /// </summary>
        /// <param name="value">The value which generate its parabolic function</param>
        /// <returns><see cref="EaseIn3(float)"/> * <see cref="EaseOut3(float)"/></returns>
        public static float BellCurve6(float value)
        {
            return EaseOut3(value) * EaseIn3(value);
        }


        /// <summary>
        /// Forces the value always being positive
        /// </summary>
        /// <param name="value">The value which makes its absolute</param>
        /// <returns>|<paramref name="value"/>|</returns>
        public static float BounceClampBottom(float value)
        {
            return Mathf.Abs(value);
        }

        /// <summary>
        /// Forces the value be inverted if its reachs values greater than 1 
        /// </summary>
        /// <param name="value">The value which makes its absolute</param>
        /// <returns>1 - |1 - <paramref name="value"/>|</returns>
        public static float BounceClampTop(float value)
        {
            return 1f - Mathf.Abs(1f - value);
        }

        /// <summary>
        /// Forces to the value being within its range of [0,1] and makes its bounce if happens
        /// </summary>
        /// <param name="value">The value which makes its absolute</param>
        /// <returns>|1 - |1 - <paramref name="value"/>||</returns>
        public static float BounceClampBottomTop(float value)
        {
            return BounceClampBottom(BounceClampTop(value));
        }


        /// <summary>
        /// An Bezier function with it forces the <paramref name="value"/> take the curve of point <paramref name="B"/> and <paramref name="C"/>
        /// </summary>
        /// <param name="B">In wich value it should be stop in first place in range[0,1]</param>
        /// <param name="C">In wich value it should be stop in second place in range[0,1]</param>
        /// <param name="value">Curvatured which pass through <paramref name="B"/> abd <paramref name="C"/> values</param>
        /// <returns></returns>
        public static float NormalizatedBezier(float B, float C, float value)
        {
            float s = 1f - value;
            float s2 = s * s;
            float v2 = value * value;
            float v3 = value * value * value;


            return (3f * B * s2 * value) + (3f * C * s * v2) + (v3);
        }

        /// <summary>
        /// Returns the fractional(decimal) value always regardles of the
        /// enter part
        /// </summary>
        public static float FractionalUnit(float value)
        {
            return value % 1;
        }
    }
}
