using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }



    List<Shake> ActiveShakes = new List<Shake>();
    Vector3 CameraPosition;
    public Transform ThisTransform;
    void Start()
    {
        ThisTransform = transform;
        CameraPosition = transform.position;
    }

    void Update()
    {
        transform.position = CameraPosition;
        ShakeAll();
    }

    public void ShakeCam(float Time, float Strength)
    {
        ActiveShakes.Add(new Shake(Time, Strength));
    }

    void ShakeAll()
    {
        if (ActiveShakes.Count > 0)
        {
            Shake StrongestShake = new Shake(1, 100);
            foreach (Shake shake in ActiveShakes)
            {
                float MaxStrength = 0;
                shake.RemainingTime -= Time.deltaTime;

                if (shake.RemainingStrength >= MaxStrength)
                {
                    StrongestShake = shake;
                }
            }
            ShakeCamera(StrongestShake);
            List<Shake> ToBeRemoved = new List<Shake>();
            foreach (Shake shake in ActiveShakes)
            {
                if (shake.Deteriorate())
                {
                    ToBeRemoved.Add(shake);
                }
            }
            foreach (Shake shake in ToBeRemoved)
            {
                ActiveShakes.Remove(shake);
            }
        }
    }

    void ShakeCamera(Shake shake)
    {
        float DegreeRotation = UnityEngine.Random.Range(0f, 90f);
        float Opposite = shake.RemainingStrength * Mathf.Sin(DegreeRotation);

        float Adjacent = shake.RemainingStrength * Mathf.Cos(DegreeRotation);
        switch (UnityEngine.Random.Range(1, 4))
        {
            case 1:
                transform.position = CameraPosition + new Vector3(Adjacent, Opposite);
                break;
            case 2:
                transform.position = CameraPosition + new Vector3(Adjacent, -Opposite);
                break;
            case 3:
                transform.position = CameraPosition + new Vector3(-Adjacent, -Opposite);
                break;
            case 4:
                transform.position = CameraPosition + new Vector3(-Adjacent, Opposite);
                break;
        }
    }
}

class Shake
{
    public float StartTime;
    public float StartStrength;
    public float RemainingTime;
    public float RemainingStrength;

    public Shake(float Time, float Strength)
    {
        StartTime = Time;
        StartStrength = Strength;
        RemainingTime = Time;
        RemainingStrength = Strength;
    }

    public bool Deteriorate()
    {
        RemainingTime -= Time.deltaTime;
        if (RemainingTime < 0f)
        {
            return true;
        }
        else
        {
            RemainingStrength = StartStrength * (RemainingTime / StartTime);
            return false;
        }
    }
}
