using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    #region Variáveis Globais
    // Referências:
    private CollisionLayersManager _collisionLayersManager;
    private ConversationManager _conversationManagerScript;

    // Componentes:
    private PlayerStats _playerStats;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        _collisionLayersManager = FindObjectOfType<CollisionLayersManager>();
        _conversationManagerScript = FindObjectOfType<ConversationManager>();
        _playerStats = GetComponent<PlayerStats>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == _collisionLayersManager.Enemy.Index)
        {
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<EnemyStats>().Damage);
            // TODO: Aplicar Knockback
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == _collisionLayersManager.ConversationTrigger.Index)
        {
            StartConversation(col.gameObject.GetComponent<ConversationTrigger>().ConversationScript);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.layer == _collisionLayersManager.HealthPack.Index)
        {
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<HealthPack>().Points);
            Destroy(col.gameObject);
        }
    }
    #endregion

    #region Funções Próprias
    private void StartConversation(NPCConversation conversation) => _conversationManagerScript.StartConversation(conversation);
    #endregion
}
