using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    public Laser laserPrefab;
    public Shotgun shotgunPrefab;
    public Grenade grenadePrefab;

    private Laser laser;
    private Shotgun shotgun;
    private Grenade grenade;

    private float speed = 5f;

    private float chargeTime = 0.5f;
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private bool isFullyCharged = false;

    public enum WeaponType { Laser, Shotgun, Grenade }
    private WeaponType currentWeapon = WeaponType.Laser;
    Rigidbody2D Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
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
            //position.x -= speed * Time.deltaTime;
            HorizontalInput -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            //position.x += speed * Time.deltaTime;
            HorizontalInput += 1;
        }

        Rigidbody.velocity = new Vector2(HorizontalInput * speed, 0);
        /*
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        transform.position = position;
        */
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
                Debug.Log("Charging laser...");
            }

            chargeTimer += Time.deltaTime;

            if (chargeTimer >= chargeTime)
            {
                isFullyCharged = true;
                Debug.Log("Laser fully charged!");
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isFullyCharged && laser == null)
            {
                FireLaser();
            }
            else
            {
                Debug.Log("Charge interrupted.");
            }

            isCharging = false;
            chargeTimer = 0f;
            isFullyCharged = false;
        }
    }

    private void FireLaser()
    {
        laser = Instantiate(laserPrefab, transform.position + new Vector3(0, 2), Quaternion.identity);
        Debug.Log("Laser fired!");
    }

    private void FireInstantWeapon()
    {
        switch (currentWeapon)
        {
            case WeaponType.Shotgun:
                shotgun = Instantiate(shotgunPrefab, transform.position + new Vector3(0, 2), Quaternion.identity);
                Debug.Log("Shotgun fired!");
                break;
            case WeaponType.Grenade:
                grenade = Instantiate(grenadePrefab, transform.position + new Vector3(0, 2), Quaternion.identity);
                Debug.Log("Grenade thrown!");
                break;
        }
    }


    private void HandleWeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = WeaponType.Laser;
            Debug.Log("Switched to Laser");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = WeaponType.Shotgun;
            Debug.Log("Switched to Shotgun");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon = WeaponType.Grenade;
            Debug.Log("Switched to Grenade");
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

