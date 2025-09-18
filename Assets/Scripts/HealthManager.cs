using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance {  get; private set; }

    public GameObject HealthUI;
    public static int HeartCount = 5;

    public List<Image> Hearts;

    private int MaxHearts = 6;

    public int DecreaseHearts()
    {
        Color HeartColor = Hearts[HeartCount].color;
        HeartColor.a = 40;
        Hearts[HeartCount].color = HeartColor;
        --HeartCount;

        return HeartCount;
    }
    
    public void RestoreHeats()
    {
        if(HeartCount < MaxHearts)
        {
            Color HeartColor = Hearts[HeartCount].color;
            HeartColor.a = 100;
            Hearts[HeartCount].color = HeartColor;
            ++HeartCount;
            Debug.Log($"HeartCount : {HeartCount}");
        }
            
    }


}
