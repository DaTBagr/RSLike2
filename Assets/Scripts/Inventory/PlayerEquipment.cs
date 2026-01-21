using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] PlayerStats cStats;

    [SerializeField] WeaponModelSlot rightWeaponSlot;
    [SerializeField] WeaponModelSlot leftWeaponSlot;

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
                if (equipmentSlots[i].currentGearItem != null)
                {
                    RemoveEquippedStats(equipmentSlots[i].currentGearItem);
                }

                equipmentSlots[i].currentGearItem = item;
                AddEquippedStats(item);

                equipmentSlots[i].DisplayItem(item);
                InstantiateItemModel((WeaponItem)item);
            }
        }
    }

    public void UnequipGearItem(EquipmentSlot slot)
    {
        GearSlot itemSlot = slot.slot;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].slot == itemSlot)
            {
                RemoveEquippedStats(equipmentSlots[i].currentGearItem);
                equipmentSlots[i].currentGearItem = null;

                UnInstantiateItemModel(itemSlot);
            }
        }
    }

    // Only works for weapon items at the moment.
    private void InstantiateItemModel(GearItem item)
    {
        if (item.gearSlot == GearSlot.LeftWeapon)
        {
            leftWeaponSlot.LoadWeapon((WeaponItem)item);
        }
        else if (item.gearSlot == GearSlot.RightWeapon)
        {
            rightWeaponSlot.LoadWeapon((WeaponItem)item);
        }
    }

    private void UnInstantiateItemModel(GearSlot weaponSlot)
    {
        if (weaponSlot == GearSlot.LeftWeapon)
        {
            leftWeaponSlot.UnloadWeapon();
        }
        else if (weaponSlot == GearSlot.RightWeapon)
        {
            rightWeaponSlot.UnloadWeapon();
        }
    }

    private void AddEquippedStats(GearItem item)
    {
        cStats.strengthBonus += item.itemStats.strengthBonus;

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

        cStats.rangeBonus -= item.itemStats.rangeBonus;
        cStats.rangeAccuracy -= item.itemStats.rangeAccuracy;
        cStats.rangeDefence -= item.itemStats.rangeDefence;

        cStats.magicBonus -= item.itemStats.magicBonus;
        cStats.magicAccuracy -= item.itemStats.magicAccuracy;
        cStats.magicDefence -= item.itemStats.magicDefence;

        cStats.prayerBonus -= item.itemStats.prayerBonus;
    }

    public bool CheckIfSlotAlreadyEquipped(GearItem item)
    {
        GearSlot gearSlot = item.gearSlot;

        foreach(EquipmentSlot slot in equipmentSlots)
        {
            if (slot.slot == gearSlot)
            {
                if (slot.currentGearItem != null)
                return true;
            }
        }

        return false;
    }
}

