using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentBulletsText;
    [SerializeField] private TextMeshProUGUI bulletsLeftText;

    private void OnEnable()
    {
        EventManager.StartListening(EventData.Instance.onUpdateAmmo, OnDisplayAmmo);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onUpdateAmmo, OnDisplayAmmo);
    }

    private void OnDisplayAmmo(Dictionary<string, object> message)
    {
        var magazineRounds = message["magazineRounds"];
        var clipsRounds = message["clipsRounds"];

        currentBulletsText.text = magazineRounds.ToString();
        bulletsLeftText.text = clipsRounds.ToString();
    }
}