using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : HealthSystem
{
    [SerializeField] protected float shield;

    private float _currentShield;

    private float CurrentShield
    {
        get => _currentShield;
        set
        {
            _currentShield = value;
            OnUpdateShield();
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.Instance.onRestorePlayerHealth, OnRestoreHealth);
        EventManager.StartListening(EventData.Instance.onRestorePlayerShield, OnRestoreShield);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onRestorePlayerHealth, OnRestoreHealth);
        EventManager.StopListening(EventData.Instance.onRestorePlayerShield, OnRestoreShield);
    }

    protected override void OnInit()
    {
        OnRestoreShield(null);
    }

    public override void TakeDamage(float amount)
    {
        var currentShield = CurrentShield;
        var currentHealth = CurrentHealth;

        currentShield -= amount * 0.75f;
        currentHealth -= amount * 0.25f;

        if (currentShield < 0)
        {
            currentHealth -= -currentShield;
            currentShield = 0;
        }

        CurrentHealth = currentHealth;
        CurrentShield = currentShield;

        if (CurrentHealth > 0) OnTakeDamage();
        else OnDie();
    }

    protected override void OnUpdateHealth()
    {
        var healthPercent = CurrentHealth / MaxHealth;
        EventManager.TriggerEvent(EventData.Instance.onUpdatePlayerHealth, new Dictionary<string, object>
        {
            {"health", healthPercent}
        });
    }

    private void OnUpdateShield()
    {
        var shieldPercent = CurrentShield / shield;
        EventManager.TriggerEvent(EventData.Instance.onUpdatePlayerShield, new Dictionary<string, object>
        {
            {"shield", shieldPercent}
        });
    }

    private void OnRestoreHealth(Dictionary<string, object> message)
    {
        CurrentHealth = MaxHealth;
    }

    private void OnRestoreShield(Dictionary<string, object> message)
    {
        CurrentShield = shield;
    }

    protected override void OnTakeDamage()
    {
        EventManager.TriggerEvent(EventData.Instance.onPlayerTakeDamage, null);
    }

    protected override void OnDie()
    {
        EventManager.TriggerEvent(EventData.Instance.onPlayerDie, null);
    }
}