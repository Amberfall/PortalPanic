using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DevilSpawner : MonoBehaviour{
    [SerializeField] public float _spawnRate = 5f;
    [SerializeField] static private int _poolCount = 10;
    [SerializeField] public Sprite spriteName;
    private ObjectPool<GameObject> devilPool;
        
    GameObject CreatePooledItem(){
        // Debug.Log("CreatePooledItem");
        var devil = Instantiate(new GameObject("Devil"+devilPool.CountAll));
        Debug.Log("devil: " + devil);
        devil.AddComponent<DevilStateManager>();
        devil.AddComponent<Rigidbody2D>();
        SpriteRenderer spriteR = devil.AddComponent<SpriteRenderer>();
        spriteR.sprite = spriteName;
        BoxCollider2D boxCol = devil.AddComponent<BoxCollider2D>();
        boxCol.size = new Vector2(0.5f, 1f);
        devil.SetActive(true);
        return devil;
    }
    void Awake(){
        
        devilPool = new ObjectPool<GameObject>(
            createFunc: () => CreatePooledItem(), 
            actionOnGet: (obj) => obj.SetActive(true), 
            actionOnRelease: (obj) => obj.SetActive(false), 
            actionOnDestroy: (obj) => Destroy(obj), 
            defaultCapacity: _poolCount
        );
        // Debug.Log("devilPool: " + devilPool);
        // Debug.Log(devilPool);
        // Debug.Log(devilPool.CountAll);
        initializePool();
        InvokeRepeating("Spawn", 1f, _spawnRate);
    }

    private void initializePool(){
        for(int i = 0; i < _poolCount; i++){
            GameObject devil = devilPool.Get();
            devilPool.Release(devil);
        }
    }
    private void Spawn(){
        // Debug.Log("Entering Spawn");
        // Debug.Log("devilPool: " + devilPool);
        // Debug.Log(devilPool.CountAll);
        GameObject devil = devilPool.Get();
        if(devil != null){
            devil.SetActive(true);
        }
    }
}
