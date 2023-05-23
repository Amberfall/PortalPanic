using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Food>()) {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
   }
}
