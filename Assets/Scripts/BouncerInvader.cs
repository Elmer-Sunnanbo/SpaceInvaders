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


    SpriteRenderer spRend;
    int animationFrame;
    // Start is called before the first frame update

    private void Awake()
    {
        spRend = GetComponent<SpriteRenderer>();
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
        if (MyCore.HasBeenHit)
        {
            Instantiate(MyDeath, transform.position, Quaternion.identity).GetComponent<DeathEffect>().Angle = MyCore.HitAngle;
            Destroy(gameObject);
            ScreenShake.Instance.ShakeCam(0.1f, 0.3f);
        }
        if(GoingDown)
        {
            Rigidbody.velocity = new Vector2(0, -GoingDownSpeed);
            GoingDownTime -= Time.deltaTime;
            if(GoingDownTime < 0)
            {
                GoingDown = false;
            }
        }
        else
        {
            Rigidbody.velocity = new Vector2(Speed * Direction, 0);
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!GoingDown)
        {
            Direction *= -1;
            transform.position += new Vector3(Direction * 0.5f, -1);
            GoingDown = true;
            GoingDownTime = GoingDownDuration;
        }
    }
}
