using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void SetMovementAnimation(float speed)
    {
        animator.SetFloat("moveSpeed", speed);
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
    }

    public void ResetAttack()
    {
        animator.ResetTrigger("attack");
    }

    public IEnumerator TakeDamage()
    {
        animator.SetTrigger("takeDamage");
        yield return new WaitForSeconds(0.5f);
        animator.ResetTrigger("takeDamage");
    }

    public void TakeDamageAnimation()
    {
        StartCoroutine(TakeDamage());
    }
}
