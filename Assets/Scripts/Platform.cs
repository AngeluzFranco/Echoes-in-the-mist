using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private float height = 1f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private Vector3 _initialPosition;
    private Coroutine _animation;

    private void Awake()
    {
        if (!FindFirstObjectByType<EventManager>())
        {
            var go = new GameObject("EventManager");
            go.AddComponent<EventManager>();
        }

        if (!platform) platform = transform;
        try
        {
            _initialPosition = platform.position;
        }
        catch (MissingReferenceException)
        {
            platform = transform;
            _initialPosition = transform.position;
        }
        EventManager.Subscribe(GlobalEvents.SwitchOn, RaisePlatform);
        EventManager.Subscribe(GlobalEvents.SwitchOff, LowerPlatform);
    }
    
    private void OnDestroy()
    {
        EventManager.Unsubscribe(GlobalEvents.SwitchOn, RaisePlatform);
        EventManager.Unsubscribe(GlobalEvents.SwitchOff, LowerPlatform);
    }
    
    private void RaisePlatform()
    {
        var target = _initialPosition + Vector3.up * height;
        StartMove(target);
    }

    private void LowerPlatform()
    {
        var target = _initialPosition;
        StartMove(target);
    }

    private void StartMove(Vector3 target)
    {
        if (_animation != null) StopCoroutine(_animation);
        _animation = StartCoroutine(AnimatePosition(target));
    }

    private System.Collections.IEnumerator AnimatePosition(Vector3 target)
    {
        var start = platform.position;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(elapsed / duration);
            var k = ease != null ? ease.Evaluate(t) : t;
            platform.position = Vector3.Lerp(start, target, k);
            yield return null;
        }
        platform.position = target;
        _animation = null;
    }
}
