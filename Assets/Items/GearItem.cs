using UnityEngine;

[RequireComponent (typeof(GearSlot))]
public class GearItem : Item
{
    public ItemStats itemStats;
    public ItemRequirements itemRequirements;

    public GearSlot gearSlot;

    private void Start()
    {
        itemStats = GetComponent<ItemStats>();
        itemRequirements = GetComponent<ItemRequirements>();
    }
}
