using UnityEngine;
using System;

// --- DEFINICIÓN DE INTERFACES (ASUME QUE ESTÁN EN EL PROYECTO) ---
// Si ya tienes estas interfaces definidas, puedes eliminar esta sección.

/* Máquina de Estados Principal para la Trampa de Púas */
public class SpikeStateMachine : MonoBehaviour, IStateMachine
{
    // --- Configuración Pública en el Inspector ---
    [Tooltip("Arrastra aquí el objeto hijo que contiene solo las púas (para mover su Transform)")]
    public GameObject spikesObject; 

    [Tooltip("Tiempo que la trampa permanece en su posición final (arriba o abajo)")]
    public float waitDuration = 3f; 

    [Tooltip("Velocidad de movimiento (unidades por segundo)")]
    public float moveSpeed = 2f; 

    [Tooltip("Distancia que las púas deben moverse hacia abajo (e.g., 0.5 para bajar 0.5 unidades)")]
    public float verticalOffset = 0.5f;

    // --- Variables de Estado Internas ---
    public Vector3 ExposedPosition { get; private set; } // Posición visible (arriba)
    public Vector3 HiddenPosition { get; private set; } // Posición oculta (abajo)

    public IState CurrentState { get; set; }

    private void Awake()
    {
        if (spikesObject != null)
        {
            // La posición expuesta es la posición inicial al colocarlo en la escena
            ExposedPosition = spikesObject.transform.localPosition;
            // La posición oculta es la posición expuesta menos el desplazamiento vertical
            HiddenPosition = ExposedPosition - new Vector3(0, verticalOffset, 0);

            // Asegurar que las púas empiecen en la posición oculta
            spikesObject.transform.localPosition = HiddenPosition;
        }
    }

    private void Start()
    {
        // El ciclo comienza esperando en la posición oculta
        ChangeState(new SpikeWaitHiddenState(this));
    }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
    
    void Update() => CurrentState?.Tick(Time.deltaTime);

    // Método que mueve las púas hacia la posición objetivo
    public bool MoveTowardsTarget(Vector3 targetPosition, float deltaTime)
    {
        if (spikesObject == null) return true; // Falla segura

        // Mover el objeto de las púas usando MoveTowards
        spikesObject.transform.localPosition = Vector3.MoveTowards(
            spikesObject.transform.localPosition, 
            targetPosition, 
            moveSpeed * deltaTime
        );
        
        // Retorna verdadero si ha alcanzado la posición objetivo
        return Vector3.Distance(spikesObject.transform.localPosition, targetPosition) < 0.001f;
    }
}

// -------------------------------------------------------------------

/* 1. Estado: Esperando Oculto (Abajo) */
public struct SpikeWaitHiddenState : IState
{
    public SpikeStateMachine StateMachine { get; set; }
    private float timer;

    public SpikeWaitHiddenState(SpikeStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        timer = 0f;
    }

    public void Enter() 
    {
        Debug.Log("Spikes: Enter Wait Hidden State");
        timer = 0f;
    }

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        
        // Esperar la duración y pasar al estado de subida
        if (timer >= StateMachine.waitDuration)
        {
            StateMachine.ChangeState(new SpikeMovingUpState(StateMachine));
        }
    }

    public void Exit() => Debug.Log("Spikes: Exit Wait Hidden State");
}

/* 2. Estado: Moviéndose Hacia Arriba */
public struct SpikeMovingUpState : IState
{
    public SpikeStateMachine StateMachine { get; set; }

    public SpikeMovingUpState(SpikeStateMachine stateMachine) => StateMachine = stateMachine;

    public void Enter() 
    {
        Debug.Log("Spikes: Enter Moving Up State");
    }

    public void Tick(float deltaTime)
    {
        // Mover hacia la posición expuesta
        bool reachedTarget = StateMachine.MoveTowardsTarget(StateMachine.ExposedPosition, deltaTime);
        
        // Si ya alcanzó la posición superior, pasar al estado de espera
        if (reachedTarget)
        {
            StateMachine.ChangeState(new SpikeWaitExposedState(StateMachine));
        }
    }

    public void Exit() => Debug.Log("Spikes: Exit Moving Up State");
}

/* 3. Estado: Esperando Expuesto (Arriba) */
public struct SpikeWaitExposedState : IState
{
    public SpikeStateMachine StateMachine { get; set; }
    private float timer;

    public SpikeWaitExposedState(SpikeStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        timer = 0f;
    }

    public void Enter() 
    {
        Debug.Log("Spikes: Enter Wait Exposed State");
        timer = 0f;
    }

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        
        // Esperar la duración y pasar al estado de bajada
        if (timer >= StateMachine.waitDuration)
        {
            StateMachine.ChangeState(new SpikeMovingDownState(StateMachine));
        }
    }

    public void Exit() => Debug.Log("Spikes: Exit Wait Exposed State");
}

/* 4. Estado: Moviéndose Hacia Abajo */
public struct SpikeMovingDownState : IState
{
    public SpikeStateMachine StateMachine { get; set; }

    public SpikeMovingDownState(SpikeStateMachine stateMachine) => StateMachine = stateMachine;

    public void Enter() 
    {
        Debug.Log("Spikes: Enter Moving Down State");
    }

    public void Tick(float deltaTime)
    {
        // Mover hacia la posición oculta
        bool reachedTarget = StateMachine.MoveTowardsTarget(StateMachine.HiddenPosition, deltaTime);
        
        // Si ya alcanzó la posición inferior, pasar al estado de espera
        if (reachedTarget)
        {
            StateMachine.ChangeState(new SpikeWaitHiddenState(StateMachine));
        }
    }

    public void Exit() => Debug.Log("Spikes: Exit Moving Down State");
}