using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SymbolInfo;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Unity.Collections;
using DG.Tweening;

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


    [SerializeField] GameObject prizePanel;
    [SerializeField] Image prizeImg;
    [SerializeField] TMP_Text prizePanelTxt, PrizeTxt;

    int prizeVal = 0;


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
                SymbolImage.GetComponent<Image>().sprite = symbols[j].symbolData.symbolSprite;
                SymbolImage.GetComponent<SymbolImage>().SymbolType = symbols[j].symbolData.SymbolType;
            }
        }
    }

    IEnumerator SpinAllRows()
    {
        lineRenderer.positionCount = 0;

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

        return _ConsidSymb;
    }

    IEnumerator CheckPaylines(GameObject[,] symbolsToCheck)
    {
        lineRenderer.transform.position = symbolsToCheck[0,0].transform.position;
        foreach (var line in paylines) 
        {
            lineRenderer.positionCount = 0;

            // Set the position count of the LineRenderer to the number of objects in the current payline
            lineRenderer.positionCount = line.GetLength(0);

            Dictionary<eSymbolType, int> symbolCounts = new Dictionary<eSymbolType, int>();
            
            HashSet <GameObject> paylineObjects = new HashSet<GameObject>();

            for (int i = 0; i < line.GetLength(0); i++) // Loop through rows in the line
            {

                int x = line[i, 0];  // X-coordinate
                int y = line[i, 1];  // Y-coordinate

                GameObject obj = symbolsToCheck[x, y];

                paylineObjects.Add(obj);

                Vector3 objectPosition = obj.transform.position;

                eSymbolType symbolType = obj.GetComponent<SymbolImage>().SymbolType;

                if (symbolCounts.ContainsKey(symbolType))
                {
                    symbolCounts[symbolType]++;
                }
                else
                {
                    symbolCounts[symbolType] = 1;
                }

                lineRenderer.positionCount = i + 1;
                // Set the position for the LineRenderer
                lineRenderer.SetPosition(i, objectPosition);

                // Get the object at (x, y)
                //Debug.Log("symbol is : " + symbolsToCheck[x, y].GetComponent<SymbolImage>().SymbolType.ToString());
                //symbolsToCheck[x,y].GetComponent<SymbolImage>().ShowGlow();
                
            }

            foreach (var kvp in symbolCounts)
            {
                if(kvp.Value >= 3)
                {
                    Debug.Log(kvp.Key + " is present more than 3 times");
                    foreach (var obj in paylineObjects)
                    {
                        if (obj.GetComponent<SymbolImage>().SymbolType == kvp.Key)
                            obj.GetComponent<SymbolImage>().ShowGlow();
                    }
                    yield return new WaitForSeconds(2f);

                    yield return StartCoroutine(SetPrizeText(kvp.Key, kvp.Value));

                    prizePanel.gameObject.SetActive(false);

                    PrizeTxt.text = "PRIZE : "+ prizeVal.ToString();
                }
                
            }

            yield return new WaitForSeconds(5f);

            foreach (var obj in paylineObjects)
            {
                obj.GetComponent<SymbolImage>().HideGlow();
            }


            Debug.Log("Next Payline");

        }

        lineRenderer.positionCount = 0;
    }


    IEnumerator SetPrizeText(eSymbolType symbolType, int symbolCount)
    {
        prizePanel.gameObject.SetActive(true);
        SymbolData data = symbols.Find(x => x.symbolData.SymbolType == symbolType).symbolData;
        prizeImg.sprite = data.symbolSprite;
        prizeVal += symbolCount * data.SymbolVal;
        prizePanelTxt.text = "+" + prizeVal.ToString();
        yield return new WaitForSeconds(3f);
    }
}


