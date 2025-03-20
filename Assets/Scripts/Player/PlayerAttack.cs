using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerInfo pInfo;
    private PlayerAnimations pAnimations;

    // Placeholder
    int damage = 100;
    float attackCoolDown = 3;

    private void Start()
    {
        pAnimations = GetComponentInChildren<PlayerAnimations>();
        pInfo = GetComponentInChildren<PlayerInfo>();
    }

    public void AttackTarget()
    {
        pInfo.isAttacking = true;
        Unit target = pInfo.GetTargetUnit();
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
