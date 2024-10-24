using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : MonoBehaviour
{
    public Vector2 StartVelocity;
    public float SlowdownFactor;
    Rigidbody2D Rigidbody;
    SpriteRenderer SR;
    [SerializeField] float VelocityModifier;
    
    [SerializeField] Color StartColor;
    [SerializeField] float MinBrightness;
    [SerializeField] float FadeTime;
    [SerializeField] float TimeBetweenTrailSpawns;
    [SerializeField] GameObject Trail;
    float TimeUntilTrailSpawn;
    float RemainingBrightness = 1;

    bool Airborne = true;
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = StartVelocity * 1.2f;
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Reduces the brightness of the color until it reaches MinBrightness
        RemainingBrightness -= Time.deltaTime/FadeTime;
        
        RemainingBrightness = Mathf.Clamp(RemainingBrightness, MinBrightness, 1);
        SR.color = new Color(StartColor.r * RemainingBrightness, StartColor.g * RemainingBrightness, StartColor.b * RemainingBrightness);
        if(Airborne)
        {
            TimeUntilTrailSpawn -= Time.deltaTime;
            if (TimeUntilTrailSpawn < 0)
            {
                TimeUntilTrailSpawn += TimeBetweenTrailSpawns;
                GameObject LatestTrail = Instantiate(Trail, transform.position, Quaternion.identity);
                LatestTrail.GetComponent<BloodTrail>().RemainingBrightness = RemainingBrightness;
                LatestTrail.GetComponent<SpriteRenderer>().color = new Color(StartColor.r * RemainingBrightness, StartColor.g * RemainingBrightness, StartColor.b * RemainingBrightness);
            }
        }
    }

    void FixedUpdate()
    {
        
        if(Airborne)
        {
            Rigidbody.velocity *= SlowdownFactor;
        }
        
        if (Rigidbody.velocity.magnitude < 0.1f)
        {
            Land();
        }
    }

    void Land()
    {
        Airborne = false;
        Rigidbody.velocity = Vector2.zero;
    }
}
