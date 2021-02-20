using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyController))]
public class EnemyAI : MonoBehaviour {
    [SerializeField] private string targetTag;
    [SerializeField] private float updatePathDelay;

    private EnemyController enemyController;
    private NavMeshAgent agent;
    private Transform target;
    private Vector2 direction;
    private float speed;
    private bool stop = false;

    public Vector2 Direction { get => direction; }
    public bool Stop { set => stop = value; }

    private void Start() {
        enemyController = GetComponent<EnemyController>();
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        speed = agent.speed;

        InvokeRepeating(nameof(UpdatePath), 0.0f, updatePathDelay);
    }

    private void Update() {
        if (enemyController.Die) return;

        if (!stop)
            direction = agent.velocity.normalized;
    }

    private void UpdatePath() {
        if (enemyController.Die) {
            agent.speed = 0.0f;
            CancelInvoke();
            return;
        }

        if (stop) {
            agent.SetDestination(transform.position);
            return;
        }

        agent.SetDestination(target.position);
    }

}
