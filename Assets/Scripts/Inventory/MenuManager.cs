using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField] public GameObject inventory;
    [SerializeField] public GameObject equipment;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        SetInventory();
    }

    public void SetInventory()
    {
        foreach (var image in equipment.GetComponentsInChildren<Image>())
        {
            image.enabled = false;
        }

        foreach (var image in inventory.GetComponentsInChildren<Image>())
        {
            image.enabled = true;
        }
    }

    public void SetEquipment()
    {
        foreach (var image in equipment.GetComponentsInChildren<Image>())
        {
            image.enabled = true;
        }

        foreach (var image in inventory.GetComponentsInChildren<Image>())
        {
            image.enabled = false;
        }
    }
}
