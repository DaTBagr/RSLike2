using Unity.VisualScripting;
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

    private void Awake()
    {
        equipSlotImage = GetComponent<Image>();

        IsImageDisplayed(false);
    }

    private void Start()
    {
        equipment = MenuManager.instance.equipment.GetComponent<PlayerEquipment>();
        inventory = MenuManager.instance.inventory.GetComponent<PlayerInventory>();
    }

    public void DisplayItem(GearItem item)
    {
        IsImageDisplayed(true);
        equipSlotImage.sprite = item.GetComponent<Item>().sprite;

        currentGearItem = item;
    }

    public void RemoveItem()
    {
        IsImageDisplayed(false);
        inventory.AddItem(currentGearItem);

        equipment.UnequipGearItem(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventory.CheckIfSpaceInInventory())
        {
            RemoveItem();
        }
    }

    public void IsImageDisplayed(bool yes)
    {
        var tempColour = equipSlotImage.color;

        if (yes)
        {
            tempColour.a = 1f;
        }
        else
        {
            tempColour.a = 0f;
        }

        equipSlotImage.color = tempColour;
    }
}
