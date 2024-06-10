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
        else if (col.gameObject.layer == CollisionLayersManager.Instance.WeaponCollectTrigger.Index)
        {
            col.gameObject.GetComponent<WeaponCollectTrigger>().CanInteract = true;
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.BookTrigger.Index)
        {
            col.gameObject.GetComponent<BookInteractTrigger>().CanInteract = true;
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
        else if (col.gameObject.layer == CollisionLayersManager.Instance.WeaponCollectTrigger.Index)
        {
            col.gameObject.GetComponent<WeaponCollectTrigger>().CanInteract = false;
        }
        else if (col.gameObject.layer == CollisionLayersManager.Instance.BookTrigger.Index)
        {
            col.gameObject.GetComponent<BookInteractTrigger>().CanInteract = false;
        }
    }

    private void StartConversation(NPCConversation conversation) => ConversationManager.Instance.StartConversation(conversation);
}
