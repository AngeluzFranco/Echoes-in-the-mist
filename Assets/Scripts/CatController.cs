using UnityEngine;

public class CatController : MonoBehaviour
{
    [SerializeField] private Transform catModel;
    private bool _isCatRotating;

    private void Awake()
    {
        EventManager.Subscribe(GlobalEvents.DoorOpen, RotateCatOn);
        EventManager.Subscribe(GlobalEvents.DoorClose, RotateCatOff);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GlobalEvents.DoorOpen, RotateCatOn);
        EventManager.Unsubscribe(GlobalEvents.DoorClose, RotateCatOff);
    }

    private void Update()
    {
        if (_isCatRotating)
            catModel.Rotate(0f, 360f * Time.deltaTime, 0f);
    }

    private void RotateCatOn() => _isCatRotating = true;
    private void RotateCatOff() => _isCatRotating = false;
}
