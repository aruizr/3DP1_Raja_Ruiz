using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float sliderSmoothing = 0.25f;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    private void OnEnable()
    {
        EventManager.StartListening(EventData.Instance.onUpdatePlayerHealth, OnDisplayHealth);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onUpdatePlayerHealth, OnDisplayHealth);
    }

    private void OnDisplayHealth(Dictionary<string, object> message)
    {
        var health = (float) message["health"];

        DOTween.To(() => slider.value, value =>
        {
            slider.value = value;
            fill.color = gradient.Evaluate(value);
        }, health, sliderSmoothing);
    }
}