using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    public const float ROTATION_SPEED = 10;
    public const float STOPPING_DISTANCE = 0.1f;
    public const float WALKING_SPEED = 3f;
    public const float RUNNING_SPEED = 7f;

    private CharacterInfo thisChar;
    private CharacterAnimations cAnimations;

    private Unit targetUnit;
    private GridPosition targetGridPosition;
    private Vector3 clickedPos;
    private int attackRange;

    private int currentIndex;
    private List<Vector3> path;
    private List<GridPosition> gridPositions;
    private GridPosition finalPosition;

    private void Start()
    {
        targetGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        transform.position = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        clickedPos = MouseWorld.Instance.clickedPos;

        LevelGrid.Instance.AddPlayerToGridObject(targetGridPosition);

        MouseWorld.Instance.OnTargetChanged += MoveToTarget;
        MouseWorld.Instance.OnTargetRemoved += RemoveTarget;

        cAnimations = GetComponent<CharacterAnimations>();
        thisChar = GetComponent<CharacterInfo>();

        thisChar.SetMoveSpeed(WALKING_SPEED);
    }

    private void Update()
    {
        if (clickedPos != MouseWorld.Instance.clickedPos)
        {
            clickedPos = MouseWorld.Instance.clickedPos;
            GridPosition clickedGridPosition = LevelGrid.Instance.GetGridPosition(clickedPos);

            if (LevelGrid.Instance.GetGridObject(clickedGridPosition) != null) //Still not working as intended.
            {
                thisChar.readyToAttack = false;
                targetGridPosition = clickedGridPosition;

                (path, gridPositions, finalPosition) = Pathfinding.Instance.FindGroundTilePath(targetGridPosition, thisChar);
                thisChar.finalGridPosition = finalPosition;

                currentIndex = 0;
                attackRange = 0;
            }
            else return;
        }

        if (targetUnit != null)
        {
            Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, direction, ROTATION_SPEED * Time.deltaTime);
        }

        // If a path exists, move to each location
        if (path != null && currentIndex < path.Count - attackRange)
        {
            Vector3 targetPosition = path[currentIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;

            if (targetUnit == null) transform.forward = Vector3.Lerp(transform.forward, direction, ROTATION_SPEED * Time.deltaTime);

            if (thisChar.GetIsRunning())
            {
                thisChar.SetMoveSpeed(RUNNING_SPEED);
            }
            else thisChar.SetMoveSpeed(WALKING_SPEED);

            cAnimations.SetMovementAnimation(thisChar.GetMoveSpeed());

            // Moves toward next tile until deemed "close enough"
            if (Vector3.Distance(transform.position, targetPosition) > STOPPING_DISTANCE)
            {
                transform.position += direction * thisChar.GetMoveSpeed() * Time.deltaTime;
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
            thisChar.SetMoveSpeed(0);
            cAnimations.SetMovementAnimation(thisChar.GetMoveSpeed());

            if (thisChar.GetTargetUnit() != null)
            {
                if (Pathfinding.Instance.CheckIfNeighbour(GetPlayerGridPosition(), thisChar.GetTargetUnit().GetGridPosition()))
                {
                    thisChar.readyToAttack = true;
                }
            } else thisChar.readyToAttack = false;
        }
    }

    public GridPosition GetPlayerGridPosition()
    {
        return LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridObject GetPlayerGridObject()
    {
        return LevelGrid.Instance.GetGridObject(targetGridPosition);
    }

    private void MoveToTarget(object sender, MouseWorld.OnTargetChangedEventArgs unit)
    {
        targetUnit = unit.targetUnit;

        if (Pathfinding.Instance.CheckIfNeighbour(GetPlayerGridPosition(), targetUnit.GetGridPosition()))
        {
            return;
        }

        attackRange = 1;
        (path, gridPositions, finalPosition) = Pathfinding.Instance.FindTargetTilePath(targetUnit, thisChar);
        thisChar.finalGridPosition = finalPosition;
        currentIndex = 0;
    }

    private void RemoveTarget(object sender, EventArgs e)
    {
        targetUnit = null;

        attackRange = 0;
    }
}