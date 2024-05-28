using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    #region Vari�veis Globais
    // Componentes:
    private PlayerStats _playerStats;
    #endregion

    #region Fun��es Unity
    private void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == CollisionLayersManager.Instance.Enemy.Index)
        {
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<EnemyStats>().Damage);
            // TODO: Aplicar Knockback
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == CollisionLayersManager.Instance.ConversationTrigger.Index)
        {
            StartConversation(col.gameObject.GetComponent<ConversationTrigger>().ConversationScript);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.HealthPack.Index)
        {
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<HealthPack>().Points);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.DoorTrigger.Index)
        {
            col.gameObject.GetComponent<OpenDoor>().CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == CollisionLayersManager.Instance.DoorTrigger.Index)
        {
            col.gameObject.GetComponent<OpenDoor>().CanInteract = false;
        }
    }
    #endregion

    #region Fun��es Pr�prias
    private void StartConversation(NPCConversation conversation) => ConversationManager.Instance.StartConversation(conversation);
    #endregion
}
