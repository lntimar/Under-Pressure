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
    Vector3 initialPosition;
    Vector3 recoilPosition;
    bool isRecoiling = false;
    public float recoilSpeed;

    [Header("Posições")] 
    public Vector3 reloadArmsPos;
    public Vector3 reloadWeaponPos;

    [Header("Efeitos")] 
    public GameObject enemyHitPrefab;
    public GameObject muzzleFlashPrefab;
    public Transform muzzlePoint;
    public GameObject bulletHolePrefab;

    [Header("Municao HUD")]
    public Material lightAmmoMaterial;
    public Material unLightAmmoMaterial;
    public List<MeshRenderer> ammos = new List<MeshRenderer>();
    public MeshRenderer bigCoral;
    public Material bigCoralLightMaterial;
    public Material bigCoralUnLightMaterial;

    [Header("Referências")]
    public Camera playerCamera;
    public Transform attackPoint;
    public LayerMask enemies;
    public LayerMask layerMaskIgnore;
    public Animator withGunStateAnimator;
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

        /*
        if (isRecoiling)
        {
            //O Lerp move a posição inicial pra posição de tiro quando o bool isRecoiling fica verdadeirol, quando o jogador atira
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * 10f);

            if (Vector3.Distance(transform.localPosition, initialPosition) < 0.01f)
            {
                isRecoiling = false;
            }

            withGunStateAnimator.Play("With Gun Shoot State Animation");
            //StartCoroutine(RecoilWhileShooting());
        }
        */
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
            withGunStateAnimator.Play("With Gun Shoot State Animation");
            var muzzleFlash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, Quaternion.LookRotation(muzzlePoint.transform.forward));
            muzzleFlash.transform.parent = transform;
            Destroy(muzzleFlash, 0.1f);
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("weapon shot");
            bulletsShot = bulletsPerTap;

            for (int i = 0; i < ammos.Count; i++)
            {
                if (ammos[i].sharedMaterial != unLightAmmoMaterial)
                {
                    ammos[i].sharedMaterial = unLightAmmoMaterial;

                    if (i == ammos.Count - 1)
                        bigCoral.sharedMaterial = bigCoralUnLightMaterial;

                    break;
                }
            }
            ShootHitscan();
        }
    }

    private void ShootHitscan()
    {
        readyToShoot = false;

        //transform.localPosition = recoilPosition;

        isRecoiling = true;

        //PROPAGAÇÃO DE BALA
        float x = Random.Range(-horizontalSpread, horizontalSpread);
        float y = Random.Range(-horizontalSpread, verticalSpread);
        Vector3 direction = playerCamera.transform.forward + new Vector3(x, y, 0);

        //RAYCAST
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, direction, out hit, range, ~layerMaskIgnore))
        {
            EnemyStats enemyStats = hit.transform.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                //Debug.Log(hit.transform.name + " is hit!");
                enemyStats.ChangeHealthPoints(damage);
                var enemyHit = Instantiate(enemyHitPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                enemyHit.transform.position += enemyHit.transform.forward / 1000;
                Destroy(enemyHit, 1f);
            }
            else
            {
                //Debug.Log(hit.transform.name);
                var bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                bulletHole.transform.position += bulletHole.transform.forward / 1000;
                Destroy(bulletHole, 10f);
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
        withGunStateAnimator.Play("With Gun Reload State Animation");
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("weapon reload");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        for (int i = 0; i < ammos.Count; i++)
        {
            if (ammos[i].sharedMaterial != lightAmmoMaterial)
                ammos[i].sharedMaterial = lightAmmoMaterial;
        }
        bigCoral.sharedMaterial = bigCoralLightMaterial;
        withGunStateAnimator.Play("With Gun Default State Animation");
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
