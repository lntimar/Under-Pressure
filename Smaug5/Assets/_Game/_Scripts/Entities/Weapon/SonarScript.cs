using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SonarScript : MonoBehaviour
{
    public float force = 700f;
    public List<GameObject> affectedObjects = new ();
    public PlayerStats playerStats;
    public int soulsRequired = 1;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (playerStats.Souls == soulsRequired)
            {
                Debug.Log("FUSRODAH");
                playerStats.Souls--;
                affectedObjects.ForEach(obj =>
                {
                    var rb = obj.GetComponent<Rigidbody>();
                    rb.AddExplosionForce(force, transform.position, 15f, 2f);
                });
            }
            else
            {
                Debug.Log("Sem almas o suficiente!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            affectedObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            affectedObjects.Remove(other.gameObject);
        }
    }
}
