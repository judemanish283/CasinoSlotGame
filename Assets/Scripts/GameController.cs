using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SymbolInfo;
using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Unity.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField] List<SymbolSO> symbols;
    [SerializeField] List<InfintyScroller> symbolsRows;

    [SerializeField] int NumberofPaylines = 2;

    [SerializeField]
    List<int[,]> paylines = new List<int[,]> 
    {
        new int[5, 2] { { 0, 1 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 1 } },
        new int[5, 2] { { 0, 0 }, { 1, 0 }, { 2, 1 }, { 3, 2 }, { 4, 2 } },
         new int[5, 2] { { 0, 1 }, { 1, 2 }, { 2, 1 }, { 3, 0 }, { 4, 1 } }
         //new int[5, 2] { { 1, 2 }, { 3, 4 }, { 1, 2 }, { 3, 4 }, { 1, 2 } }
    };
    [SerializeField] Button SpinBtn;
    [SerializeField] int[,] multiDimensionalArray1 ;
    [SerializeField] GameObject imageObj;
    [SerializeField] float MaxscrollDuration = 5f;
    [SerializeField] float MinscrollDuration = 3f;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject[,] _ConsidSymb = new GameObject[5,3];

    // This is the co-odinates by which the symbols are stored
    // (0,0) (1,0) (2,0) (3,0) (4,0)
    // (0,1) (1,1) (2,1) (3,1) (4,1)
    // (0,2) (1,2) (2,2) (3,2) (4,2)


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
        StartCoroutine(CheckPaylines(ReturnConsideredSymbols()));
    }

    GameObject[,] ReturnConsideredSymbols()
    {
        int row = 0;
        int column = 0;

        for (int i = 0; i < symbolsRows.Count && row < 5; i++) // Loop over the symbolsRows
        {
            for (int j = 0; j < 3 && row < 5; j++) // Loop to get the three children (indices 4, 5, 6)
            {
                // Get the child from the content and set its color to red
                symbolsRows[i].content.GetChild(j + 4).gameObject.GetComponentInChildren<Image>().color = Color.red;
                
                // Assign the GameObject to the 2D array
                _ConsidSymb[i, j] = symbolsRows[i].content.GetChild(j + 4).gameObject;

                // Log the current object position and its coordinates
                //Debug.Log("Obj" + i + "," + j + "    row :" + row + "   col : " + column, symbolsRows[i].content.GetChild(j + 4));

                // Increment the column, and move to the next row if the end of the row is reached
                column++;
                if (column == 5) // Check if we reached the end of a 5-column row
                {
                    column = 0;
                    row++; // Move to the next row
                }
            }
        }

        // Debug.Log("Number of items is : " + _ConsidSymb.Length + "should be 15");
        // Debug.Log("4,1 game object name is : " + _ConsidSymb[4,1].gameObject.name, _ConsidSymb[4,1].gameObject);
        // Debug.Log("4,2 game object name is : " + _ConsidSymb[4,2].gameObject.name, _ConsidSymb[4,2].gameObject);
        // Debug.Log("3,2 game object name is : " + _ConsidSymb[3,2].gameObject.name, _ConsidSymb[3,2].gameObject);
        // Debug.Log("3,1 game object name is : " + _ConsidSymb[3,1].gameObject.name, _ConsidSymb[3,1].gameObject);
        

        return _ConsidSymb;
    }

    IEnumerator CheckPaylines(GameObject[,] symbolsToCheck)
    {
        foreach (var line in paylines) 
        {
            lineRenderer.positionCount = 0;

            // Set the position count of the LineRenderer to the number of objects in the current payline
            lineRenderer.positionCount = line.GetLength(0);

            for (int i = 0; i < line.GetLength(0); i++) // Loop through rows in the line
            {
                int x = line[i, 0];  // X-coordinate
                int y = line[i, 1];  // Y-coordinate

                Vector3 objectPosition = symbolsToCheck[x, y].transform.position;

                // Set the position for the LineRenderer
                lineRenderer.SetPosition(i, objectPosition);

                // Get the object at (x, y)
                Debug.Log("symbol is : " + symbolsToCheck[x, y].GetComponent<SymbolImage>().SymbolType.ToString());
                
                yield return new WaitForSeconds(0.5f);
                symbolsToCheck[x,y].GetComponent<Image>().color = Color.white;

            }

            Debug.Log("Next Payline");
        }
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
