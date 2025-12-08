using UnityEngine;

// Máquina de estados para la trampa de púas
public class SpikeStateMachine : MonoBehaviour, IStateMachine
{
    [Header("Configuración")]
    public GameObject spikesObject;
    public float waitDuration = 3f;
    public float moveSpeed = 2f;
    public float verticalOffset = 0.5f;

    public Vector3 ExposedPosition { get; private set; }
    public Vector3 HiddenPosition { get; private set; }
    public IState CurrentState { get; set; }

    private void Awake()
    {
        if (spikesObject == null) return;

        ExposedPosition = spikesObject.transform.localPosition;
        HiddenPosition = ExposedPosition - new Vector3(0, verticalOffset, 0);

        spikesObject.transform.localPosition = HiddenPosition;
    }

    private void Start()
    {
        ChangeState(new SpikeWaitHiddenState(this));
    }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    private void Update()
    {
        CurrentState?.Tick(Time.deltaTime);
    }

    public bool MoveTowardsTarget(Vector3 targetPosition, float deltaTime)
    {
        if (spikesObject == null) return true;

        spikesObject.transform.localPosition = Vector3.MoveTowards(
            spikesObject.transform.localPosition,
            targetPosition,
            moveSpeed * deltaTime
        );

        return Vector3.Distance(spikesObject.transform.localPosition, targetPosition) < 0.001f;
    }
}

public struct SpikeWaitHiddenState : IState
{
    public SpikeStateMachine StateMachine { get; set; }
    private float timer;

    public SpikeWaitHiddenState(SpikeStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        timer = 0f;
    }

    public void Enter() => timer = 0f;

    public void Tick(float deltaTime)
    {
        timer += deltaTime;

        if (timer >= StateMachine.waitDuration)
            StateMachine.ChangeState(new SpikeMovingUpState(StateMachine));
    }

    public void Exit() { }
}

public struct SpikeMovingUpState : IState
{
    public SpikeStateMachine StateMachine { get; set; }

    public SpikeMovingUpState(SpikeStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public void Enter() { }

    public void Tick(float deltaTime)
    {
        if (StateMachine.MoveTowardsTarget(StateMachine.ExposedPosition, deltaTime))
            StateMachine.ChangeState(new SpikeWaitExposedState(StateMachine));
    }

    public void Exit() { }
}

public struct SpikeWaitExposedState : IState
{
    public SpikeStateMachine StateMachine { get; set; }
    private float timer;

    public SpikeWaitExposedState(SpikeStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        timer = 0f;
    }

    public void Enter() => timer = 0f;

    public void Tick(float deltaTime)
    {
        timer += deltaTime;

        if (timer >= StateMachine.waitDuration)
            StateMachine.ChangeState(new SpikeMovingDownState(StateMachine));
    }

    public void Exit() { }
}

public struct SpikeMovingDownState : IState
{
    public SpikeStateMachine StateMachine { get; set; }

    public SpikeMovingDownState(SpikeStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public void Enter() { }

    public void Tick(float deltaTime)
    {
        if (StateMachine.MoveTowardsTarget(StateMachine.HiddenPosition, deltaTime))
            StateMachine.ChangeState(new SpikeWaitHiddenState(StateMachine));
    }

    public void Exit() { }
}
