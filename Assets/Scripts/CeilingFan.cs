using UnityEngine;

public class CeilingFan : MonoBehaviour
{
    [SerializeField] private Transform blades;
    [SerializeField] private float rotationSpeed = 720f; // Dos vueltas por segundo
    private float _currentRotationSpeed;
    private bool _isFanOn;

  


    private void OnEnable()
    {
        EventManager.Subscribe(GlobalEvents.SwitchOn, SwitchFanOn);
        EventManager.Subscribe(GlobalEvents.SwitchOff, SwitchFanOff);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GlobalEvents.SwitchOn, SwitchFanOn);
        EventManager.Unsubscribe(GlobalEvents.SwitchOff, SwitchFanOff);
    }

    private void Update()
    {
        var targetValue = _isFanOn ? rotationSpeed : 0f;
        _currentRotationSpeed = Mathf.Lerp(_currentRotationSpeed, targetValue, Time.deltaTime);
        blades.Rotate(Vector3.up, _currentRotationSpeed * Time.deltaTime);
    }

    private void SwitchFanOn() => _isFanOn = true;
    private void SwitchFanOff() => _isFanOn = false;
}
