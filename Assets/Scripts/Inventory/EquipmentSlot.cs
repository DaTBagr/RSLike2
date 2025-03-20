using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    public GearItem currentGearItem;
    public GearSlot slot;

    private Image equipSlotImage;

    private PlayerEquipment equipment;
    private PlayerInventory inventory;

    private void Start()
    {
        equipSlotImage = GetComponent<Image>();
        equipSlotImage.enabled = false;

        equipment = GetComponentInParent<PlayerEquipment>();
        inventory = GetComponentInParent<PlayerInventory>();
    }

    public void DisplayItem(GearItem item)
    {
        equipSlotImage.enabled = true;
        equipSlotImage.sprite = item.GetComponent<Item>().sprite;

        currentGearItem = item;
    }

    public void RemoveItem()
    {
        if (inventory.CheckIfSpaceInInventory())
        {
            equipSlotImage.enabled = false;
            equipment.UnequipGearItem(this);

            currentGearItem = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        RemoveItem();
    }
}
