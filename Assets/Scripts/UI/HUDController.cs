using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentBulletsText;
    [SerializeField] private TextMeshProUGUI bulletsLeftText;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient healthGradient;
    [SerializeField] private Image healthFill;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private float sliderSmoothing = 0.25f;

    private void OnEnable()
    {
        EventManager.StartListening(EventData.instance.onDisplayAmmoInfoEventName, OnDisplayAmmoInfo);
        EventManager.StartListening(EventData.instance.onDisplayHealthInfoEventName, OnDisplayHealthInfo);
        EventManager.StartListening(EventData.instance.onDisplayInteractionInfo, OnDisplayInteractionInfo);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.instance.onDisplayAmmoInfoEventName, OnDisplayAmmoInfo);
        EventManager.StopListening(EventData.instance.onDisplayHealthInfoEventName, OnDisplayHealthInfo);
        EventManager.StopListening(EventData.instance.onDisplayInteractionInfo, OnDisplayInteractionInfo);
    }

    private void OnDisplayInteractionInfo(object[] args)
    {
        var obj = (GameObject) args[0];
        var itemName = obj?.GetComponent<IInteractiveItem>()?.GetName();
        interactionText.text = itemName != null ? "Press <style=\"C1\">E</style> to pick up " + itemName : "";
    }

    private void OnDisplayHealthInfo(object[] args)
    {
        var health = (float) args[0];
        var shield = (float) args[1];

        DOTween.To(() => healthSlider.value, SetHealthSlider, health, sliderSmoothing);
        DOTween.To(() => shieldSlider.value, value => shieldSlider.value = value, shield, sliderSmoothing);
    }

    private void OnDisplayAmmoInfo(object[] args)
    {
        var currentBullets = (int) args[0];
        var bulletsLeft = (int) args[1];

        currentBulletsText.text = currentBullets.ToString();
        bulletsLeftText.text = bulletsLeft.ToString();
    }

    private void SetHealthSlider(float value)
    {
        healthSlider.value = value;
        healthFill.color = healthGradient.Evaluate(value);
    }
}