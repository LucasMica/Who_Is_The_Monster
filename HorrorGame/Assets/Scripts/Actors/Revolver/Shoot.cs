using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public static event System.Action OnShoot;

    public GameObject bulletPrefab;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    public Rigidbody playerRb;
    public float recoilForce;

    bool shooting, reloading;
    public bool readyToShoot;

    public Camera mainCamera;
    public Transform attackPoint;

    public bool allowInvoke = true;

    public ParticleSystem muzzleFlash;

    private PlayerInput playerInput;
    private InputAction shootAction;
    private InputActionMap interactionMap;

    private AudioSource audioSource;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = false;

        playerInput = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerInput>();
        interactionMap = playerInput.actions.FindActionMap("Interaction");
        shootAction = interactionMap.FindAction("Shooting");

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        MyInput();
    }

    public static void TriggerOnShoot()
    {
        OnShoot?.Invoke();
    }

    private void MyInput()
    {
        if(allowButtonHold) shooting = shootAction.IsPressed();
        else shooting = shootAction.WasPressedThisFrame();

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            ShootBullet();
            TriggerOnShoot();
        }
    }

    private void ShootBullet()
    {
        readyToShoot = false;
        muzzleFlash.Play();
        audioSource.PlayOneShot(shootingSound, 0.7f);

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        if (bulletsShot < bulletPerTap && bulletsLeft > 0)
        {
            Invoke("ShootBullet", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        Debug.Log("Reloading");
        reloading = true;
        StartCoroutine(ReloadAnimation());
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private IEnumerator ReloadAnimation()
    {
        audioSource.PlayOneShot(reloadingSound, 0.9f);
        float elapsedTime = 0f;

        Quaternion initialRotation = transform.localRotation;
        Quaternion objectiveRotation = transform.localRotation * Quaternion.Euler(-90f, 0f, 0f);

        while (elapsedTime < reloadTime)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, objectiveRotation, (elapsedTime / reloadTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = objectiveRotation;

        elapsedTime = 0f;

        while (elapsedTime < reloadTime / 2)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, (elapsedTime / reloadTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = initialRotation;

        ReloadFinished();
    }
}
