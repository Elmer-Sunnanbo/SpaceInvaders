using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    public bool HasBeenHit = false;
    public float HitAngle;

    /// <summary>
    /// Kills the enemy
    /// </summary>
    public void Hit()
    {
        GameManager.Instance.ActiveEnemies.Remove(gameObject);
        HasBeenHit = true;
    }

    /// <summary>
    /// Kills the enemy, and sends the gore at the specified angle
    /// </summary>
    public void Hit(float Angle)
    {
        GameManager.Instance.ActiveEnemies.Remove(gameObject);
        HasBeenHit = true;
        HitAngle = Angle;
    }
}
