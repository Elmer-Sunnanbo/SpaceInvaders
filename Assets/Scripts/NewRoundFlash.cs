using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoundFlash : MonoBehaviour
{
    [SerializeField] float FlashTime;
    float FlashRemainingTime;
    SpriteRenderer SR;

    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FlashRemainingTime -= Time.deltaTime; //Reduce the remaining
        float DurationRemainingFraction = FlashRemainingTime / FlashTime; //Gets the remaining duration as a number between 1 and 0

        if(DurationRemainingFraction > 0 )
        {
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, DurationRemainingFraction); //Change the alpha to the number
        }
        else
        {
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 0); //Change the alpha to 0
        }
    }

    /// <summary>
    /// Causes the flash to activate.
    /// </summary>
    public void Flash()
    {
        FlashRemainingTime = FlashTime;
    }
}
