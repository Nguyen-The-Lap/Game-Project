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
    private int BurstBulletsLeft;

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

    void Awake()
    {
        readyToShoot = true;
        BurstBulletsLeft = bulletsPerBurst;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) return;

        // Nhấn R để reload thủ công
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Xác định trạng thái bắn theo chế độ
        if (currentShootingMode == ShootingMode.Auto)
            isShooting = Input.GetKey(KeyCode.Mouse0);
        else
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Bắn nếu đủ đạn
        if (readyToShoot && isShooting && currentAmmo > 0)
        {
            BurstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        // Tự reload khi hết đạn
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

        // Muzzle flash
        if (muzzleEffect != null)
            muzzleEffect.GetComponent<ParticleSystem>().Play();

        // Tính hướng bắn
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // Tạo đạn
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        // Reset bắn
        if (allowReset)
            Invoke("ResetShot", shootingDelay);

        // Burst mode
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

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(100);
        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = Random.Range(-spreadInsensity, spreadInsensity);
        float y = Random.Range(-spreadInsensity, spreadInsensity);
        return direction + new Vector3(x, y, 0);
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
            weaponAnimator.SetTrigger("ReloadTrigger");

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);
        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;
        readyToShoot = true;
    }
}
