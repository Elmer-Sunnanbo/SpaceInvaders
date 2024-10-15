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

    public enum WeaponType { Laser, Shotgun, Grenade }
    private WeaponType currentWeapon = WeaponType.Laser;

    void Update()
    {
        HandleMovement();
        HandleShooting();
        HandleWeaponSwitching();
    }

    private void HandleMovement()
    {
        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            position.x += speed * Time.deltaTime;
        }

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);
        transform.position = position;
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentWeapon)
            {
                case WeaponType.Laser:
                    if (laser == null)
                    {
                        laser = Instantiate(laserPrefab, transform.position + new Vector3(0, 2), Quaternion.identity);
                    }
                    break;
                case WeaponType.Shotgun:
                    if (shotgun == null)
                    {
                        shotgun = Instantiate(shotgunPrefab, transform.position + new Vector3(0, 2), Quaternion.identity);
                    }
                    break;
                case WeaponType.Grenade:
                    if (grenade == null)
                    {
                        grenade = Instantiate(grenadePrefab, transform.position + new Vector3(0, 2), Quaternion.identity);
                    }
                    break;
            }
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
