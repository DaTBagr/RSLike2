using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCMovement : MonoBehaviour
{
    public const float ROTATION_SPEED = 10;
    public const float STOPPING_DISTANCE = 0.1f;

    private NPC thisNPC;
    private SkeletonAnimations sAnimation;

    private float moveSpeed;
    private int attackRange;

    private GridPosition targetGridPosition;
    private int currentIndex;
    private List<Vector3> path;
    private List<GridPosition> gridPositions;

    private EnemyDetect detect;
    private bool targetFound;

    private void Start()
    {
        thisNPC = GetComponent<NPC>();
        sAnimation = GetComponent<SkeletonAnimations>();

        moveSpeed = thisNPC.GetSpeed();
        attackRange = thisNPC.GetAttackRange();

        targetGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        transform.position = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        detect = GetComponent<EnemyDetect>();
        detect.TargetFound += TargetFound;
    }

    private void Update()
    {
        // Once the targets entered detection range, if it moves the npc will follow it.
        if (targetFound)
        {
            // If player is under NPC, move one tile away.
            if (LevelGrid.Instance.GetPlayerGridPosition() == LevelGrid.Instance.GetGridPosition(transform.position))
            {
                Vector3 moveAwayLocation = LevelGrid.Instance.GetWorldPosition(new GridPosition(targetGridPosition.x - 1, targetGridPosition.z));
                Vector3 direction = (moveAwayLocation - transform.position).normalized;

                if (Vector3.Distance(transform.position, moveAwayLocation) > STOPPING_DISTANCE)
                {
                    transform.position += direction * moveSpeed * Time.deltaTime;
                }

                LevelGrid.Instance.RemoveNPCFromGridObject(thisNPC, LevelGrid.Instance.GetPlayerGridPosition());
                LevelGrid.Instance.SetNPCAtGridObject(thisNPC, LevelGrid.Instance.GetGridPosition(moveAwayLocation));
            }

            if (LevelGrid.Instance.GetPlayerGridPosition() != targetGridPosition)
            {
                MoveToTarget();
            }
        }

        // If a path exists, move to each location
        if (path != null && currentIndex < path.Count - attackRange)
        {
            Vector3 targetPosition = path[currentIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, direction, ROTATION_SPEED * Time.deltaTime);

            // Moves toward next tile until deemed "close enough"
            if (Vector3.Distance(transform.position, targetPosition) > STOPPING_DISTANCE)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            else // Once close enough incremement index. 
            {
                if (currentIndex > 0 && currentIndex < path.Count)
                {
                    LevelGrid.Instance.RemoveNPCFromGridObject(thisNPC, gridPositions[currentIndex - 1]);
                }

                LevelGrid.Instance.SetNPCAtGridObject(thisNPC, gridPositions[currentIndex]);
                currentIndex++;
            }
        }
        else
            sAnimation.SetMovementAnimation(0);
    }

    private List<Vector3> FindTilePath(GridPosition targetPosition)
    {
        gridPositions = Pathfinding.Instance.FindPath(GetNPCGridPosition(), targetPosition, out int pathLength);
        List<Vector3> pathList = new List<Vector3>();

        foreach (GridPosition gridPosition in gridPositions)
        {
            pathList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        return pathList;
    }

    public GridPosition GetNPCGridPosition()
    {
        return LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridObject GetNPCGridObject()
    {
        return LevelGrid.Instance.GetGridObject(targetGridPosition);
    }

    private void MoveToTarget()
    {
        targetGridPosition = LevelGrid.Instance.GetPlayerGridPosition();
        path = FindTilePath(targetGridPosition);

        GridPosition lastGridPos = LevelGrid.Instance.GetGridPosition(path[path.Count - 2]);

        if (lastGridPos.x != targetGridPosition.x && lastGridPos.z != targetGridPosition.z)
        {
            GridPosition newLastTile;
            GridPosition firstTile = LevelGrid.Instance.GetGridPosition(path[0]);

            if (firstTile.x < firstTile.z) newLastTile = new GridPosition(targetGridPosition.x, lastGridPos.z);
                 else newLastTile = new GridPosition(lastGridPos.x, targetGridPosition.z);

            path.Insert(path.Count - 1, (LevelGrid.Instance.GetWorldPosition(newLastTile)));
        }

        currentIndex = 0;
        sAnimation.SetMovementAnimation(1);
    }

    private void TargetFound(object sender, EventArgs e)
    {
        MoveToTarget();
        targetFound = true;
    }
}
