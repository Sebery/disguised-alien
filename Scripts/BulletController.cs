using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private Timer resetTimer;

    private Rigidbody2D rb;
    private Vector2 dir = Vector2.zero;
    private bool firstTimeEnabled = false;
    private ObjectPooler bulletPool;
    private Vector2 mousePos;

    private void OnEnable() {
        //Set direction to move
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir = (mousePos - (Vector2)transform.position).normalized;

        //Start timer since second time the object is enabled
        if (firstTimeEnabled)
            resetTimer.StartTimer();

        firstTimeEnabled = true;
    }

    private void OnDisable() {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void Start() {
        Init();
    }

    private void FixedUpdate() {
        MovePlayer();

        if (resetTimer.GetIsTimeCompleted())
            Reset();
    }

    private void Init() {
        bulletPool = GetComponentInParent<ObjectPooler>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void MovePlayer() {
        rb.velocity = dir * Time.deltaTime * speed;
    }

    private void Reset() {
        resetTimer.SetIsTimeCompleted(false);
        bulletPool.ReturnPoolPrefab(gameObject);
    }

}
