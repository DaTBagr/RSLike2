using System;
using UnityEngine;
using static MouseWorld;

public class CharacterInfo : MonoBehaviour
{
    private HealthSystem healthSystem;

    [SerializeField] private bool isRunning;
    private float moveSpeed;
    private int health = 250;

    private NPC enemyTarget;

    private void Start()
    {
        healthSystem = new HealthSystem();
        healthSystem.SetMaxHealth(health);

        CanvasButtonController.Instance.OnRunButtonClicked += ToggleRun;
    }

    public void ToggleRun(object sender, EventArgs e)
    {
        isRunning = !isRunning;
    }

    public bool GetIsRunning()
    {
        return isRunning;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public NPC GetCurrentEnemy()
    {
        return enemyTarget;
    }
}
