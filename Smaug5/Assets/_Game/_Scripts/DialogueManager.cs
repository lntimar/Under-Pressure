using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public DialogueScript dialogueScript;
    public List<string[]> dialogues; //di�logo espec�fico pro ponto de colis�o

    public void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue()
    {

    }
}
