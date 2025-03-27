using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Item currentItem;
    private Image invSlotImage;

    private PlayerEquipment equipment;
    private PlayerInventory inventory;

    private void Awake()
    {
        invSlotImage = GetComponent<Image>();
        IsImageDisplayed(false);
    }

    private void Start()
    {
        equipment = MenuManager.instance.equipment.GetComponent<PlayerEquipment>();
        inventory = MenuManager.instance.inventory.GetComponent<PlayerInventory>();
    }

    public void DisplayItem(Item item)
    {
        IsImageDisplayed(true);
        invSlotImage.sprite = item.GetComponent<Item>().sprite;

        currentItem = item;
    }

    public void RemoveItem()
    {
        IsImageDisplayed(false);
        inventory.RemoveItemFromSlot(this);

        currentItem = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem is GearItem) 
        {
            if (!equipment.CheckIfSlotAlreadyEquipped((GearItem)currentItem))
            {
                equipment.EquipGearItem((GearItem)currentItem);

                RemoveItem();
                return;
            }

            if (equipment.CheckIfSlotAlreadyEquipped((GearItem)currentItem) && inventory.CheckIfSpaceInInventory())
            {
                equipment.EquipGearItem((GearItem)currentItem);

                RemoveItem();
            }
        }
    }

    public void IsImageDisplayed(bool yes)
    {
        var tempColour = invSlotImage.color;        

        if (yes)
        {
            tempColour.a = 1f;
        } else
        {
            tempColour.a = 0f;
        }

        invSlotImage.color = tempColour;
    }
}
