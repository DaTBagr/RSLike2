using System.Collections;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private CharacterInfo cInfo;
    private CharacterAnimations cAnimations;

    // Placeholder
    int damage = 100;
    float attackCoolDown = 3;

    private void Start()
    {
        cAnimations = GetComponentInChildren<CharacterAnimations>();
        cInfo = GetComponentInChildren<CharacterInfo>();
    }

    public void AttackTarget()
    {
        cInfo.isAttacking = true;
        Unit target = cInfo.GetTargetUnit();
        StartCoroutine(AttackAction(target, damage, attackCoolDown));
    }
    private IEnumerator AttackAction(Unit target, int damage, float coolDown)
    {
        cAnimations.Attack();
        target.TakeDamage(damage);

        yield return new WaitForSeconds(attackCoolDown);

        cAnimations.ResetAttack();
        cInfo.isAttacking = false;
    }
}
