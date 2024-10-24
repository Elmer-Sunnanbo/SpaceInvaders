using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Laser : Projectile
{

    private void Awake()
    {
        direction = Vector3.up;
        speed = 100;
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    void CheckCollision(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyCore FoundCore)) //If the hit object has an EnemyCore
        {
            FoundCore.Hit(); //Inform it that it's been hit
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boundary")) //If the hit object is a boundary
        {
            Destroy(gameObject); //Time to die
        }
    }
}
