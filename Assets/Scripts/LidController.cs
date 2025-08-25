using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class LidController : MonoBehaviour
{
    // Start is called before the first frame update
    Transform connectedVessel;
    Transform connectedTube;
    Transform connectedPhSensor;
    Transform connectedTempSensor;

    int connectedTubeId = -1;
    float phValue = 0;
    float tempValue = 0;

    public TMPro.TextMeshPro textTube;

    Transform pipeStrengthVisualization;



    void Start()
    {
        pipeStrengthVisualization = transform.Find("Arrow");
    }

    // Update is called once per frame
    void Update()
    {
        if(connectedVessel != null)
        {
            if (connectedPhSensor != null)
            {
                phValue = connectedPhSensor.GetComponentInChildren<PhSensorController>().currentValue;
                connectedVessel.GetComponent<VesselController>().setPhText(phValue);
            }

            if (connectedTempSensor != null)
            {
                tempValue = connectedTempSensor.GetComponentInChildren<TempSensorController>().currentValue;
                connectedVessel.GetComponent<VesselController>().setTempText(tempValue);
            }
        }
    }

    public Transform getConnectedVessel()
    {
        return connectedVessel;
    }

    public void onVesselConnected(GameObject vessel) //called from vessel
    {
        Debug.Log("Vessel connected - " + vessel.name);
        connectedVessel = vessel.transform;
        Debug.Log("juice in connected vessel: " + connectedVessel.GetComponent<VesselController>().getFluidMaterial().color.ToString());

        //log event
        GameManager.Instance.eventManager.assembleObjects(vessel.transform, transform);
        Debug.Log("lid attached to vessel " + vessel.name + " " + transform.name);
    }

    public void onTubeConnected(SelectEnterEventArgs args)
    {
        connectedTube = args.interactableObject.transform;
        connectedTubeId = getTubeId(connectedTube);

        GameManager.Instance.connectLidAndTube(transform, connectedTubeId-1);
        
        //makePipeArrow visible and show strength
        //textTube.text = (connectedTubeId).ToString();

        togglePumpStrengthArrowVisualization(true);

        if(connectedVessel != null)
            connectedVessel.GetComponent<VesselController>().setPumpIdText(connectedTubeId);

        Debug.Log("Tube connected - " + connectedTube.name);

        //TODO set lid gamemanager  //???

        //log event
        GameManager.Instance.eventManager.assembleObjects(connectedTube, transform);
        Debug.Log("tube attached to lid " + connectedTube.name + " " + transform.name);

    }

    public void setPumpStrengthText()
    {
        if(connectedTube != null)
        {
            float strenght = GameManager.Instance.getPumpStrengthWithTubeId(connectedTubeId - 1);
            pipeStrengthVisualization.GetComponentInChildren<TextMeshPro>().text = strenght.ToString("F2");

            pipeStrengthVisualization.localScale = new Vector3(strenght, strenght, strenght);
        }

    }

    void togglePumpStrengthArrowVisualization(bool activate)
    {
        /*
        pipeStrengthVisualization.gameObject.SetActive(activate);

        if (activate)
            setPumpStrengthText();

        */
    }

    public void onPhSensorConnected(SelectEnterEventArgs args)
    {
        connectedPhSensor = args.interactableObject.transform;
        Debug.Log("PhSensor connected - " + connectedPhSensor.name);

        //log event
        GameManager.Instance.eventManager.assembleObjects(connectedPhSensor, transform);
        Debug.Log("ph sensor attached to lid " + connectedPhSensor.name + " " + transform.name);
    }

    public void onTempSensorConnected(SelectEnterEventArgs args)
    {
        connectedTempSensor = args.interactableObject.transform;
        Debug.Log("TempSensor connected - " + connectedTempSensor.name);

        //log event
        GameManager.Instance.eventManager.assembleObjects(connectedTempSensor, transform);
        Debug.Log("temp sensor attached to lid " + connectedTempSensor.name + " " + transform.name);
    }



    //-------
    public void onVesselDisconnected(GameObject vessel) //called from vessel
    {
        Debug.Log("Vessel disconnected - " + vessel.name);
        connectedVessel = null;


        //log event
        GameManager.Instance.eventManager.disassembleObjects(vessel.transform, transform);
        Debug.Log("vessel removed from lid " + vessel.name + " " + transform.name);
    }

    public void onTubeDisconnected( )
    {
        Debug.Log("Tube disconnected - ");
        GameManager.Instance.disconnectLidAndTube(connectedTubeId-1);
        textTube.text = "0";

        togglePumpStrengthArrowVisualization(false);

        if (connectedVessel != null)
            connectedVessel.GetComponent<VesselController>().setPumpIdText(connectedTubeId);

        //log event
        GameManager.Instance.eventManager.disassembleObjects(connectedTube.transform, transform);
        Debug.Log("tube removed from lid " + connectedTube.name + " " + transform.name);

        //TODO setlid gamemanager
        connectedTube = null;
        connectedTubeId = -1;
    }

    public void onPhSensorDisconnected( )
    {
        //log event
        GameManager.Instance.eventManager.disassembleObjects(connectedPhSensor.transform, transform);
        Debug.Log("ph sensor removed from lid " + connectedPhSensor.name + " " + transform.name);


        Debug.Log("PhSensor disconnected - ");
        connectedPhSensor=null;
        phValue=0;
        connectedVessel.GetComponent<VesselController>().setPhText(999);

    }

    public void onTempSensorDisconnected( )
    {
        GameManager.Instance.eventManager.disassembleObjects(connectedTempSensor.transform, transform);
        Debug.Log("temp sensor removed from lid " + connectedTempSensor.name + " " + transform.name);


        Debug.Log("TempSensor disconnected - ");
        connectedTempSensor=null;
        tempValue=0;
        connectedVessel.GetComponent<VesselController>().setTempText(999);

        //log event
    }

    int getTubeId(Transform tubeBone)
    {
        int id = -1;
        string tag = tubeBone.gameObject.tag;

        if (tag.Contains("Tube"))
        {
            string tubeId = tag.Substring(tag.Length - 1);
            Debug.Log("last char in string: " + tubeId);
            id = int.Parse(tubeId);
            Debug.Log("last char in string: " + tubeId + " " + id);
        }
        else
        {
            Debug.Log("Connected Tube has no tag");
        }

        return id;
    }

   




}
