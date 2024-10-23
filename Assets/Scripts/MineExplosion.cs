using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Spike;
    [SerializeField] float SpikeSpeed;
    [SerializeField] float SpikeCount;
    void Start()
    {
        RaycastHit2D[] Hits = Physics2D.CircleCastAll(transform.position, 2, Vector2.zero, 0);
        foreach(RaycastHit2D Hit in Hits)
        {
            if(Hit.collider.gameObject.TryGetComponent(out EnemyCore FoundCore))
            {
                FoundCore.Hit();
            }
        }
        for (int i = 0; i < SpikeCount; i++)
        {
            float DegreeDirection = 360 * i / SpikeCount;
            Vector2 VectorDirection = (Vector2)(Quaternion.Euler(0, 0, DegreeDirection) * Vector2.right);
            GameObject RecentSpike = Instantiate(Spike, transform.position, Quaternion.identity);
            RecentSpike.GetComponent<MineDeathSpike>().Go(SpikeSpeed, VectorDirection);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
