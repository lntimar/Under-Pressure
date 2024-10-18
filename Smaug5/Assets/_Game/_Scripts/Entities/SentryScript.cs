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

    void Update()
    {
        Detect();

        if (playerTransform != null && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
