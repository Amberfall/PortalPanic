using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsMovement : MonoBehaviour
{
    Vector3 startPos;
    public float minTimeTillNextBird = 2f;
    public float maxTimeTillNextBird = 10f;
    float timeTillNextBird;
    public float speed = 0.5f;
    bool isMoving = false;
    void Start()
    {
        startPos = transform.position;
        timeTillNextBird = GetRandomTime();
    }

    void Update()
    {
        MoveBird();
        
    }
    void MoveBird(){
        if(!isMoving){
            timeTillNextBird -= Time.deltaTime;
            if(timeTillNextBird <= 0){
                isMoving = true;
            }
        }
        if(isMoving){
            if(transform.position.x >= 20){
                transform.position = startPos;
                isMoving = false;
                timeTillNextBird = GetRandomTime();
            }
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right, speed * Time.deltaTime);
        }
        
    }
    float GetRandomTime(){
        return Random.Range(minTimeTillNextBird, maxTimeTillNextBird);
    }
}
