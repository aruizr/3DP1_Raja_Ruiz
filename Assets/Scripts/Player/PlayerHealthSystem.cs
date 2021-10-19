using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IDamageTaker
{
    [SerializeField] private float shield;
    [SerializeField] private float health;

    [SerializeField] [ReadOnly] private float _currentHealth;
    [SerializeField] [ReadOnly] private float _currentShield;

    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = value;

            DisplayHealthInfo();

            if (_currentHealth > 0) return;

            _currentHealth = 0;
            Die();
        }
    }

    public float CurrentShield
    {
        get => _currentShield;
        private set
        {
            _currentShield = value;

            DisplayHealthInfo();
        }
    }

    private void Awake()
    {
        CurrentHealth = health;
        CurrentShield = shield;
    }

    private void Start()
    {
        DisplayHealthInfo();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.instance.onRestorePlayerHealth, RestoreHealth);
        EventManager.StartListening(EventData.instance.onRestorePlayerShield, RestoreShield);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.instance.onRestorePlayerHealth, RestoreHealth);
        EventManager.StopListening(EventData.instance.onRestorePlayerShield, RestoreShield);
    }

    public void TakeDamage(float amount)
    {
        if (CurrentShield <= 0)
        {
            CurrentHealth -= amount;
            return;
        }

        CurrentShield -= amount * 0.75f;
        CurrentHealth -= amount * 0.25f;

        if (CurrentShield >= 0) return;

        CurrentHealth -= -CurrentShield;
        CurrentShield = 0;
    }

    private void DisplayHealthInfo()
    {
        EventManager.TriggerEvent(
            EventData.instance.onDisplayHealthInfoEventName,
            CurrentHealth / health,
            CurrentShield / shield);
    }

    private void Die()
    {
        EventManager.TriggerEvent(EventData.instance.onPlayerDieEventName);
    }

    private void RestoreHealth(params object[] args)
    {
        CurrentHealth = health;
    }

    private void RestoreShield(params object[] args)
    {
        CurrentShield = shield;
    }
}