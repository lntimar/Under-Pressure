using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Estatísticas Gerais")]
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

    [Header("Gráficos")]
    public GameObject gunModel;
    //public GameObject muzzleFlash;
    public TextMeshProUGUI ammoDisplay;
    public GameObject impactEffect;
    Vector3 initialPosition;
    Vector3 recoilPosition;
    bool isRecoiling = false;
    public float recoilSpeed;

    [Header("Referências")]
    public Camera playerCamera;
    public Transform attackPoint;
    public LayerMask enemies;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        initialPosition = transform.localPosition; //Pega posição inicial
        recoilPosition = initialPosition + Vector3.up * recoilForce; //Posição que a arma vai ao atirar
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
            StartCoroutine(RotateWhileReloading());
            //Adicionar animação de recarregar quando tiver uma
        }
        else
        {
            gunModel.transform.localRotation = Quaternion.Euler(-90f, 0f, 90f);
        }

        if (isRecoiling)
        {
            //O Lerp move a posição inicial pra posição de tiro quando o bool isRecoiling fica verdadeirol, quando o jogador atira
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * 10f);

            if (Vector3.Distance(transform.localPosition, initialPosition) < 0.01f)
            {
                isRecoiling = false;
            }

            //StartCoroutine(RecoilWhileShooting());
        }
    }
    #endregion

    #region Funções Próprias
    private void ShootingInput()
    {
        if (allowButtonHold) //Pode manter o botão segurado pra atirar
        {
            shooting = Input.GetMouseButton(0);
        }
        else
        {
            shooting = Input.GetMouseButtonDown(0);
        }

        //RECARREGAR se as balas acabarem, ou se apertar R
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
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("weapon shot");
            bulletsShot = bulletsPerTap;
            ShootHitscan();
        }
    }

    private void ShootHitscan()
    {
        readyToShoot = false;

        transform.localPosition = recoilPosition;

        isRecoiling = true;

        //PROPAGAÇÃO DE BALA
        float x = Random.Range(-horizontalSpread, horizontalSpread);
        float y = Random.Range(-horizontalSpread, verticalSpread);
        Vector3 direction = playerCamera.transform.forward + new Vector3(x, y, 0);

        //RAYCAST
        int layerMaskIgnore = ~(1 << LayerMask.NameToLayer("IgnoreRaycast"));

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, direction, out hit, range, layerMaskIgnore))
        {
            EnemyStats enemyStats = hit.transform.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                Debug.Log(hit.transform.name + " is hit!");
                enemyStats.ChangeHealthPoints(damage);
            }
            else
            {
                //Debug.Log(hit.transform.name);
                GameObject impactPrefab = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactPrefab, 3f);
            }
        }

        //BALANÇAR CAMERA
        //BALANÇAR CAMERA
        //StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));

        //MUZZLE EFFECT
        //GameObject muzzleFlashInstance = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        //Destroy(muzzleFlashInstance, 0.1f);

        bulletsLeft--;
        bulletsShot--;

        //RECÚO
        //playerRb.AddForce(-direction * recoilForce, ForceMode.Impulse);

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
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("weapon reload");
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
            // Adicione a lógica de rotação aqui
            gunModel.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);

            yield return null;
        }
    }

    /*
    IEnumerator RecoilWhileShooting()
    {
        while (isRecoiling)
        {
            gunModel.transform.Rotate(0, Time.deltaTime * recoilSpeed, 0);

            yield return null;
        }
    }
    */
    #endregion
}
