using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitTypeEnum
    {
        Warrior,
        Archer
    }

    [SerializeField] private UnitTypeEnum unitType = UnitTypeEnum.Warrior;

    public string UnitType => unitType.ToString();

    public int UnitLevel { get; set; } = 1;
    public int HP { get; private set; }
    public int ATK { get; private set; }
    public Tile CurrentTile { get; set; }

    [SerializeField] private UnitData unitData;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 3f;

    private Unit targetUnit;
    private bool isAttacking = false;
    private Animator animator;

    private void Start()
    {
        UpdateStats();
        DisplayUnitInfo();
        CurrentTile = FindNearestTile();
        if (CurrentTile != null) CurrentTile.CanSpawn = false;
        
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (targetUnit == null)
        {
            FindTarget();
        }
        else
        {
            if (unitType == UnitTypeEnum.Warrior)
            {
                MoveTowardsTarget();
            }
            else if (unitType == UnitTypeEnum.Archer)
            {
                if (Vector3.Distance(transform.position, targetUnit.transform.position) <= attackRange)
                {
                    Attack();
                }
            }
        }
    }

    public void UpdateStats()
    {
        if (unitData != null && UnitLevel <= unitData.maxLevel)
        {
            HP = unitData.hpByLevel[UnitLevel - 1];
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
        Debug.Log($"Unit: {unitData.unitType}, Level: {UnitLevel}, HP: {HP}, ATK: {ATK}, Tag: {gameObject.tag}");
    }

    public void FindTarget()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        float shortestDistance = Mathf.Infinity;
        Unit nearestUnit = null;

        foreach (Unit unit in allUnits)
        {
            if (unit == this || unit.CompareTag(gameObject.tag)) continue;

            float distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestUnit = unit;
            }
        }

        targetUnit = nearestUnit;
    }

    public void MoveTowardsTarget()
    {
        if (targetUnit == null) return;
        
        if (animator != null) animator.SetBool("Run", true);

        Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetUnit.transform.position) <= attackRange)
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (targetUnit == null || isAttacking) return;

        isAttacking = true;
        
        if (animator != null) animator.SetTrigger("Attack");
        
        Invoke("DealDamage", 1f);
    }

    private void DealDamage()
    {
        if (targetUnit != null)
        {
            targetUnit.TakeDamage(ATK);
        }
        isAttacking = false;
        
        if (animator != null) animator.SetBool("Run", false);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Hp: {HP}");
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (CurrentTile != null)
        {
            CurrentTile.SetUnit(null);
            CurrentTile.CanSpawn = true;
        }
        
        if (animator != null) animator.SetTrigger("Die");
        // Destroy(gameObject);
    }
}