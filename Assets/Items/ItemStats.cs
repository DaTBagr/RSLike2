using UnityEngine;

public class ItemStats : MonoBehaviour
{
    [Header("Melee Attack Stats")]
    public int strengthBonus;

    [Header("Melee Defence Stats")]
    public int meleeDefence;

    [Header("Range Attack Stats")]
    public int rangeBonus;
    public int rangeAccuracy;

    [Header("Range Defence Stats")]
    public int rangeDefence;

    [Header("Magic Attack Stats")]
    public int magicBonus;
    public int magicAccuracy;

    [Header("Magic Defence Stats")]
    public int magicDefence;

    [Header("Prayer Bonus")]
    public int prayerBonus;
}
