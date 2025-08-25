using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public enum JuiceType
{
    Empty,
    Yellow,
    Orange,
    Red,
    Green,
    Blue,
    Purple
}


public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public GameObject knobs;
    public GameObject volumeKnobs;
    public GameObject tubes;
    public GameObject lids;

    [HideInInspector]
    public bool mqttConnected = false;

    public GameObject targetVessel;
    public float pumpSpeed = 0.1f;

    [HideInInspector]
    public LogManager logManager;
    [HideInInspector]
    public EventManager eventManager;

    KnobController[] knobControllers;
    VolumeKnobController[] volumeKnobControllers;
    TubeController[] tubeControllers;
    LidController[] lidControllers;

    List<int> activePumps;
    List<Material> activeJuiceMaterial;
    List<float> activePumpStrength;
    List<LidController> activeLids;
    List<VesselController> activeVessels;





    private void Awake()
    {
        _instance = this;

        activePumps = new List<int>();
        activeJuiceMaterial = new List<Material>();
        activePumpStrength = new List<float>();
        activeLids = new List<LidController>();
        activeVessels = new List<VesselController>();
    }


    // Start is called before the first frame update
    void Start()
    {

        knobControllers = knobs.GetComponentsInChildren<KnobController>();
        volumeKnobControllers = volumeKnobs.GetComponentsInChildren<VolumeKnobController>();
        tubeControllers = tubes.GetComponentsInChildren<TubeController>();
        lidControllers = lids.GetComponentsInChildren<LidController>();

        logManager = GetComponent<LogManager>();
        eventManager = GetComponent<EventManager>();

        Debug.Log("knobs, volume knobs, tubes, lids: " + knobControllers.Length + ", " + volumeKnobControllers.Length + ", " + tubeControllers.Length + ", " + lidControllers.Length);
    }

    // Update is called once per frame
    void Update()
    {
        activeJuiceMaterial.Clear();
        activePumpStrength.Clear();
        activeLids.Clear();
        activeVessels.Clear();

        foreach (int id in activePumps)
        {
            Transform activeLid = tubeControllers[id].getConnectedLid();
            Transform activeVessel = null;

            if (activeLid != null)
                activeVessel = activeLid.GetComponent<LidController>().getConnectedVessel();

            //only pump if everything is connected and juice is filled
            if (activeLid != null && activeVessel != null && !isTargetFull())
            {
                Material activeMat = activeVessel.GetComponent<VesselController>().getFluidMaterial();
                float pumpStrenght = volumeKnobControllers[id].value;

                if (activeVessel.GetComponent<VesselController>().isFilled() && pumpStrenght > 0f)
                {
                    tubeControllers[id].turnOnTube(activeMat);
                    Debug.Log("active: " + id + " " + tubeControllers[id].name + " " + activeLid.name + " " +
                        activeVessel + " " + activeMat.color.ToString() + " " + pumpStrenght);

                    activeJuiceMaterial.Add(activeMat);
                    activePumpStrength.Add(pumpStrenght * Time.deltaTime * pumpSpeed);
                    activeLids.Add(activeLid.GetComponent<LidController>());
                    activeVessels.Add(activeVessel.GetComponent<VesselController>());

                }
            }
        }


        if (activeJuiceMaterial.Count > 0 && activePumpStrength.Count > 0)
        {

            fillTargetVesselWith(activePumpStrength, activeJuiceMaterial, activeLids, activeVessels);
        }


    }

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogError("Game Manager is NULL");

            return _instance;
        }
    }

    public void knobIsTurnedOn(int id) //ohlala
    {
        activePumps.Remove(id);
        activePumps.Add(id);
    }

    public void knobIsTurnedOff(int id)
    {
        activePumps.Remove(id);

        if (tubeControllers != null && id >= 0)
            tubeControllers[id].turnOffTube();
    }

    public void connectLidAndTube(Transform lid, int tubeId)
    {
        tubeControllers[tubeId].onLidConnected(lid);
        Debug.Log("tube and lid " + tubeControllers[tubeId].name + " " + lid.name);
    }

    public void disconnectLidAndTube(int tubeId)
    {
        tubeControllers[tubeId].onLidDisconnected();
    }



    /*
    public void fillTargetVessel(float fillSpeed, Material juiceMat, Dictionary<JuiceType, float> juiceTypeAndAmount)
    {
        //TODO change

        Transform fluid = targetVessel.transform.Find("Fluid");
        //JuiceType juiceType = fluid.GetComponent<FluidController>().currentComposition.Aggregate((l, r) => l.Value > r.Value ? l : r).Key; //TODO just allow one juiceType in container
      
        fluid.GetComponent<FluidController>().increaseFluid(fillSpeed, juiceMat, juiceTypeAndAmount);
    }
    //fill with mat, pump, lids, vessel
    */

    public void fillTargetVesselWith(List<float> strength, List<Material> mats, List<LidController> lids, List<VesselController> vessels)
    {
     
        List<JuiceType> juiceTypes = new List<JuiceType>();

        foreach(Material m in mats)
        {
            juiceTypes.Add(Helper.getJuiceTypeFromMaterial(m));
        }

        Transform fluid = targetVessel.transform.Find("Fluid");

        fluid.GetComponent<FluidController>().increaseTargetFluid(strength, mats, juiceTypes, lids, vessels);

    }

    bool isTargetFull()
    {
        Transform fluid = targetVessel.transform.Find("Fluid");

        return fluid.GetComponent<FluidController>().isFull();

    }

    public void emptyVessel()
    {
        Debug.Log("button clicked!!! empty vessel!");

        FluidController targetFluidController = targetVessel.transform.GetComponentInChildren<FluidController>();
        targetFluidController.completelyEmptyFluid();

    }

    public void restartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    public void connectedToBase(SelectEnterEventArgs args)
    {
        Transform passive = args.interactableObject.transform;
        Transform active = args.interactorObject.transform;
       
        //log event
        GameManager.Instance.eventManager.assembleObjects(active.parent.parent, passive);
        Debug.Log("something attached to base " + active.parent.parent.name + " " + passive.name);

    }

    public void disconnectedFromBase(SelectExitEventArgs args)
    {
        Transform passive = args.interactableObject.transform;
        Transform active = args.interactorObject.transform;

        //log event
        GameManager.Instance.eventManager.disassembleObjects(active.parent.parent, passive);
        Debug.Log("something removed from base " + active.parent.parent.name + " " + passive.name);

    }

    public float getPumpStrengthWithTubeId(int id)
    {
        return volumeKnobControllers[id].value;
    }

    public void updatePumpStrenghtVisualiation()
    {
        if(lidControllers != null)
        {
            foreach (LidController lid in lidControllers)
            {
                lid.setPumpStrengthText();
            }
        }

    }
}
