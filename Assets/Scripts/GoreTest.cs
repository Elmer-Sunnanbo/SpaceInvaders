using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoreTest : MonoBehaviour
{
    EnemyCore MyCore;
    [SerializeField] GameObject MyDeath;
    // Start is called before the first frame update
    void Start()
    {
        MyCore = GetComponent<EnemyCore>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MyCore.HasBeenHit)
        {
            Destroy(gameObject);
            Instantiate(MyDeath);
            ScreenShake.Instance.ShakeCam(0.5f, 1f);
        }
    }
}
