using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GunData : MonoBehaviour {
    [SerializeField] private bool isActive;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private int damage;
    [SerializeField] private bool selectable;

    public SpriteRenderer Sprite { get => _spriteRenderer; }
    public bool Selectable { get => selectable; }
    public bool IsActive { set => isActive = value; }
    public int Damage { get => damage; }
    public bool ActiveObject {
        set {
            if (value)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
        }
    }

}
