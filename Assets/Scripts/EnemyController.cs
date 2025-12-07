using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // --- VARIABLES DE PERSECUCIÓN Y ANIMACIÓN ---
    public Transform target;
    public float detectionRadius = 15f;
    [Range(0, 360)]
    public float fieldOfViewAngle = 90f;
    public float attackRange = 2f;
    public float timeBetweenAttacks = 1.5f;
    private float attackTimer;
    private NavMeshAgent agent;
    private Animator animator;

    // --- AJUSTES DE ATAQUE ---
    public float damageAmount = 10f;
    public Collider swordHitCollider;
    public LayerMask playerLayer;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public bool isChasingPlayer = false;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;

        if (agent != null)
            agent.stoppingDistance = attackRange;

        if (swordHitCollider != null)
            swordHitCollider.enabled = false;
    }

    void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        attackTimer -= Time.deltaTime;

        // --- ATAQUE ---
        if (distanceToTarget <= attackRange)
        {
            isChasingPlayer = false;
            agent.ResetPath();
            animator.SetFloat("Velocidad", 0f);
            AttackPlayer();
            return;
        }

        // --- DETECCIÓN Y PERSECUCIÓN ---
        if (IsPlayerInSight())
        {
            isChasingPlayer = true;
            agent.speed = chaseSpeed;
            agent.SetDestination(target.position);

            float speed = agent.velocity.magnitude;
            animator.SetFloat("Velocidad", speed / agent.speed);
        }
        else
        {
            isChasingPlayer = false;
        }

        // --- PATRULLAJE ---
        if (!isChasingPlayer && patrolPoints.Length > 0)
        {
            Patrolling();
        }

        // --- IDLE ---
        if (!isChasingPlayer && patrolPoints.Length == 0)
        {
            if (agent.hasPath) agent.ResetPath();
            animator.SetFloat("Velocidad", 0f);
        }
    }



    // --- DETECCIÓN ---
    bool IsPlayerInSight()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > detectionRadius)
            return false;

        Vector3 direction = (target.position + Vector3.up * 1f) - (transform.position + Vector3.up * 1.6f);
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > fieldOfViewAngle / 2f)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1.6f, direction.normalized, out hit, detectionRadius))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }



    // --- ATAQUE ---
    void AttackPlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (attackTimer <= 0)
        {
            animator.SetTrigger("Atacar");
            attackTimer = timeBetweenAttacks;
        }
    }



    // --- HITBOX ---
    void OnTriggerEnter(Collider other)
    {
        if (swordHitCollider == null || !swordHitCollider.enabled)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
            playerHealth.TakeDamage(damageAmount);

        swordHitCollider.enabled = false;
    }



    public void ActivateSwordHitbox()
    {
        if (swordHitCollider != null)
            swordHitCollider.enabled = true;
    }

    public void DeactivateSwordHitbox()
    {
        if (swordHitCollider != null)
            swordHitCollider.enabled = false;
    }



    // --- PATRULLAJE CON NAVMESH ---
    void Patrolling()
    {
        if (patrolPoints.Length == 0) return;

        agent.speed = patrolSpeed;

        // Si no tiene destino, asignar uno
        if (!agent.hasPath)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

        // Si llegó al punto, pasar al siguiente
        if (agent.remainingDistance <= 0.5f && !agent.pathPending)
        {
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
                currentPatrolIndex = 0;

            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Velocidad", speed / agent.speed);
    }
}
