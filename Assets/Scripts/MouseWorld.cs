using System;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance { get; private set; }

    public event EventHandler<OnTargetNPCChangedEventArgs> OnTargetNPCChanged;

    public class OnTargetNPCChangedEventArgs : EventArgs
    {
        public NPC targetNPC;
        public OnTargetNPCChangedEventArgs(NPC targetNPC)
        {
            this.targetNPC = targetNPC;
        }
    }

    [SerializeField] LayerMask npc;
    [SerializeField] LayerMask groundLayer;

    private Vector3 mousePos;

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
        mousePos = transform.position;
    }
    private void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit npcHit, float.MaxValue, npc))
        {
            if (Input.GetMouseButton(0))
            {
                if (npcHit.transform.TryGetComponent<NPC>(out NPC npc))
                {
                    OnTargetNPCChanged?.Invoke(this, new OnTargetNPCChangedEventArgs(npc));
                }
            }
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit groundHit, float.MaxValue, groundLayer))
        {
            mousePos = groundHit.point;
        }
    }

    public Vector3 GetMousePos()
    {
        return mousePos;
    }
}
