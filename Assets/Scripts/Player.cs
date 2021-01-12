using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Rigidbody2D rb;
    public Animator anim;
    public float speed;
    private Vector2 velocity;

    private void Awake() {
        velocity = new Vector2(0.0f, 0.0f);
    }

    private void Update() {
        GetDataToMove();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + (Vector2)Vector3.Normalize(velocity) * speed * Time.fixedDeltaTime);
    }

    private void GetDataToMove() {
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("horizontal", velocity.x);
        anim.SetFloat("vertical", velocity.y);
        anim.SetFloat("speed", Vector2.SqrMagnitude(velocity));

        if (velocity != Vector2.zero)
            anim.SetFloat("lastspeed", ((velocity.x != 0.0f) ? velocity.x : (velocity.y > 0.0f) ? 2.0f : 0.0f));
    }

}
