using UnityEngine;

namespace SMaths
{
    public static class SVector3 {

      
        private const int FakeDistanceDigits = 6;

        public static Vector3 AllSameValue(float value)
        {
            return Vector3.one * value;
        }
        public static void AllSameValue(Vector3 vector, float value)
        {
            vector.x = value;
            vector.y = value;
            vector.z = value;
        }
        public static bool IsDistanceLower(Vector3 pointA, Vector3 pointB, float distance)
        {
            return (pointA -pointB).sqrMagnitude < distance * distance;
        }
        public static bool IsDistanceBigger(Vector3 pointA, Vector3 pointB, float distance)
        {
            return (pointA - pointB).sqrMagnitude > distance * distance;
        }

        public static float SqrMagnitude(Vector3 vector)
        {
            return vector.x * vector.x +
                   vector.y * vector.y +
                   vector.z * vector.z;
        }

        /// <summary>Returns true if bigger or equals</summary>
        /// <returns>Returns true if bigger or equals</returns>
        public static bool Fake_DistanceComparation(Vector3 vectorA, Vector3 vectorB)
        {
            return SqrMagnitude(vectorA) >= SqrMagnitude(vectorB);
        }

        /// <summary>Returns true if bigger or equals</summary>
        /// <returns>Returns true if bigger or equals</returns>
        public static bool Fake_DistanceComparation(Vector3 vector, float distance)
        {
            return SqrMagnitude(vector) >= distance * distance;
        }

        public static float Fake_DistanceComparation_Ranged01(Vector3 vector, float distanceOfZero, float distanceDropOf)
        {
            SRange sRange = new SRange(distanceOfZero * distanceOfZero, distanceDropOf * distanceDropOf);
            return sRange.PercentageUnitInterval(SqrMagnitude(vector));
        }

        public static float GetSquareDistance(Vector3 vector)
        {
            return SqrMagnitude(vector);
        }

        public static float GetSquareDistance(Vector3 pointA, Vector3 pointB)
        {
            return SqrMagnitude(pointA - pointB);
        }

        public static float GetSquareDistance_WithoutY(Vector3 vector)
        {
            return vector.x * vector.x + vector.z * vector.z;
        }


        public static float FakeDistance(Vector3 vector)
        {
            return SMaths.SquareRoot(
                GetSquareDistance(vector),
                FakeDistanceDigits);
        }
        public static float FakeDistance_WithoutY(Vector3 vector)
        {
            return FakeDistance_WithoutY(vector, FakeDistanceDigits);
        }

        public static float FakeDistance_WithoutY(Vector3 vector,int distanceDigits)
        {
            return SMaths.SquareRoot(
                GetSquareDistance_WithoutY(vector),
                distanceDigits);
        }

        public static float Sin(Vector3 pointA, Vector3 pointB)
        {
            return Mathf.Sin(Vector3.Angle(pointA, pointB));
        }

        public static float SqrtSin(Vector3 pointA, Vector3 pointB)
        {
            return Sin(pointA, pointB) * Sin(pointA, pointB);
        }

        public static float Cos(Vector3 pointA, Vector3 pointB)
        {
            return Mathf.Cos(Vector3.Angle(pointA, pointB));
        }

        public static float SqrtCos(Vector3 pointA, Vector3 pointB)
        {
            return Cos(pointA, pointB) * Sin(pointA, pointB);
        }

        public static Vector3 GenerateVector(Vector3 pointA, Vector3 pointB)
        {
            return pointB - pointA;
        }

        public static Vector3 RemoveY(Vector3 point)
        {
            point.y = 0;
            return point;
        }

        public static Vector3 MultiplyXYZ(Vector3 applyTo, Vector3 byVector)
        {
            applyTo.x *= byVector.x;
            applyTo.y *= byVector.y;
            applyTo.z *= byVector.z;
            return applyTo;
        }



        public static Vector3 ProjectOnTransformUp(Transform onTransform, Vector3 point)
        {
            Vector3 localPosition = onTransform.InverseTransformPoint(point);
            localPosition.y = 0;
            return onTransform.TransformPoint(Vector3.ProjectOnPlane((localPosition), onTransform.up));
        }

        public static Vector3 ProjectOnSameHeigh(Vector3 onProjected, Vector3 point)
        {
            return new Vector3(point.x,onProjected.y,point.z);
        }

        public static Vector2 Vector3ToVector2NotY(Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        /// <summary>
        /// Rounds the <param name="vector"> into decimal while maintaining the <seealso cref="Vector3"/></param>
        /// <para><paramref name="decimalsInPow10"/> needs to be in Pow of 10</para>
        /// </summary>
        /// <param name="decimalsInPow10">Decimals (Use pow of TEN)</param>
        public static Vector3 RoundVector3(Vector3 vector, float decimalsInPow10)
        {
            vector.x = SMaths.Round(vector.x, decimalsInPow10);
            vector.y = SMaths.Round(vector.y, decimalsInPow10);
            vector.z = SMaths.Round(vector.z, decimalsInPow10);
            return vector;
        }

        /// <summary>
        /// Rounds the <param name="vector"> into decimal (in lower values) while maintaining the <seealso cref="Vector3"/></param>
        /// <para><paramref name="decimalsInPow10"/> needs to be in Pow of 10</para>
        /// </summary>
        /// <param name="decimalsInPow10">Decimals (Use pow of TEN)</param>
        public static Vector3 FloorVector3(Vector3 vector, float decimalsInPow10)
        {
            vector.x = SMaths.Floor(vector.x, decimalsInPow10);
            vector.y = SMaths.Floor(vector.y, decimalsInPow10);
            vector.z = SMaths.Floor(vector.z, decimalsInPow10);
            return vector;
        }

        public static float FakeDistance(Vector3 vector, int numberOfDigits)
        {
            return SMaths.SquareRoot(SqrMagnitude(vector),numberOfDigits);
        }

        public static Vector3 VectorSameParamProduct(Vector3 reference, Vector3 multiplier)
        {
            Vector3 initial = reference;
            initial.x *= multiplier.x;
            initial.y *= multiplier.y;
            initial.z *= multiplier.z;

            return initial;
        }

        public static Vector3 VectorSameParamDiv(Vector3 reference, Vector3 divisor)
        {
            Vector3 initial = reference;
            initial.x /= divisor.x;
            initial.y /= divisor.y;
            initial.z /= divisor.z;

            return initial;
        }
    }
}
