using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class Invader : MonoBehaviour
{
    /// <summary>
    /// This script is obsolete
    /// </summary>
    public Sprite[] animationSprites = new Sprite[2];
    public float animationTime;
    EnemyCore MyCore;
    [SerializeField] GameObject MyDeath;


    SpriteRenderer spRend;
    int animationFrame;
    // Start is called before the first frame update

    private void Awake()
    {
        spRend = GetComponent<SpriteRenderer>(); 
        spRend.sprite = animationSprites[0];
        MyCore = GetComponent<EnemyCore>();
    }

    void Start()
    {
        //Anropar AnimateSprite med ett visst tidsintervall
        InvokeRepeating( nameof(AnimateSprite) , animationTime, animationTime);
    }

    //pandlar mellan olika sprited för att skapa en animation
    private void AnimateSprite()
    {
        animationFrame++;
        if(animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }
        spRend.sprite = animationSprites[animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            //GameManager.Instance.OnInvaderKilled(this);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Boundary")) //nått nedre kanten
        {
            //GameManager.Instance.OnBoundaryReached();
        }
    }

    private void Update()
    {
        if (MyCore.HasBeenHit)
        {
            Instantiate(MyDeath, transform.position, Quaternion.identity);
            Destroy(gameObject);
            ScreenShake.Instance.ShakeCam(0.1f, 0.3f);
        }
    }
}
