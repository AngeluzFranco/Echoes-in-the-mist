using System;
using UnityEngine;

public class TableLamp : MonoBehaviour
{
    [SerializeField] private GameObject lampLight;

    private void Awake()
    {
        EventManager.Subscribe(GlobalEvents.SwitchOn, SwitchLightOn);
        EventManager.Subscribe(GlobalEvents.SwitchOff, SwitchLightOff);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GlobalEvents.SwitchOn, SwitchLightOn);
        EventManager.Unsubscribe(GlobalEvents.SwitchOff, SwitchLightOff);
    }

    private void SwitchLightOn() => lampLight.SetActive(true);
    private void SwitchLightOff() => lampLight.SetActive(false);
}
