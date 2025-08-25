using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


[System.Serializable]
public class Sensor
{
    [field: SerializeField]  public int id { get; set; } 
    [field: SerializeField]  public double value { get; set; }
    public DateTime time { get; set; }

    public enum Stype 
    {
        Temperature,
        PH,
        Depth,
        Level
    }

    public static Dictionary<Stype, string> type = new Dictionary<Stype, string>{
        { Stype.Temperature, "temp" },
        { Stype.PH, "pH" },
        { Stype.Depth, "depth" },
        { Stype.Level, "level" },
    };

}

[System.Serializable]
public class Pump
{
    [field: SerializeField] public int id { get; set; }
    [field: SerializeField]  public int active { get; set; }
    [field: SerializeField]  public int inverted { get; set; }
    [field: SerializeField]  public double speed { get; set; }
    public DateTime time { get; set; }

    public enum Ptype 
    {
        Active,
        Inverted,
        Speed
    }

    public static Dictionary<Ptype, string> type = new Dictionary<Ptype, string>{
        { Ptype.Active, "active" },
        { Ptype.Inverted, "inverted" },
        { Ptype.Speed, "speed" },
    };

    public float getSpeed()
    {
        if (active == 1) {
            if (inverted == 1)
                return (float)speed * -1;
            return (float)speed;
        }
        return 0;
    }
}

[System.Serializable]
public class SensorList
{
    public List<Sensor> sensors = new List<Sensor>();
}

[System.Serializable]
public class ContainerPumpList
{
    public List<Pump> container = new List<Pump>();
}

[System.Serializable]
public class ContainerSensorList
{
    public List<SensorList> container = new List<SensorList>();
}
