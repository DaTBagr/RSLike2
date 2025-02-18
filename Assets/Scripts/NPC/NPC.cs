public class NPC : Unit
{
    public bool hostile;
    public int detectionRange;
    public int moveSpeed;
    public int attackDamage;
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
        LevelGrid.Instance.SetNPCAtGridObject(this, GetGridPosition());

        if (hostile)
        {
            target = GameManager.instance.GetPlayer();
        }
    }

    public int GetSpeed() { return moveSpeed; }

    public int GetAttackDamage() { return attackDamage; }

    public float GetAttackCoolDown() { return attackCooldown; }

    public int GetAttackRange() { return attackRange; }

    public int GetDetectionRange() { return detectionRange; }

    public bool GetHostile() { return hostile; }
}
