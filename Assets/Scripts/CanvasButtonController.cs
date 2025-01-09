using System;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButtonController : MonoBehaviour
{
    public static CanvasButtonController Instance { get; private set; }

    public event EventHandler OnRunButtonClicked;

    [SerializeField] Button runButton;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"CanvasButtonController instance was not null");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        runButton.onClick.AddListener(() => OnRunButtonClicked?.Invoke(this, EventArgs.Empty));
    }
}
