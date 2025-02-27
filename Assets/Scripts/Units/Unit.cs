using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitTypeEnum
    {
        Warrior,
        Archer
    }

    [SerializeField] private UnitTypeEnum unitType = UnitTypeEnum.Warrior;

    public UnitTypeEnum UnitType => unitType;

    [SerializeField] private int unitLevel;
    public int UnitLevel 
    {
        get { return unitLevel; }
        set { unitLevel = value; }
    }

    public UnitHealth UnitHealth { get; private set; }
    public int ATK { get; private set; }
    public Tile CurrentTile { get; set; }

    [SerializeField] private UnitData unitData;
    public UnitData UnitData => unitData;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3f;

    private Unit targetUnit;
    private bool isAttacking = false;
    private Animator animator;
    private CheckVictory checkVictory;
    private int totalDamageDealt;

    [SerializeField] private GameObject warriorAttackEffectPrefab;
    [SerializeField] private GameObject archerAttackEffectPrefab;
    [SerializeField] private Transform positionArcherHitEffect;
    [SerializeField] private Transform positionWarriorHitEffect;

    private void Awake()
    {
        UnitHealth = GetComponent<UnitHealth>();
        checkVictory = GetComponent<CheckVictory>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateStats();
        CurrentTile = FindNearestTile();
        if (CurrentTile != null) CurrentTile.CanSpawn = false;

        if (gameObject.CompareTag("Enemy"))
        {
            EnemyManager.Instance.RegisterEnemy(this);
        }
    }

    private void Update()
    {
        if (BattleManager.Instance != null && BattleManager.Instance.IsBattleActive())
        {
            FindTarget();
            if (unitType == UnitTypeEnum.Warrior)
            {
                MoveTowardsTarget();
            }
            else if (unitType == UnitTypeEnum.Archer)
            {
                if (targetUnit != null && Vector3.Distance(transform.position, targetUnit.transform.position) <= attackRange)
                {
                    Attack();
                }
            }
            checkVictory.CheckForPlayerVictory();
            checkVictory.CheckForEnemyVictory();
        }
    }

    public void UpdateStats()
    {
        if (unitData != null && UnitLevel <= unitData.maxLevel)
        {
            UnitHealth.Initialize(unitData.hpByLevel[UnitLevel - 1]);
            ATK = unitData.atkByLevel[UnitLevel - 1];
        }
    }

    public Tile FindNearestTile()
    {
        Tile[] allTiles = FindObjectsOfType<Tile>();
        Tile nearestTile = null;
        float minDistance = Mathf.Infinity;

        foreach (Tile tile in allTiles)
        {
            float distance = Vector3.Distance(transform.position, tile.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTile = tile;
            }
        }
        return nearestTile;
    }

    public void FindTarget()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        float shortestDistance = Mathf.Infinity;
        Unit nearestUnit = null;

        foreach (Unit unit in allUnits)
        {
            if (unit == this || unit.CompareTag(gameObject.tag) || unit.UnitHealth.HP <= 0) continue;
            float distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestUnit = unit;
            }
        }
        
        if (targetUnit != null && targetUnit.UnitHealth.HP <= 0) targetUnit = null;

        if (nearestUnit != null && (targetUnit == null || shortestDistance < Vector3.Distance(transform.position, targetUnit.transform.position)))
        {
            targetUnit = nearestUnit;
        }
    }

    public void MoveTowardsTarget()
    {
        if (targetUnit == null || isAttacking) return;
        float distanceToTarget = Vector3.Distance(transform.position, targetUnit.transform.position);

        if (distanceToTarget <= attackRange)
        {
            LookAtTarget();
            animator.SetBool("Run", false);
            animator.SetBool("Attack", true);
            Attack();
            return;
        }

        animator.SetBool("Run", true);
        animator.SetBool("Attack", false);
        
        LookAtTarget();

        Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void Attack()
    {
        if (targetUnit == null || targetUnit.UnitHealth.HP <= 0 || isAttacking) return;
        isAttacking = true;
        
        LookAtTarget();
        
        if (animator != null) animator.SetTrigger("Attack");
        
        // Invoke("PlayAttackEffect", 1.25f);
        PlayAttackEffect();
        Invoke("DealDamage", 1.25f);
    }

    private void PlayAttackEffect()
    {
        GameObject attackEffectPrefab = null;

        switch (unitType)
        {
            case UnitTypeEnum.Warrior:
                attackEffectPrefab = warriorAttackEffectPrefab;
                if (attackEffectPrefab != null && targetUnit != null)
                {
                    GameObject effect = Instantiate(attackEffectPrefab, positionWarriorHitEffect.transform.position, Quaternion.Euler(0, 180, 0));
                    Destroy(effect, 1f);
                }
                break;

            case UnitTypeEnum.Archer:
                attackEffectPrefab = archerAttackEffectPrefab;
                if (attackEffectPrefab != null && targetUnit != null)
                {
                    GameObject effectAtTarget = Instantiate(attackEffectPrefab, targetUnit.transform.position, Quaternion.identity);
                    Destroy(effectAtTarget, 1f);

                    GameObject effectAtArcher = Instantiate(attackEffectPrefab, positionArcherHitEffect.position, Quaternion.identity);
                    Destroy(effectAtArcher, 1f);
                }
                break;
        }
    }
    
    private void LookAtTarget()
    {
        if (targetUnit != null)
        {
            Vector3 direction = targetUnit.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void DealDamage()
    {
        if (targetUnit == null || targetUnit.UnitHealth.HP <= 0) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetUnit.transform.position);

        if (distanceToTarget <= attackRange)
        {
            targetUnit.UnitHealth.TakeDamage(ATK);
        }
        isAttacking = false;
    }

    public void OnUnitDied()
    {
        if (CurrentTile != null)
        {
            CurrentTile.SetUnit(null);
            CurrentTile.CanSpawn = true;
        }

        if (gameObject.CompareTag("Enemy"))
        {
            EnemyManager.Instance.UnregisterEnemy(this);
        }
        
        ObjectPool.Instance.ReturnToPool(UnitType, gameObject);
        this.enabled = false;
    }
    
    public void VictoryAnimation()
    {
        if (animator != null) animator.SetTrigger("Victory");
    }
}