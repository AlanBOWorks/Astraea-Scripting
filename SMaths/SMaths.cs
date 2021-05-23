using System.Collections.Generic;
using UnityEngine;

namespace SMaths
{
    /// <summary>
    /// Math library for projects
    /// </summary>
    public static class SMaths
    {

        /// <summary>
        /// Returns -<see cref="multiplier"/> if is NOT pair; otherwise returns <see cref="multiplier"/> in its original value
        /// </summary>
        public static float NegativeNotPairModifier(float pair, float multiplier)
        {
            return (-2*(pair % 2) +1) * multiplier;
        }

        public static float NotNegative(float amount)
        {
            return Mathf.Max(0, amount);
        }

        //--------
        //--------
        //-- SQUARES --
        //--------
        //--------

        /// <summary>
        /// Calculates the square value of <paramref name="value"/>
        /// </summary>
        /// <param name="value">The desired value^2</param>
        /// <returns>Resultant square value of <paramref name="value"/></returns>
        public static float Square(float value)
        {
            return value * value;
        }

        /// <summary>
        /// Generates power of ten of the number passed
        /// </summary>
        /// <param name="num">what value of power of ten we want</param>
        /// <returns>Power of ten of the desired number</returns>
        public static double PowerOfTenDouble(int num)
        {
            double rst = 1.0;
            if (num < 0)
            {
                for (int i = 0; i < (0 - num); i++)
                {
                    rst *= 0.1;
                }
            }
            else
            {
                for (int i = 0; i < num; i++)
                {
                    rst *= 10.0;
                }
            }

            return rst;
        }

        /// <summary>
        /// Generates power of ten of the number passed
        /// </summary>
        /// <param name="num">what value of power of ten we want</param>
        /// <returns>Power of ten of the desired number</returns>
        public static float PowerOfTen(int num)
        {
            float rst = 1.0f;
            if (num < 0)
            {
                for (int i = 0; i < (0 - num); i++)
                {
                    rst *= 0.1f;
                }
            }
            else
            {
                for (int i = 0; i < num; i++)
                {
                    rst *= 10.0f;
                }
            }

            return rst;
        }

        /// <summary>
        /// Defines the squareRoot value of the number passed
        /// </summary>
        /// <param name="num">What square value we want</param>
        /// <returns>Resulting value of <param name="num"></returns>
        public static double SquareRootDouble(double num, int maxDigits)
        {
            /*
                  find more detail of this method on wiki methods_of_computing_square_roots
    
                  *** Babylonian method cannot get exact zero but approximately value of the square_root
             */
            double z = num;
            double rst = 0.0;
            int i;
            double j = 1.0;
            for (i = maxDigits; i > 0; i--)
            {
                // value must be bigger then 0
                if (z - ((2 * rst) + (j * PowerOfTenDouble(i))) * (j * PowerOfTenDouble(i)) >= 0)
                {
                    while (z - ((2 * rst) + (j * PowerOfTenDouble(i))) * (j * PowerOfTenDouble(i)) >= 0)
                    {
                        j++;
                        if (j >= 10) break;

                    }

                    j--; //correct the extra value by minus one to j
                    z -= ((2 * rst) + (j * PowerOfTenDouble(i))) * (j * PowerOfTenDouble(i)); //find value of z

                    rst += j * PowerOfTenDouble(i); // find sum of a

                    j = 1.0;


                }

            }

            for (i = 0; i >= 0 - maxDigits; i--)
            {
                if (z - ((2 * rst) + (j * PowerOfTenDouble(i))) * (j * PowerOfTenDouble(i)) >= 0)
                {
                    while (z - ((2 * rst) + (j * PowerOfTenDouble(i))) * (j * PowerOfTenDouble(i)) >= 0)
                    {
                        j++;

                    }

                    j--;
                    z -= ((2 * rst) + (j * PowerOfTenDouble(i))) * (j * PowerOfTenDouble(i)); //find value of z

                    rst += j * PowerOfTenDouble(i); // find sum of a
                    j = 1.0;
                }
            }

            // find the number on each digit
            return rst;
        }

