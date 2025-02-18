public class EnemySkeletonTargetFoundState : EnemySkeletonBaseState
{
    NPCMovement movement;
    FindTilePath findTilePath = new FindTilePath();
    public override void EnterState(EnemySkeletonStateManager eSkeleton)
    {
        movement = eSkeleton.thisUnit.GetComponent<NPCMovement>();
        movement.target = eSkeleton.target;
        movement.path = findTilePath.FindTargetTilePath(eSkeleton.target, eSkeleton.thisUnit).pathList;
        movement.gridPositions = findTilePath.FindTargetTilePath(eSkeleton.target, eSkeleton.thisUnit).gridPositions;
    }

    public override void UpdateState(EnemySkeletonStateManager eSkeleton)
    {
        if (movement.readyToAttack)
        {
            eSkeleton.SwitchState(eSkeleton.attackState);
        }
    }
}
