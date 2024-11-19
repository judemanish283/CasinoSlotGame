using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SymbolInfo;
using UnityEngine.UI;

public class SymbolImage : MonoBehaviour
{
    public eSymbolType SymbolType;
    [SerializeField] GameObject GlowImg;

    public void ShowGlow()
    {
        //gameObject.transform.localScale = Vector3.one * 1.5f;
        GlowImg.SetActive(true);
    }

    public void HideGlow() 
    {
        //gameObject.transform.localScale = Vector3.one ;
        GlowImg.SetActive(false);
    }
}
