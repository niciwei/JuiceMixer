using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour
{
    Vector3 phPos;
    Vector3 tempPos;

    Quaternion phRot;
    Quaternion tempRot;

    string phName;
    string tempName;

    // Start is called before the first frame update
    void Start()
    {
        phPos = transform.GetChild(1).position;
        tempPos = transform.GetChild(2).GetChild(0).position;

        phRot = transform.GetChild(1).rotation;
        tempRot = transform.GetChild(2).GetChild(0).rotation;

        phName = transform.GetChild(1).name;
        tempName = transform.GetChild(2).GetChild(0).name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.transform.IsChildOf(transform)) //only for sensor children
        {
            //move sensor back to initial position
            if (other.name == phName)
            {
                Debug.Log("Restore ph!");
                other.transform.position = phPos;
                other.transform.rotation = phRot;
            }

            if (other.name == tempName)
            {
                Debug.Log("Restore temp!");
                other.transform.position = tempPos;
                other.transform.rotation = tempRot;
            }
        }




    }
}
