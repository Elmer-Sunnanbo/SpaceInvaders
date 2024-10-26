using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodTrail : MonoBehaviour
{
    SpriteRenderer SR;

    [SerializeField] Color StartColor;
    [SerializeField] float MinBrightness;
    [SerializeField] float FadeTime;
    [SerializeField] float ShrinkTime;
    float TimeElapsed = 0;
    public float RemainingBrightness = 1;
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Reduces the brightness of the color until it reaches MinBrightness
        RemainingBrightness -= Time.deltaTime / FadeTime;
        RemainingBrightness = Mathf.Clamp(RemainingBrightness, MinBrightness, 1); //Stop the brightness from going below MinBrightness
        SR.color = new Color(StartColor.r * RemainingBrightness, StartColor.g * RemainingBrightness, StartColor.b * RemainingBrightness); //Applies the brightness to the color

        
        if (TimeElapsed>ShrinkTime) //If the object is done shrinking away
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localScale = Vector2.one * (1 - (TimeElapsed / ShrinkTime)); //Shrinks the object
        }
        TimeElapsed += Time.deltaTime;
    }
}
