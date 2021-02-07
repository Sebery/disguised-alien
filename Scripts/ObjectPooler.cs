using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int poolSize;

    private Queue<GameObject> pool;

    private void Awake() {
        //Initialize the pool queue
        pool = new Queue<GameObject>();
    }

    private void Start() {
        //Fill the pool
        for (int i = 0; i < poolSize; ++i) {
            GameObject obj = Instantiate(objectToPool, transform);
            pool.Enqueue(obj);
            obj.SetActive(false);
        }
    }

    //Get the object int the pool
    public GameObject GetPoolPrefab() {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    //Return the object to the pool
    public void ReturnPoolPrefab(GameObject obj) {
        pool.Enqueue(obj);
        obj.SetActive(false);
    }

    public bool HasObjectsToPool() {
        return (pool.Count > 0);
    }
}
