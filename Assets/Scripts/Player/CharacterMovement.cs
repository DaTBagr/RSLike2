using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static MouseWorld;

public class CharacterMovement : MonoBehaviour
{
    public const float ROTATION_SPEED = 10;
    public const float STOPPING_DISTANCE = 0.1f;
    public const float WALKING_SPEED = 3f;
    public const float RUNNING_SPEED = 7f;

    private CharacterInfo thisChar;
    private CharacterAnimations cAnimations;

    private FindTilePath tilePath = new FindTilePath();

    private GridPosition targetGridPosition;
    private Vector3 targetPosition;
    private int tileStoppingDistance;

    private int currentIndex;
    private List<Vector3> path;
    private List<GridPosition> gridPositions;

    private void Start()
    {
        targetGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        transform.position = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        LevelGrid.Instance.AddPlayerToGridObject(targetGridPosition);

        MouseWorld.Instance.OnTargetNPCChanged += MoveToTargetNPC;

        cAnimations = GetComponent<CharacterAnimations>();
        thisChar = GetComponent<CharacterInfo>();

        thisChar.SetMoveSpeed(WALKING_SPEED);
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

                var pathResult = tilePath.FindGroundTilePath(targetGridPosition, thisChar);
                path = pathResult.pathList;
                gridPositions = pathResult.gridPositions;

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

    private void MoveToTargetNPC(object sender, OnTargetNPCChangedEventArgs npc)
    {
        NPC targetNPC = npc.targetNPC;
        tileStoppingDistance = 1;

        path = tilePath.FindTargetTilePath(targetNPC, thisChar).pathList;
        gridPositions = tilePath.FindTargetTilePath(targetNPC, thisChar).gridPositions;

        currentIndex = 0;
    }
}