using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    #region Variáveis Globais
    // Componentes:
    private PlayerStats _playerStats;
    #endregion

    #region Funções Unity
    private void Awake() => _playerStats = GetComponent<PlayerStats>();

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == CollisionLayersManager.Instance.EnemyAttack.Index)
        {
            _playerStats.ChangeHealthPoints(-EnemyStats.Damage);
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.HealthPack.Index)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("orb catch");
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<HealthPack>().Points);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.SoulOrb.Index)
        {
            Debug.Log("bolas");
        }
    }
    #endregion
}
