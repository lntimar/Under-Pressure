using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public DialogueScript dialogueScript;
    public List<string[]> dialogues; //diálogo específico pro ponto de colisão

    public void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue()
    {

    }
}
