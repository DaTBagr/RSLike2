public class EnemySkeleton : NPC
{
    public override void Awake()
    {
        base.Awake();

        hostile = true;
        moveSpeed = 3;
        attackCooldown = 3;
        attackDamage = 50;
        attackRange = 1;
        detectionRange = 3;
        name = "EnemySkeleton";
    }
    public override void Start()
    {
        base.Start();

        SetHealth(100);
    }
}
