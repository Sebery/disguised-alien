using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour {
    public SpriteRenderer sprite;


    private void OnTriggerStay2D(Collider2D collision) {
        if (transform.position.y < collision.transform.position.y) {
            sprite.sortingOrder = 5;
        } else {
            sprite.sortingOrder = 1;
        }
    }
}
