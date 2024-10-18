using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SentryScript : MonoBehaviour
{
    #region Variáveis
    public float projectileSpeed;
    [SerializeField] private float fireRate = 5;
    public GameObject sentryProjectile;
    public Transform projectileSpawnPoint;
    public float detectionRange = 20f;
    public LayerMask playerLayer;
    private float nextFireTime;
    private Transform playerTransform;
    private bool canDrawLine = true;
    [SerializeField] private float drawLineCooldown = 0.5f;
    private LineRenderer lineRenderer;
    #endregion

    #region Funções Unity
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        Detect();
        VerifyToShoot();
        DrawLine();
    }
    #endregion

    #region Funções Próprias
    private void Detect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        if (hitColliders.Length > 0)
        {
            playerTransform = hitColliders[0].transform;
            transform.LookAt(playerTransform);
        }
        else
        {
            playerTransform = null;
        }
    }

    private void VerifyToShoot()
    {
        if (playerTransform != null && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        //Debug.Log("POW");

        GameObject bulletObj = Instantiate(sentryProjectile, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody bulletRigidbody = bulletObj.GetComponent<Rigidbody>();

        //direção jogador
        Vector3 directionToPlayer = (playerTransform.position - projectileSpawnPoint.position).normalized;

        bulletRigidbody.velocity = directionToPlayer * projectileSpeed;

        canDrawLine = false;
        StartCoroutine(StartDrawLineCooldown());
    }

    private IEnumerator StartDrawLineCooldown()
    {
        yield return new WaitForSeconds(drawLineCooldown);
        canDrawLine = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void DrawLine() 
    {
        if (canDrawLine && playerTransform != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, projectileSpawnPoint.position);
            lineRenderer.SetPosition(1, playerTransform.position + Vector3.down * 0.35f);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
    #endregion
}
