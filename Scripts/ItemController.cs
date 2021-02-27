using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {
    [SerializeField] private string itemType;

    private PlayerController playerController;

    private void Start() {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            AddItem();
            Destroy(gameObject);
        }
    }

    private void AddItem() {
        switch (itemType) {
            case "Coin":
                ++playerController.Coins;
                Debug.Log("Coin Added!");
                break;
            case "Potion":
                ++playerController.Potions;
                Debug.Log("Potion Added!");
                break;
        }
    }
}
