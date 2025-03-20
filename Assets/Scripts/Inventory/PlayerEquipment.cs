using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] PlayerStats cStats;

    private EquipmentSlot[] equipmentSlots = new EquipmentSlot[8];

    private void Start()
    {
        equipmentSlots = GetComponentsInChildren<EquipmentSlot>();
    }

    public void EquipGearItem(GearItem item)
    {
        GearSlot itemSlot = item.gearSlot;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].slot == itemSlot)
            {                
                RemoveEquippedStats(equipmentSlots[i].currentGearItem);
                equipmentSlots[i].currentGearItem = item;
                AddEquippedStats(item);

                equipmentSlots[i].DisplayItem(item);
            }
        }
    }

    public void UnequipGearItem(EquipmentSlot slot)
    {
        GearSlot itemSlot = slot.currentGearItem.gearSlot;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].slot == itemSlot)
            {
                RemoveEquippedStats(equipmentSlots[i].currentGearItem);
                equipmentSlots[i].currentGearItem = null;

                equipmentSlots[i].RemoveItem();
            }
        }
    }

    private void AddEquippedStats(GearItem item)
    {
        cStats.strengthBonus += item.itemStats.strengthBonus;
        cStats.stabAccuracy += item.itemStats.stabAccuracy;
        cStats.slashAccuracy += item.itemStats.slashAccuracy;
        cStats.crushAccuracy += item.itemStats.crushAccuracy;
        cStats.stabDefence += item.itemStats.stabDefence;
        cStats.slashDefence += item.itemStats.slashDefence;
        cStats.crushDefence += item.itemStats.crushDefence;

        cStats.rangeBonus += item.itemStats.rangeBonus;
        cStats.rangeAccuracy += item.itemStats.rangeAccuracy;
        cStats.rangeDefence += item.itemStats.rangeDefence;

        cStats.magicBonus += item.itemStats.magicBonus;
        cStats.magicAccuracy += item.itemStats.magicAccuracy;
        cStats.magicDefence += item.itemStats.magicDefence;

        cStats.prayerBonus += item.itemStats.prayerBonus;
    }

    private void RemoveEquippedStats(GearItem item)
    {
        cStats.strengthBonus -= item.itemStats.strengthBonus;
        cStats.stabAccuracy -= item.itemStats.stabAccuracy;
        cStats.slashAccuracy -= item.itemStats.slashAccuracy;
        cStats.crushAccuracy -= item.itemStats.crushAccuracy;
        cStats.stabDefence -= item.itemStats.stabDefence;
        cStats.slashDefence -= item.itemStats.slashDefence;
        cStats.crushDefence -= item.itemStats.crushDefence;

        cStats.rangeBonus -= item.itemStats.rangeBonus;
        cStats.rangeAccuracy -= item.itemStats.rangeAccuracy;
        cStats.rangeDefence -= item.itemStats.rangeDefence;

        cStats.magicBonus -= item.itemStats.magicBonus;
        cStats.magicAccuracy -= item.itemStats.magicAccuracy;
        cStats.magicDefence -= item.itemStats.magicDefence;

        cStats.prayerBonus -= item.itemStats.prayerBonus;
    }

    public bool CheckIfNotEquipped(GearItem item)
    {
        GearSlot gearSlot = item.gearSlot;

        foreach(EquipmentSlot slot in equipmentSlots)
        {
            if (slot.currentGearItem != null)
            {
                return false;
            }
        }

        return true;
    }
}

