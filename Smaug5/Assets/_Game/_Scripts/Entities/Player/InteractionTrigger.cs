using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == CollisionLayersManager.Instance.ConversationTrigger.Index)
        {
            StartConversation(col.gameObject.GetComponent<ConversationTrigger>().ConversationScript);
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
            print("Saiu");
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.KeyTrigger.Index)
        {
            col.gameObject.GetComponent<GetKey>().CanInteract = false;
        }
    }

    private void StartConversation(NPCConversation conversation) => ConversationManager.Instance.StartConversation(conversation);
}
