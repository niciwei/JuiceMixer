using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisTempController : MonoBehaviour
{
    TMPro.TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TMPro.TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setTempText(float value)
    {
        if (value == 999)
            text.text = "--";
        else
            text.text = value.ToString("F1") + "°";
    }

    public void setPhText(float value)
    {
        if (value == 999)
            text.text = "--";
        else
            text.text = value.ToString("F1");
    }
}
