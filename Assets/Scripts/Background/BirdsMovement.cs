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

    Animator anim;
    private void Awake() {
        startPos = transform.position;
        anim = GetComponent<Animator>();
        StopBird();
    }

    void Update()
    {
        MoveBird();
        
    }
    void MoveBird(){
        if(!isMoving){
            timeTillNextBird -= Time.deltaTime;
            if(timeTillNextBird <= 0){
                StartBird();
            }
        }
        if(isMoving){
            if(transform.position.x >= 20){
                StopBird();
            }
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right, speed * Time.deltaTime);
        }
        
    }
    float GetRandomTime(){
        return Random.Range(minTimeTillNextBird, maxTimeTillNextBird);
    }
    private void StartBird(){
        anim.enabled = true;
        isMoving = true;
    }
    private void StopBird(){
        anim.enabled = false;
        isMoving = false;
        timeTillNextBird = GetRandomTime();
        transform.position = startPos;
    }
}
