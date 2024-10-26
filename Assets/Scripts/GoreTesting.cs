using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoreTesting : MonoBehaviour
{
    /// <summary>
    /// This script is a test script.
    /// </summary>
    [SerializeField] GameObject AttackPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(AttackPrefab, new Vector2(0,-12), Quaternion.identity);
        }
    }
}
