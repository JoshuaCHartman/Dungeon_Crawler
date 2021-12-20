using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public PlayerMovement swordCollision;
    public GameObject hitParticle;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && swordCollision.isAttacking)
        {
            Instantiate(hitParticle, new Vector3(other.transform.position.x, transform.position.y,
                other.transform.position.z), other.transform.rotation);
        }
    }
}
