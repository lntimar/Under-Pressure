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
    public int soulsRequired = 3;

    [Header("Objetos sendo Afetados:")]
    public List<GameObject> affectedObjects = new();

    [Header("Efeitos:")] 
    public GameObject sonarEffect;

    [Header("Referências:")]
    public Animator withGunStateAnimator;

    // Referências:
    private PlayerStats playerStats;
    private GameMenu _gameMenuScript;
    #endregion

    #region Funções Unity
    private void Awake() => _gameMenuScript = FindObjectOfType<GameMenu>();

    private void Start() => playerStats = FindObjectOfType<PlayerStats>();

    private void Update()
    {
        if (_gameMenuScript.IsPaused()) return;

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Almas: " + PlayerStats.Souls);

            if (PlayerStats.Souls >= soulsRequired)
            {
                withGunStateAnimator.Play("With Gun Shoot State Animation");
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("weapon sonar");
                Debug.Log("FUSRODAH");
                sonarEffect.gameObject.SetActive(true);
                playerStats.ChangeOrbSouls(-3);
                affectedObjects.ForEach(obj =>
                {
                    // Caso for o inimigo, ativar ragdoll
                    if (obj.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour enemyBehaviour))
                    {
                        var ragdollScript = enemyBehaviour.transform.Find("Enemy Body").gameObject.GetComponent<EnemyRagdoll>();
                        ragdollScript.StartRagdoll();
                        Invoke("ragdollScript.StopRagdoll", 3f);
                    }

                    var rb = obj.GetComponent<Rigidbody>();
                    rb.AddExplosionForce(force, transform.position, 15f, 2f);

                    playerStats.ChangeOrbSouls(-3);
                });
            }
           else if (PlayerStats.Souls < soulsRequired)
            {
                Debug.Log("Sem almas o suficiente!");
                return;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (rb != null && other.gameObject.CompareTag("Enemy"))
        {
            affectedObjects.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            affectedObjects.Remove(other.gameObject);
        }
    }
    #endregion
}
