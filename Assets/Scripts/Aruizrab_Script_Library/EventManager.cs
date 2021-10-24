using System;
using System.Collections.Generic;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    private Dictionary<string, Action<Dictionary<string, object>>> _events;

    private Dictionary<string, Action<Dictionary<string, object>>> Events => _events ??= new Dictionary<string, Action<Dictionary<string, object>>>();

    private void Awake()
    {
        _events = new Dictionary<string, Action<Dictionary<string, object>>>();
    }

    public static void StartListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (!Instance) return;
        if (Instance.Events.TryGetValue(eventName, out var @event))
        {
            @event += listener;
            Instance.Events[eventName] = @event;
            return;
        }

        Instance.Events.Add(eventName, listener);
    }

    public static void StopListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (!Instance) return;
        if (!Instance.Events.TryGetValue(eventName, out var @event)) return;
        @event -= listener;
        Instance.Events[eventName] = @event;
    }

    public static void TriggerEvent(string eventName, Dictionary<string, object> message)
    {
        if (!Instance) return;
        if (!Instance.Events.TryGetValue(eventName, out var @event)) return;
        @event?.Invoke(message);
    }
}