using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private GunController gunController;

    private Vector2 dir;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 animDir = new Vector2(0.0f, -1.0f);

    private void Start() {
        Init();
    }

    private void Update() {
        CalcMoveDirection();
        CalcAnimDirection();
    }

    private void FixedUpdate() {
        rb.velocity = speed * Time.deltaTime * dir;
    }

    private void CalcMoveDirection() {
        dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        dir = (dir.sqrMagnitude > 1.0f) ? dir.normalized : dir;
        anim.SetBool("isMoving", dir != Vector2.zero);
    }

    private void Init() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void CalcAnimDirection() {
        animDir = gunController.GetDirection().normalized;
        anim.SetFloat("dirX", animDir.x);
        anim.SetFloat("dirY", animDir.y);
    }

    //Getters And Setters
    public int GetCurrentSortingLayer() => sprite.sortingOrder;
    public Vector2 GetAnimDirection() => animDir;
}
