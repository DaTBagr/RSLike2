public class EnemySkeletonTargetFoundState : EnemySkeletonBaseState
{
    NPCMovement movement;
    public override void EnterState(EnemySkeletonStateManager eSkeleton)
    {
        movement = eSkeleton.thisUnit.GetComponent<NPCMovement>();
        movement.target = eSkeleton.target;
        (movement.path, movement.gridPositions, _) = Pathfinding.Instance.FindTargetTilePath(eSkeleton.target, eSkeleton.thisUnit);
    }

    public override void UpdateState(EnemySkeletonStateManager eSkeleton)
    {
        if (movement.readyToAttack)
        {
            eSkeleton.SwitchState(eSkeleton.attackState);
        }
    }
}
