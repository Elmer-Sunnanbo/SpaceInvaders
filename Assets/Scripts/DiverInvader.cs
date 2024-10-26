using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverInvader : MonoBehaviour
{
    EnemyCore MyCore;
    [SerializeField] GameObject MyDeath;
    [SerializeField] float EntryTime;
    [SerializeField] float EntrySpeed;
    [SerializeField] float BackSpeed;
    [SerializeField] float BackTime;
    [SerializeField] float ReadyTime;
    [SerializeField] float DiveAcc;
    Rigidbody2D Rigidbody;
    float SetupTimer; //Tracks how far trough the phases before diving the invader is

    private void Awake()
    {
        MyCore = GetComponent<EnemyCore>();
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
        SetupTimer += Time.deltaTime;

        if (MyCore.HasBeenHit) //"On death"
        {
            Instantiate(MyDeath, transform.position, Quaternion.identity).GetComponent<DeathEffect>().Angle = MyCore.HitAngle;
            Destroy(gameObject);
            ScreenShake.Instance.ShakeCam(0.2f, 0.5f);
        }

        if(SetupTimer > EntryTime) //If we're past entering the screen
        {
            if(SetupTimer > EntryTime + BackTime) //If we're past backing up
            {
                if (SetupTimer < EntryTime + BackTime + ReadyTime) //If we're not past the wait time
                {
                    Rigidbody.velocity = new Vector2(0, 0); //Sit still
                }
            }
            else
            {
                Rigidbody.velocity = new Vector2(0, BackSpeed); //Move back at a fixed speed
            }
        }
        else
        {
            Rigidbody.velocity = new Vector2(0, -EntrySpeed); //Move onto the screen at a fixed speed
        }
    }
    private void FixedUpdate()
    {
        if(SetupTimer > EntryTime + BackTime + ReadyTime) //If we're past all entry steps
        {
            Rigidbody.velocity += new Vector2(0, -DiveAcc); //Accelerate downwards
        }
    }
}
