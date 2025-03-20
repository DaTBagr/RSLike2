using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerInventory : MonoBehaviour
{
    public Item[] items = new Item[27];
    public InventorySlot[] inventorySlots = new InventorySlot[27];

    [SerializeField] Item testItem;

    private void Start()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>();

        AddItem(testItem);
    }

    public void AddItem(Item item)
    {
        InventorySlot invSlot = null;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                invSlot = inventorySlots[i];
                break;
            }
        }

        if (invSlot != null)
        {
           invSlot.DisplayItem(item);
        }
    }

    public void RemoveItemFromSlot(InventorySlot inventorySlot)
    {
        Item removedItem = inventorySlot.currentItem;

        for (int i = 0; i < items.Length; ++i)
        {
            if(items[i] == removedItem)
            {
                items[i] = null;
            }
        }
    }

    public bool CheckIfSpaceInInventory()
    {
        foreach(Item item in items)
        {
            if (item == null)
            {
                return true;
            }
        }
        return false;
    }
}
