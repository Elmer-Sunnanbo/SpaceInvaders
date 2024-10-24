using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    // Start is called before the first frame update
    public bool HasBeenHit = false;
    public float HitAngle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        GameManager.Instance.ActiveEnemies.Remove(gameObject);
        HasBeenHit = true;
    }
    public void Hit(float Angle)
    {
        GameManager.Instance.ActiveEnemies.Remove(gameObject);
        HasBeenHit = true;
        HitAngle = Angle;
    }
}
