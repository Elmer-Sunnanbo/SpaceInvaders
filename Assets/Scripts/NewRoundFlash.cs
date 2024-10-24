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
        FlashRemainingTime -= Time.deltaTime;
        float DurationRemainingFraction = FlashRemainingTime / FlashTime;
        if(DurationRemainingFraction > 0 )
        {
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, DurationRemainingFraction);
        }
        else
        {
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 0);
        }
    }

    public void Flash()
    {
        FlashRemainingTime = FlashTime;
    }
}
