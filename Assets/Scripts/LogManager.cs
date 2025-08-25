using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum MsgType
{
    Player,
    Event,
    EyeTracking
}

public enum EventType
{
    Grab, 
    Release, 
    PipeIntensityChanged, 
    PipeOn, 
    PipeOff, 
    FillVessel, 
    EmptyVessel,
    Assemble,
    Disassemble //TODO? reset scene, empty target vessel,
}

public class LogManager : MonoBehaviour
{

    List<string> fileNames;// = new List<string>(new string[] { "Player", "Events", "EyeTracking" });
    List<string> filePaths;// = new List<string>();
    
    string pathToFolder;

    // Start is called before the first frame update
    void Start()
    {
        fileNames = new List<string>(new string[] { "Player", "Events", "EyeTracking" });
        filePaths = new List<string>();

        createFolder();
        createFiles();
        createFileHeadings();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void createFolder()
    {
        string currentTime = System.DateTime.Now.Hour.ToString() + "_" + System.DateTime.Now.Minute.ToString() + "_" + System.DateTime.Now.Second.ToString();
        // The target file path e.g.
        string today = System.DateTime.Now.Year.ToString() + "_" + System.DateTime.Now.Month.ToString() + "_" + System.DateTime.Now.Day.ToString();
        
        pathToFolder = Application.persistentDataPath + "/" + today + "/" + currentTime;


        if (!Directory.Exists(pathToFolder))
            Directory.CreateDirectory(pathToFolder);

    }


    void createFiles()
    {

        foreach (string file_name in fileNames)
        {
            string filePath = Path.Combine(pathToFolder, (file_name + ".csv"));
            filePaths.Add(filePath);
        }

    }

    void addHeadingToFile(MsgType type, string heading)
    {
        string path = getFilePath(type);
        //Player
        //time, player/camera position, player/camera rotation
        using (StreamWriter w = new StreamWriter(path, false))
        {
            w.WriteLine(heading);
            w.Close();
        }

        //Player, Events, EyeTracking
    }

    void createFileHeadings()
    {
        //TODO split vector3s!!
        //addHeadingToFile(MsgType.Player, "Timestamp; HmdPos; HmdRot; ControllerLPos; ControllerLRot; ControllerRPos; ControllerRRot");
        addHeadingToFile(MsgType.Player, "Timestamp;" + vector3Heading("HmdPos") +";" + vector3Heading("HmdRot") + ";" + vector3Heading("CLPos") + ";" + vector3Heading("CLRot") + ";" + vector3Heading("CRPos") + ";" + vector3Heading("CRRot") + ";");
        //addHeadingToFile(MsgType.Event, "Timestamp; EventType; Interactable; Ivalue; IPos, IRot, CLpos, CLrot, CRpos, CRrot, HmdPos, HmdRot, JuiceType"); //I value --> interactable value (for pipe intensity change); JuiceType for vessel fill
        addHeadingToFile(MsgType.Event, "Timestamp; EventType; Interactable; Ivalue;" + vector3Heading("iPos") + ";" + vector3Heading("iRot") + ";" + vector3Heading("CLPos") + ";" + vector3Heading("CLRot") + ";" + vector3Heading("CRPos") + ";" + vector3Heading("CRRot") + ";"
            + vector3Heading("HmdPos") + ";" +vector3Heading("HmdRot") + ";" + "JuiceType");
        addHeadingToFile(MsgType.EyeTracking, "Timestamp"); //TODO
        //TODO Sensors?

        Debug.Log("Headings created");
    }

    string vector3Heading(string title)
    {
        return title + "_x;" + title + "_y;" + title + "_z";
    }

    string getFilePath(MsgType type)
    {
        string path = "";
        switch (type)
        {
            case MsgType.Player:
                path = filePaths[0];
                break;
            case MsgType.Event:
                path = filePaths[1];
                break;
            case MsgType.EyeTracking:
                path = filePaths[2];
                break;
        }

        return path;
    }

    public void logEvent(MsgType msgtype, string msg)
    {
        string path = getFilePath(msgtype);

        using (StreamWriter w = new StreamWriter(path, true))
        {
            w.WriteLine(msg);
            w.Close();
        }
    }



}


