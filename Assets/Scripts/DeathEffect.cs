using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<GameObject> GorePieces = new List<GameObject>();
    void Start()
    {
        foreach (GameObject Piece in GorePieces)
        {
            Instantiate(Piece, transform.position, Quaternion.identity);
            GorePiece PieceScript = Piece.GetComponent<GorePiece>();
            PieceScript.StartVelocity = new Vector2(Random.Range(-20f, 20f), Random.Range(10f, 50f)); 
            if(Random.Range(1,0)  == 0 )
            {
                PieceScript.StartRotation = Random.Range(100f, 600f);
            }
            else
            {
                PieceScript.StartRotation = Random.Range(-600f, -100f);
            }
            
            PieceScript.SlowdownFactor = 0.8f;
            PieceScript.SlowdownFactorRotation = 0.8f;
            Piece.transform.rotation = Quaternion.Euler(0,0,Random.Range(0f, 360f));
}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
