using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Vector3 moveDirection = Vector3.up;
    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float waitTimeOnTop = 2f;
    [SerializeField] private float moveSpeed = 0.5f;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private bool _isMoving = false;
    private bool _isAtTop = false;
    private float _moveProgress = 0f;

    private void OnEnable()
    {
        EventManager.Subscribe(GlobalEvents.SwitchOn, MoveUp);
        EventManager.Subscribe(GlobalEvents.SwitchOff, MoveDown);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GlobalEvents.SwitchOn, MoveUp);
        EventManager.Unsubscribe(GlobalEvents.SwitchOff, MoveDown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Invoke(GlobalEvents.SwitchOn);
            other.transform.SetParent(platform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    private void Start()
    {
        _startPosition = platform.position;
        _targetPosition = _startPosition + moveDirection.normalized * moveDistance;
    }

    private void Update()
    {
        if (_isMoving)
        {
            _moveProgress += Time.deltaTime * moveSpeed;
            if (!_isAtTop)
            {
                platform.position = Vector3.Lerp(_startPosition, _targetPosition, _moveProgress);
                if (_moveProgress >= 1f)
                {
                    platform.position = _targetPosition;
                    _isMoving = false;
                    _isAtTop = true;
                    _moveProgress = 0f;
                    StartCoroutine(WaitOnTopAndReturn());
                }
            }
            else
            {
                platform.position = Vector3.Lerp(_targetPosition, _startPosition, _moveProgress);
                if (_moveProgress >= 1f)
                {
                    platform.position = _startPosition;
                    _isMoving = false;
                    _isAtTop = false;
                    _moveProgress = 0f;
                }
            }
        }
    }

    private System.Collections.IEnumerator WaitOnTopAndReturn()
    {
        yield return new WaitForSeconds(waitTimeOnTop);
        EventManager.Invoke(GlobalEvents.SwitchOff);
    }

    private void MoveUp() 
    { 
        if (!_isMoving && !_isAtTop) 
        { 
            _isMoving = true; 
            _moveProgress = 0f; 
        } 
    }

    private void MoveDown() 
    { 
        if (!_isMoving && _isAtTop) 
        { 
            _isMoving = true; 
            _moveProgress = 0f; 
        } 
    }
}