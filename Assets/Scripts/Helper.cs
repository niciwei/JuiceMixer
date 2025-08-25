using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper 
{
    
    public static Material mixMaterial(List<Material> mats, List<float> strength)
    {
        Material endMaterial = new Material(mats[0]); // mats[0];

        float r = 0;
        float g = 0;
        float b = 0;

        float sumStrenth = getFloatListSum(strength);

        for(int i = 0; i < mats.Count; i++)
        {
            r += (mats[i].color.r / sumStrenth) * strength[i];
            g += (mats[i].color.g / sumStrenth) * strength[i];
            b += (mats[i].color.b / sumStrenth) * strength[i];
        }

        Color newColor = new Color(r, g, b);
        endMaterial.color = newColor;


        return endMaterial;
    }

    public static Dictionary<JuiceType, float> getJuiceTypeAndAmount(List<Material> mats, List<float> strength)
    {
        Dictionary<JuiceType, float> amount = new Dictionary<JuiceType, float>();

        for(int i = 0; i < mats.Count; i++)
        {
            JuiceType type = getJuiceTypeFromMaterial(mats[i]);
            amount.Add(type, strength[i]);
        }

        return amount;
    }

    public static JuiceType getJuiceTypeFromMaterial(Material mat) //TODO fix! check for color
    {
        JuiceType type = JuiceType.Empty;

        /*
        if (mat.name.Contains("Juice_yellow"))
            type = JuiceType.Yellow;
        else if (mat.name.Contains("Juice_orange"))
            type = JuiceType.Orange;
        else if (mat.name.Contains("Juice_red"))
            type = JuiceType.Red;
        else if (mat.name.Contains("Juice_Green"))
            type = JuiceType.Green;
        else if (mat.name.Contains("Juice_blue"))
            type = JuiceType.Blue;
        else if (mat.name.Contains("Juice_purple"))
            type = JuiceType.Purple;
                */
        Debug.Log("Material color and name: " + mat.color.ToString() + " " + mat.name);

        if (mat.color.ToString() == "RGBA(0.485, 0.000, 1.000, 1.000)")
            type = JuiceType.Purple;
        else if (mat.color.ToString() == "RGBA(0.000, 1.000, 0.000, 1.000)")
            type = JuiceType.Green;
        else if (mat.color.ToString() == "RGBA(1.000, 0.500, 0.000, 1.000)")
            type = JuiceType.Orange;
        else if (mat.color.ToString() == "RGBA(0.000, 0.500, 1.000, 1.000)")
            type = JuiceType.Blue;
        else if (mat.color.ToString() == "RGBA(1.000, 0.000, 0.000, 1.000)")
            type = JuiceType.Red;
        else if (mat.color.ToString() == "RGBA(1.000, 1.000, 0.000, 1.000)")
            type = JuiceType.Purple;

        Debug.Log("type: " + type.ToString());

        return type;
    }


    public static float getFloatListSum(List<float> strength)
    {
        float sumStrenth = 0;

        foreach (float s in strength)
        {
            sumStrenth += s;
        }

        return sumStrenth;
    }


    public static float getRandomNumber(float minRange, float maxRange)
    {
        float n = Random.Range(minRange, maxRange);

        return n;

    }
 

}