        /// <summary>
        /// Defines the squareRoot value of the number passed
        /// </summary>
        /// <param name="num">What square value we want</param>
        /// <param name="maxDigits"/>Max of decimal digits</param>
        /// <returns>Resulting value of <param name="num"></returns>
        public static float SquareRoot(float num, int maxDigits)
        {
            /*
                  find more detail of this method on wiki methods_of_computing_square_roots
    
                  *** Babylonian method cannot get exact zero but approximately value of the square_root
             */
            float z = num;
            float rst = 0.0f;
            int i;
            float j = 1.0f;
            for (i = maxDigits; i > 0; i--)
            {
                // value must be bigger then 0
                if (z - ((2 * rst) + (j * PowerOfTen(i))) * (j * PowerOfTen(i)) >= 0)
                {
                    while (z - ((2 * rst) + (j * PowerOfTen(i))) * (j * PowerOfTen(i)) >= 0)
                    {
                        j++;
                        if (j >= 10) break;

                    }

                    j--; //correct the extra value by minus one to j
                    z -= ((2 * rst) + (j * PowerOfTen(i))) * (j * PowerOfTen(i)); //find value of z

                    rst += j * PowerOfTen(i); // find sum of a

                    j = 1.0f;


                }

            }

            for (i = 0; i >= 0 - maxDigits; i--)
            {
                if (z - ((2 * rst) + (j * PowerOfTenDouble(i))) * (j * PowerOfTenDouble(i)) >= 0)
                {
                    while (z - ((2 * rst) + (j * PowerOfTenDouble(i))) * (j * PowerOfTenDouble(i)) >= 0)
                    {
                        j++;

                    }

                    j--;
                    z -= ((2 * rst) + (j * PowerOfTen(i))) * (j * PowerOfTen(i)); //find value of z

                    rst += j * PowerOfTen(i); // find sum of a
                    j = 1.0f;
                }
            }

            // find the number on each digit
            return rst;
        }

        public static float SquareRoot(float num)
        {
            double result = SquareRoot(num, 8);

            return (float) result;
        }

        /// <summary>
        /// Generic formula for normalizated Sine which return a value within the wave in a range of [-1,1] in a period of 1
        /// </summary>
        /// <param name="value">The value of the funtion (e.g: t) in that part</param>
        /// <returns>Sin(<seealso cref="Mathf.PI"/> * <paramref name="value"/></returns>
        public static float Sin_Normalized(float value)
        {
            return Sin_Normalized(value, 0f);
        }

        /// <summary>
        /// Generic formula for normalizated Sine which return a value within the wave in a range of [-1,1] in a period of 1
        /// </summary>
        /// <param name="value">The value of the funtion (e.g: t) in that part</param>
        /// <param name="offset">The offset of this function which it starts</param>
        /// <returns>Sin(<seealso cref="Mathf.PI"/> * <paramref name="value"/></returns>
        public static float Sin_Normalized(float value, float offset)
        {
            return Mathf.Sin(Mathf.PI / 2 * (value + offset));
        }

        /// <summary>
        /// Generic formula for normalizated Sine which return a value within the wave in a range of [-1,1] in a period of <paramref name="period"/>
        /// </summary>
        /// <param name="value">The value of the funtion (e.g: t) in that part</param>
        /// <param name="offset">The offset of this function which it starts</param>
        /// <param name="period">In which value the function should repeat its value after reaching this value</param>
        /// <returns>Sin(<seealso cref="Mathf.PI"/> * <paramref name="value"/> in the <paramref name="period"/></returns>
        public static float Sin_Normalized(float value, float offset, float period)
        {
            return Sin_Normalized(value / period, offset / period);
        }




