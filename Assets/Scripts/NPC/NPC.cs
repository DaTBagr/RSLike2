public class NPC : Unit
{
    public EnemyStats stats;
    public bool hostile;
    public int detectionRange;
    public int moveSpeed;
    public float attackCooldown;
    public int attackRange;
    public bool attackReady;

    public Unit target;

    public override void Awake()
    {
        base.Awake();
    }

    public virtual void Start()
    {
        stats = GetComponent<EnemyStats>();
        LevelGrid.Instance.SetNPCAtGridObject(this, GetGridPosition());

        if (hostile)
        {
            target = GameManager.instance.GetPlayer();
        }
    }

    public int GetSpeed() { return moveSpeed; }

    //need to make damage calc for this
    public int GetAttackDamage() { return stats.attackLevel; }

    public float GetAttackCoolDown() { return attackCooldown; }

    public int GetAttackRange() { return attackRange; }

    public int GetDetectionRange() { return detectionRange; }

    public bool GetHostile() { return hostile; }
}
