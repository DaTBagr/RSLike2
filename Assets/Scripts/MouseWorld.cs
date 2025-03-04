using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance { get; private set; }

    public event EventHandler<OnTargetChangedEventArgs> OnTargetChanged;
    public event EventHandler OnTargetRemoved;

    public class OnTargetChangedEventArgs : EventArgs
    {
        public Unit targetUnit;
        public OnTargetChangedEventArgs(Unit targetUnit)
        {
            this.targetUnit = targetUnit;
        }
    }

    [SerializeField] LayerMask npc;
    [SerializeField] LayerMask groundLayer;

    public Vector3 clickedPos { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"MouseWorld instance was not null");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        clickedPos = transform.position;
    }
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit npcHit, float.MaxValue, npc))
        {
            if (Input.GetMouseButton(0))
            {
                if (npcHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    OnTargetChanged?.Invoke(this, new OnTargetChangedEventArgs(unit));
                    return;
                }
            }
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit groundHit, float.MaxValue, groundLayer))
        {
            if (Input.GetMouseButton(0))
            {
                clickedPos = groundHit.point;
                OnTargetRemoved?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
    }
}
