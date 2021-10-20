using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : ExtendedMonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance) return _instance;

            _instance = FindObjectOfType<T>() ?? CreateInstance();

            return _instance;
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private static T CreateInstance()
    {
        var container = new GameObject(typeof(T).Name);
        return container.AddComponent<T>();
    }
}