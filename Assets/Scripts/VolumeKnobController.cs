using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeKnobController : MonoBehaviour
{
    public TMPro.TextMeshPro text;

    [HideInInspector]
    public float value = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        value = GetComponent<VolumeKnobController>().value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onValueChanged(float val)
    {
        value = val;
        text.text = val.ToString("F2");

       GameManager.Instance.updatePumpStrenghtVisualiation();

        if(GameManager.Instance.eventManager != null)
            GameManager.Instance.eventManager.pipeIntensityChanged(transform, val);
    }
}
