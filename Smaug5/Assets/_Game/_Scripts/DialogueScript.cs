using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class DialogueScript : MonoBehaviour
{
    #region Variáveis Globais
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    public float textSpeed;

    private int index;
    #endregion

    #region Funções Unity
    void Start()
    {
        dialogueText.text = string.Empty;
        StartDialogue();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == dialogueLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[index];
            }
        }
    }
    #endregion

    #region Funções Próprias
    private void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() //escreve as linhas do diálogo
    {
        foreach (char c in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
}
