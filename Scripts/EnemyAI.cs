using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyController))]
public class EnemyAI : MonoBehaviour {
    [SerializeField] private string targetTag;
    [SerializeField] private float updatePathDelay;
    [SerializeField] private Timer updatePathTimer;

    private EnemyController enemyController;
    private GameController gameController;
    private NavMeshAgent agent;
    private Transform target;
    private Vector2 direction;
    private bool stop = false;

    public Vector2 Direction { get => direction; }
    public bool Stop { get => stop; set => stop = value; }

    private void OnEnable() {
        if (!agent) return;

        updatePathTimer.StartTimer();
    }

    private void Start() {
        enemyController = GetComponent<EnemyController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        updatePathTimer.StartTimer();
    }

    private void Update() {
        if (enemyController.Die) {
            agent.speed = 0.0f;
            updatePathTimer.CancelTimer();
            return;
        }

        if (stop || gameController.AttackingPlayer) {
            if (agent.destination != transform.position)
                agent.SetDestination(transform.position);

            return;
        }

        direction = agent.velocity.normalized;

        if (updatePathTimer.TimeCompleted)
            UpdatePath();
    }

    private void UpdatePath() {
        if (agent.destination != target.position)
            agent.SetDestination(target.position);
    }

}
