using System;
using UnityEngine;

public class CharacterInfo : Unit
{
    private HealthBar healthBar;

    [SerializeField] private bool isRunning;
    private float moveSpeed;
    private int maxHealth = 250;

    private NPC enemyTarget;

    public override void Awake()
    {
        base.Awake();
        name = "Player";
    }

    public void Start()
    {
        SetHealth(maxHealth);
        healthBar = HealthBar.instance;

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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        int currentHealth = GetHealth();
        healthBar.ChangeHealth(currentHealth, maxHealth);
    }
}
