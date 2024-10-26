using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Spike;
    [SerializeField] float SpikeSpeed;
    [SerializeField] float SpikeCount;
    float Timer;
    void Start()
    {
        RaycastHit2D[] Hits = Physics2D.CircleCastAll(transform.position, 3, Vector2.zero, 0);
        foreach(RaycastHit2D Hit in Hits)
        {
            if(Hit.collider.gameObject.TryGetComponent(out EnemyCore FoundCore))
            {
                float AngleToHit = Mathf.Atan2(Hit.collider.gameObject.transform.position.y - transform.position.y, Hit.collider.gameObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                FoundCore.Hit(AngleToHit - 90);
            }
        }
        for (int i = 0; i < SpikeCount; i++)
        {
            float DegreeDirection = 360 * i / SpikeCount;
            Vector2 VectorDirection = (Vector2)(Quaternion.Euler(0, 0, DegreeDirection) * Vector2.right);
            GameObject RecentSpike = Instantiate(Spike, transform.position, Quaternion.identity);
            RecentSpike.GetComponent<MineDeathSpike>().Go(SpikeSpeed, VectorDirection);
            RecentSpike.GetComponent<MineDeathSpike>().Angle = DegreeDirection;
        }
        //Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > 5)
        {
            Destroy(gameObject);
        }
    }
}
