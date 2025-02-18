using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    private GridPosition nPCGridPosition;
    private GridPosition targetGridPosition;

    private int detectionRange;
    private bool hasTarget;

    private NPC thisNPC;
    [SerializeField] Unit target;

    private void Start()
    {
        thisNPC = GetComponent<NPC>();
        detectionRange = thisNPC.GetDetectionRange();
    }

    private void Update()
    {
        if (hasTarget) return;

        nPCGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        targetGridPosition = LevelGrid.Instance.GetPlayerGridPosition();

        if (targetGridPosition.x < nPCGridPosition.x - detectionRange || targetGridPosition.x > nPCGridPosition.x + detectionRange ||
            targetGridPosition.z < nPCGridPosition.z - detectionRange || targetGridPosition.z > nPCGridPosition.z + detectionRange)
        {
            return;
        }

        if (!hasTarget)
        {
            hasTarget = true;
        }
    }

    public bool CheckIfHasTarget()
    {
        return hasTarget;
    }

    public void SetTarget(Unit target)
    {
        this.target = target;
    }

    public Unit GetTarget()
    {
        return target;
    }
}
