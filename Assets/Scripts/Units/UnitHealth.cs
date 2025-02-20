using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] private int maxHP;
    public int HP { get; private set; }

    private Animator animator;
    private Unit unit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        unit = GetComponent<Unit>();
    }

    public void Initialize(int hp)
    {
        maxHP = hp;
        HP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (HP <= 0) return;

        HP -= damage;
        Debug.Log($"Unit took {damage} damage. Remaining HP: {HP}");

        if (unit.CompareTag("Enemy"))
        {
            CoinManager.Instance.AddCoin(damage);
        }
        if (HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (unit != null)
        {
            unit.OnUnitDied();
        }
    }

    public void ResetHealth()
    {
        HP = maxHP;
    }
}