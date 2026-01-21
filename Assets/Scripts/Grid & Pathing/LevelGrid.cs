using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [SerializeField] Transform gridDebug;
    [SerializeField] int width = 20;
    [SerializeField] int height = 20;
    [SerializeField] float cellSize = 2;

    private GridSystem<GridObject> gridSystem;

    private GridPosition playerGridPosition;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"LevelGrid instance was not null");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
    }
    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
        CreateDebugObjects();
    }

    public GridPosition GetGridPosition(Vector3 worldPos)
    {
        return gridSystem.GetGridPosition(worldPos);
    }

    public Vector3 GetWorldPosition(GridPosition gridPos)
    {
        return gridSystem.GetWorldPosition(gridPos);
    }

    public GridObject GetGridObject(GridPosition gridPos)
    {
        return gridSystem.GetGridObject(gridPos);
    }

    public void SetNPCAtGridObject(NPC npc, GridPosition gridPos)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPos);
        gridObject.AddNPCToGrid(npc);
    }
    public void RemoveNPCFromGridObject(NPC npc, GridPosition gridPos)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPos);
        gridObject.RemoveNPCFromGrid(npc);
    }

    public void AddPlayerToGridObject(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddPlayerToGridObject();
        playerGridPosition = gridPosition;
    }

    public void RemovePlayerFromGridObject(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemovePlayerFromGridObject();
    }

    public bool GridObjectHasPlayer(GridPosition gridPos)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPos);
        return gridObject.PlayerOnGridObject();
    }

    public void GetNPCListAtPosition(GridPosition gridPos)
    {
        gridSystem.GetGridObject(gridPos).GetNPCList();
    }

    public GridPosition GetPlayerGridPosition()
    {
        return playerGridPosition;
    }

    public Vector3 GetPlayerWorldPosition()
    {
        return GetWorldPosition(playerGridPosition);
    }

    private void CreateDebugObjects()
    {
        GridObject[,] gridArray = gridSystem.GetGridObjectArray();
        foreach (GridObject gridObject in gridArray)
        {
            Instantiate(gridDebug, new Vector3(0, 0.05f, 0) + gridSystem.GetWorldPosition(gridObject.GetGridPosition()), Quaternion.identity);
        }
    }
}