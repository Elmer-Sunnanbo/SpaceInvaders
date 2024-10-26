using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerInvader : MonoBehaviour
{
    EnemyCore MyCore;
    [SerializeField] GameObject MyDeath;
    [SerializeField] float Speed;
    Rigidbody2D Rigidbody;
    int Direction = 1;
    bool GoingDown;
    [SerializeField] float GoingDownDuration;
    [SerializeField] float GoingDownSpeed;
    float GoingDownTime;

    private void Awake()
    {
        MyCore = GetComponent<EnemyCore>();
        if(Random.Range(0,2) == 0) //50% Chance
        {
            Direction = -1;
        }
    }

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boundary")) //When the invader reaches the lower boundary
        {
            GameManager.Instance.GameOver(); //End the game
        }
    }

    private void Update()
    {
        if (MyCore.HasBeenHit) //"On death"
        {
            Instantiate(MyDeath, transform.position, Quaternion.identity).GetComponent<DeathEffect>().Angle = MyCore.HitAngle;
            Destroy(gameObject);
            ScreenShake.Instance.ShakeCam(0.15f, 0.4f);
        }

        if(GoingDown) //If the invader is currently moving downwards
        {
            Rigidbody.velocity = new Vector2(0, -GoingDownSpeed);
            GoingDownTime -= Time.deltaTime;
            if(GoingDownTime < 0)
            {
                GoingDown = false;
            }
        }
        else //If the invader is currently moving sideways
        {
            Rigidbody.velocity = new Vector2(Speed * Direction, 0);
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision) //When colliding with a wall or another bouncer
    {
        if(!GoingDown)
        {
            Direction *= -1; //Reverse the direction
            transform.position += new Vector3(Direction * 0.5f, -1); //Move a bit away from the collision spot to avoid things getting stuck in each other (still happens sometimes)

            //Enter the "moving down phase"
            GoingDown = true;
            GoingDownTime = GoingDownDuration;
        }
    }
}