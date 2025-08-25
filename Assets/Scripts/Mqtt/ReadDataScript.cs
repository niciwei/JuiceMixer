using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class ReadDataScript : MonoBehaviour
{
    [Tooltip("Set the max amount of containers you have in total.")]
    public int max_containers;
    [Space]
    [Header("Sensors")]
    public ContainerSensorList temp_values = new ContainerSensorList();
    public ContainerSensorList depth_values = new ContainerSensorList();
    public ContainerSensorList ph_values = new ContainerSensorList();
    public ContainerPumpList pump_values = new ContainerPumpList();


    public void ReceiveMsg(string msg)
    {
        if (msg.Length < 2)
            return;

        if (msg[1] > '0' && msg[1] <= ('0' + max_containers))
        {
            int id = int.Parse(msg[1].ToString()) - 1;

            int pFrom = msg.IndexOf("[") + 1;
            int pTo = msg.LastIndexOf("]");

            String result = msg.Substring(pFrom, pTo - pFrom);
            //Debug.Log(msg);

            char type = msg[0];
            //result = result.Replace(".", ",");
            HandleMessage(id, type, double.Parse(result, System.Globalization.CultureInfo.InvariantCulture));
        }

    }

    double convertStringToDouble(string result)
    {
        double res = double.Parse(result);
        if (res < 0)
            return 0;
        else if (res > 1)
            return 1;
        else
            return res;
    }

    void HandleMessage(int id, char type, double value)
    {
        switch (type)
        {
            case 'T':
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

            default:
                //Debug.Log("Type '" + type + "' not implemented!");
                break;
        }
    }
}
