using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class DialogueScript : MonoBehaviour
{
    #region Vari�veis Globais
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public string[] dialogueLines;
    public float textSpeed;

    private int index;
    private BoxCollider _boxCollider;
    private bool isDialogueActive = false;
    #endregion

    #region Fun��es Unity
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDialogueActive)
        {
            isDialogueActive = true;
            dialogueCanvas.SetActive(true);
            dialogueText.text = string.Empty;
            StartDialogue();

            _boxCollider.enabled = false;
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
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

    #region Fun��es Pr�prias
    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() //escreve as linhas do di�logo
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
            dialogueCanvas.SetActive(false);
            isDialogueActive = false;
        }
    }
    #endregion
}
