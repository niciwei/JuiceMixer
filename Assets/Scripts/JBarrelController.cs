using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JBarrelController : MonoBehaviour
{
    public JuiceType juiceType = JuiceType.Orange;
    public float fillSpeed = 0.1f;

    Material juiceMat;

    // Start is called before the first frame update
    void Start()
    {
       Renderer renderer = gameObject.transform.parent.Find("Juice").gameObject.GetComponent<Renderer>();
       juiceMat = renderer.material;
        gameObject.GetComponent<Renderer>().material = juiceMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Vessel")
        {
            other.gameObject.GetComponent<VesselController>().fillVessel(fillSpeed, juiceMat, juiceType);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //get fill level and juice type
        if (other.tag == "Vessel")
        {
            float level = other.GetComponent<VesselController>().getFillLevel();
            string mat = other.GetComponent<VesselController>().getFluidMaterial().color.ToString(); ;

            //log event
            GameManager.Instance.eventManager.fillVessel(other.transform, level, mat);

        }
    }


}
