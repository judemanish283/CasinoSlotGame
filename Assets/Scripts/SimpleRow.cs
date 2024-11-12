using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRow : MonoBehaviour
{
    int randomVal;
    float timeInterval;
    public bool rowStopped;
    string stoppedSlot;

    // Start is called before the first frame update
    void Start()
    {
        rowStopped = true;
        SimpleController.SpinButtonClicked += StartRotating;
    }

    void StartRotating()
    {
        stoppedSlot = "";
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(randomVal);
    }
}
