using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Vari�veis Globais
    [Header("Estat�sticas Gerais")]
    public int damage;
    public float timeBetweenShooting;
    public float horizontalSpread;
    public float verticalSpread;
    public float range;
    public float reloadTime;
    public float timeBetweenShots;
    public int magazineSize;
    public int bulletsPerTap;
    public bool allowButtonHold;
    public float rotationSpeed;
    public float recoilForce;
    int bulletsLeft;
    int bulletsShot;
    bool shooting;
    bool readyToShoot;
    bool reloading;

    [Header("Gr�ficos")]
    public GameObject gunModel;
    //public GameObject muzzleFlash;
    public TextMeshProUGUI ammoDisplay;
    public GameObject impactEffect;

    [Header("Refer�ncias")]
    public Camera playerCamera;
    public Transform attackPoint;
    public LayerMask enemies;
    public Rigidbody playerRb;
    #endregion

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        ShootingInput();

        if (ammoDisplay != null)
        {
            ammoDisplay.SetText(bulletsLeft + " / " + magazineSize);
        }

        if (reloading)
        {
            //gunSprite.transform.localRotation = Quaternion.Euler(60f, 0f, 0f);
            StartCoroutine(RotateWhileReloading());
            //Adicionar anima��o de recarregar quando tiver uma
        }
        else
        {
            gunModel.transform.localRotation = Quaternion.identity;
        }
    }

    private void ShootingInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetMouseButton(0);
        }
        else
        {
            shooting = Input.GetMouseButtonDown(0);
        }

        //RECARREGAR
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        //ATIRAR
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            ShootHitscan();
        }
    }

    private void ShootHitscan()
    {
        readyToShoot = false;

        //PROPAGA��O DE BALA
        float x = Random.Range(-horizontalSpread, horizontalSpread);
        float y = Random.Range(-horizontalSpread, verticalSpread);
        Vector3 direction = playerCamera.transform.forward + new Vector3(x, y, 0);

        //RAYCAST
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);

            EnemyStats enemyStats = hit.transform.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                //enemyStats.TakeDamage(damage);
            }
            else
            {
                GameObject impactPrefab = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactPrefab, 3f);
            }
        }

        //BALAN�AR CAMERA
        //StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));

        //MUZZLE EFFECT
        //GameObject muzzleFlashInstance = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        //Destroy(muzzleFlashInstance, 0.1f);

        bulletsLeft--;
        bulletsShot--;

        //REC�O
        playerRb.AddForce(-direction * recoilForce, ForceMode.Impulse);

        Invoke("ResetShot", timeBetweenShooting);
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("ShootHitscan", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        Debug.Log("RECARREGANDO!");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    IEnumerator RotateWhileReloading()
    {
        while (reloading)
        {
            // Adicione a l�gica de rota��o aqui
            gunModel.transform.Rotate(Time.deltaTime * rotationSpeed, 0, 0);

            yield return null;
        }
    }
}
