using System;
using UnityEngine;

/* Ejercicio 3 - PlatformStateMachine */
/* 1.- Crear una nueva máquina de estados PlatformStateMachine
   2.- Crear un nuevo cubo con la escena para que tenga plataforma
   3.- Crear los estados necesarios para hacer lo siguiente:
      - Esperar 3 segundos
      - Moverse a la izquierda por 3 segundos
      - Esperar 3 segundos  
      - Moverse a la derecha por 3 segundos
*/

public class PlatformStateMachine : MonoBehaviour, IStateMachine
{
    public IState CurrentState { get; set; }

    private void Start() => ChangeState(new PlatformWaitState(this));

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
    
    void Update() => CurrentState?.Tick(Time.deltaTime);
}

public struct PlatformWaitState : IState
{
    public PlatformStateMachine StateMachine { get; set; }
    private float timer;
    private bool movingLeft; // Para alternar entre izquierda y derecha

    public PlatformWaitState(PlatformStateMachine stateMachine, bool shouldMoveLeft = true)
    {
        StateMachine = stateMachine;
        timer = 0f;
        movingLeft = shouldMoveLeft;
    }

    public void Enter() 
    {
        Debug.Log("Platform: Enter Wait State");
        timer = 0f;
    }

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        
        // Esperar 3 segundos
        if (timer >= 3f)
        {
            if (movingLeft)
                StateMachine.ChangeState(new PlatformMoveLeftState(StateMachine));
            else
                StateMachine.ChangeState(new PlatformMoveRightState(StateMachine));
        }
    }

    public void Exit() => Debug.Log("Platform: Exit Wait State");
}

public struct PlatformMoveLeftState : IState
{
    public PlatformStateMachine StateMachine { get; set; }
    private float timer;

    public PlatformMoveLeftState(PlatformStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        timer = 0f;
    }

    public void Enter() 
    {
        Debug.Log("Platform: Enter Move Left State");
        timer = 0f;
    }

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        
        // Moverse a la izquierda por 3 segundos
        StateMachine.transform.Translate(-1f * deltaTime, 0f, 0f);
        
        if (timer >= 3f)
        {
            // Ir al estado de espera (próximo movimiento será a la derecha)
            StateMachine.ChangeState(new PlatformWaitState(StateMachine, false));
        }
    }

    public void Exit() => Debug.Log("Platform: Exit Move Left State");
}

public struct PlatformMoveRightState : IState
{
    public PlatformStateMachine StateMachine { get; set; }
    private float timer;

    public PlatformMoveRightState(PlatformStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        timer = 0f;
    }

    public void Enter() 
    {
        Debug.Log("Platform: Enter Move Right State");
        timer = 0f;
    }

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        
        // Moverse a la derecha por 3 segundos
        StateMachine.transform.Translate(1f * deltaTime, 0f, 0f);
        
        if (timer >= 3f)
        {
            // Ir al estado de espera (próximo movimiento será a la izquierda)
            StateMachine.ChangeState(new PlatformWaitState(StateMachine, true));
        }
    }

    public void Exit() => Debug.Log("Platform: Exit Move Right State");
}