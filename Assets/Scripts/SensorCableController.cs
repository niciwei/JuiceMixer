using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCableController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform startPosition;
    public Transform midPosition;
    public Transform sensor;

    Transform sensorAttachTransform;

    float width = 0.002f;


    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        sensorAttachTransform = sensor.Find("AttachPointCable");

        if (sensorAttachTransform == null)
            sensorAttachTransform = sensor.GetChild(0).Find("AttachPointCable");

        if (sensorAttachTransform == null)
            Debug.Log("Upsi, can't find cable attach transform");

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 3;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        lineRenderer.SetPosition(0, startPosition.position);
        lineRenderer.SetPosition(1, midPosition.position);
        lineRenderer.SetPosition(2, sensorAttachTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        setLastPosition();

    }

    void setLastPosition()
    {
        lineRenderer.SetPosition(lineRenderer.positionCount-1, sensorAttachTransform.position);
    }
}


