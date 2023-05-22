using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinball : MonoBehaviour
{
    public enum BounceDir { LeftRight, UpDown, RightUpDown, LeftUpDown }

    [SerializeField] private BounceDir _bounceDir;
    [SerializeField] private float _bounceForce = 25f;

    const string BOUNCEABLE_STRING = "Bounceable";
    readonly int FLASH_HASH = Animator.StringToHash("Flash");

    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag(BOUNCEABLE_STRING)) {
            Bounce(other.gameObject);
            _animator.SetTrigger(FLASH_HASH);
        }
    }

    private void Bounce(GameObject other) {
        Rigidbody2D otherRB = other.gameObject.GetComponent<Rigidbody2D>();

        switch (_bounceDir)
        {
            case BounceDir.LeftRight:
                Vector3 targetDirection = other.transform.position - this.transform.position;

                // left -1 right 1
                int dir = (targetDirection.x > 0) ? 1 : (targetDirection.x < 0) ? -1 : 0;

                if (dir == 1) {
                    otherRB.velocity = new Vector2(_bounceForce, 0f);
                } else if (dir == -1) {
                    otherRB.velocity = new Vector2(-_bounceForce, 0f);
                }

                break;

            case BounceDir.UpDown:
                if (this.transform.position.y >= other.transform.position.y)
                {
                    otherRB.velocity = new Vector2(0f, -_bounceForce);
                }
                else if (this.transform.position.y < other.transform.position.y)
                {
                    otherRB.velocity = new Vector2(0f, _bounceForce);
                }

                break;

            case BounceDir.RightUpDown:
                if (this.transform.position.y >= other.transform.position.y)
                {
                    otherRB.velocity = new Vector2(_bounceForce, -_bounceForce);
                }
                else if (this.transform.position.y < other.transform.position.y)
                {
                    otherRB.velocity = new Vector2(_bounceForce, _bounceForce);
                }

                break;

            case BounceDir.LeftUpDown:
                if (this.transform.position.y >= other.transform.position.y)
                {
                    otherRB.velocity = new Vector2(-_bounceForce, -_bounceForce);
                }
                else if (this.transform.position.y < other.transform.position.y)
                {
                    otherRB.velocity = new Vector2(-_bounceForce, _bounceForce);
                }

                break;

            default:
                Debug.Log("Invalid Bounce Type");
                break;
        }
    }
}
