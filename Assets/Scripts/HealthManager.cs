using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance {  get; private set; }

    public GameObject HealthUI;
    public int HeartCount =6;

    public List<Image> Hearts;

    private int MaxHearts = 5;

    public Color Dcolor;
    public Color Icolor;

    public float CoolDownGeneration = 2f;
    public float BlinkInterval = 0.2f;

    public SpriteRenderer playerSprite;

    private bool CanTakeDamage = true;

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
        if(CanTakeDamage == false)
        {
            
            return HeartCount;
        }
        Debug.Log($"Inside decrease hearts {CanTakeDamage}");

        if (HeartCount > 0)
        {

            Hearts[HeartCount - 1].color = Dcolor;
            HeartCount--;
            Debug.Log($"HeartCount : {HeartCount}");
            
            StartCoolDownEffect();


        }
        return HeartCount;
    }
    

    public void RestoreHeats()
    {
        if((HeartCount-1) < MaxHearts && HeartCount > 0)
        {

            Hearts[HeartCount].color = Icolor ;
            HeartCount++;
            Debug.Log($"HeartCount : {HeartCount}");
        }
        
    }

    private void StartCoolDownEffect()
    {
        if(CanTakeDamage == false)
        {
            return;
        }
        CanTakeDamage = false;
        playerSprite.DOKill();
        playerSprite.DOFade(0.3f, BlinkInterval).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        StartCoroutine(StopCoolDownEffect());
    }

    IEnumerator StopCoolDownEffect()
    {
        yield return new WaitForSeconds(CoolDownGeneration);
        playerSprite.DOKill();
        playerSprite.DOFade(1, 0.02f);
        CanTakeDamage = true;
    }


}
