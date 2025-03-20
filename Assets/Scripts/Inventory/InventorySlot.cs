using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Item currentItem;
    private Image invSlotImage;

    private PlayerEquipment equipment;
    private PlayerInventory inventory;

    private void Start()
    {
        invSlotImage = GetComponent<Image>();
        invSlotImage.enabled = false;

        equipment = GetComponentInParent<PlayerEquipment>();
        inventory = GetComponentInParent<PlayerInventory>();
    }

    public void DisplayItem(Item item)
    {
        invSlotImage.enabled = true;
        invSlotImage.sprite = item.GetComponent<Item>().sprite;

        currentItem = item;
    }

    public void RemoveItem()
    {
        invSlotImage.enabled = false;
        currentItem = null;

        inventory.RemoveItemFromSlot(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem is GearItem) 
        {
            if (equipment.CheckIfNotEquipped((GearItem)currentItem) && inventory.CheckIfSpaceInInventory())
            {
                equipment.EquipGearItem((GearItem)currentItem);

                RemoveItem();
            }
        }
    }
}
