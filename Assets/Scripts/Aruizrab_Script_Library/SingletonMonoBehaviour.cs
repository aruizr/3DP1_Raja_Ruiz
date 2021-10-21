using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : ExtendedMonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance) return _instance;

            _instance = FindObjectOfType<T>();

            if (!_instance) Debug.LogError("Singleton<" + typeof(T) + "> instance not found.");

            return _instance;
        }
    }
}