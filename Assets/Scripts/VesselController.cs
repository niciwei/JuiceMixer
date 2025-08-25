using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VesselController : MonoBehaviour
{

    GameObject connectedLid;

    Transform fluid;
    FluidController fluidController;
    VisVolumeController visVolumeController;

    VesselVisualizationController[] visualizations;


    // Start is called before the first frame update
    void Start()
    {
        fluid = transform.Find("Fluid");
        fluidController = fluid.GetComponent<FluidController>();
        visVolumeController = GetComponentInChildren<VisVolumeController>();
        visualizations = GetComponentsInChildren<VesselVisualizationController>();
        Debug.Log("found viscontrollers: " + visualizations.Length);

       // if (visVolumeController != null)
        //    visVolumeController.setVolumeText(fluidController.getLiquidLevel());


        if (getFillLevel() > 0.5f)
            setJuiceVisMaterial(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getFillLevel()
    {
        return fluid.localScale.y;
    }


    public void onLidConnected(SelectEnterEventArgs args)
    {
        connectedLid = args.interactableObject.transform.gameObject;
        Debug.Log("Lid connected - " + connectedLid.name);

        if(connectedLid.GetComponent<LidController>() != null)
        {
            connectedLid.GetComponent<LidController>().onVesselConnected(gameObject);
        }

        //log event --> now done in lid controller
        //GameManager.Instance.eventManager.assembleObjects(transform, connectedLid.transform);
        //Debug.Log("lid attached to vessel " + transform.name + " " + connectedLid.name);
        
    }

    public void onLidDisconnected() 
    {
        Debug.Log("Lid disconnected");
        if(connectedLid != null)
        {
            connectedLid.GetComponent<LidController>().onVesselDisconnected(gameObject);

            //log event --> now done in lid controller
            //GameManager.Instance.eventManager.disassembleObjects(transform, connectedLid.transform);
            //Debug.Log("lid removed from vessel " + transform.name + " " + connectedLid.name);
        }

        connectedLid = null;
    }


    public void fillVessel(float fillSpeed, Material juiceMat, JuiceType juiceType)
    {
        fluidController.increaseFluid(fillSpeed, juiceMat);

        //setVisualization
        //visVolumeController.setVolumeText(fluidController.getLiquidLevel());
        setJuiceVisMaterial(true);
 
    }

    public void emptyVessel(float speed)
    {
        fluidController.emptyFluid(speed);
        //setVisualization

        //visVolumeController.setVolumeText(fluidController.getLiquidLevel());
        if (getFillLevel() < 0.1)
            setJuiceVisMaterial(false);

    }

    public Material getFluidMaterial()
    {
        return fluid.GetComponentInChildren<Renderer>().material;
    }

    public bool isFilled()
    {
        return fluidController.isFilled();
    }

    public void setPhText(float value)
    {
        foreach(VesselVisualizationController vis in visualizations)
            vis.setPhText(value);
    }

    public void setTempText(float value)
    {
        foreach (VesselVisualizationController vis in visualizations)
            vis.setTempText(value);
    }

    public void setPumpIdText(int id)
    {
        foreach (VesselVisualizationController vis in visualizations)
            vis.setPumpIdText(id);

    }

    public void setJuiceVisMaterial(bool full)
    {
        Material emptyMat = new Material(getFluidMaterial());
        emptyMat.color = Color.gray;

        //Debug.Log("change juice vis material!! number of vis: " + visualizations.Length);
        foreach (VesselVisualizationController vis in visualizations)
            vis.setJuiceMatVis(full? getFluidMaterial(): emptyMat);
    }
}
