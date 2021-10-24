using JetBrains.Annotations;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : ExtendedMonoBehaviour where T : Component
{
    [CanBeNull] private static T _instance;
    [NotNull] private static readonly object Lock = new object();

    public static bool IsApplicationQuitting { get; private set; }

    public static T Instance
    {
        get
        {
            if (IsApplicationQuitting)
                //Debug.LogWarning($"[SingletonMonoBehaviour<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                return null;

            lock (Lock)
            {
                if (_instance) return _instance;
                var instances = FindObjectsOfType<T>();
                var count = instances.Length;
                switch (count)
                {
                    case 0:
                        return _instance = new GameObject($"{typeof(T)}").AddComponent<T>();
                    case 1:
                        return _instance = instances[0];
                }

                Debug.LogWarning(
                    $"[SingletonMonoBehaviour<{typeof(T)}>] There should never be more than one Singleton of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                for (var i = 1; i < instances.Length; i++) Destroy(instances[i]);
                return _instance = instances[0];
            }
        }
    }

    private void OnApplicationQuit()
    {
        IsApplicationQuitting = true;
    }
}