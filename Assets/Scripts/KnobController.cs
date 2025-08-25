using System.Collections;
using System.Collections.Generic;
using Unity.VRTemplate;
using UnityEngine;

public class KnobController : MonoBehaviour
{
    public TMPro.TextMeshPro text;

    public int id = 0;
    bool isOn = false;
    float maxAngle = 45f;
    float minAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        turnOff();

        if (gameObject.GetComponent<XRKnob>() != null)
        {
            maxAngle = gameObject.GetComponent<XRKnob>().maxAngle;
            minAngle = gameObject.GetComponent<XRKnob>().minAngle;
        }
        else
            Debug.LogError("[ButtonController] gameobject has no XRKnob attached");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onValueChanged(float val)
    {
        if (val >= 0.9 || val <= 0.1)
            turnOn();
        else
            turnOff();

        //log event
    }

    public void turnOn()
    {
        isOn = true;

        GameManager.Instance.knobIsTurnedOn(id-1);

        if(text != null)
            text.text = "On";

        GetComponentInChildren<Renderer>().material.color = Color.red;

        //log event
        GameManager.Instance.eventManager.pipeStatusChanged(id, transform, true);
    }

    public void turnOff()
    {
        isOn = false;
        GameManager.Instance.knobIsTurnedOff(id-1);

        if (text != null)
            text.text = "Off";

        GetComponentInChildren<Renderer>().material.color = Color.black;

        //log evenet
        if (GameManager.Instance.eventManager != null)
            GameManager.Instance.eventManager.pipeStatusChanged(id, transform, false);
    }

    public void toogleStatus()
    {
        if (text.text == "On")
            turnOff();
        else
            turnOn();

    }




    public void onSelectExited() //when button is not touched - move back to inital position
    {
        //don't snap back when knob is turned to the left
        if (gameObject.GetComponent<XRKnob>().value > 0.1){
            turnOff();
            gameObject.GetComponent<XRKnob>().value = 0.5f;
        }


    }

}
