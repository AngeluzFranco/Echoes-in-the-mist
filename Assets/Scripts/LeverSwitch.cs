using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    [SerializeField] private Transform switchModel;
    [SerializeField] private float angle = 60f;
    [SerializeField] private Vector3 localAxis = Vector3.right;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private bool _isSwitchOn;
    private Quaternion _initialLocalRotation;
    private Coroutine _animation;

    private void Awake()
    {
        _initialLocalRotation = switchModel ? switchModel.localRotation : Quaternion.identity;
    }

    private void OnMouseDown()
    {
        Switch();
        var status = _isSwitchOn ? GlobalEvents.SwitchOn : GlobalEvents.SwitchOff;
        EventManager.Invoke(status);
    }

    private void Switch()
    {
        _isSwitchOn = !_isSwitchOn;
        var target = _initialLocalRotation * (_isSwitchOn ? Quaternion.AngleAxis(angle, localAxis.normalized) : Quaternion.identity);
        if (_animation != null) StopCoroutine(_animation);
        _animation = StartCoroutine(AnimateRotation(target));
    }

    private System.Collections.IEnumerator AnimateRotation(Quaternion target)
    {
        var start = switchModel.localRotation;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(elapsed / duration);
            var k = ease != null ? ease.Evaluate(t) : t;
            switchModel.localRotation = Quaternion.Slerp(start, target, k);
            yield return null;
        }
        switchModel.localRotation = target;
        _animation = null;
    }
}
