using UnityEngine;

public class AutoDisable : ExtendedMonoBehaviour
{
    [SerializeField] private float delay;

    private void OnEnable()
    {
        Invoke(() => gameObject.SetActive(false)).AfterSeconds(delay).CancelOnDisable(true);
    }
}