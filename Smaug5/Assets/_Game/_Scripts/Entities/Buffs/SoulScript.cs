using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulScript : MonoBehaviour
{
    public int soulValue;
    public LayerMask playerLayer;
    //public TextMeshProUGUI soulDisplay;
    PlayerStats _playerStats;

    private void OnTriggerEnter(Collider other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            if (_playerStats == null)
            {
                _playerStats = other.gameObject.GetComponent<PlayerStats>();
                if (_playerStats == null)
                {
                    Debug.LogError("PlayerStats não encontrado no objeto do jogador");
                    return;
                }
            }

            _playerStats.Souls += soulValue;
            /*if (soulDisplay == null)
            {
                soulDisplay.SetText("Almas: " + _playerStats.Souls);
            }*/
            Destroy(gameObject);
        }
    }
}
