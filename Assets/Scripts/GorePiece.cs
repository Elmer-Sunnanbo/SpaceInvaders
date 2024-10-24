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
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity = StartVelocity * VelocityModifier;//Applies a modifier to the velocity. Smaller/More aerodynamic pieces usually have higher velocity
        Rigidbody.angularVelocity = StartRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Slows down speed over time
        Rigidbody.velocity *= SlowdownFactor;
        Rigidbody.angularVelocity *= SlowdownFactorRotation;
    }
}
