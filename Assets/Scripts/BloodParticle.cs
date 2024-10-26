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
        Rigidbody.velocity = StartVelocity;
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Reduces the brightness of the color until it reaches MinBrightness
        RemainingBrightness -= Time.deltaTime/FadeTime;
        RemainingBrightness = Mathf.Clamp(RemainingBrightness, MinBrightness, 1); //Stop the brightness from going below MinBrightness
        SR.color = new Color(StartColor.r * RemainingBrightness, StartColor.g * RemainingBrightness, StartColor.b * RemainingBrightness); //Applies the brightness to the color

        if(Airborne)
        {
            TimeUntilTrailSpawn -= Time.deltaTime;
            if (TimeUntilTrailSpawn < 0)
            {
                TimeUntilTrailSpawn += TimeBetweenTrailSpawns;
                //Spawn a trail particle and match it's values to those of this particle
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
            Rigidbody.velocity *= SlowdownFactor; //Slow the particle down a bit.
        }
        
        if (Rigidbody.velocity.magnitude < 0.1f)  //If the particle is slow enough, land it.
        {
            Land();
        }
    }

    /// <summary>
    /// Turns the particle into a still object
    /// </summary>
    void Land()
    {
        Airborne = false;
        Rigidbody.velocity = Vector2.zero;
    }
}
