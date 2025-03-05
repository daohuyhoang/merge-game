using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public Unit targetUnit;

    private void Update()
    {
        if (targetUnit == null || targetUnit.UnitHealth.HP <= 0)
        {
            ReturnToPool();
            return;
        }
        
        Vector3 direction = (targetUnit.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            Unit enemyUnit = other.GetComponent<Unit>();
            if (enemyUnit != null && enemyUnit == targetUnit)
            {
                HitTarget();
            }
        }
    }

    private void HitTarget()
    {
        if (targetUnit != null)
        {
            targetUnit.UnitHealth.TakeDamage(damage);
        }
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        WeaponPool.Instance.ReturnProjectileToPool(this);
    }
}