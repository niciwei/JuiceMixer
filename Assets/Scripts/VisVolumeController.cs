using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisVolumeController : MonoBehaviour
{

    TMPro.TextMeshPro volumeText;

    // Start is called before the first frame update
    void Start()
    {
        volumeText = GetComponentInChildren<TMPro.TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setVolumeText(float value)
    {
        float v = value * 100.0f;
        if(volumeText != null)
            volumeText.text = v.ToString("F0") + "%";
    }
}
