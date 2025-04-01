using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    public Camera PlayerCamera;
    public Animator weaponAnimator;

    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    public int reserveAmmo = 90;
    public float reloadTime = 2f;
    private bool isReloading = false;

    [Header("Shooting")]
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 0.2f;

    [Header("Burst")]
    public int bulletsPerBurst = 3;

    [Header("Spread")]
    public float spreadInsensity;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;

    public enum ShootingMode { Single, Burst, Auto }
    public ShootingMode currentShootingMode;

    private bool isAiming;

    void Awake()
    {
        readyToShoot = true;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) return;

        isAiming = Input.GetKey(KeyCode.Mouse1);

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        isShooting = (currentShootingMode == ShootingMode.Auto) ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && isShooting && currentAmmo > 0)
        {
            if (currentShootingMode == ShootingMode.Burst)
                StartCoroutine(FireBurst());
            else
                FireWeapon();
        }

        if (currentAmmo <= 0 && reserveAmmo > 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void FireWeapon()
    {
        if (currentAmmo <= 0) return;

        currentAmmo--;
        readyToShoot = false;

        if (muzzleEffect != null)
            muzzleEffect.GetComponent<ParticleSystem>().Play();

        Vector3 shootingDirection = CalculateDirectionAndSpread();

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
            Invoke("ResetShot", shootingDelay);
    }

    private IEnumerator FireBurst()
    {
        for (int i = 0; i < bulletsPerBurst; i++)
        {
            if (currentAmmo <= 0) break;
            FireWeapon();
            yield return new WaitForSeconds(shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 direction = ray.direction;

        float spread = isAiming ? spreadInsensity * 0.5f : spreadInsensity;
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        direction += PlayerCamera.transform.right * x + PlayerCamera.transform.up * y;
        return direction.normalized;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        readyToShoot = false;

        if (weaponAnimator != null)
        {
            weaponAnimator.SetTrigger("ReloadTrigger");
            yield return new WaitForSeconds(weaponAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            yield return new WaitForSeconds(reloadTime);
        }

        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);
        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;
        readyToShoot = true;
    }
}
