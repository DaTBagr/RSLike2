using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    private HealthSystem healthSystem;

    public GridPosition finalGridPosition;

    public virtual void Awake()
    {
        healthSystem = new HealthSystem();
        finalGridPosition = GetGridPosition(transform.position);
    }

    public virtual void TakeDamage(int damage) => healthSystem.TakeDamage(damage);
    public void Heal(int health) => healthSystem.Heal(health);
    public void SetHealth(int health) => healthSystem.SetMaxHealth(health);
    public int GetHealth() => healthSystem.GetHealth();
    public GridPosition GetGridPosition()
    {
        return LevelGrid.Instance.GetGridPosition(transform.position);
    }
    public GridPosition GetGridPosition(Vector3 position)
    {
        return LevelGrid.Instance.GetGridPosition(position);
    }

    public GridPosition GetPlayerGridPosition()
    {
        return LevelGrid.Instance.GetPlayerGridPosition();
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return LevelGrid.Instance.GetWorldPosition(gridPosition);
    }
}
