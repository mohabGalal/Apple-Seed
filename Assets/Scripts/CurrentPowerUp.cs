using UnityEngine;
using UnityEngine.UI;
public class CurrentPowerUp : MonoBehaviour
{
    public GameObject Key;
    public GameObject Heart;
    public GameObject Liquid;
    public GameObject Star;


    public void HandleCurrentPowerUp_UI(string name)
    {
        switch (name)
        {
            case "DoubleJump":
                {
                    Heart.SetActive(true);
                    Liquid.SetActive(false);
                    Star.SetActive(false);
                    Key.SetActive(false);
                    break;
                }
            case "SpinJump":
                {
                    Heart.SetActive(false);
                    Liquid.SetActive(true);
                    Star.SetActive(false);
                    Key.SetActive(false);
                    break;
                }
            case "Throw":
                {
                    Heart.SetActive(false);
                    Liquid.SetActive(false);
                    Star.SetActive(true);
                    Key.SetActive(false);
                    break;
                }
            case "Key":
                {
                    Heart.SetActive(false);
                    Liquid.SetActive(false);
                    Star.SetActive(false);
                    Key.SetActive(true);
                    break;
                }
            default:
                {
                    Heart.SetActive(false);
                    Liquid.SetActive(false);
                    Star.SetActive(false);
                    Key.SetActive(false);
                    break;
                }
        }

    }

}
