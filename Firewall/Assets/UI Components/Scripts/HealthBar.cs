using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance;

    public float healthAmount;
    public Image healthBarFill;

    private void Awake()
    {
        Instance = this;
    }

    public void DecreaseHealth(float amountToDecrease)
    {
        healthAmount -= amountToDecrease;
        healthBarFill.fillAmount = healthAmount;
    }

    public void IncreaseHealth(float amountToIncrease)
    {
        healthAmount += amountToIncrease;
        healthBarFill.fillAmount = healthAmount;
    }
}