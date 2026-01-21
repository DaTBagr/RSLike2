using UnityEngine;

public class EnemySkeletonStateManager : MonoBehaviour
{
    public EnemySkeletonBaseState currentState;
    public EnemySkeletonBaseState idleState = new EnemySkeletonIdleState();
    public EnemySkeletonBaseState foundState = new EnemySkeletonTargetFoundState();
    public EnemySkeletonBaseState attackState = new EnemySkeletonAttackState();

    public EnemySkeleton thisUnit;
    public Unit target;

    private void Start()
    {
        thisUnit = GetComponent<EnemySkeleton>();
        target = thisUnit.target;

        currentState = idleState;

        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemySkeletonBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