        /// <summary>
        /// Generic formula for normalizated Cosine which return a value within the wave in a range of [-1,1]
        /// </summary>
        /// <param name="value">The value of the funtion (e.g: t) in that part</param>
        /// <returns>Sin(<seealso cref="Mathf.PI"/> * <paramref name="value"/></returns>
        public static float Cos_Normalized(float value)
        {
            return Cos_Normalized(value, 0f);
        }

        /// <summary>
        /// Generic formula for normalizated Cosine which return a value within the wave in a range of [-1,1] in a period of 1
        /// </summary>
        /// <param name="value">The value of the funtion (e.g: t) in that part</param>
        /// <param name="offset">The offset of this function which it starts</param>
        /// <returns>Sin(<seealso cref="Mathf.PI"/> * <paramref name="value"/></returns>
        public static float Cos_Normalized(float value, float offset)
        {
            return Mathf.Cos(Mathf.PI / 2 * (value + offset));
        }

        /// <summary>
        /// Generic formula for normalizated Cosine which return a value within the wave in a range of [-1,1] in a period of <paramref name="period"/>
        /// </summary>
        /// <param name="value">The value of the funtion (e.g: t) in that part</param>
        /// <param name="offset">The offset of this function which it starts</param>
        /// <param name="period">In which value the function should repeat its value after reaching this value</param>
        /// <returns>Sin(<seealso cref="Mathf.PI"/> * <paramref name="value"/> in the <paramref name="period"/></returns>
        public static float Cos_Normalized(float value, float offset, float period)
        {
            return Cos_Normalized(value / period, offset / period);
        }



        /// <summary>
        /// Rounds the <param name="value"> into decimals</param>
        /// <para><paramref name="decimalsInPow10"/> needs to be in Pow of 10</para>
        /// </summary>
        /// <param name="decimalsInPow10">Decimals (Use pow of TEN)</param>
        public static float Round(float value, float decimalsInPow10)
        {
            return (Mathf.Round(value * decimalsInPow10)) / decimalsInPow10;
        }

        /// <summary>
        /// Rounds the <param name="value"> into decimals in (lower value)</param>
        /// <para><paramref name="decimalsInPow10"/> needs to be in Pow of 10</para>
        /// </summary>
        /// <param name="decimalsInPow10">Decimals (Use pow of TEN)</param>
        public static float Floor(float value, float decimalsInPow10)
        {
            return (Mathf.Floor(value * decimalsInPow10)) / decimalsInPow10;
        }


        public static float Ceil(float value, float decimalsInPow10)
        {
            return (Mathf.Ceil(value * decimalsInPow10)) / decimalsInPow10;
        }

        private static readonly SRange Range01 = new SRange(0, 1);

        public static float InRange01(float value)
        {
            return Range01.ForceInRange(value);
        }

        /// <summary>
        /// Generates Non-Consecutive randoms numbers (Shouldn't be too large)
        /// </summary>
        public static int[] SmallNonConsecutiveRandoms(int amountOfRandoms,int lowPick)
        {
            int[] randoms = new int[amountOfRandoms];
            List<int> pool = new List<int>();
            int i;
            for (i = 0; i < randoms.Length; i++)
            {
                pool.Add(i+lowPick);
            }
            for (i = 0; i < randoms.Length; i++)
            {
                randoms[i] = pool[UnityEngine.Random.Range(lowPick, amountOfRandoms + lowPick - i)];
                pool.Remove(randoms[i]);
            }
            return randoms;
        }

        /// <summary>
        /// Generates Non-Consecutive randoms numbers (Shouldn't be too large)
        /// </summary>
        public static void SmallNonConsecutiveRandoms(int[] randomsHolder, int lowPick)
        {
            List<int> pool = new List<int>();
            int i;
            for (i = 0; i < randomsHolder.Length; i++)
            {
                pool.Add(i+lowPick);
            }
            for (i = 0; i < randomsHolder.Length; i++)
            {
                randomsHolder[i] = pool[UnityEngine.Random.Range(lowPick, randomsHolder.Length + lowPick - i)];
                pool.Remove(randomsHolder[i]);
            }
        }
    }
}