using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineInvader : MonoBehaviour
{
    EnemyCore MyCore;
    [SerializeField] GameObject MyDeath;
    [SerializeField] GameObject MineExplosion;
    [SerializeField] float Speed;
    Rigidbody2D Rigidbody;

    private void Awake()
    {
        MyCore = GetComponent<EnemyCore>();
    }

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = new Vector2(0, -Speed); //Move downwards, velocity will never change
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
            Instantiate(MineExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            ScreenShake.Instance.ShakeCam(0.4f, 1.5f);
        }
    }
}
