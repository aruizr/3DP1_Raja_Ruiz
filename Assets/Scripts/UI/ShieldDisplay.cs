using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float sliderSmoothing = 0.25f;

    private void OnEnable()
    {
        EventManager.StartListening(EventData.instance.onUpdatePlayerShield, OnDisplayHealth);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.instance.onUpdatePlayerShield, OnDisplayHealth);
    }

    private void OnDisplayHealth(Dictionary<string, object> message)
    {
        var shield = (float) message["shield"];

        DOTween.To(() => slider.value, value => slider.value = value, shield, sliderSmoothing);
    }
}