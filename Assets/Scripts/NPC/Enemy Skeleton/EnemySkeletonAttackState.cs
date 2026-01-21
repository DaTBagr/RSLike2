using System.Diagnostics;

public class EnemySkeletonAttackState : EnemySkeletonBaseState
{
    NPCMovement movement;
    EnemyAttack enemyAttack;
    public override void EnterState(EnemySkeletonStateManager eSkeleton)
    {
        movement = eSkeleton.thisUnit.GetComponent<NPCMovement>();
        enemyAttack = eSkeleton.thisUnit.GetComponent<EnemyAttack>();
    }

    public override void UpdateState(EnemySkeletonStateManager eSkeleton)
    {
        if (!movement.readyToAttack)
        {
            eSkeleton.SwitchState(eSkeleton.foundState);
        }
        else
        {
            enemyAttack.Attack(eSkeleton.target);
        }
    }
}
