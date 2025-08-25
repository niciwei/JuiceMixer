using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsUser : MonoBehaviour
{
    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (Camera.main.transform.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        Vector3 r = lookRot.eulerAngles;

        float newX = rotateX ? r.x : transform.rotation.eulerAngles.x;
        float newY = rotateY ? r.y : transform.rotation.eulerAngles.y;
        float newZ = rotateZ ? r.z : transform.rotation.eulerAngles.z;

        //transform.rotation = Quaternion.Euler(0f, r.y, 0f);
        transform.rotation = Quaternion.Euler(newX, newY, newZ);



    }
}
