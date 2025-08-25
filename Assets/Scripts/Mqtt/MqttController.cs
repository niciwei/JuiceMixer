using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MqttController : MonoBehaviour
{
    public string nameController = "Controller 1";
    public MqttReceiver _eventSender;
    public ReadDataScript dataReader;

    void Start()
    {
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {
        dataReader.ReceiveMsg(newMsg);
        //Debug.Log("Event Fired. The message, from Object " + nameController + " is = " + newMsg);
    }
}
