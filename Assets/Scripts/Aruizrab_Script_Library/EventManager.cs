using System;
using System.Collections.Generic;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    private Dictionary<string, Action<Dictionary<string, object>>> _events;

    private void OnEnable()
    {
        _events ??= new Dictionary<string, Action<Dictionary<string, object>>>();
    }

    public static void StartListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (Instance._events.TryGetValue(eventName, out var @event))
        {
            @event += listener;
            Instance._events[eventName] = @event;
            return;
        }

        Instance._events.Add(eventName, listener);
    }

    public static void StopListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (!Instance._events.TryGetValue(eventName, out var @event)) return;
        @event -= listener;
        Instance._events[eventName] = @event;
    }

    public static void TriggerEvent(string eventName, Dictionary<string, object> message)
    {
        if (!Instance._events.TryGetValue(eventName, out var @event)) return;
        @event?.Invoke(message);
    }
}