using System.Collections.Generic;
using UnityEngine;

public class FindTilePath
{
    List<GridPosition> gridPositions;
    List<Vector3> pathList;
    public (List<Vector3> pathList, List<GridPosition> gridPositions) FindGroundTilePath(GridPosition targetPosition, Unit thisChar)
    {
        gridPositions = Pathfinding.Instance.FindPath(thisChar.GetGridPosition(), targetPosition, out int pathLength);
        pathList = new List<Vector3>();

        foreach (GridPosition gridPosition in gridPositions)
        {
            pathList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        return (pathList, gridPositions);
    }

    public (List<Vector3> pathList, List<GridPosition> gridPositions) FindTargetTilePath(Unit target, Unit thisChar)
    {
        gridPositions = Pathfinding.Instance.FindPath(thisChar.GetGridPosition(), target.GetGridPosition(), out int pathLength);
        pathList = new List<Vector3>();

        foreach (GridPosition gridPosition in gridPositions)
        {
            pathList.Add(LevelGrid.Instance.GetWorldPosition(gridPosition));
        }

        GridPosition lastGridPos = LevelGrid.Instance.GetGridPosition(pathList[pathList.Count - 2]);

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

        return (pathList, gridPositions);
    }
}
