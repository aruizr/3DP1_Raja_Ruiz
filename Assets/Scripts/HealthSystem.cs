using UnityEngine;

public abstract class HealthSystem : MonoBehaviour, IDamageTaker
{
    [SerializeField] protected float health;

    private float _currentHealth;

    protected float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value < 0 ? 0 : value;
            OnUpdateHealth();
        }
    }

    private void Awake()
    {
        CurrentHealth = health;
        OnInit();
    }

    public virtual void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth > 0) OnTakeDamage();
        else OnDie();
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnTakeDamage()
    {
    }

    protected virtual void OnDie()
    {
    }

    protected virtual void OnUpdateHealth()
    {
    }
}