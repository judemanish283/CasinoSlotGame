using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SymbolInfo;
using System;
using UnityEditor.ShaderGraph.Internal;

public class GameController : MonoBehaviour
{
    [SerializeField] List<SymbolSO> symbols;
    [SerializeField] List<InfintyScroller> symbolsRows;

    [SerializeField]
    List<List<int>> paylines = new List<List<int>>
    {
        new List<int> { 5,5,5,5,5}
    };
    [SerializeField] Button SpinBtn;
    [SerializeField] int[,] multiDimensionalArray1 ;
    [SerializeField] GameObject imageObj;
    [SerializeField] float MaxscrollDuration = 5f;
    [SerializeField] float MinscrollDuration = 3f;
    List<GameObject> ConsideredSymbols = new List<GameObject>();
    GameObject[,] _ConsidSymb = new GameObject[3,5];

    private void Start()
    {
        SpinBtn.onClick.AddListener(() => StartCoroutine(SpinAllRows()));
        //int vakl = Mathf.C
        GenerateSymbolRow();
    }    

    void GenerateSymbolRow()
    {
        for(int i = 0; i < symbolsRows.Count; i++)
        {
            symbols.Shuffle();
            for( int j = 0; j < symbols.Count; j++ ) 
            {
                GameObject SymbolImage = Instantiate(imageObj , symbolsRows[i].content);
                SymbolImage.name = $"({i.ToString()},{j.ToString()})";
                SymbolImage.GetComponent<Image>().sprite = symbols[j].symbolSprite;
                SymbolImage.GetComponent<SymbolImage>().SymbolType = symbols[j].SymbolType;
            }
        }
    }

    IEnumerator SpinAllRows()
    {
        foreach(var row in symbolsRows)
        {
            float spinDuration = UnityEngine.Random.Range(MaxscrollDuration, MinscrollDuration);
            StartCoroutine(row.ScrollForDuration(spinDuration));
            yield return new WaitForSeconds(spinDuration);
        }
        yield return new WaitForSeconds(1f);
        CheckPaylines(ReturnConsideredSymbols());
    }

    List<GameObject> ReturnConsideredSymbols()
    {
        //for (int i = 0; i < symbolsRows.Count; i++)
        //{  // Loop through the rows
        //    for (int j = 0; j < 5; j++)
        //    {  // Loop through the columns
        //       // Instantiate a new GameObject and assign it to the array at position (i, j)
        //        _ConsidSymb[i, j] = symbolsRows[i].content.GetChild()
        //    }
        //}

        foreach (var row in symbolsRows)
        {
            ConsideredSymbols.Add(row.content.GetChild(4).gameObject);
            ConsideredSymbols.Add(row.content.GetChild(5).gameObject);
            ConsideredSymbols.Add(row.content.GetChild(6).gameObject);
            //Debug.Log(row.content.GetChild(4).gameObject.name, row.content.GetChild(4));
            //Debug.Log(row.content.GetChild(5).gameObject.name, row.content.GetChild(5));
            //Debug.Log(row.content.GetChild(6).gameObject.name, row.content.GetChild(6));
        } 
        return ConsideredSymbols;
    }

    void CheckPaylines(List<GameObject> symbolsToCheck)
    {
        foreach (var row in symbolsRows) { }
    }
}

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
