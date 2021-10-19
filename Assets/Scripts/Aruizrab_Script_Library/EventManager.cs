using System.Collections.Generic;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    public delegate void CustomEvent(params object[] args);

    private Dictionary<string, CustomEventWrapper> _events;

    private void OnEnable()
    {
        _events ??= new Dictionary<string, CustomEventWrapper>();
    }

    public static void StartListening(string eventName, CustomEvent listener)
    {
        if (Instance._events.TryGetValue(eventName, out var customEvent))
        {
            customEvent.AddListener(listener);
            return;
        }

        Instance._events.Add(eventName, new CustomEventWrapper(listener));
    }

    public static void StopListening(string eventName, CustomEvent listener)
    {
        if (!Instance._events.TryGetValue(eventName, out var customEvent)) return;
        customEvent.RemoveListener(listener);
    }

    public static void TriggerEvent(string eventName, params object[] args)
    {
        if (!Instance._events.TryGetValue(eventName, out var customEvent)) return;
        customEvent.Invoke(args);
    }

    private class CustomEventWrapper
    {
        private CustomEvent _customEvent;

        public CustomEventWrapper(CustomEvent customEvent)
        {
            _customEvent = customEvent;
        }

        public void AddListener(CustomEvent customEvent)
        {
            _customEvent += customEvent;
        }

        public void RemoveListener(CustomEvent customEvent)
        {
            _customEvent -= customEvent;
        }

        public void Invoke(params object[] args)
        {
            _customEvent?.Invoke(args);
        }
    }
}