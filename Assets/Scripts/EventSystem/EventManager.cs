using System;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-1000)]
public class EventManager : MonoBehaviour
{
    private static readonly Dictionary<GlobalEvents, List<Delegate>> Events = new();

    private void Awake()
    {
        foreach (var eventName in Enum.GetValues(typeof(GlobalEvents)))
        {
            Events.Add((GlobalEvents)eventName, new List<Delegate>());
        }
    }

    public static void Subscribe<T>(GlobalEvents globalEvent, Action<T> method)
    {
        Events[globalEvent].Add(method);
    }

    public static void Subscribe(GlobalEvents globalEvent, Action method)
    {
        Events[globalEvent].Add(method);
    }

    public static void Unsubscribe<T>(GlobalEvents globalEvent,
        Action<T> method)
    {
        Events[globalEvent].Remove(method);
    }

    public static void Unsubscribe(GlobalEvents globalEvent, Action method)
    {
        Events[globalEvent].Remove(method);
    }

    public static void Invoke<T>(GlobalEvents globalEvent, T value = default)
    {
        foreach (var @delegate in Events[globalEvent])
        {
            switch (@delegate)
            {
                case Action<T> action:
                    action.Invoke(value);
                    break;
            }
        }
    }

    public static void Invoke(GlobalEvents globalEvent)
    {
        foreach (var @delegate in Events[globalEvent]) if (@delegate is Action action) action.Invoke();
    }
}