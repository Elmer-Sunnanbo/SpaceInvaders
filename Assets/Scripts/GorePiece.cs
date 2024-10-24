using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorePiece : MonoBehaviour
{
    public Vector2 StartVelocity;
    public float StartRotation;
    public float SlowdownFactor;
    public float SlowdownFactorRotation;
    Rigidbody2D Rigidbody;
    [SerializeField] float VelocityModifier;

    [SerializeField] static float MinBrightness = 0.6f;
    [SerializeField] static float FadeTime = 5;
    float RemainingBrightness = 1;

    SpriteRenderer SR;
    
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        
        Rigidbody.velocity = StartVelocity * VelocityModifier;//Applies a modifier to the velocity. Smaller/More aerodynamic pieces usually have higher velocity
        Rigidbody.angularVelocity = StartRotation;
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Reduces the brightness of the color until it reaches MinBrightness
        RemainingBrightness -= Time.deltaTime / FadeTime;

        RemainingBrightness = Mathf.Clamp(RemainingBrightness, MinBrightness, 1);
        SR.color = new Color(RemainingBrightness, RemainingBrightness, RemainingBrightness);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Slows down speed over time
        Rigidbody.velocity *= SlowdownFactor;
        Rigidbody.angularVelocity *= SlowdownFactorRotation;
    }
}
