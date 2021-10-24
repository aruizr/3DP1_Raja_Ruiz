using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private HealthSystem _currentDisplayTarget;
    private float _currentVelocity;

    private void Start()
    {
        slider.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!_currentDisplayTarget)
        {
            slider.gameObject.SetActive(false);
            return;
        }
        slider.value = _currentDisplayTarget.CurrentHealth / _currentDisplayTarget.MaxHealth;
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.Instance.onUpdateInteractionTarget, OnDisplayInteractionInfo);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onUpdateInteractionTarget, OnDisplayInteractionInfo);
    }

    private void OnDisplayInteractionInfo(Dictionary<string, object> message)
    {
        Debug.Log("New Target!");
        var target = (GameObject) message["target"];
        _currentDisplayTarget = target?.GetComponentInParent<HealthSystem>();
        _currentDisplayTarget ??= target?.GetComponentInChildren<HealthSystem>();

        slider.gameObject.SetActive(_currentDisplayTarget);

        if (!_currentDisplayTarget) return;

        Debug.Log("Enemy target received");

        slider.value = _currentDisplayTarget.CurrentHealth / _currentDisplayTarget.MaxHealth;
    }
}