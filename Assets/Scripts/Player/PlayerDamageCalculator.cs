using System;
using UnityEngine;

public class PlayerDamageCalculator : MonoBehaviour
{
    private PlayerStats playerStats;
    public EnemyStats enemyStats;

    private int baseStatBonus = 8;
    private int styleLevel;
    private int strengthBonus;
    private int hitAccuracy;
    private int enemyDefence;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        MouseWorld.Instance.OnTargetChanged += SetTarget;
    }

    public int HitCalc(AttackStyles style)
    {
        int hit;

        SetDamageType(style);

        int attack = styleLevel + baseStatBonus;
        int maxHit = Convert.ToInt32(((attack * (strengthBonus + 64)) + 320) / 640);
        int attackRoll = Convert.ToInt32(attack * (hitAccuracy + 64));
        float enemyDefenceRoll = Convert.ToInt32((enemyDefence + 9) * 64);

        int possibleHit = Convert.ToInt32(CalculateHitChance(attackRoll, enemyDefenceRoll) * ((maxHit/2) + (1/maxHit + 1)));

        hit = UnityEngine.Random.Range(0, possibleHit);

        return hit;
    }

    private float CalculateHitChance(int attackRoll, float defenceRoll)
    {
        float hitChance;

        if (attackRoll > defenceRoll)
        {
            hitChance = 1 - ((defenceRoll + 2) / (2 * (attackRoll + 1)));
        }
        else
        {
             hitChance = attackRoll/(2 * (defenceRoll + 1));
        }

        return hitChance;
    }

    private void SetDamageType(AttackStyles style)
    {
        switch (style)
        {
            case AttackStyles.Melee:
                styleLevel = playerStats.attackLevel;
                strengthBonus = playerStats.strengthBonus;
                hitAccuracy = playerStats.attackAccuracy;
                enemyDefence = enemyStats.meleeDefence;
                break;
            case AttackStyles.Range:
                styleLevel = playerStats.rangeLevel;
                strengthBonus = playerStats.rangeBonus;
                hitAccuracy = playerStats.rangeAccuracy;
                enemyDefence = enemyStats.rangeDefence;
                break;
            case AttackStyles.Magic:
                styleLevel = playerStats.mageLevel;
                strengthBonus = playerStats.magicBonus;
                hitAccuracy = playerStats.magicAccuracy;
                enemyDefence = enemyStats.magicDefence;
                break;
        }
    }

    private void SetTarget(object sender, MouseWorld.OnTargetChangedEventArgs e)
    {
        enemyStats = (e.targetUnit as NPC).stats;
    }
}
