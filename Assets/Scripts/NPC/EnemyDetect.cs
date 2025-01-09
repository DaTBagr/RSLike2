using System;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public event EventHandler TargetFound;
    private GridPosition nPCGridPosition;
    private GridPosition targetGridPosition;

    private int detectionRange;
    private bool hasTarget;

    private NPC thisNPC;

    private void Start()
    {
        thisNPC = GetComponent<NPC>();
        detectionRange = thisNPC.GetDetectionRange();
    }

    private void Update()
    {
        nPCGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        targetGridPosition = LevelGrid.Instance.GetPlayerGridPosition();

        if (targetGridPosition.x < nPCGridPosition.x - detectionRange || targetGridPosition.x > nPCGridPosition.x + detectionRange ||
            targetGridPosition.z < nPCGridPosition.z - detectionRange || targetGridPosition.z > nPCGridPosition.z + detectionRange)
        {
            return;
        }

        if (!hasTarget)
        {
            TargetFound?.Invoke(this, EventArgs.Empty);
            hasTarget = true;
        }
    }
}
