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
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3f;

    private Unit targetUnit;
    private bool isAttacking = false;
    private Animator animator;

    private void Awake()
    {
        UnitHealth = GetComponent<UnitHealth>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateStats();
        DisplayUnitInfo();
        CurrentTile = FindNearestTile();
        if (CurrentTile != null) CurrentTile.CanSpawn = false;
    }

    private void CheckForVictory()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        bool hasEnemy = false;

        foreach (Unit unit in allUnits)
        {
            if (!unit.CompareTag(gameObject.tag) && unit.UnitHealth.HP > 0)
            {
                hasEnemy = true;
                break;
            }
        }

        if (!hasEnemy)
        {
            VictoryAnimation();
            UIManager.Instance.ShowVictoryPanel();
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

            CheckForVictory();
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

    public void DisplayUnitInfo()
    {
        Debug.Log($"Unit: {unitData.unitType}, Level: {UnitLevel}, HP: {UnitHealth.HP}, ATK: {ATK}, Tag: {gameObject.tag}");
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
            animator.SetBool("Run", false);
            animator.SetBool("Attack", true);
            LookAtTarget();
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
        
        Invoke("DealDamage", 1f);
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
        if (targetUnit != null && targetUnit.UnitHealth.HP > 0)
        {
            targetUnit.UnitHealth.TakeDamage(ATK);
        }
        else
        {
            targetUnit = null;
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
        
        ObjectPool.Instance.ReturnToPool(UnitType, gameObject);
        this.enabled = false;
    }
    
    public void VictoryAnimation()
    {
        if (animator != null) animator.SetTrigger("Victory");
    }

    public void ResetUnit()
    {
        UnitHealth.ResetHealth();
        ATK = unitData.atkByLevel[UnitLevel - 1];
        isAttacking = false;
        targetUnit = null;
        animator.SetTrigger("Idle");
    }
}