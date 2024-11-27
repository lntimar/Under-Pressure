using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class SonarScript : MonoBehaviour
{
    #region Vari�veis Globais
    // Inspector:
    [Header("Configura��es:")]
    public float force = 700f;
    public int soulsRequired = 3;

    [Header("Objetos sendo Afetados:")]
    public List<GameObject> affectedObjects = new();

    [Header("Efeitos:")] 
    public GameObject sonarEffect;

    [Header("Refer�ncias:")]
    public Animator withGunStateAnimator;

    // Refer�ncias:
    private PlayerStats playerStats;
    private GameMenu _gameMenuScript;
    #endregion

    #region Fun��es Unity
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
                    Debug.Log("xixi");
                    // Caso for o inimigo, ativar ragdoll
                    if (obj.TryGetComponent<NewEnemyBehaviour>(out NewEnemyBehaviour enemyBehaviour))
                    {
                        Debug.Log("cocô");
                        var ragdollScript = enemyBehaviour.gameObject.GetComponent<EnemyRagdoll>();
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
        //_rb != null
        if (other.gameObject.CompareTag("Enemy"))
        {
            affectedObjects.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        //_rb != null
        if (other.CompareTag("Enemy"))
        {
            affectedObjects.Remove(other.gameObject);
        }
    }
    #endregion
}
