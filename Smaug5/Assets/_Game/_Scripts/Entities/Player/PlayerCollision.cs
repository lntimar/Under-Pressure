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
        if (col.gameObject.layer == CollisionLayersManager.Instance.Enemy.Index)
        {
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<EnemyStats>().Damage);
            //Debug.Log("Bateu");
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.HealthPack.Index)
        {
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<HealthPack>().Points);
            Destroy(col.gameObject);
        }
    }
    #endregion
}
