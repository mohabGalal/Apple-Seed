using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance {  get; private set; }

    public GameObject HealthUI;
    public int HeartCount =6;

    public List<Image> Hearts;

    private int MaxHearts = 5;

    public Color Dcolor;
    public Color Icolor;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int DecreaseHearts()
    {
        if (HeartCount > 0)
        {
            Hearts[HeartCount - 1].color = Dcolor;
            HeartCount--;
            Debug.Log($"HeartCount : {HeartCount}");
            
        }
        return HeartCount;
    }
    
    public void RestoreHeats()
    {
        if((HeartCount-1) <= MaxHearts && HeartCount > 0)
        {

            Hearts[HeartCount].color = Icolor ;
            HeartCount++;
            Debug.Log($"HeartCount : {HeartCount}");
        }
            
    }


}
