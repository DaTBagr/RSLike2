using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void SetMovementAnimation(float speed)
    {
        animator.SetFloat("moveSpeed", speed);
    }
}
