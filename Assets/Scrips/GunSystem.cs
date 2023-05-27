using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Rendering.HighDefinition;

// https://www.youtube.com/watch?v=bqNW08Tac0Y&ab_channel=Dave%2FGameDevelopment
// https://www.youtube.com/watch?v=_NjErIEp9tU&ab_channel=TripleQuotes
// https://forum.unity.com/threads/light-intensity-doesnt-work-with-hdrp.706382/

public class GunSystem : MonoBehaviour
{
    [Header("General References")]
    public GameObject Parent;
    public GameObject AimingParent;
    public GameObject RifleObject;
    public GameObject fpsArms;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI damageText;

    [Header("Gun Audio")]
    public AudioSource shootingAudio;
    public AudioSource reloadAudio;
    public AudioSource noAmmo;


    [Header("Camera")]
    public Camera fpsCam;
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;

    [Header("Aiming")]
    public Transform originalAimingPosition;
    public Transform aimingPosition;
    public float aimingSpeed = 0.1f;


    //Gun stats
    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    // public Transform attackPoint;
    public bool IsAiming = false;
    public RaycastHit rayHit;
    public LayerMask Shootable;

    [Header("Gun Particles")]
    public ParticleSystem muzzleFlash;
    public GameObject lightObject;
    private HDAdditionalLightData muzzleFlashLight;

    public GameObject damageGraphic, bulletHoleGraphic;
    public float damageEffectDuration = 3f;
    public float bulletHoleDuration = 15f;

    RecoilSystem RecoilSystem;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Start()
    {
        RecoilSystem = GetComponent<RecoilSystem>();
        muzzleFlashLight = lightObject.GetComponent<HDAdditionalLightData>();
    }

    private void Update()
    {
        if (fpsArms != null)
        {
            fpsArms.SetActive(false);
        }


        if (transform.parent != null && (transform.parent.gameObject == Parent || transform.parent.gameObject == AimingParent))
        {
            MyInput();

            ammoText.SetText(bulletsLeft + " / " + magazineSize);
            damageText.SetText(damage + " DMG");

            if (fpsArms != null)
            {
                fpsArms.SetActive(true);
            }
        }
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        IsAiming = Input.GetKey(KeyCode.Mouse1);
        if (IsAiming)
        {
            RifleObject.transform.SetParent(aimingPosition, false);
            transform.position = Vector3.Lerp(transform.position, aimingPosition.position, aimingSpeed * Time.deltaTime);
        }
        else
        {
            RifleObject.transform.SetParent(originalAimingPosition, false);
            transform.position = Vector3.Lerp(transform.position, originalAimingPosition.position, aimingSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            RecoilSystem.ApplyRecoil();
            Shoot();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            bulletsShot = bulletsPerTap;
            noAmmo.Play();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, Shootable))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
                GameObject damageEffect = Instantiate(damageGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
                Destroy(damageEffect, damageEffectDuration);
            }
        }

        // Camera Shake
        StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));
        StartCoroutine(ResetCameraPosition(camShakeDuration));

        // Muzzle Flash
        muzzleFlash.Play();
        StartCoroutine(MuzzleFlashLight());

        //Audio
        shootingAudio.Play();

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        reloadAudio.Play();
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private IEnumerator ResetCameraPosition(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.localPosition = Vector3.zero; // Reset the camera position
    }

    IEnumerator MuzzleFlashLight()
    {
        muzzleFlashLight.intensity = 800f;
        yield return new WaitForSeconds(0.1f);
        muzzleFlashLight.intensity = 0f;
    }
}