using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Transform destination;
    private NavMeshAgent agent;
    private Collider[] detectedObjects = new Collider[10];
    public float detectionRadius = 10f;
    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();

        while (true)
        {
            var numCol = Physics.OverlapSphereNonAlloc(
            position: transform.position,
            radius: detectionRadius,
            results: detectedObjects);

            for (int i = 0; i < numCol; i++)
            {
                var col = detectedObjects[i];
                if (!col.CompareTag("Player")) continue; // 1er filtro
                var player = col.transform;
                var vectorToPlayer = player.position - transform.position;
                var dot = Vector3.Dot(vectorToPlayer, transform.forward);
                if (dot < 0) continue; // 2do filtro
                Physics.Raycast(transform.position, vectorToPlayer, out RaycastHit hit);
                if (hit.collider.transform != player) continue; // 3er filtro
                agent.destination = player.position; // Seguir al jugador
            }
            yield return new WaitForSeconds(0.2f);
        }
    }


    void Update()
    {
        
    }
}