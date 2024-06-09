using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulScript : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Configurações:")]
    public int soulValue;
    public float rotationSpeed = 1.0f;
    public float floatStrength = 0.5f;
    public float chaseSpeed;

    [Header("Referências:")]
    public LayerMask PlayerLayer;

    private static PlayerStats playerStatsScript;
    //public TextMeshProUGUI soulDisplay;

    private Vector3 floatStartPosition;

    private bool chasePlayer = false;
    #endregion

    private void Start()
    {
        playerStatsScript = FindObjectOfType<PlayerStats>();
        floatStartPosition = transform.position;
    } 
    
    private void Update()
    {
        if (chasePlayer)
        {
            Vector3 directionToPlayer = (playerStatsScript.transform.position - transform.position).normalized;
            transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;
            print("Foi!");
        }
        else
        {
            // Faça o objeto flutuar
            transform.position = floatStartPosition + new Vector3(0.0f, Mathf.Sin(Time.time) * floatStrength, 0.0f);
        }

        // Gire o objeto em torno do eixo Y
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    #region Funções Unity
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerLayer == (PlayerLayer | (1 << other.gameObject.layer)))
        {
            if (playerStatsScript == null)
            {
                playerStatsScript = other.gameObject.GetComponent<PlayerStats>();
                if (playerStatsScript == null)
                {
                    Debug.LogError("PlayerStats não encontrado no objeto do jogador");
                    return;
                }
            }

            playerStatsScript.ChangeOrbSouls(soulValue);
            /*if (soulDisplay == null)
            {
                soulDisplay.SetText("Almas: " + _playerStats.Souls);
            }*/

            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("orb catch");

            GetComponent<SphereCollider>().enabled = false;
            chasePlayer = true;
            transform.localScale /= 3f;
            Destroy(gameObject, 0.15f);
        }
    }
    #endregion
}
