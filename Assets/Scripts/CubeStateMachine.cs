using System;
using UnityEngine;

public class CubeStateMachine : MonoBehaviour, IStateMachine
{
    public IState CurrentState { get; set; }

    public IdleState IdleState;
    public MovingState MovingState;
    public DuplicateState DuplicateState;

    private void Start()
    {
    IdleState = new IdleState(this);
    MovingState = new MovingState(this);
    DuplicateState = new DuplicateState(this);
    ChangeState(IdleState);
    }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
    
    void Update() => CurrentState?.Tick(Time.deltaTime);
}

public struct IdleState : IState
{
    public CubeStateMachine StateMachine { get; set; }

    public IdleState(CubeStateMachine stateMachine) => StateMachine = stateMachine;

    public void Enter() => Debug.Log("Enter Idle State");

    public void Tick(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StateMachine.ChangeState(new RotatingState(StateMachine));
        if (Input.GetKeyDown(KeyCode.M))
            StateMachine.ChangeState(StateMachine.MovingState);
        if (Input.GetKeyDown(KeyCode.D))
            StateMachine.ChangeState(StateMachine.DuplicateState);
    }

    public void Exit() => Debug.Log("Exit Idle State");
}

public struct DuplicateState : IState
{
    public CubeStateMachine StateMachine { get; set; }
    private GameObject duplicateCube;

    public DuplicateState(CubeStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        duplicateCube = null;
    }

    public void Enter()
    {
        duplicateCube = GameObject.Instantiate(StateMachine.gameObject);
        duplicateCube.transform.position = StateMachine.transform.position + Vector3.right * 2f;
    }

    public void Tick(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.D))
            StateMachine.ChangeState(StateMachine.IdleState);
    }

    public void Exit()
    {
        if (duplicateCube != null)
            GameObject.Destroy(duplicateCube);
    }
}

public struct RotatingState : IState

{
    public CubeStateMachine StateMachine { get; set; }

    public RotatingState(CubeStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public void Enter() => Debug.Log("Enter Rotating State");

    public void Tick(float deltaTime)
    {
        StateMachine.transform.Rotate(0f, 360f * deltaTime, 0f);
        if (Input.GetKeyDown(KeyCode.Space)) StateMachine.ChangeState(new IdleState(StateMachine));
    }

    public void Exit() => Debug.Log("Exit Rotating State");
}

public struct MovingState : IState
{
    public CubeStateMachine StateMachine { get; set; }

    public MovingState(CubeStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public void Enter() => Debug.Log("Enter Moving State");

    public void Tick(float deltaTime)
    {
        StateMachine.transform.Translate(Vector3.left * deltaTime);
        if (Input.GetKeyDown(KeyCode.M))
            StateMachine.ChangeState(StateMachine.IdleState);
    }

    public void Exit() => Debug.Log("Exit Moving State");
}



/*
1- Crear un nuevo estado MovingState
2-Implementar la interfaz IState
3- Mover el cubo a la izquierda 1 unidad por segundo
4-Para cambiar el IdleState al MovingState, usar la tecla "M"
5-Para regresar el MovingState al IdleState, usar la tecla "M"
*/



/*

* 1 .- Crear un nuevo estado DuplicateState
* 2 .- Implementar la interfaz IState
* 3 .- AL ENTRAR al estado, duplicar el cubo con GameObject.Instantiate
* 4 .- AL SALIR del estado, deberán destruir la copia con GameObject.Destroy
* 5 .- Utilizar la tecla "D" para entrar al estado y salir de él.
*/


/*
Ejercicio 3 --



/*
* 1 .- Crear una nueva máquina de estados PlatformStateMachine
* 2 .- Crear un nuevo cubo y escalar para que parezca plataforma
* 3 .- Crear los estados necesarios para hacer lo siguiente
- Esperar 3 segundos
- Moverse a la izquierda por 3 segundos
- Esperar 3 segundos
- Moverse a la derecha por 3 segundos

*/


