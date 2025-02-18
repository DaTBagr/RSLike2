using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar instance;

    private Slider slider;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Multiple healthbar instances");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void ChangeHealth(int value, int maxHealth)
    {
        slider.value = (value / maxHealth) * 100;
    }
}
