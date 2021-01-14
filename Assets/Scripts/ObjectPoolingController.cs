using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingController : MonoBehaviour {
    public GameObject prefab;
    public int poolSize;
    public Transform parent;
    public Queue<GameObject> pool;

    private void Awake() {
        pool = new Queue<GameObject>();
    }

    private void Start() {
        for (int i = 0; i < poolSize; ++i) {
            GameObject poolObject = Instantiate(prefab, parent);
            pool.Enqueue(poolObject);
            poolObject.SetActive(false);
        }
    }

    public GameObject GetPoolPrefab() {
        GameObject poolObject = pool.Dequeue();
        poolObject.SetActive(true);
        return poolObject;
    }

    public bool HasObjectsToPool() {
        return (pool.Count > 0);
    }

    public void ReturnPoolPrefab(GameObject poolObject) {
        pool.Enqueue(poolObject);
        poolObject.SetActive(false);
    }
}
