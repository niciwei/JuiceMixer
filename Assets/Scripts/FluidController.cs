using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidController : MonoBehaviour
{
    public JuiceType type;

    public bool isFinalFluid = false;
    
    [HideInInspector]
    public Dictionary<JuiceType, float> currentComposition;

    void Start()
    {
        currentComposition = new Dictionary<JuiceType, float>();

        if (!isFinalFluid && type != JuiceType.Empty)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


  
    public void increaseFluid(float fillSpeed, Material juiceMat)
    {
        if (transform.localScale.y < 1.0)
        {
            float addedYScale = fillSpeed * Time.deltaTime;
            float newYScale = transform.localScale.y + addedYScale;

            List<Material> mats = new List<Material>();
            mats.Add(getFluidMaterial());
            mats.Add(juiceMat);

            List<float> strength = new List<float>();
            strength.Add(transform.localScale.y);
            strength.Add(addedYScale);

            Material newJuiceMat = Helper.mixMaterial(mats, strength);


            transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);
            transform.GetComponentInChildren<Renderer>().material = newJuiceMat;
        }
    }

    public void setScale(float value)
    {
        transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
    }


    public void increaseTargetFluid(List<float> strength, List<Material> mats, List<JuiceType> juiceType, List<LidController> lids, List<VesselController> vessels)
    {
        //TODO ph and temperature
        if (transform.localScale.y < 1.0)
        {

            float totalStrenght = Helper.getFloatListSum(strength);
            Material mat = Helper.mixMaterial(mats, strength);

            float addedYScale = totalStrenght;// * Time.deltaTime;
            float newYScale = transform.localScale.y + addedYScale;


            List<Material> materials = new List<Material>();
            materials.Add(getFluidMaterial());
            materials.Add(mat);


            List<float> s = new List<float>();
            s.Add(transform.localScale.y);
            s.Add(addedYScale);

            Material newJuiceMat = Helper.mixMaterial(materials, s);


            transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);
            transform.GetComponentInChildren<Renderer>().material = newJuiceMat;

            calculateNewComposition(strength, juiceType);
        }
    }



    void calculateNewComposition(List<float> strength, List<JuiceType> types)
    {
        for(int i = 0; i < strength.Count; i++)
        {
            float s = strength[i];
            JuiceType t = types[i];
            //TODO set juice type and amount for final juice - maybe also for others
            if (currentComposition.ContainsKey(t))
            {
                currentComposition[t] += s;
            }
            else
            {
                currentComposition.Add(t, s);
            }

        }
    }



    public void emptyFluid(float speed)
    {
        float removedYScale = speed * Time.deltaTime;
        float newYScale = transform.localScale.y - removedYScale;

        if (newYScale < 0f)
            newYScale = 0f;

        transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);
    }

    public void completelyEmptyFluid()
    {
        Debug.Log("button clicked!!! empty vessel!");
        transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);

    }

    public Material getFluidMaterial()
    {
        return transform.GetComponentInChildren<Renderer>().material;
    }

    public bool isFilled()
    {
        if(transform.localScale.y > 0.5) //should be filled at least halfway?
            return true;
        else
            return false;
    }

    public bool isFull()
    {
        if (transform.localScale.y == 1.0)
            return true;
        else
            return false;
    }

    public float getLiquidLevel()
    {
        return transform.localScale.y;
    }
}
