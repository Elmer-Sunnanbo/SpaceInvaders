using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverInvader : MonoBehaviour
{
    EnemyCore MyCore;
    [SerializeField] GameObject MyDeath;
    //[SerializeField] float Speed;
    [SerializeField] float EntryTime;
    [SerializeField] float EntrySpeed;
    [SerializeField] float BackSpeed;
    [SerializeField] float BackTime;
    [SerializeField] float ReadyTime;
    [SerializeField] float DiveAcc;
    Rigidbody2D Rigidbody;
    float SetupTimer;
    /*
    enum States
    {
        Entering,
        Backing,
        Ready,
        Diving,
    }
    States State = States.Entering;
    */
    SpriteRenderer spRend;
    int animationFrame;
    // Start is called before the first frame update

    private void Awake()
    {
        spRend = GetComponent<SpriteRenderer>();
        MyCore = GetComponent<EnemyCore>();
    }

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            //GameManager.Instance.OnInvaderKilled(this);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Boundary")) //nått nedre kanten
        {
            GameManager.Instance.OnBoundaryReached();
        }
    }

    private void Update()
    {
        SetupTimer += Time.deltaTime;
        if (MyCore.HasBeenHit)
        {
            Instantiate(MyDeath, transform.position, Quaternion.identity);
            Destroy(gameObject);
            ScreenShake.Instance.ShakeCam(0.1f, 0.3f);
        }

        if(SetupTimer > EntryTime)
        {
            if(SetupTimer > EntryTime + BackTime)
            {
                if (SetupTimer < EntryTime + BackTime + ReadyTime)
                {
                    Rigidbody.velocity = new Vector2(0, 0);
                }
            }
            else
            {
                Rigidbody.velocity = new Vector2(0, BackSpeed);
            }
        }
        else
        {
            Rigidbody.velocity = new Vector2(0, -EntrySpeed);
        }
    }
    private void FixedUpdate()
    {
        if(SetupTimer > EntryTime + BackTime + ReadyTime)
        {
            Rigidbody.velocity += new Vector2(0, -DiveAcc);
        }
    }
}
