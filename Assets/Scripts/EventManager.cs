using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class EventManager : MonoBehaviour
{
    GameManager gameManager;
    LogManager logManager;
    GameObject player;
    GameObject controllerLeft;
    GameObject controllerRight;

    float tempTime = 0;
    float logInterval = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        logManager = gameManager.logManager;

        player = GameObject.FindWithTag("Player");

        if (player == null)
            Debug.Log("Player not found!!!");

        //TODO check for hand or controller tracking
        //controllerRight = getController(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller);
        //controllerLeft = getController(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller);


        //TODO nullreferenceexception
        Transform cameraParent = Camera.main.transform.parent;
        controllerLeft = cameraParent.Find("Left Controller Stabilized").gameObject;
        controllerRight = cameraParent.Find("Right Controller Stabilized").gameObject;

        if (controllerLeft == null)
            Debug.Log("controller left is null!!!");

        if (controllerRight == null)
            Debug.Log("controller right is null!!!");

    }

    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > logInterval)
        {
            tempTime = 0;
            logPlayerData();
        }



    }

    void logPlayerData()
    {
        //LogPlayer data
        float timestamp = Time.realtimeSinceStartup;
        Vector3 clPos = controllerLeft.transform.position;
        Vector3 clRot = controllerLeft.transform.rotation.eulerAngles;
        Vector3 crPos = controllerRight.transform.position;
        Vector3 crRot = controllerRight.transform.rotation.eulerAngles;
        Vector3 hmdPos = Camera.main.transform.position;
        Vector3 hmdRot = Camera.main.transform.rotation.eulerAngles;

        //                                    "Timestamp;" + vector3Heading("HmdPos") +";" + vector3Heading("HmdRot") + ";" + vector3Heading("CLPos") + ";" + vector3Heading("CLRot") + ";" + vector3Heading("CRPos") + ";" + vector3Heading("CRRot") + ";");
        string[] msg = new string[] { timestamp.ToString(), vectorToString(hmdPos), vectorToString(hmdRot), vectorToString(clPos), vectorToString(clRot), vectorToString(crPos), vectorToString(crRot) };

        logManager.logEvent(MsgType.Player, arrayToCsvFormat(msg));
    }

    InputDevice getController(InputDeviceCharacteristics characteristics)
    {
        InputDevice controller = new InputDevice();

        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

        //debug
        Debug.Log("found devices: ");
        foreach (InputDevice d in devices)
            Debug.Log(d.name + " " + d.characteristics);

        if(devices.Count > 0)
            controller = devices[0];

        

        return controller;
    }

    public void objectGrabbedEvent(SelectEnterEventArgs args)
    {
        float timestamp = Time.realtimeSinceStartup;
        EventType eventType = EventType.Grab;
        string interactable = args.interactableObject.transform.name;
        string iValue = ""; //just for intensity knob
        Vector3 iPos = args.interactableObject.transform.position;
        Vector3 iRot = args.interactableObject.transform.rotation.eulerAngles;
        Vector3 clPos = controllerLeft.transform.position;
        Vector3 clRot = controllerLeft.transform.rotation.eulerAngles;
        Vector3 crPos = controllerRight.transform.position;
        Vector3 crRot = controllerRight.transform.rotation.eulerAngles;
        Vector3 hmdPos = Camera.main.transform.position;
        Vector3 hmdRot = Camera.main.transform.rotation.eulerAngles;
        string juiceType = ""; // not needed for this event

        Debug.Log("Grabbed!!!! " + vectorToString(iRot));

        //Timestamp;                                         EventType;            Interactable; Ivalue; IPos,                        IRot,          CLpos,                   CLrot,             CRpos,                    CRrot,
        string[] msg = new string[] { timestamp.ToString(), eventType.ToString(), interactable, iValue, vectorToString(iPos), vectorToString(iRot), vectorToString(clPos), vectorToString(clRot), vectorToString(crPos), vectorToString(crRot),
            vectorToString(hmdPos),vectorToString(hmdRot), juiceType}; //TODO
        //              HmdPos,           HmdRot,        JuiceType

        logManager.logEvent(MsgType.Event, arrayToCsvFormat(msg));
    }

    public void objectReleasedEvent(SelectExitEventArgs args)
    {
        float timestamp = Time.realtimeSinceStartup;
        EventType eventType = EventType.Release;
        string interactable = args.interactableObject.transform.name;
        string iValue = ""; //just for intensity knob
        Vector3 iPos = args.interactableObject.transform.position;
        Vector3 iRot = args.interactableObject.transform.rotation.eulerAngles;
        Vector3 clPos = controllerLeft.transform.position;
        Vector3 clRot = controllerLeft.transform.rotation.eulerAngles;
        Vector3 crPos = controllerRight.transform.position;
        Vector3 crRot = controllerRight.transform.rotation.eulerAngles;
        Vector3 hmdPos = Camera.main.transform.position;
        Vector3 hmdRot = Camera.main.transform.rotation.eulerAngles;
        string juiceType = ""; // not needed for this event

        Debug.Log("Released!!!! " + vectorToString(iRot));

        //Timestamp;                                         EventType;            Interactable; Ivalue; IPos,                        IRot,          CLpos,                   CLrot,             CRpos,                    CRrot,
        string[] msg = new string[] { timestamp.ToString(), eventType.ToString(), interactable, iValue, vectorToString(iPos), vectorToString(iRot), vectorToString(clPos), vectorToString(clRot), vectorToString(crPos), vectorToString(crRot), 
            vectorToString(hmdPos),vectorToString(hmdRot), juiceType};
        //              HmdPos,           HmdRot,        JuiceType

        logManager.logEvent(MsgType.Event, arrayToCsvFormat(msg));
    }

    public void pipeIntensityChanged(Transform knob, float value)
    {
        float timestamp = Time.realtimeSinceStartup;
        EventType eventType = EventType.PipeIntensityChanged;
        string interactable = knob.name; //TODO id??????
        string iValue = value.ToString("F2"); //just for intensity knob
        Vector3 iPos = knob.position;
        Vector3 iRot = knob.rotation.eulerAngles;
        Vector3 clPos = controllerLeft.transform.position;
        Vector3 clRot = controllerLeft.transform.rotation.eulerAngles;
        Vector3 crPos = controllerRight.transform.position;
        Vector3 crRot = controllerRight.transform.rotation.eulerAngles;
        Vector3 hmdPos = Camera.main.transform.position;
        Vector3 hmdRot = Camera.main.transform.rotation.eulerAngles;
        string juiceType = ""; // not needed for this event

        //Timestamp;                                         EventType;            Interactable; Ivalue; IPos,                        IRot,          CLpos,                   CLrot,             CRpos,                    CRrot,
        string[] msg = new string[] { timestamp.ToString(), eventType.ToString(), interactable, iValue, vectorToString(iPos), vectorToString(iRot), vectorToString(clPos), vectorToString(clRot), vectorToString(crPos), vectorToString(crRot),
            vectorToString(hmdPos),vectorToString(hmdRot), juiceType};
        //              HmdPos,           HmdRot,        JuiceType

        logManager.logEvent(MsgType.Event, arrayToCsvFormat(msg));
    }

    public void pipeStatusChanged(int id, Transform knob, bool on)
    {
        float timestamp = Time.realtimeSinceStartup;
        EventType eventType = EventType.PipeIntensityChanged;
        string interactable = knob.name + id; //TODO check id!!!!
        string iValue = on? "on" : "off";
        Vector3 iPos = knob.position;
        Vector3 iRot = knob.rotation.eulerAngles;
        Vector3 clPos = controllerLeft.transform.position;
        Vector3 clRot = controllerLeft.transform.rotation.eulerAngles;
        Vector3 crPos = controllerRight.transform.position;
        Vector3 crRot = controllerRight.transform.rotation.eulerAngles;
        Vector3 hmdPos = Camera.main.transform.position;
        Vector3 hmdRot = Camera.main.transform.rotation.eulerAngles;
        string juiceType = ""; // not needed for this event

        //Timestamp;                                         EventType;            Interactable; Ivalue; IPos,                        IRot,          CLpos,                   CLrot,             CRpos,                    CRrot,
        string[] msg = new string[] { timestamp.ToString(), eventType.ToString(), interactable, iValue, vectorToString(iPos), vectorToString(iRot), vectorToString(clPos), vectorToString(clRot), vectorToString(crPos), vectorToString(crRot),
            vectorToString(hmdPos),vectorToString(hmdRot), juiceType};
        //              HmdPos,           HmdRot,        JuiceType

        logManager.logEvent(MsgType.Event, arrayToCsvFormat(msg));
    }

    public void emptyVessel(Transform vessel, float fillLevel, string juiceMat)
    {
        float timestamp = Time.realtimeSinceStartup;
        EventType eventType = EventType.EmptyVessel;
        string interactable = vessel.name;
        string iValue = fillLevel.ToString();
        Vector3 iPos = vessel.position;
        Vector3 iRot = vessel.rotation.eulerAngles;
        Vector3 clPos = controllerLeft.transform.position;
        Vector3 clRot = controllerLeft.transform.rotation.eulerAngles;
        Vector3 crPos = controllerRight.transform.position;
        Vector3 crRot = controllerRight.transform.rotation.eulerAngles;
        Vector3 hmdPos = Camera.main.transform.position;
        Vector3 hmdRot = Camera.main.transform.rotation.eulerAngles;
        string juiceType = juiceMat;

        Debug.Log("empty vessel: " + vessel.name + " " + iValue + " " + juiceType);

        //Timestamp;                                         EventType;            Interactable; Ivalue; IPos,                        IRot,          CLpos,                   CLrot,             CRpos,                    CRrot,
        string[] msg = new string[] { timestamp.ToString(), eventType.ToString(), interactable, iValue, vectorToString(iPos), vectorToString(iRot), vectorToString(clPos), vectorToString(clRot), vectorToString(crPos), vectorToString(crRot),
            vectorToString(hmdPos),vectorToString(hmdRot), juiceType};
        //              HmdPos,           HmdRot,        JuiceType

        logManager.logEvent(MsgType.Event, arrayToCsvFormat(msg));
    }

    public void fillVessel(Transform vessel, float fillLevel, string juiceMat)
    {
        float timestamp = Time.realtimeSinceStartup;
        EventType eventType = EventType.FillVessel;
        string interactable = vessel.name;
        string iValue = fillLevel.ToString();
        Vector3 iPos = vessel.position;
        Vector3 iRot = vessel.rotation.eulerAngles;
        Vector3 clPos = controllerLeft.transform.position;
        Vector3 clRot = controllerLeft.transform.rotation.eulerAngles;
        Vector3 crPos = controllerRight.transform.position;
        Vector3 crRot = controllerRight.transform.rotation.eulerAngles;
        Vector3 hmdPos = Camera.main.transform.position;
        Vector3 hmdRot = Camera.main.transform.rotation.eulerAngles;
        string juiceType = juiceMat;

        Debug.Log("fill vessel: " + vessel.name + " " + iValue + " " + juiceType);

        //Timestamp;                                         EventType;            Interactable; Ivalue; IPos,                        IRot,          CLpos,                   CLrot,             CRpos,                    CRrot,
        string[] msg = new string[] { timestamp.ToString(), eventType.ToString(), interactable, iValue, vectorToString(iPos), vectorToString(iRot), vectorToString(clPos), vectorToString(clRot), vectorToString(crPos), vectorToString(crRot),
            vectorToString(hmdPos),vectorToString(hmdRot), juiceType};
        //              HmdPos,           HmdRot,        JuiceType

        logManager.logEvent(MsgType.Event, arrayToCsvFormat(msg));
    }

    public void assembleObjects(Transform active, Transform passive)
    {
        float timestamp = Time.realtimeSinceStartup;
        EventType eventType = EventType.Assemble;
        string interactable = active.gameObject.name;
        string iValue = passive.gameObject.name;
        Vector3 iPos = active.transform.position;
        Vector3 iRot = active.transform.rotation.eulerAngles;
        Vector3 clPos = controllerLeft.transform.position;
        Vector3 clRot = controllerLeft.transform.rotation.eulerAngles;
        Vector3 crPos = controllerRight.transform.position;
        Vector3 crRot = controllerRight.transform.rotation.eulerAngles;
        Vector3 hmdPos = Camera.main.transform.position;
        Vector3 hmdRot = Camera.main.transform.rotation.eulerAngles;
        string juiceType = ""; // not needed for this event

        Debug.Log("Released!!!! " + vectorToString(iRot));

        //Timestamp;                                         EventType;            Interactable; Ivalue; IPos,                        IRot,          CLpos,                   CLrot,             CRpos,                    CRrot,
        string[] msg = new string[] { timestamp.ToString(), eventType.ToString(), interactable, iValue, vectorToString(iPos), vectorToString(iRot), vectorToString(clPos), vectorToString(clRot), vectorToString(crPos), vectorToString(crRot),
            vectorToString(hmdPos),vectorToString(hmdRot), juiceType};
        //              HmdPos,           HmdRot,        JuiceType

        logManager.logEvent(MsgType.Event, arrayToCsvFormat(msg));
    }

    public void disassembleObjects(Transform active, Transform passive)
    {
        float timestamp = Time.realtimeSinceStartup;
        EventType eventType = EventType.Disassemble;
        string interactable = active.gameObject.name;
        string iValue = passive.gameObject.name;
        Vector3 iPos = active.transform.position;
        Vector3 iRot = active.transform.rotation.eulerAngles;
        Vector3 clPos = controllerLeft.transform.position;
        Vector3 clRot = controllerLeft.transform.rotation.eulerAngles;
        Vector3 crPos = controllerRight.transform.position;
        Vector3 crRot = controllerRight.transform.rotation.eulerAngles;
        Vector3 hmdPos = Camera.main.transform.position;
        Vector3 hmdRot = Camera.main.transform.rotation.eulerAngles;
        string juiceType = ""; // not needed for this event

        Debug.Log("Released!!!! " + vectorToString(iRot));

        //Timestamp;                                         EventType;            Interactable; Ivalue; IPos,                        IRot,          CLpos,                   CLrot,             CRpos,                    CRrot,
        string[] msg = new string[] { timestamp.ToString(), eventType.ToString(), interactable, iValue, vectorToString(iPos), vectorToString(iRot), vectorToString(clPos), vectorToString(clRot), vectorToString(crPos), vectorToString(crRot),
            vectorToString(hmdPos),vectorToString(hmdRot), juiceType};
        //              HmdPos,           HmdRot,        JuiceType

        logManager.logEvent(MsgType.Event, arrayToCsvFormat(msg));
    }


    string vectorToString(Vector3 vector)
    {
        string v = vector.x.ToString("F2") + ";" + vector.y.ToString("F2") + ";" + vector.z.ToString("F2");

        return v;
    }

    string arrayToCsvFormat(string[] strings)
    {
        string s = "";

        foreach(string str in strings)
        {
            s += str;
            s += ";";
        }

        return s;
    }

    


}
