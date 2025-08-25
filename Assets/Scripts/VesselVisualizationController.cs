using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesselVisualizationController : MonoBehaviour
{
    TMPro.TextMeshPro textPh;
    TMPro.TextMeshPro textTemp;
    TMPro.TextMeshPro textPump;
    GameObject juiceVis;

    // Start is called before the first frame update
    void Start()
    {

        textTemp = transform.Find("VisTemp").GetChild(0).GetComponentInChildren<TMPro.TextMeshPro>();
        textPh = transform.Find("VisPh").GetChild(0).GetComponentInChildren<TMPro.TextMeshPro>();
        textPump = transform.Find("VisJuice").GetChild(0).GetComponentInChildren<TMPro.TextMeshPro>();
        juiceVis = transform.Find("VisJuice").GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTempText(float value)
    {
        if (value == 999)
            textTemp.text = "--";
        else
            textTemp.text = value.ToString("F1") + "°";
    }

    public void setPhText(float value)
    {
        if (value == 999)
            textPh.text = "--";
        else
            textPh.text = value.ToString("F1");
    }

    public void setPumpIdText(int id)
    {
        textPump.text = id.ToString();
    }

    public void setJuiceMatVis(Material mat)
    {
        Debug.Log("change juice mat vis!!! to " + mat.color.ToString());
        juiceVis.GetComponent<Renderer>().material = mat;
    }
}
