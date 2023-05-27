using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableBG : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        if(transform.position.x >= spriteRenderer.size.x/3)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right, scrollSpeed * Time.deltaTime);
    }
}
