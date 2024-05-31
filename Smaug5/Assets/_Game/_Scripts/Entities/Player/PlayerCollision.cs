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
        else if (col.gameObject.layer == CollisionLayersManager.Instance.ConversationTrigger.Index)
        {
            StartConversation(col.gameObject.GetComponent<ConversationTrigger>().ConversationScript);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.HealthPack.Index)
        {
            _playerStats.ChangeHealthPoints(col.gameObject.GetComponent<HealthPack>().Points);
            Destroy(col.gameObject);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.layer == CollisionLayersManager.Instance.DoorTrigger.Index)
        {
            col.gameObject.GetComponent<OpenDoor>().CanInteract = true;
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.KeyTrigger.Index)
        {
            col.gameObject.GetComponent<GetKey>().CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == CollisionLayersManager.Instance.DoorTrigger.Index)
        {
            col.gameObject.GetComponent<OpenDoor>().CanInteract = false;
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.KeyTrigger.Index)
        {
            col.gameObject.GetComponent<GetKey>().CanInteract = false;
        }
    }
    #endregion

    #region Funções Próprias
    private void StartConversation(NPCConversation conversation) => ConversationManager.Instance.StartConversation(conversation);
    #endregion
}
