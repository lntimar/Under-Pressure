using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SentryScript : MonoBehaviour
{
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

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        Detect();

        if (playerTransform != null && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (canDrawLine && playerTransform != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, projectileSpawnPoint.position);
            lineRenderer.SetPosition(1, playerTransform.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void Detect()
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

    void Shoot()
    {
        Debug.Log("POW");

        GameObject bulletObj = Instantiate(sentryProjectile, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody bulletRigidbody = bulletObj.GetComponent<Rigidbody>();

        //direção jogador
        Vector3 directionToPlayer = (playerTransform.position - projectileSpawnPoint.position).normalized;

        bulletRigidbody.velocity = directionToPlayer * projectileSpeed;

        canDrawLine = false;
        StartCoroutine(StartDrawLineCooldown());
    }

    IEnumerator StartDrawLineCooldown()
    {
        yield return new WaitForSeconds(drawLineCooldown);
        canDrawLine = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
