using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public const float ROTATION_SPEED = 10;
    public const float STOPPING_DISTANCE = 0.1f;

    private float moveSpeed;
    private int attackRange;
    public bool readyToAttack = false;

    public Unit target;
    private NPC thisNPC;
    private Animations sAnimation;

    private int currentIndex;

    private GridPosition targetGridPosition;
    private GridPosition finalPosition;
    public List<Vector3> path;
    public List<GridPosition> gridPositions;

    private void Start()
    {
        thisNPC = GetComponent<NPC>();
        sAnimation = GetComponent<Animations>();

        moveSpeed = thisNPC.GetSpeed();
        attackRange = thisNPC.GetAttackRange();

        targetGridPosition = thisNPC.GetGridPosition(transform.position);
        transform.position = thisNPC.GetWorldPosition(targetGridPosition);
    }

    private void Update()
    {
        if (target != null)
        {
            // If player is under NPC, move one tile away.
            if (LevelGrid.Instance.GetPlayerGridPosition() == thisNPC.GetGridPosition(transform.position))
            {
                Vector3 moveAwayLocation = thisNPC.GetWorldPosition(new GridPosition(targetGridPosition.x - 1, targetGridPosition.z));
                Vector3 moveDirection = (moveAwayLocation - transform.position).normalized;

                if (Vector3.Distance(transform.position, moveAwayLocation) > STOPPING_DISTANCE)
                {
                    transform.position += moveDirection * moveSpeed * Time.deltaTime;
                }

                LevelGrid.Instance.RemoveNPCFromGridObject(thisNPC, LevelGrid.Instance.GetPlayerGridPosition());
                LevelGrid.Instance.SetNPCAtGridObject(thisNPC, thisNPC.GetGridPosition(moveAwayLocation));

                return;
            }

            Vector3 direction = (thisNPC.GetWorldPosition(LevelGrid.Instance.GetPlayerGridPosition()) - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, direction, ROTATION_SPEED * Time.deltaTime);
        }

        // If a path exists, move to each location
        if (path != null && currentIndex < path.Count - attackRange)
        {
            Vector3 targetPosition = path[currentIndex];
            Vector3 direction = (targetPosition - transform.position).normalized;

            targetGridPosition = thisNPC.GetGridPosition(targetPosition);

            if (target == null) transform.forward = Vector3.Lerp(transform.forward, direction, ROTATION_SPEED * Time.deltaTime);

            // Moves toward next tile until deemed "close enough"
            if (Vector3.Distance(transform.position, targetPosition) > STOPPING_DISTANCE)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
                sAnimation.SetMovementAnimation(1);
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
        else if (path.Count == currentIndex + attackRange && target != null)
        {
            // Check if target is on neighbour tile. If not, follow target by finding new path.
            if (!Pathfinding.Instance.CheckIfNeighbour(target.GetGridPosition(), thisNPC.GetGridPosition()))
            {
                readyToAttack = false;
                (path, gridPositions, finalPosition) = Pathfinding.Instance.FindTargetTilePath(target, thisNPC);
                thisNPC.finalGridPosition = finalPosition;
                currentIndex = 0;
                return;
            }

            sAnimation.SetMovementAnimation(0);
            readyToAttack = true;
        } 
        else
            sAnimation.SetMovementAnimation(0);
    }
}