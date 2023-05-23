using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterSpawner : MonoBehaviour{
    
    [SerializeField] public float _spawnRate = 5f;
    [SerializeField] static private int _poolCount = 10;
    [SerializeField] private Sprite _spriteName;
    
    private ObjectPool<GameObject> _monsterPool;

    void Awake()
    {
        _monsterPool = new ObjectPool<GameObject>(
            createFunc: () => CreatePooledItem(),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            defaultCapacity: _poolCount
        );
        InitializePool();
        InvokeRepeating("Spawn", 1f, _spawnRate);
    }
        
    GameObject CreatePooledItem(){
        var monster = Instantiate(new GameObject("Monster" + _monsterPool.CountAll), this.transform);
        Debug.Log("monster: " + monster);
        monster.AddComponent<MonsterStateManager>();
        monster.AddComponent<Rigidbody2D>();
        SpriteRenderer spriteR = monster.AddComponent<SpriteRenderer>();
        spriteR.sprite = _spriteName;
        BoxCollider2D boxCol = monster.AddComponent<BoxCollider2D>();
        boxCol.size = new Vector2(0.5f, 1f);
        monster.SetActive(true);
        return monster;
    }
    

    private void InitializePool(){
        for(int i = 0; i < _poolCount; i++){
            GameObject monster = _monsterPool.Get();
            _monsterPool.Release(monster);
        }
    }
    private void Spawn(){
        GameObject monster = _monsterPool.Get();
        if(monster != null){
            monster.SetActive(true);
        }
    }
}
