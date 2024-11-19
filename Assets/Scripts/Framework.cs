using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SymbolInfo
{
    [Serializable]

    public class Payline
    {
        [SerializeField] public int[,] nums;
    }
    public enum eSymbolType
    {
        A,
        K,
        Ten,
        Q,
        J,
        Leaf,
        Bear,
        Deer,
        Fox,
        Squirrel,
        Mushroom,
        Wild
    }
    [Serializable]
    public class SymbolData
    {
        public eSymbolType SymbolType;
        public Sprite symbolSprite;
        public int SymbolVal;
    }

    public static class IListExtensions
    {
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}
