using System.Collections.Generic;
using UnityEngine;

namespace SMaths
{
    public static class SRandom 
    {

        public static int[] GenerateRandomUniqueIndexes(int amount)
        {
            List<int> pollList = SOrderly.OrderlyList(amount,0,1);
            int[] generatedIndexes = new int[amount];
           
            int i = 0;
            while (pollList.Count > 0)
            {
                int pollPickIndex = Random.Range(0, pollList.Count);
                generatedIndexes[i] = pollList[pollPickIndex];
                pollList.RemoveAt(pollPickIndex);
                i++;
            }
            pollList.Clear();
            return generatedIndexes;
        }

        public static List<int> GenerateRandomUniqueIndexList(int amount)
        {
            List<int> pollList = SOrderly.OrderlyList(amount,0,1);
            List<int> generatedList = new List<int>();
            while (pollList.Count > 0)
            {
                int pollPickIndex = Random.Range(0, pollList.Count);
                generatedList.Add(
                    pollList[pollPickIndex]
                    );
                pollList.RemoveAt(pollPickIndex);
            }
            pollList.Clear();
            return generatedList;
        }

        
    }

    /// <summary>
    /// Orderly randoms (orderly Increment)
    /// </summary>
    public static class SOrderly
    {
        /// <summary>
        /// Returns an array of int in orderly increment
        /// </summary>
        public static int[] OrderlyArray(int amountOfElements, int initialPos, int increment)
        {
            int[] array = new int[amountOfElements];
            for (int i = initialPos; i < amountOfElements; i++)
            {
                array[i] = i * increment;
            }

            return array;
        }
        public static List<int> OrderlyList(int amountOfElements, int initialPos, int increment)
        {
            List<int> pollList = new List<int>();
            for (int i = initialPos; i < amountOfElements; i++)
            {
                pollList.Add(i*increment);
            }

            return pollList;
        }
    }
}
