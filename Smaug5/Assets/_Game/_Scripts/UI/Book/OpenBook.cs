using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OpenBook : MonoBehaviour
{
    [SerializeField] private Button openBtn;
    [SerializeField] private GameObject openedBook;
    [SerializeField] private GameObject insideBackCover;

    private Vector3 _rotation;

    private bool _isOpenClicked = false;
    private bool _isCloseClicked = false;

    private DateTime _startTime;
    private DateTime _endTime;

    private void Start()
    {
        if (openBtn != null)
            openBtn.onClick.AddListener( () => ClickOpen());
    }

    private void Update()
    {
        if (_isOpenClicked || _isCloseClicked)
        {
            transform.Rotate(_rotation * Time.deltaTime);
            _endTime = DateTime.Now;

            if (_isOpenClicked)
            {
                if ((_endTime - _startTime).TotalSeconds >= 1)
                {
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
                }
            }
        }
    }

    private void ClickOpen()
    {
        _isOpenClicked = true;
        _startTime  = DateTime.Now;
        _rotation = new Vector3(0, 180, 0);

        // TODO: SFX abrindo livro
    }

    public void ClickClose()
    {
        gameObject.SetActive(true);
        openedBook.SetActive(false);
        insideBackCover.SetActive(true);

        _isCloseClicked = true;
        _startTime = DateTime.Now;
        _rotation = new Vector3(0, -180, 0);

        // TODO: SFX fechando Livro
    }
}
