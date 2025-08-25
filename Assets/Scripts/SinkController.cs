using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkController : MonoBehaviour
{
    public float emptySpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Vessel")
        {
            other.gameObject.GetComponent<VesselController>().emptyVessel(emptySpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //get fill level and juice type
        if (other.tag == "Vessel")
        {
            float level = other.GetComponent<VesselController>().getFillLevel();
            string mat = other.GetComponent<VesselController>().getFluidMaterial().color.ToString();

            //log event
            GameManager.Instance.eventManager.emptyVessel(other.transform, level, mat);

        }

    }
}
