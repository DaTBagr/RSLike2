using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerInfo pInfo;
    private PlayerAnimations pAnimations;
    private PlayerDamageCalculator pDamageCalc;

    // Placeholder
    private AttackStyles attackStyle = AttackStyles.Melee;

    private int damage;
    float attackCoolDown = 3;

    private void Start()
    {
        pAnimations = GetComponentInChildren<PlayerAnimations>();
        pInfo = GetComponentInChildren<PlayerInfo>();
        pDamageCalc = GetComponent<PlayerDamageCalculator>();
    }

    public void AttackTarget()
    {
        pInfo.isAttacking = true;
        Unit target = pInfo.GetTargetUnit();
        damage = pDamageCalc.HitCalc(attackStyle);
        StartCoroutine(AttackAction(target, damage, attackCoolDown));
    }
    private IEnumerator AttackAction(Unit target, int damage, float coolDown)
    {
        pAnimations.Attack();
        target.TakeDamage(damage);

        yield return new WaitForSeconds(attackCoolDown);

        pAnimations.ResetAttack();
        pInfo.isAttacking = false;
    }
}
