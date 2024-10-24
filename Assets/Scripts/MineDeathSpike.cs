using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDeathSpike : MonoBehaviour
{
    //[SerializeField] float Speed;
    Rigidbody2D Rigidbody;
    public float Angle;
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    void CheckCollision(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyCore FoundCore)) //If the hit object has an EnemyCore
        {
            FoundCore.Hit(Angle-90); //Inform it that it's been hit
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boundary") || collision.gameObject.layer == LayerMask.NameToLayer("Sidebarrier")) //If the hit object is a boundary or wall
        {
            Destroy(gameObject); //Time to die
        }
    }

    public void Go(float Speed, Vector2 Direction)
    {
        //Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = Direction*Speed; //Move in the specified direction at the specified speed.
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg); //Rotate to face that direction.
    }
}
