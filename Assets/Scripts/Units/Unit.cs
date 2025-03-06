using System.Collections;
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

    [SerializeField] private Transform positionArcherHitEffect;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private string tagProjectile;

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
        LookAtTarget();
        animator.SetBool("Run", true);
        animator.SetBool("Attack", false);

        Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void Attack()
    {
        if (targetUnit == null || targetUnit.UnitHealth.HP <= 0 || isAttacking) return;
        isAttacking = true;

        LookAtTarget();
        if (animator != null  && UnitType == UnitTypeEnum.Archer) animator.SetTrigger("Start");
        if (animator != null) animator.SetTrigger("Attack");
    }
    
    public void OnAttackHit()
    {
        if (unitType == UnitTypeEnum.Archer)
        {
            ShootProjectile();
        }
        else if (unitType == UnitTypeEnum.Warrior)
        {
            DealDamage();
        }

        isAttacking = false;
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
        if (unitType != UnitTypeEnum.Warrior) return;
    
        if (targetUnit == null || targetUnit.UnitHealth.HP <= 0) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetUnit.transform.position);

        if (distanceToTarget <= attackRange)
        {
            targetUnit.UnitHealth.TakeDamage(ATK);
            Debug.Log("Dealt damage!");

            if (targetUnit.UnitHealth.HP <= 0)
            {
                targetUnit = null;
                isAttacking = false;
            }
        }
    }
    
    private void ShootProjectile()
    {
        if (targetUnit == null) return;

        Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject projectile = WeaponPool.Instance.SpawnProjectile(tagProjectile, positionArcherHitEffect.position, rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.targetUnit = targetUnit;
            projectileScript.damage = ATK;
        }
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