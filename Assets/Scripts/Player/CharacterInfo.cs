using System;
using UnityEngine;

public class CharacterInfo : Unit
{
    private HealthBar healthBar;
    private CharacterAnimations cAnimations;
    private CharacterAttack cAttack;

    [SerializeField] bool isRunning;

    // From movement script
    public bool readyToAttack;
    // From attack script
    public bool isAttacking;

    private float moveSpeed;
    private int maxHealth = 250;

    [SerializeField] private Unit targetUnit;

    public override void Awake()
    {
        base.Awake();
        name = "Player";
    }

    public void Start()
    {
        SetHealth(maxHealth);
        healthBar = HealthBar.instance;

        cAnimations = GetComponentInChildren<CharacterAnimations>();
        cAttack = GetComponentInChildren<CharacterAttack>();

        CanvasButtonController.Instance.OnRunButtonClicked += ToggleRun;
        MouseWorld.Instance.OnTargetChanged += ChangeTarget;
        MouseWorld.Instance.OnTargetRemoved += RemoveTarget;
    }

    private void Update()
    {
        if (readyToAttack && !isAttacking)
        {
            cAttack.AttackTarget();
        }
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

    private void ChangeTarget(object sender, MouseWorld.OnTargetChangedEventArgs e)
    {
        targetUnit = e.targetUnit;
    }
    private void RemoveTarget(object sender, EventArgs e)
    {
        targetUnit = null;
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        cAnimations.TakeDamageAnimation();

        int currentHealth = GetHealth();
        healthBar.ChangeHealth(currentHealth, maxHealth);
    }
}
