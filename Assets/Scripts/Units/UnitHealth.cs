using UnityEngine;
using System;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] private int maxHP;
    public int HP { get; private set; }
    public int MaxHP => maxHP;

    public event Action OnHealthChanged;

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
        OnHealthChanged?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        if (HP <= 0) return;

        HP -= damage;
        Debug.Log($"Unit took {damage} damage. Remaining HP: {HP}");

        OnHealthChanged?.Invoke();

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
        OnHealthChanged?.Invoke();
    }
}