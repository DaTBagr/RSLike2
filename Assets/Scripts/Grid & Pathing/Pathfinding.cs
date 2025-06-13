using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    [SerializeField] LayerMask obstacleLayer;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private int height;
    private int width;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"Pathfinding instance was not null");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(int height, int width, float cellSize)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffset = 5;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffset, Vector3.up, raycastOffset * 2, obstacleLayer))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startPosition, GridPosition endPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startPosition);
        PathNode endNode = gridSystem.GetGridObject(endPosition);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPosition, endPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // No path found
        Debug.Log("no path found");
        pathLength = 0;
        return null;
    }

    private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridDistance.x);
        int zDistance = Mathf.Abs(gridDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowFCostNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowFCostNode.GetFCost())
            {
                lowFCostNode = pathNodeList[i];
            }
        }

        return lowFCostNode;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z));
            if (gridPosition.z - 1 >= 0)
            {
                //LeftDown
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //LeftUp
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.x + 1 <= gridSystem.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z));
            if (gridPosition.z - 1 >= 0)
            {
                //RightDown
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //RightUp
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //Up
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z + 1));
        }

        if (gridPosition.z - 1 >= 0)
        {
            //Down
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z - 1));
        }

        return neighbourList;
    }

    public PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    public PathNode GetNode(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition);
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode node in pathNodeList)
        {
            gridPositionList.Add(node.GetGridPosition());
        }

        return gridPositionList;
    }

    public bool CheckIfNeighbour(GridPosition gridPosition, GridPosition targetGridPosition)
    {
        List<PathNode> neighbourNodes = GetNeighbourList(GetNode(gridPosition));

        foreach (PathNode node in neighbourNodes)
        {
            if (node == GetNode(targetGridPosition)) 
            { 
                if (gridPosition.x == targetGridPosition.x || gridPosition.z == targetGridPosition.z)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public (List<Vector3> pathList, List<GridPosition> gridPositions, GridPosition finalPosition) FindGroundTilePath(GridPosition targetPosition, Unit thisChar)
    {
        List<GridPosition> gridPositions = FindPath(thisChar.GetGridPosition(), targetPosition, out int pathLength);
        List<Vector3> pathList = new List<Vector3>();
        GridPosition finalPosition;

        foreach (GridPosition gridPosition in gridPositions)
        {
            pathList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        finalPosition = gridPositions[gridPositions.Count - 1];

        return (pathList, gridPositions, finalPosition);
    }

    public (List<Vector3> pathList, List<GridPosition> gridPositions, GridPosition finalPosition) FindTargetTilePath(Unit target, Unit thisChar)
    {
        List<GridPosition> gridPositions = FindPath(thisChar.GetGridPosition(), target.finalGridPosition, out int pathLength);
        List<Vector3> pathList = new List<Vector3>();
        GridPosition finalPosition;

        foreach (GridPosition gridPosition in gridPositions)
        {
            pathList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        GridPosition lastGridPos;

        if (pathList.Count > 1)
        {
            lastGridPos = LevelGrid.Instance.GetGridPosition(pathList[pathList.Count - 2]);
        } else
        {
            lastGridPos = LevelGrid.Instance.GetGridPosition(pathList[pathList.Count - 1]);
        }

        // Moves one tile away from target, but not diagonally.
        if (lastGridPos.x != target.GetGridPosition().x && lastGridPos.z != target.GetGridPosition().z)
        {
            GridPosition newLastTile;
            GridPosition firstTile = LevelGrid.Instance.GetGridPosition(pathList[0]);

            if (firstTile.x < firstTile.z) newLastTile = new GridPosition(target.GetGridPosition().x, lastGridPos.z);
            else newLastTile = new GridPosition(lastGridPos.x, target.GetGridPosition().z);

            pathList.Insert(pathList.Count - 1, (LevelGrid.Instance.GetWorldPosition(newLastTile)));
            gridPositions.Insert(gridPositions.Count - 1, (LevelGrid.Instance.GetGridPosition(pathList[pathList.Count - 1])));
        }

        finalPosition = gridPositions[gridPositions.Count - 1];

        return (pathList, gridPositions, finalPosition);
    }

    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
    {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startPosition, GridPosition endPosition)
    {
        return FindPath(startPosition, endPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startPosition, GridPosition endPosition)
    {
        FindPath(startPosition, endPosition, out int pathLength);
        return pathLength;
    }
}
