using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour
{
    [SerializeField] GameObject Spike;
    [SerializeField] float SpikeSpeed;
    [SerializeField] float SpikeCount;
    float Timer;
    void Start()
    {
        RaycastHit2D[] Hits = Physics2D.CircleCastAll(transform.position, 3, Vector2.zero, 0); //Get all objects in a 3 unit radius
        foreach(RaycastHit2D Hit in Hits)
        {
            if(Hit.collider.gameObject.TryGetComponent(out EnemyCore FoundCore)) //If the object is an enemy
            {
                //Get the angle of the hit (for gore splash purposes)
                float AngleToHit = Mathf.Atan2(Hit.collider.gameObject.transform.position.y - transform.position.y, Hit.collider.gameObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                FoundCore.Hit(AngleToHit - 90);
            }
        }

        for (int i = 0; i < SpikeCount; i++) //Spawns spikes evenly distributed in a 360 degree arc
        {
            float DegreeDirection = 360 * i / SpikeCount;
            Vector2 VectorDirection = (Vector2)(Quaternion.Euler(0, 0, DegreeDirection) * Vector2.right);
            GameObject RecentSpike = Instantiate(Spike, transform.position, Quaternion.identity);
            RecentSpike.GetComponent<MineDeathSpike>().Go(SpikeSpeed, VectorDirection);
            RecentSpike.GetComponent<MineDeathSpike>().Angle = DegreeDirection;
        }
    }

    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > 5) //The object removes itself after 5 seconds (when it has no purpose anymore)
        {
            Destroy(gameObject);
        }
    }
}
