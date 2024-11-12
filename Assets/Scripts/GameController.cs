using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class GameController : MonoBehaviour
{
    [SerializeField] List<Sprite> symbols;
    [SerializeField] List<SymbolRow> symbolsRows;

    [SerializeField] Button SpinBtn;
    [SerializeField] ScrollView scroll;

    private void Start()
    {
        //SpinBtn.onClick.AddListener(() => GenerateSymbolRow());
        //int vakl = Mathf.C
    }

    void GenerateSymbolRow()
    {
        foreach( var row in symbolsRows ) 
        {
            for( int i = 0; i < symbols.Count; i++ ) 
            {
                //var SymbolImage = Instantiate()
            }
        }
    }
}
