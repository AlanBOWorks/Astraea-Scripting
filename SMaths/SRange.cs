using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SMaths
{

    [Serializable]
    public struct SRange
    {
        public float minValue;
        public float maxValue;

        public SRange(float minValue, float maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        #region OPERATORS
        public static SRange operator +(SRange a, float f)
        {
            a.maxValue += f;
            a.minValue += f;
            return a;
        }
        public static SRange operator -(SRange a, float f)
        {
            a.maxValue -= f;
            a.minValue -= f;
            return a;
        }
        public static SRange operator *(SRange a, float f)
        {
            a.maxValue *= f;
            a.minValue *= f;
            return a;
        }

        public static bool operator >=(SRange a, float f)
        {
            return (a.maxValue - a.minValue) >= f;
        }
        public static bool operator >(SRange a, float f)
        {
            return (a.maxValue - a.minValue) > f;
        }
        public static bool operator ==(SRange a, float f)
        {
            return (a.maxValue - a.minValue) == f;
        }
        public static bool operator !=(SRange a, float f)
        {
            return (a.maxValue - a.minValue) != f;
        }
        public static bool operator <=(SRange a, float f)
        {
            return (a.maxValue - a.minValue) <= f;
        }
        public static bool operator <(SRange a, float f)
        {
            return (a.maxValue - a.minValue) < f;
        }


        public static SRange operator +(SRange a, SRange b)
        {
            a.maxValue += b.maxValue;
            a.minValue += b.minValue;
            return a;
        }
        public static SRange operator -(SRange a, SRange b)
        {
            a.maxValue -= b.maxValue;
            a.minValue -= b.minValue;
            return a;
        }
        public static SRange operator *(SRange a, SRange b)
        {
            a.maxValue *= b.maxValue;
            a.minValue *= b.minValue;
            return a;
        }

        public static bool operator >=(SRange a, SRange b)
        {
            return (a.maxValue - a.minValue) >= (b.maxValue - b.minValue);
        }
        public static bool operator >(SRange a, SRange b)
        {
            return (a.maxValue - a.minValue) > (b.maxValue - b.minValue);
        }
        public static bool operator <=(SRange a, SRange b)
        {
            return (a.maxValue - a.minValue) <= (b.maxValue - b.minValue);
        }
        public static bool operator <(SRange a, SRange b)
        {
            return (a.maxValue - a.minValue) < (b.maxValue - b.minValue);
        }

        public static bool operator ==(SRange a, SRange b)
        {
            return (a.maxValue == b.maxValue) && (a.minValue == b.minValue);
        }
        public static bool operator !=(SRange a, SRange b)
        {
            return (a.maxValue != b.maxValue) || (a.minValue != b.minValue);
        }

        public bool Equals(SRange other)
        {
            return minValue.Equals(other.minValue) && maxValue.Equals(other.maxValue);
        }
        public override bool Equals(object obj)
        {
            return obj is SRange other && Equals(other);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return (minValue.GetHashCode() * 397) ^ maxValue.GetHashCode();
            }
        }

        #endregion

        public float RandomInRange()
        {
            return Random.Range(minValue, maxValue);
        }

        public bool IsInRange(float value)
        {
            return IsInMaxRange(value) && IsInMinRange(value);
        }

        public bool IsInMaxRange(float value)
        {
            return value <= this.maxValue;
        }

        public bool IsInMinRange(float value)
        {
            return value >= this.minValue;
        }
        public static int ForceInRange(int value, int minValue, int maxValue)
        {
            return Mathf.Min(maxValue
                ,
                Mathf.Max(minValue, value)
            );
        }
       

        public static float ForceInRange(float value, SRange sRange)
        {
            return Mathf.Clamp(value, sRange.minValue, sRange.maxValue);
        }
        public float ForceInRange(float value)
        {
            return ForceInRange(value, this);
        }





        public static float Percentage(float value, float minValue, float maxValue)
        {
            return (value - minValue)
                   /
                   (maxValue - minValue);
        }

        public static float Percentage(float value, SRange sRange)
        {
            return Percentage(value, sRange.minValue, sRange.maxValue);
        }

        /// <summary>
        /// Returns the <paramref name="value"/> in percentage 
        /// </summary>
        public float Percentage(float value)
        {
            return Percentage(value, this);
        }

        /// <summary>
        /// Returns the <paramref name="value"/> in percentage with a value within (0,1)
        /// </summary>
        public float PercentageUnitInterval(float value)
        {
            return Mathf.Clamp01(this.Percentage(value));
        }

        /// <summary>
        /// Returns the <paramref name="value"/> in percentage with a value within unitInterval ( 1>= will return 0( instead )
        /// </summary>
        public float PercentageUnitIntervalWithoutOne(float value)
        {
            float returnValue = this.PercentageUnitInterval(value);
            return Mathf.Max(0 ,
                             1 - Mathf.Floor(returnValue) ) 
                              * returnValue;
        }

        /// <summary>
        /// Returns the LerpUnclamped value by %times
        /// </summary>
        public float Interpolation(float interpolation)
        {

            return Mathf.LerpUnclamped(this.minValue, this.maxValue,
                interpolation);
        }

        /// <summary>
        /// Returns the LerpUnclamped in maxValue (minValue will be clamped)
        /// (negative interpolation will return the <see cref="minValue"/>)
        /// </summary>
        public float InterpolationClampInMin(float interpolation)
        {
            float decimals = interpolation % 1;
            float integer = interpolation - decimals;
            return (InterpolationInRange(interpolation) - minValue) * (integer)
                   +
                   InterpolationInRange(decimals);
        }

        /// <summary>
        /// Returns the Lerp value multiply by %times (everytime interpolations
        /// becomes 1xTimes the minValue will bump the value up)
        /// </summary>
        public float InterpolationWithBump(float interpolation)
        {
            float decimals = interpolation % 1;
            return InterpolationInRange(interpolation) * (interpolation - decimals)
                   +
                   InterpolationInRange(decimals);
        }

        /// <summary>
        /// Returns the Lerp value within min/max values
        /// </summary>
        ///
        /// 
        public float InterpolationInRange(float interpolation)
        {
            return Mathf.Lerp(this.minValue, this.maxValue, interpolation);
        }

        
        /// <summary>
        /// Returns the interpolation in the range or 0 if off interpolation
        /// </summary>
        public float InterpolationInRangeOrZero(float interpolation)
        {
            return (interpolation < 0 || interpolation > 1) 
                ? 0 
                : this.InterpolationInRange(interpolation);
        }
    }
    [Serializable]
    public struct SWeightRange
    {
        public SRange range;
        public float weight;

        public SWeightRange(float min,float max, float weight)
        {
            range = new SRange(min,max);
            this.weight = weight;
        }

        public SWeightRange(SRange range, float weight)
        {
            this.range = range;
            this.weight = weight;
        }

        public float Interpolation()
        {
            return range.Interpolation(weight);
        }

        public float InterpolationInRange()
        {
            return range.InterpolationInRange(weight);
        }

        public float Percentage(float value)
        {
            return range.Percentage(value);
        }
    }

    public static class SRangeStatics
    {
        /// <summary>
        /// R(0,1)
        /// </summary>
        public static SRange unitRange;
        /// <summary>
        /// R(0,1) / w = 0.5
        /// </summary>
        public static SWeightRange unitWeightRange;

        /// <summary>
        /// R(-1,1)
        /// </summary>
        public static SRange negativeOneRange;
        /// <summary>
        /// R(-1,1) / w = 0.5
        /// </summary>
        public static SWeightRange negativeOneWeightRange;

        /// <summary>
        /// R(-0.1,0.1) 
        /// </summary>
        public static SRange decimalRange;
        /// <summary>
        /// R(-0.1,0.1) / w = 0.5
        /// </summary>
        public static SWeightRange decimalWeightRange;

        /// <summary>
        /// R(0,1)
        /// </summary>
        public static SRangeVector3 unitRange3;
        /// <summary>
        /// R(0,1) / w = 0.5
        /// </summary>
        public static SRange3Weighted unitWeightRange3;

        /// <summary>
        /// R(-1,1)
        /// </summary>
        public static SRangeVector3 negativeOneRange3;
        /// <summary>
        /// R(-1,1) / w = 0.5
        /// </summary>
        public static SRange3Weighted negativeOneWeightRange3;

        /// <summary>
        /// R(-0.1,0.1) 
        /// </summary>
        public static SRangeVector3 decimalRange3;
        /// <summary>
        /// R(-0.1,0.1) / w = 0.5
        /// </summary>
        public static SRange3Weighted decimalWeightRange3;


        static SRangeStatics()
        {
            unitRange = new SRange(0,1);
            unitWeightRange = new SWeightRange(unitRange,0.5f);

            negativeOneRange = new SRange(-1,1);
            negativeOneWeightRange = new SWeightRange(negativeOneRange,0.5f);

            decimalRange = new SRange(-.1f,.1f);
            decimalWeightRange = new SWeightRange(decimalRange,.5f);

            unitRange3 = new SRangeVector3(unitRange,unitRange,unitRange);
            unitWeightRange3 = new SRange3Weighted(unitWeightRange,unitWeightRange,unitWeightRange);

            negativeOneRange3 = new SRangeVector3(negativeOneRange,negativeOneRange,negativeOneRange);
            negativeOneWeightRange3 = new SRange3Weighted(negativeOneWeightRange,negativeOneWeightRange,negativeOneWeightRange);

            decimalRange3 = new SRangeVector3(decimalRange,decimalRange,decimalRange);
            decimalWeightRange3 = new SRange3Weighted(decimalWeightRange,decimalWeightRange,decimalWeightRange);
        }
    }
}
