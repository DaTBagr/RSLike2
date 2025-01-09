using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static MouseWorld;

public class CharacterMovement : MonoBehaviour
{
    public const float ROTATION_SPEED = 10;
    public const float STOPPING_DISTANCE = 0.1f;
    public const float WALKING_SPEED = 3f;
    public const float RUNNING_SPEED = 7f;

    private CharacterInfo cInfo;
    private CharacterAnimations cAnimations;

    private GridPosition targetGridPosition;
    private Vector3 targetPosition;
    private int tileStoppingDistance;

    private int currentIndex;
    private List<Vector3> path;
    private List<GridPosition> gridPositions;

    private uint tick;
    uint previousTick = 0;

    private void Start()
    {
        targetGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        transform.position = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        LevelGrid.Instance.AddPlayerToGridObject(targetGridPosition);

        MouseWorld.Instance.OnTargetNPCChanged += MoveToTargetNPC;
        TimeTickSystem.OnTick += OnNextTick;

        cAnimations = GetComponent<CharacterAnimations>();
        cInfo = GetComponent<CharacterInfo>();

        cInfo.SetMoveSpeed(WALKING_SPEED);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector3 clickedPosition = MouseWorld.Instance.GetMousePos();

            if (LevelGrid.Instance.GetGridPosition(clickedPosition) != null) //Still not working as intended.
            {
                targetGridPosition = LevelGrid.Instance.GetGridPosition(clickedPosition);
                path = FindTilePath(targetGridPosition);
                currentIndex = 0;
                tileStoppingDistance = 0;
            }
            else return;
        }

        // If a path exists, move to each location
        if (path != null && currentIndex < path.Count - tileStoppingDistance)
        {
            targetPosition = path[currentIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, direction, ROTATION_SPEED * Time.deltaTime);

            if (cInfo.GetIsRunning())
            {
                cInfo.SetMoveSpeed(RUNNING_SPEED);
            }
            else cInfo.SetMoveSpeed(WALKING_SPEED);

            cAnimations.SetMovementAnimation(cInfo.GetMoveSpeed());

            // Moves toward next tile until deemed "close enough"
            if (Vector3.Distance(transform.position, targetPosition) > STOPPING_DISTANCE)
            {
                transform.position += direction * cInfo.GetMoveSpeed() * Time.deltaTime;
            }
            else // Once close enough incremement index. 
            {
                if (currentIndex > 0 && currentIndex < path.Count)
                {
                    LevelGrid.Instance.RemovePlayerFromGridObject(gridPositions[currentIndex - 1]);
                }

                LevelGrid.Instance.AddPlayerToGridObject(gridPositions[currentIndex]);
                currentIndex++;
            }
        }
        else // Once at final index set speed to 0, setting idle animation.
        {
            cInfo.SetMoveSpeed(0);
            cAnimations.SetMovementAnimation(cInfo.GetMoveSpeed());
        }
    }

    private List<Vector3> FindTilePath(GridPosition targetPosition)
    {
        gridPositions = Pathfinding.Instance.FindPath(GetPlayerGridPosition(), targetPosition, out int pathLength);
        List<Vector3> pathList = new List<Vector3>();

        foreach (GridPosition gridPosition in gridPositions)
        {
            pathList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        return pathList;
    }

    public GridPosition GetPlayerGridPosition()
    {
        return LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridObject GetPlayerGridObject()
    {
        return LevelGrid.Instance.GetGridObject(targetGridPosition);
    }

    private void MoveToTargetNPC(object sender, OnTargetNPCChangedEventArgs npc)
    {
        NPC targetNPC = npc.targetNPC;
        tileStoppingDistance = 1;
        targetGridPosition = targetNPC.GetGridPosition();
        
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
    }
    private void OnNextTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        tick++;
    }
}
