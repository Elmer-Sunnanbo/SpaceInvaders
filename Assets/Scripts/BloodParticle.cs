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
    float RemainingBrightness = 1;
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
    }

    void FixedUpdate()
    {
        
        Rigidbody.velocity *= SlowdownFactor;
    }
}
