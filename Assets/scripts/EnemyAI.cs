using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Attack, Chase }

    [Header("States")]
    [SerializeField] private AIState currentState = AIState.Patrol;

    [Header("AI settings")]
    [SerializeField] private float radiusDetection = 10f;
    [SerializeField] private float radiusAttack = 2.5f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackWindup = 0.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private Transform[] controlPoints;

    private int currentPointIndex;
    private bool isWaiting;
    private bool isAttacking = false;

    private NavMeshAgent agent;
    private Animator anim;
    private Transform playerTransform;
    private HealthSystem healthSystemPlayer;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
            healthSystemPlayer = player.GetComponent<HealthSystem>();
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("[EnemyAI] El agente NO está sobre el NavMesh. Rebakea el NavMesh.");
        }

        // Clave: el agente tiene que poder llegar hasta el jugador
        agent.stoppingDistance = 0.5f;
    }

    private void Update()
    {
        if (!agent.isOnNavMesh) return;
        if (playerTransform == null) return;

        if (healthSystemPlayer != null && healthSystemPlayer.IsDead)
        {
            if (currentState != AIState.Patrol)
            {
                currentState = AIState.Patrol;
                agent.isStopped = false;
                GoNext();
            }
            UpdateAnimation();
            return;
        }

        if (playerMovement != null && playerMovement.EsInvisible)
        {
            if (currentState != AIState.Patrol)
            {
                currentState = AIState.Patrol;
                agent.isStopped = false;
                GoNext();
            }
            UpdateAnimation();
            return;
        }

        // Distancia horizontal: ignora la altura del pivot
        float distanceToPlayer = DistanciaHorizontal();

        switch (currentState)
        {
            case AIState.Patrol:
                UpdatePatrolState(distanceToPlayer);
                break;

            case AIState.Chase:
                UpdateChaseState(distanceToPlayer);
                break;

            case AIState.Attack:
                UpdateAttackState(distanceToPlayer);
                break;
        }

        UpdateAnimation();
    }

private float DistanciaHorizontal()
    {
    Vector3 a = transform.position;
    Vector3 b = playerTransform.position;
    a.y = 0f;
    b.y = 0f;

    // Resta el radio de los colliders: mide borde a borde, no centro a centro
    float radioEnemigo = 0.5f;
    float radioJugador = 0.5f;

    Collider colEnemigo = GetComponent<Collider>();
    if (colEnemigo != null) radioEnemigo = colEnemigo.bounds.extents.x;

    Collider colJugador = playerTransform.GetComponent<Collider>();
    if (colJugador != null) radioJugador = colJugador.bounds.extents.x;

    return Vector3.Distance(a, b) - radioEnemigo - radioJugador;
    }


    private void UpdateAnimation()
    {
        if (anim == null) return;

        bool moviendo = agent.velocity.magnitude > 0.1f;
        anim.SetBool("isWalking", moviendo);
    }

    private void UpdatePatrolState(float distance)
    {
        if (distance <= radiusDetection)
        {
            currentState = AIState.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                StartCoroutine(WaitAndMove());
            }
        }
    }

    private void UpdateChaseState(float distance)
    {
        agent.SetDestination(playerTransform.position);

        if (distance <= radiusAttack)
        {
            currentState = AIState.Attack;
            return;
        }

        if (distance > radiusDetection * 1.2f)
        {
            currentState = AIState.Patrol;
            GoNext();
        }
    }

    private void UpdateAttackState(float distance)
    {
        if (distance > radiusAttack && !isAttacking)
        {
            currentState = AIState.Chase;
            return;
        }

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        if (controlPoints.Length > 0)
        {
            currentPointIndex = (currentPointIndex + 1) % controlPoints.Length;
            GoNext();
        }

        isWaiting = false;
    }

    private IEnumerator AttackCooldown()
    {
        agent.isStopped = true;

        yield return new WaitForSeconds(attackWindup);

        if (playerTransform != null && DistanciaHorizontal() <= radiusAttack)
        {
            ExecuteAttack();
        }

        yield return new WaitForSeconds(attackCooldown);

        agent.isStopped = false;

        isAttacking = false;
    }

    private void ExecuteAttack()
    {
        if (healthSystemPlayer == null) return;
        if (healthSystemPlayer.IsDead) return;

        healthSystemPlayer.TakeDamage(attackDamage);
    }

    private void GoNext()
    {
        if (controlPoints.Length == 0) return;
        if (!agent.isOnNavMesh) return;

        agent.SetDestination(controlPoints[currentPointIndex].position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusDetection);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
}
