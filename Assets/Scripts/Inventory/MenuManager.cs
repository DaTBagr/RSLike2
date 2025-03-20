using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public PlayerInventory inventory;
    public PlayerEquipment equipment;

    private void Start()
    {
        inventory = GetComponentInChildren<PlayerInventory>();
        equipment = GetComponentInChildren<PlayerEquipment>();
    }
}
