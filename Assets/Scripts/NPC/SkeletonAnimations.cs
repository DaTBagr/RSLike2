using UnityEngine;

public class SkeletonAnimations : MonoBehaviour
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
}
