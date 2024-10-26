using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectile
{
    /// <summary>
    /// This script is obsolete
    /// </summary>
    private void Awake()
    {
        direction = Vector3.up;
        speed = 200;
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject); //The boundary is the only object with a collider (not a trigger)
    }

    void CheckCollision(Collider2D collision)
    {
        Bunker bunker = collision.gameObject.GetComponent<Bunker>();
        if (bunker == null) //Om det inte är en bunker vi träffat så ska skottet försvinna.
        {
            //Destroy(gameObject);
        }
        if (collision.gameObject.TryGetComponent(out EnemyCore FoundCore)) //If the hit object has an EnemyCore
        {
            FoundCore.Hit(); //Inform it that it's been hit
        }
    }
}

