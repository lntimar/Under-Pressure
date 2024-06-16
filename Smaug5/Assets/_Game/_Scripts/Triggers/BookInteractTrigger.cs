using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class BookInteractTrigger : MonoBehaviour
{
    #region Vari�veis Globais
    // Inspector:
    [Header("Refer�ncias:")]
    [SerializeField] private Outline outlineEffect;
    [SerializeField] private GameObject book;

    [HideInInspector] public bool CanInteract = false;

    // Refer�ncias:
    private GameObject _crossHairUI;
    private Rigidbody _playerRb;
    #endregion

    #region Fun��es Unity

    private void Awake()
    {
        _crossHairUI = GameObject.FindGameObjectWithTag("CrossHairUI");
        _playerRb = FindObjectOfType<PlayerMove>().gameObject.GetComponent<Rigidbody>();
    } 

    private void Update()
    {
        if (CanInteract)
        {
            outlineEffect.eraseRenderer = false;

            if (Input.GetKeyDown(KeyCode.E) && !book.active)
                Open();
        }
        else
        {
            outlineEffect.eraseRenderer = true;
        }
    }
    #endregion

    private void Open()
    {
        //FindObjectOfType<Weapon>().enabled = false;
        //FindObjectOfType<SonarScript>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _crossHairUI.SetActive(false);
        Camera.main.enabled = false;
        book.SetActive(true);
    }
}
