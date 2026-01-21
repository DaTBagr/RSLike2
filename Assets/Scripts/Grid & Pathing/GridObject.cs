using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridPosition gridPosition;
    private GridSystem<GridObject> gridSystem;
    private List<NPC> listNPC;
    private bool playerOnGridObject;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
        listNPC = new List<NPC>();
    }

    public GridPosition GetGridPosition()
    {
        return this.gridPosition;
    }

    public Vector3 GetWorldPositon()
    {
        return gridSystem.GetWorldPosition(gridPosition);
    }

    public void AddNPCToGrid(NPC npc)
    {
        listNPC.Add(npc);
    }
    public void RemoveNPCFromGrid(NPC npc)
    {
        listNPC.Remove(npc);
    }

    public void AddPlayerToGridObject()
    {
        playerOnGridObject = true;
    }
    public void RemovePlayerFromGridObject()
    {
        playerOnGridObject = false;
    }

    public bool PlayerOnGridObject()
    {
        return playerOnGridObject;
    }

    public List<NPC> GetNPCList()
    {
        return listNPC;
    }

    public void DebugNPCOnGrid()
    {
        Debug.Log(listNPC + " on " + gridPosition.ToString());
    }
}
