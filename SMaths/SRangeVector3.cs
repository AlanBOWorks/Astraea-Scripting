using System;
using UnityEngine;

namespace SMaths
{
    [Serializable]
    public class SRangeVector3
    {
        public SRange x;
        public SRange y;
        public SRange z;

        public SRangeVector3(SRange x, SRange y, SRange z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 InterpolationInRange(Vector3 weights)
        {
            return InterpolationInRange(weights.x, weights.y, weights.z);
        }
        public Vector3 InterpolationInRange(float xWeight, float yWeight, float zWeight)
        {
            Vector3 percentage;

            percentage.x = x.InterpolationInRange(xWeight);
            percentage.y = y.InterpolationInRange(yWeight);
            percentage.z = z.InterpolationInRange(zWeight);

            return percentage;
        }

        public Vector3 Interpolation(float xWeight, float yWeight, float zWeight)
        {
            Vector3 percentage;

            percentage.x = x.Interpolation(xWeight);
            percentage.y = y.Interpolation(yWeight);
            percentage.z = z.Interpolation(zWeight);

            return percentage;
        }
        public Vector3 Interpolation(Vector3 weights)
        {
            return Interpolation(weights.x, weights.y, weights.z);
        }
    }

    [Serializable]
    public struct SRange3Weighted
    {
        public SWeightRange x;
        public SWeightRange y;
        public SWeightRange z;


        public SRange3Weighted(SWeightRange x, SWeightRange y, SWeightRange z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 Interpolation()
        {
            Vector3 generated;
            generated.x = x.Interpolation();
            generated.y = y.Interpolation();
            generated.z = z.Interpolation();

            return generated;
        }
        public Vector3 InterpolationInRange()
        {
            Vector3 generated;
            generated.x = x.InterpolationInRange();
            generated.y = y.InterpolationInRange();
            generated.z = z.InterpolationInRange();

            return generated;
        }

        public void RecalculateWeights(Vector3 calculateThis)
        {
            x.weight = XPercentage(calculateThis.x);
            y.weight = YPercentage(calculateThis.y);
            z.weight = ZPercentage(calculateThis.z);
        }

        public float XPercentage(float value)
        {
            return x.Percentage(value);
        }
        public float YPercentage(float value)
        {
            return y.Percentage(value);
        }
        public float ZPercentage(float value)
        {
            return z.Percentage(value);
        }
        
    }
}
