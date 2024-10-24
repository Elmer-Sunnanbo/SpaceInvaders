using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoreTestMine : MonoBehaviour
{
    EnemyCore MyCore;
    [SerializeField] GameObject MyDeath;
    [SerializeField] GameObject MineExplosion;
    void Start()
    {
        MyCore = GetComponent<EnemyCore>();
    }

    void Update()
    {
        if (MyCore.HasBeenHit)
        {
            Destroy(gameObject);
            Instantiate(MineExplosion, transform.position, Quaternion.identity);
            Instantiate(MyDeath, transform.position, Quaternion.identity).GetComponent<DeathEffect>().Angle = MyCore.HitAngle;
            ScreenShake.Instance.ShakeCam(0.1f, 0.3f);
        }
    }
}
