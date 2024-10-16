using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SentryScript : MonoBehaviour
{
    public float projectileSpeed;
    [SerializeField] private float timer = 5;
    public GameObject sentryProjectile;
    public Transform projectileSpawnPoint;
    public float detectionRange = 20f;
    public LayerMask playerLayer;
    private float bulletTime;
    private Transform playerTransform;

    void Update()
    {
        Detect();

        if (playerTransform != null)
        {
            Shoot();
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
        bulletTime -= Time.time;

        if (bulletTime > 0)
        {
            return;
        }

        bulletTime = timer;

        GameObject bulletObj = Instantiate(sentryProjectile, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation) as GameObject;
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * projectileSpeed);
        Destroy(bulletObj, 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
