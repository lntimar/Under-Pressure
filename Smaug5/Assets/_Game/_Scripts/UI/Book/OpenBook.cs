using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class OpenBook : MonoBehaviour
{
    #region Variáveis Globais
    // Unity Inspector:
    [Header("Referências:")] 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject crossHairUI;
    [SerializeField] private GameObject bookParent;

    [Header("Botões:")]
    [SerializeField] private Button openBtn;

    [Header("Game Objects:")]
    [SerializeField] private GameObject openedBook;
    [SerializeField] private GameObject insideBackCover;

    [Header("Scripts:")] 
    [SerializeField] private PageEditor pageEditorScript;

    [Header("Textos:")]
    [SerializeField] private TextMeshProUGUI[] tempTexts;

    private Vector3 _rotation;

    private bool _isOpenClicked = false;
    private bool _isCloseClicked = false;

    private DateTime _startTime;
    private DateTime _endTime;

    [HideInInspector] public int LastPageIndex = 0;

    private Rigidbody _playerRb;

    private bool _canOpen = true;
    #endregion

    #region Funções Unity
    /*
    private void Start()
    {
        if (openBtn != null)
            openBtn.onClick.AddListener(() => ClickOpen());
        
    }
    */

    private void OnEnable()
    {
        _playerRb = FindObjectOfType<PlayerMove>().gameObject.GetComponent<Rigidbody>();

        if (_canOpen)
        {
            Invoke("ClickOpen", 0.75f);
            _canOpen = false;
        }
    }

    private void Update()
    {
        _playerRb.velocity = Vector3.zero;
        if (_isOpenClicked || _isCloseClicked)
        {
            transform.Rotate(_rotation * Time.deltaTime * 0.8f);
            _endTime = DateTime.Now;

            if (_isOpenClicked)
            {
                if ((_endTime - _startTime).TotalSeconds >= 1)
                {
                    for (int i = 0; i < tempTexts.Length; i++)
                        tempTexts[i].transform.parent.gameObject.SetActive(false);

                    _isOpenClicked = false;
                    openedBook.SetActive(true);
                    insideBackCover.SetActive(false);
                    gameObject.SetActive(false);
                }
            }

            if (_isCloseClicked)
            {
                if ((_endTime - _startTime).TotalSeconds >= 1)
                {
                    _isCloseClicked = false;

                    for (int i = 0; i < tempTexts.Length; i++)
                        tempTexts[i].transform.parent.gameObject.SetActive(false);

                    FindObjectOfType<GameMenu>()._canPause = true;
                    FindObjectOfType<BookInteractTrigger>()._crossHairUI.SetActive(true);
                }
            }
        }
    }

    private void OnDisable()
    {
        LastPageIndex = 0;
        for (int i = 0; i < tempTexts.Length; i++)
            tempTexts[i].text = pageEditorScript.BookPages[LastPageIndex].Texts[i];
    }
    #endregion

    #region Funções Próprias
    private void ClickOpen()
    {
        _isOpenClicked = true;
        _startTime = DateTime.Now;
        _rotation = new Vector3(0, 180, 0);

        for (int i = 0; i < tempTexts.Length; i++)
        {
            tempTexts[i].transform.parent.gameObject.SetActive(true);
            tempTexts[i].text = pageEditorScript.BookPages[LastPageIndex].Texts[i];
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("book open");
    }

    public void ClickClose()
    {
        gameObject.SetActive(true);
        openedBook.SetActive(false);
        insideBackCover.SetActive(true);

        _isCloseClicked = true;
        _startTime = DateTime.Now;
        _rotation = new Vector3(0, -180, 0);

        for (int i = 0; i < tempTexts.Length; i++)
        {
            tempTexts[i].transform.parent.gameObject.SetActive(true);
            tempTexts[i].text = pageEditorScript.BookPages[LastPageIndex].Texts[i];
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("book close");

        Invoke("CloseBook", 1.5f);
    }

    private void CloseBook()
    {
        gameObject.SetActive(true);
        insideBackCover.SetActive(true);
        openedBook.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        crossHairUI.SetActive(true);
        mainCamera.enabled = true;
        bookParent.SetActive(false);

        if (FindObjectOfType<Weapon>() != null)
        {
            FindObjectOfType<Weapon>().enabled = true;
            FindObjectOfType<SonarScript>().enabled = true;
        }
        
        _canOpen = true;
    }
    #endregion
}
