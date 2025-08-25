using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeController : MonoBehaviour
{
    public Transform[] connectedTubes;

    [HideInInspector]
    Transform connectedLid;

    Transform tubeObject;
    Material standardMaterial;

    // Start is called before the first frame update
    void Start()
    {
        tubeObject = transform.Find("rope");
        standardMaterial = tubeObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onLidConnected(Transform lid)
    {
        Debug.Log("Tube: lid connected " + lid.name);
        connectedLid = lid;
    }

    public void onLidDisconnected()
    {
        connectedLid = null;
    }

    public Transform getConnectedLid()
    {
        return connectedLid;
    }

    public void turnOnTube(Material mat)
    {
        if (connectedLid != null && connectedLid.GetComponent<LidController>().getConnectedVessel() != null)
        {

            tubeObject.GetComponent<Renderer>().material = mat;

            foreach (Transform tube in connectedTubes)
            {
                tube.GetComponent<Renderer>().material = mat;
            }

            /*
            Material juiceMat = connectedLid.GetComponent<LidController>().getConnectedVessel().GetComponent<VesselController>().getFluidMaterial();
            Debug.Log("juiceMat in TubeController: " + juiceMat.color.ToString());
            tubeObject.GetComponent<Renderer>().material = juiceMat;

            foreach(Transform tube in connectedTubes)
            {
                tube.GetComponent<Renderer>().material = juiceMat;
            }
            */
        }

    }

    public void showTubeIsActive(Material mat)
    {

        tubeObject.GetComponent<Renderer>().material = mat;

        foreach (Transform tube in connectedTubes)
        {
            tube.GetComponent<Renderer>().material = mat;
        }
    }

    public void turnOffTube()
    {
        if(tubeObject != null)
        {
            tubeObject.GetComponent<Renderer>().material = standardMaterial;

            foreach (Transform tube in connectedTubes)
            {
                tube.GetComponent<Renderer>().material = standardMaterial;
            }
        }

    }

}
