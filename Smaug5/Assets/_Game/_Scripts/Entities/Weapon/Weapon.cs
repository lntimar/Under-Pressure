using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Variáveis Globais
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

    [Header("Ataque Corpo a Corpo")]
    public float meleeDistance;
    public float meleeDelay;
    public float meleeSpeed;
    public int meleeDamage;
    public LayerMask meleeAttackLayer;

    bool _attacking = false;
    bool _readyToAttack = true;
    //int _meleeAttackCount;

    //public const string MELEEATTACK = "With Gun MeleeAttack State Animation";
    //string currentAnimationState;

    //public GameObject hitEffect;
    //public AudioClip swordSwing;
    //public AudioClip hitSound;

    [Header("Gráficos:")]
    public GameObject gunModel;
    public TextMeshProUGUI ammoDisplay;
    Vector3 initialPosition;
    Vector3 recoilPosition;
    bool isRecoiling = false;
    public float recoilSpeed;

    [Header("Posições:")] 
    public Vector3 reloadArmsPos;
    public Vector3 reloadWeaponPos;

    [Header("Efeitos:")] 
    public GameObject enemyHitPrefab;
    public GameObject muzzleFlashPrefab;
    public Transform muzzlePoint;
    public GameObject bulletHolePrefab;

    [Header("Munição HUD:")]
    public Material lightAmmoMaterial;
    public Material unLightAmmoMaterial;
    public List<MeshRenderer> ammos = new List<MeshRenderer>();
    public MeshRenderer bigCoral;
    public Material bigCoralLightMaterial;
    public Material bigCoralUnLightMaterial;

    [Header("Referências:")]
    public Camera playerCamera;
    public Transform attackPoint;
    public LayerMask enemies;
    public LayerMask layerMaskIgnore;
    public Animator withGunStateAnimator;
    [SerializeField] private ScannerHUD scannerHud;

    // Referências:
    private GameMenu _gameMenuScript;
    #endregion

    #region Fun��es Unity
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        initialPosition = transform.localPosition; //Pega posi��o inicial
        recoilPosition = initialPosition + Vector3.up * recoilForce; //Posi��o que a arma vai ao atirar

        _gameMenuScript = FindObjectOfType<GameMenu>();
    }

    private void Update()
    {
        if (_gameMenuScript.IsPaused()) return;

        ShootingInput();
        VeriyReloadAnimation();

        if (Input.GetKeyDown(KeyCode.F))
        {
            MeleeAttack();
        }
    }
    #endregion

    #region Fun��es Pr�prias
    private void ShootingInput()
    {
        if (allowButtonHold) //Pode manter o bot�o segurado pra atirar
            shooting = Input.GetMouseButton(0);
        else
            shooting = Input.GetMouseButtonDown(0);

        //RECARREGAR se as balas acabarem, ou se apertar R
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
            Reload();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
            Reload();

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
                Debug.Log(hit.transform.name + " is hit!");
                enemyStats.ChangeHealthPoints(damage);
                var enemyHit = Instantiate(enemyHitPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                enemyHit.transform.position += enemyHit.transform.forward / 1000;
                Destroy(enemyHit, 1f);
            }
            else if (hit.transform.gameObject.layer != CollisionLayersManager.Instance.Door.Index)
            {
                var bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                bulletHole.transform.position += bulletHole.transform.forward / 1000;
                Destroy(bulletHole, 10f);
            }
        }

        // BALANÇAR CAMERA
        // BALANÇAR CAMERA
        //StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));

        //MUZZLE EFFECT
        //GameObject muzzleFlashInstance = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        //Destroy(muzzleFlashInstance, 0.1f);

        bulletsLeft--;
        bulletsShot--;

        // UI Munição
        if (scannerHud != null)
            scannerHud.SetAmmoBar();

        //REC�O
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
        
        scannerHud.ResetAmmoBar();
        
        bigCoral.sharedMaterial = bigCoralLightMaterial;
        withGunStateAnimator.Play("With Gun Default State Animation");
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private IEnumerator RotateWhileReloading()
    {
        while (reloading)
        {
            // Adicione a l�gica de rota��o aqui
            gunModel.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);

            yield return null;
        }
    }

    private void VeriyReloadAnimation() 
    {
        if (reloading)
            StartCoroutine(RotateWhileReloading());
        else
            gunModel.transform.localRotation = Quaternion.Euler(-90f, 0f, 90f);
    }

    private void MeleeAttack()
    {
        if (!_readyToAttack || _attacking) return;

        _readyToAttack = false;
        _attacking = true;

        withGunStateAnimator.SetTrigger("MeleeAttack");
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("enemy attack 2");

        Invoke(nameof(ResetMAttack), meleeSpeed);
        Invoke(nameof(MAttackRaycast), meleeDelay);

        //ChangeAnimationState(MELEEATTACK);

        //audioSource.pitch = Random.Range(0.9f, 1.1f);
        //audioSource.PlayOneShot(swordSwing);
    }

    private void ResetMAttack()
    {
        _attacking = false;
        _readyToAttack = true;
    }

    private void MAttackRaycast()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, meleeDistance, meleeAttackLayer))
        {
            Debug.Log("ACERTOU");

            //audioSource.pitch = 1;
            //audioSource.PlayOneShot(hitSound);

            EnemyStats enemyStats = hit.transform.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                //Debug.Log(hit.transform.name + " is hit!");
                enemyStats.ChangeHealthPoints(damage);
                var enemyHit = Instantiate(enemyHitPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                enemyHit.transform.position += enemyHit.transform.forward / 1000;
                Destroy(enemyHit, 1f);
            }
            else if (hit.transform.gameObject.layer != CollisionLayersManager.Instance.Door.Index)
            {
                var bulletHole = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                bulletHole.transform.position += bulletHole.transform.forward / 1000;
                Destroy(bulletHole, 10f);
            }
        }
    }

    /*public void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState) return;

        currentAnimationState = newState;
        withGunStateAnimator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }*/
    #endregion
}
