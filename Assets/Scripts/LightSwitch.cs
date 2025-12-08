using System;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] private Transform switchModel;
    private bool _isSwitchOn;

    private void OnMouseDown()
    {
        Switch();
        var status = _isSwitchOn ? GlobalEvents.SwitchOn : GlobalEvents.SwitchOff;
        EventManager.Invoke(status);
        
        // Invocar eventos de la puerta
        var doorStatus = _isSwitchOn ? GlobalEvents.DoorOpen : GlobalEvents.DoorClose;
        EventManager.Invoke(doorStatus);
    }

    private void Switch()
    {
        _isSwitchOn = !_isSwitchOn;
        var rotation = _isSwitchOn ? Quaternion.Euler(-15f, 0f, 0f) : Quaternion.identity;
        switchModel.rotation = rotation;
    }
}
