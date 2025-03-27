using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Weapon : MonoBehaviour
{


    public Camera PlayerCamera;


    [Header("Shooting")]
    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;


    [Header("Burst")]
    public int bulletsPerBurst = 3;
    public int BurstBulletsLeft;


    [Header("Spread")]
    public float spreadInsensity;


    [Header("Bullet Setting")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;   // seconds




    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    public void Awake()
    {
        readyToShoot = true;
        BurstBulletsLeft = bulletsPerBurst;
    }


    // Update is called once per frame
    void Update()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);


        }
        else if (currentShootingMode == ShootingMode.Single || 
            currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting)
        {
            BurstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }



        
        
    }

    private void FireWeapon()
    {

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;


        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        // Poiting the bullet to the shooting direction
        bullet.transform.forward = shootingDirection;
        // shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        // destroy the bullet after time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));


        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
        }


        //Burst mode
        if (currentShootingMode == ShootingMode.Burst && BurstBulletsLeft > 1)
        {
            BurstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }


    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {

            // hitting sth
            targetPoint = hit.point;
        }

        else
        {
            // shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadInsensity, spreadInsensity);
        float y = UnityEngine.Random.Range(-spreadInsensity, spreadInsensity);


        // Returnning shooting direction and spread
        return direction + new Vector3(x, y, 0);

    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
