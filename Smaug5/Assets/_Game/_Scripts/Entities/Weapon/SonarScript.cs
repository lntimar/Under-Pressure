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
            if (PlayerStats.Souls >= soulsRequired)
            {
                withGunStateAnimator.Play("With Gun Shoot State Animation");
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("weapon sonar");
                sonarEffect.gameObject.SetActive(true);
                playerStats.ChangeOrbSouls(-3);
                affectedObjects.ForEach(obj =>
                {
                    // Caso for o inimigo, ativar ragdoll
                    if (obj.TryGetComponent<NewEnemyBehaviour>(out NewEnemyBehaviour enemyBehaviour))
                    {
                        var ragdollScript = enemyBehaviour.gameObject.GetComponent<EnemyRagdoll>();
                        ragdollScript.StartRagdoll();
                        Invoke("ragdollScript.StopRagdoll", 0.1f);
                    }
                    
                    var rb = obj.GetComponent<Rigidbody>();
                    rb.velocity = Vector3.zero;
                    rb.AddExplosionForce(force, transform.position, 15f, 0.8f);
                });
                
                playerStats.ChangeOrbSouls(-3);
                affectedObjects.Clear();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            affectedObjects.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            affectedObjects.Remove(other.gameObject);
        }
    }
    #endregion
}
