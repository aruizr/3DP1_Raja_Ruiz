using UnityEngine;

public class Test : ExtendedMonoBehaviour
{
    private bool _condition;

    private void Start()
    {
        _condition = true;
        Invoke(Scream).EverySeconds(0.25f).While(() => _condition);
        Invoke(() => _condition = false).AfterSeconds(1);
    }

    private void Scream()
    {
        Debug.Log("A");
    }
}