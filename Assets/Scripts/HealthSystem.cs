using UnityEngine;

public abstract class HealthSystem : ExtendedMonoBehaviour, IDamageTaker
{
    [SerializeField] protected float health;

    private float _currentHealth;

    public float CurrentHealth
    {
        get => _currentHealth;
        protected set
        {
            _currentHealth = value < 0 ? 0 : value;
            OnUpdateHealth();
        }
    }

    public float MaxHealth => health;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
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