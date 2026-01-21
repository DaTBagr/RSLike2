using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public GameObject hitSplat;

    private HealthSystem healthSystem;

    public GridPosition finalGridPosition;

    public virtual void Awake()
    {
        healthSystem = new HealthSystem();
        finalGridPosition = GetGridPosition(transform.position);
    }

    public virtual void TakeDamage(int damage) 
    {
        healthSystem.TakeDamage(damage);

        if (hitSplat != null)
        {
            ShowHitSplat(damage);
        }

        if (healthSystem.GetHealth() <= 0)
        {
            Destroy(gameObject);
        }
    } 

    private void ShowHitSplat(int damage)
    {
        Vector3 posOffset = new Vector3(0, 4, 0);

        var splat = Instantiate(hitSplat, transform.position, Quaternion.identity, transform);
        var splatText = splat.GetComponent<TextMeshPro>();

        splat.transform.position += posOffset;

        splatText.text = damage.ToString();

        if (damage > 0) splatText.color = Color.darkRed;
        else splatText.color = Color.darkBlue;
    }

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

    public bool CheckIfUnitIsMoving()
    {
        if (GetGridPosition() != finalGridPosition)
        {
            return true;
        }
        return false;
    }
}
