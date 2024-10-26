using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public Laser laserPrefab;
    private Laser laser;

    //public Shotgun shotgunPrefab;
    //public Grenade grenadePrefab;

    //private Shotgun shotgun;
    //private Grenade grenade;



    [SerializeField] float speed;

    private float chargeTime = 1f;
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private bool isFullyCharged = false;

    [SerializeField] AudioSource ChargeSource;
    [SerializeField] AudioSource PingSource;
    [SerializeField] AudioSource FireSource;

    /*
    public enum WeaponType { Laser, Shotgun, Grenade }
    private WeaponType currentWeapon = WeaponType.Laser;
    */

    private Rigidbody2D Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        //HandleWeaponSwitching();
        HandleLaserChargingAndFiring();
    }

    /// <summary>
    /// Handles movement
    /// </summary>
    private void HandleMovement()
    {
        //Gets the horizontal direction to move in
        int HorizontalInput = 0;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            HorizontalInput -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            HorizontalInput += 1;
        }

        //Applies the horizontal direction to the velocity
        Rigidbody.velocity = new Vector2(HorizontalInput * speed, 0);
    }

    /// <summary>
    /// Handles the charging laser
    /// </summary>
    private void HandleLaserChargingAndFiring()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isCharging) //Start charging if we're not
            {
                isCharging = true;
                chargeTimer = 0f;
                isFullyCharged = false;

                ChargeSource.Play(); //Start playing the charge sound
            }

            chargeTimer += Time.deltaTime;

            if (chargeTimer >= chargeTime && !isFullyCharged) //If the wait is over, set the laser to done charging.
            {
                isFullyCharged = true;

                ChargeSource.Stop(); //Stop the charge sound
                PingSource.Play(); // DING!
                ScreenShake.Instance.ShakeCam(0.1f, 0.1f);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) //When the charge key is released
        {
            ChargeSource.Stop(); //Stop the charge sound

            if (isFullyCharged && laser == null) //If the charge was completed
            {
                FireLaser();
            }

            //Reset the charge to 0
            isCharging = false;
            chargeTimer = 0f;
            isFullyCharged = false;
        }
    }

    /// <summary>
    /// Fires the laser projectile
    /// </summary>
    private void FireLaser()
    {
        laser = Instantiate(laserPrefab, transform.position + new Vector3(0, 2), Quaternion.identity);

        FireSource.Play();
        ScreenShake.Instance.ShakeCam(0.15f, 0.3f);
    }

    /*
    private void FireInstantWeapon()
    {
        switch (currentWeapon)
        {
            case WeaponType.Shotgun:
                if (shotgunPrefab != null)
                {
                    shotgun = Instantiate(shotgunPrefab, transform.position + new Vector3(0, 2), Quaternion.identity);
                }
                break;
            case WeaponType.Grenade:
                if (grenadePrefab != null)
                {
                    grenade = Instantiate(grenadePrefab, transform.position + new Vector3(0, 2), Quaternion.identity);
                }
                break;
        }
    }
    */

    /*
    private void HandleWeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = WeaponType.Laser;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = WeaponType.Shotgun;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = WeaponType.Grenade;
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D collision) //When hit by something
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BouncyInvader") || collision.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            GameManager.Instance.GameOver(); //You died
        }
    }
}