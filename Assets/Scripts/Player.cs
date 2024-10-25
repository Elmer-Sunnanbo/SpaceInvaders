using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public Laser laserPrefab;
    public Shotgun shotgunPrefab;
    public Grenade grenadePrefab;

    private Laser laser;
    private Shotgun shotgun;
    private Grenade grenade;

    [SerializeField] float speed;

    private float chargeTime = 1f;
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private bool isFullyCharged = false;

    [SerializeField] private AudioClip chargingSound;     
    [SerializeField] private AudioClip fullyChargedSound;
    [SerializeField] private AudioClip fireSound;
    private AudioSource audioSource; 

    public enum WeaponType { Laser, Shotgun, Grenade }
    private WeaponType currentWeapon = WeaponType.Laser;
    private Rigidbody2D Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleMovement();
        HandleWeaponSwitching();

        if (currentWeapon == WeaponType.Laser)
        {
            HandleLaserChargingAndFiring();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireInstantWeapon();
            }
        }
    }

    private void HandleMovement()
    {
        Vector3 position = transform.position;

        int HorizontalInput = 0;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            HorizontalInput -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            HorizontalInput += 1;
        }

        Rigidbody.velocity = new Vector2(HorizontalInput * speed, 0);
    }

    private void HandleLaserChargingAndFiring()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isCharging)
            {
                isCharging = true;
                chargeTimer = 0f;
                isFullyCharged = false;

                if (chargingSound != null)
                {
                    audioSource.clip = chargingSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }

            chargeTimer += Time.deltaTime;

            if (chargeTimer >= chargeTime && !isFullyCharged)
            {
                isFullyCharged = true;

                if (chargingSound != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                if (fullyChargedSound != null)
                {
                    audioSource.PlayOneShot(fullyChargedSound);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (audioSource.isPlaying && audioSource.clip == chargingSound)
            {
                audioSource.Stop();
            }

            if (isFullyCharged && laser == null)
            {
                FireLaser();
            }

            isCharging = false;
            chargeTimer = 0f;
            isFullyCharged = false;
        }
    }


    private void FireLaser()
    {
        laser = Instantiate(laserPrefab, transform.position + new Vector3(0, 2), Quaternion.identity);

        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

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
        
        if(fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Missile") || collision.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            GameManager.Instance.OnPlayerKilled(this);
        }
    }
}