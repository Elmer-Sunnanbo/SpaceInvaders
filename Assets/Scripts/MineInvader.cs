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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boundary")) //nått nedre kanten
        {
            GameManager.Instance.OnBoundaryReached();
        }
    }

    private void Update()
    {
        if (MyCore.HasBeenHit)
        {
            Instantiate(MyDeath, transform.position, Quaternion.identity).GetComponent<DeathEffect>().Angle = MyCore.HitAngle;
            Instantiate(MineExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            ScreenShake.Instance.ShakeCam(0.4f, 1.5f);
        }
        Rigidbody.velocity = new Vector2(0, -Speed);
    }
}
