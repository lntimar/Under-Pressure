using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    #region Vari�veis Globais
    // Refer�ncias:
    private CollisionLayersManager _collisionLayersManager;

    // Componentes:
    private PlayerStats _playerStats;
    #endregion

    #region Fun��es Unity
    private void Awake()
    {
        _collisionLayersManager = FindObjectOfType<CollisionLayersManager>();
        _playerStats = GetComponent<PlayerStats>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == _collisionLayersManager.Enemy.Index)
        {
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<EnemyStats>().Damage);
            // TODO: Aplicar Knockback
        }
        else if (col.gameObject.layer == _collisionLayersManager.HealthPack.Index)
        {
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<HealthPack>().Points);
            Destroy(col.gameObject);
        }
    }
    #endregion
}
