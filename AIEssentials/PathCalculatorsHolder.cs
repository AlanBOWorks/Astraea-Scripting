using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AIEssentials
{
    public static class PathCalculatorsConstructor
    {
        public static IPathCalculator[] FragmentPathCalculators(IPathCalculator[] fullArray, out IPathCalculator firstCalculator)
        {
            if(fullArray.Length <= 1) 
                throw new ArgumentException("The array should have a length of 2 or more");
            
            IPathCalculator[] helpers = new IPathCalculator[fullArray.Length -1];
            for (int i = 1; i < fullArray.Length; i++)
            {
                helpers[i - 1] = fullArray[i];
            }

            firstCalculator = fullArray[0];
            return helpers;
        }

    }
}
