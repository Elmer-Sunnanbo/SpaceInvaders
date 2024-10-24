using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public float Angle;
    [SerializeField] List<GameObject> GorePieces = new List<GameObject>();
    [SerializeField] GameObject BloodParticle;
    [SerializeField] int BloodCount;
    List<GameObject> ActivePieces = new List<GameObject>();
    void Start()
    {
        foreach (GameObject SelectedPiece in GorePieces)
        {
            GameObject Piece = Instantiate(SelectedPiece, transform.position, Quaternion.identity);
            ActivePieces.Add(Piece);
            GorePiece PieceScript = Piece.GetComponent<GorePiece>();
            PieceScript.StartVelocity = new Vector2(Random.Range(-20f, 20f), Random.Range(10f, 50f));
            PieceScript.StartVelocity = Quaternion.Euler(0, 0, Angle) * PieceScript.StartVelocity;
            if (Random.Range(1,0)  == 0 ) //Randomizes a rotation speed
            {
                PieceScript.StartRotation = Random.Range(100f, 600f);
            }
            else
            {
                PieceScript.StartRotation = Random.Range(-600f, -100f);
            }
            
            PieceScript.SlowdownFactor = 0.8f;
            PieceScript.SlowdownFactorRotation = 0.8f;
            Piece.transform.rotation = Quaternion.Euler(0,0,Random.Range(0f, 360f));
        }

        for(int i = 0; i < BloodCount; i++)
        {
            GameObject Particle = Instantiate(BloodParticle, transform.position, Quaternion.identity);
            ActivePieces.Add(Particle);
            BloodParticle ParticleScript = Particle.GetComponent<BloodParticle>();

            //ParticleScript.StartVelocity = new Vector2(Random.Range(-15f, 15f), GetVelocityNumber()*150);
            ParticleScript.StartVelocity = GetLaunchVectorCenterWeighted(90) * GetWeightedNumber(15) * 150;
            ParticleScript.StartVelocity = Quaternion.Euler(0, 0, Angle) * ParticleScript.StartVelocity;

            ParticleScript.SlowdownFactor = 0.8f;
            Particle.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }
    }

    /// <summary>
    /// Gets a number between 1 and 0 weighted towards 0
    /// </summary>
    /// <returns>
    /// The number as a float
    /// </returns>
    float GetWeightedNumber()
    {
        return (1 / Random.Range(1f, 10f)) - 0.1f;
    }

    /// <summary>
    /// Gets a number between 1 and 0 weighted towards 0.
    /// </summary>
    /// <param name="Strength"></param>
    /// <returns>
    /// The number as a float.
    /// </returns>
    float GetWeightedNumber(float Strength)
    {
        return (1 / Random.Range(1f, Strength)) - 1/Strength;
    }

    /// <summary>
    /// Gets a random upwards direction from an angle.
    /// </summary>
    /// <param name="Angle"></param>
    /// <returns>
    /// The direction as a Vector2.
    /// </returns>
    Vector2 GetLaunchVector(float Angle)
    {
        float DegreeRotation = Random.Range(0f, Angle);  //Get an angle (right of center)
        Vector2 Vector = new Vector2(Mathf.Sin(DegreeRotation * Mathf.Deg2Rad), Mathf.Cos(DegreeRotation * Mathf.Deg2Rad));//Get a vector from the angle
        if (Random.Range(0,2) ==  0) //50% chance to flip angle to left of center
        {
            Vector.x *= -1;
        }
        return Vector.normalized;
    }

    /// <summary>
    /// Gets a random upwards direction from an angle weighted towards straight up.
    /// </summary>
    /// <param name="Angle"></param>
    /// <param name="Strength"></param>
    /// <returns>
    /// The direction as a Vector2.
    /// </returns>
    Vector2 GetLaunchVectorCenterWeighted(float Angle)
    {
        float DegreeRotation = Random.Range(0f, Angle) * GetWeightedNumber() * 2; //Get a weighted angle (right of center)
        Vector2 Vector = new Vector2(Mathf.Sin(DegreeRotation * Mathf.Deg2Rad), Mathf.Cos(DegreeRotation * Mathf.Deg2Rad)); //Get a vector from the angle
        if (Random.Range(0, 2) == 0) //50% chance to flip angle to left of center
        {
            Vector.x *= -1;
        }
        return Vector.normalized;
    }

    public void Clear()
    {
        foreach(GameObject Object in ActivePieces)
        {
            Destroy(Object);
        }
        Destroy(gameObject);
    }
}
