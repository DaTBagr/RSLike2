public class EnemySkeletonIdleState : EnemySkeletonBaseState
{
    EnemyDetect enemyDetect;
    public override void EnterState(EnemySkeletonStateManager eSkeleton)
    {
        enemyDetect = eSkeleton.thisUnit.GetComponent<EnemyDetect>();
        enemyDetect.SetTarget(eSkeleton.target);
    }

    public override void UpdateState(EnemySkeletonStateManager eSkeleton)
    {
        if (enemyDetect.CheckIfHasTarget())
        {
            eSkeleton.SwitchState(eSkeleton.foundState);
        }
    }
}
