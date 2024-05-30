using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulScript : MonoBehaviour
{
    #region Vari�veis Globais
    // Inspector:
    public int soulValue;

    [Header("Refer�ncias:")]
    public LayerMask PlayerLayer;
    public PlayerStats PlayerStats;
    //public TextMeshProUGUI soulDisplay;
    #endregion

    #region Fun��es Unity
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerLayer == (PlayerLayer | (1 << other.gameObject.layer)))
        {
            if (PlayerStats == null)
            {
                PlayerStats = other.gameObject.GetComponent<PlayerStats>();
                if (PlayerStats == null)
                {
                    Debug.LogError("PlayerStats n�o encontrado no objeto do jogador");
                    return;
                }
            }

            PlayerStats.ChangeOrbSouls(soulValue);
            /*if (soulDisplay == null)
            {
                soulDisplay.SetText("Almas: " + _playerStats.Souls);
            }*/

            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("orb catch");

            Destroy(gameObject, 0.1f);
        }
    }
    #endregion
}
