using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private Timer resetTimer;

    [SerializeField]  private Manager manager;
    private Rigidbody2D rb;
    private Vector2 dir = Vector2.zero;
    private bool firstTimeEnabled = false;
    private ObjectPooler bulletPool;
    private Vector2 mousePos;

    private void OnEnable() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();

        //Set direction to move
        if (!manager.Mobile) {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir = (mousePos - (Vector2)transform.position).normalized;
        } else {
            dir = manager.ShootJoystick.Direction.normalized;
        }
        

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
        rb.velocity = dir * Time.deltaTime * speed;

        if (resetTimer.IsTimeCompleted)
            ResetBullet();
    }

    private void Init() {
        bulletPool = GetComponentInParent<ObjectPooler>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void ResetBullet() {
        resetTimer.IsTimeCompleted = false;
        bulletPool.ReturnPoolPrefab(gameObject);
    }

}
