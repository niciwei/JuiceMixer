using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MqttToVisHandler : MonoBehaviour
{
    public ReadDataScript data;

    public int pumpId = 1;

    public FluidController fluidController;
    public TubeController tubeController;
    public GameObject arrow;

    VesselVisualizationController[] visControllers;

    float temp;
    float ph;
    int active;
    float fillLevel;

    // Start is called before the first frame update
    void Start()
    {
        visControllers = GetComponentsInChildren<VesselVisualizationController>();
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        temp = (float) data.temp_values.container[pumpId-1].sensors[0].value;
        ph = (float) data.ph_values.container[pumpId-1].sensors[0].value;
        active = data.pump_values.container[pumpId-1].active;
        fillLevel = (float) data.depth_values.container[pumpId-1].sensors[0].value;

        setData();
    }

    void setData()
    {
        foreach(var controller in visControllers)
        {
            controller.setPumpIdText(pumpId);
            controller.setPhText(ph);
            controller.setTempText(temp);
        }

        if (active == 1)
        {
            arrow.SetActive(true);
        }
        else if(active == 0)
        {
            arrow.SetActive(false);
        }
        fluidController.setScale(fillLevel);

        /*
         *  case 'T':
                temp_values.container[id].sensors[0].value = value;
                break;
            case 'L':
                depth_values.container[id].sensors[0].value = value;
                break;
            case 'P':
                ph_values.container[id].sensors[0].value = value;
                break;
            case 'S':
                pump_values.container[id].speed = value;
                break;
            case 'A':
                pump_values.container[id].active = (int)value;
                break;
            case 'I':
                pump_values.container[id].inverted = (int)value;
                break;
        */
    }
}
