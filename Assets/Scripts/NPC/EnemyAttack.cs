using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private NPC thisNPC;
    private Animations animations;
    public bool attacking;

    private void Start()
    {
        thisNPC = GetComponent<NPC>();
        animations = GetComponent<Animations>();
    }

    public void Attack(Unit target)
    {
        if (!attacking)
        {
            attacking = true;
            StartCoroutine(AttackAction(target));
        }
    }

    private IEnumerator AttackAction(Unit target)
    {
        animations.Attack();

        // This wait for seconds will have to be dependant on weapon animation speed
        yield return new WaitForSeconds(0.5f);
        target.TakeDamage(thisNPC.GetAttackDamage());
        Debug.Log($"Damaging {target.name}");

        yield return new WaitForSeconds(thisNPC.GetAttackCoolDown());

        animations.ResetAttack();
        attacking = false;
    }
}
