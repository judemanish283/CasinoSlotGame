using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SymbolInfo;

[CreateAssetMenu(fileName ="Symbol")]
public class SymbolSO : ScriptableObject
{
    public eSymbolType SymbolType;
    public Sprite symbolSprite;
    public int symbolVal;
}
