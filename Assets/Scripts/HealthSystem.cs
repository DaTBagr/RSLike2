public class HealthSystem
{
    private int health;
    private int maxHealth;

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0) health = 0;
    }

    public void Heal(int heal)
    {
        health += heal;

        if (health > maxHealth) health = maxHealth;
    }
}
