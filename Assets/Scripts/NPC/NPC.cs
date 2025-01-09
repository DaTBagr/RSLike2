using UnityEngine;

public class NPC : MonoBehaviour
{
    private bool hostile;
    private int moveSpeed;
    private int attackRange;
    private int detectionRange;

    private HealthSystem healthSystem;

    private GridPosition gridPosition;

    private void Awake()
    {
        healthSystem = new HealthSystem();
    }

    public virtual void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetNPCAtGridObject(this, gridPosition);
    }

    public void SetSpeed(int speed)
    {
        moveSpeed = speed;
    }

    public int GetSpeed()
    {
        return moveSpeed;
    }

    public void SetAttackRange(int range)
    {
        attackRange = range;
    }

    public int GetAttackRange()
    {
        return attackRange;
    }

    public void SetDetectionRange(int range)
    {
        detectionRange = range;
    }

    public int GetDetectionRange()
    {
        return detectionRange;
    }

    public void SetHealth(int health)
    {
        healthSystem.SetMaxHealth(health);
    }

    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }

    public void Heal(int health)
    {
        healthSystem.Heal(health);
    }

    public void SetHostile(bool hostile)
    {
        this.hostile = hostile;
    }

    public bool GetHostile()
    {
        return hostile;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
}
