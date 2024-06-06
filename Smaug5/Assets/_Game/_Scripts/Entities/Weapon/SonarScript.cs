using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class SonarScript : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Configurações:")]
    public float force = 700f;
    public int soulsRequired = 1;

    [Header("Objetos sendo Afetados:")]
    public List<GameObject> affectedObjects = new();

    private PlayerStats playerStats;
    #endregion

    #region Funções Unity
    private void Start() => playerStats = FindObjectOfType<PlayerStats>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (PlayerStats.Souls == soulsRequired)
            {
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("weapon sonar");
                Debug.Log("FUSRODAH");
                playerStats.ChangeOrbSouls(-1);
                affectedObjects.ForEach(obj =>
                {
                    // Caso for o inimigo, ativar ragdoll
                    if (obj.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemyBehaviour))
                    {
                        var ragdollScript = enemyBehaviour.transform.Find("Enemy Body").gameObject.GetComponent<EnemyRagdoll>();
                        ragdollScript.StartRagdoll();
                    }

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
    #endregion
}
