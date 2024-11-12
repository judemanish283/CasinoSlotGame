using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleController : MonoBehaviour
{
    public static Action SpinButtonClicked;

    [SerializeField] Button SpinBtn;
    [SerializeField] TMP_Text prizeTxt;
    [SerializeField] SimpleRow[] rows;
    int prizeVal;
    bool resultsChecked;
    // Start is called before the first frame update
    void Start()
    {
        prizeTxt.enabled = false;
        SpinBtn.onClick.AddListener(() => StartCoroutine(PullHandle()));
    }   

    // Update is called once per frame
    void Update()
    {
        if (!rows[0].rowStopped || !rows[1].rowStopped || !rows[2].rowStopped)
        {
            resultsChecked = false;
        }
        if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped && !resultsChecked)
        {
            CheckResults();
        }
    }

    IEnumerator PullHandle()
    {
        if (rows[0].rowStopped && rows[1].rowStopped && rows[2].rowStopped)
        {
            SpinButtonClicked();
        }
        yield return null;
    }

    void CheckResults()
    {
        resultsChecked = true;
    }
}
