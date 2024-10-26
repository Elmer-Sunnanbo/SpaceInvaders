using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoreTest : MonoBehaviour
{
    /// <summary>
    /// This script is a test script.
    /// </summary>

    EnemyCore MyCore;
    [SerializeField] GameObject MyDeath;

    void Start()
    {
        MyCore = GetComponent<EnemyCore>();
    }

    void Update()
    {
        if (MyCore.HasBeenHit)
        {
            Destroy(gameObject);
            Instantiate(MyDeath, transform.position, Quaternion.identity).GetComponent<DeathEffect>().Angle = MyCore.HitAngle;
            ScreenShake.Instance.ShakeCam(0.1f, 0.3f);
        }
    }
}
