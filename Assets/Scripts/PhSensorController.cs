using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhSensorController : MonoBehaviour
{

    [HideInInspector]
    public float currentValue;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.Instance.mqttConnected)
            currentValue = Helper.getRandomNumber(0.0f, 7.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.mqttConnected)
        {
            currentValue = currentValue + Helper.getRandomNumber(-0.001f, 0.001f);
        }
    }
}
