using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private NPC thisNPC;

    private Animations animations;

    private void Start()
    {
        thisNPC = GetComponent<NPC>();
        animations = GetComponent<Animations>();
    }

    public void Attack(Unit target)
    {
        StartCoroutine(AttackAction(target));
    }

    private IEnumerator AttackAction(Unit target)
    {
        animations.Attack();
        target.TakeDamage(thisNPC.GetAttackDamage());
        Debug.Log($"Damaging {target.name}");

        yield return new WaitForSeconds(thisNPC.GetAttackCoolDown());

        animations.ResetAttack();
    }
}
