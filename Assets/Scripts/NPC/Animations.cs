using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMovementAnimation(float speed)
    {
        animator.SetFloat("speed", speed);
    }

    public void Attack()
    {
        animator.SetTrigger("attack");
    }

    public void ResetAttack()
    {
        animator.ResetTrigger("attack");
    }
}
