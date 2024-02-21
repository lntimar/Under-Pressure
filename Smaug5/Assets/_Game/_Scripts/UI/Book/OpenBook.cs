using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OpenBook : MonoBehaviour
{
    [SerializeField] private Button openBtn;

    private Vector3 _rotation;

    private bool _isOpenClicked;

    private DateTime _startTime;
    private DateTime _endTime;

    private void Start()
    {
        if (openBtn != null)
            openBtn.onClick.AddListener( () => ClickOpen());
    }

    private void Update()
    {
        if (_isOpenClicked)
        {
            transform.Rotate(_rotation * Time.deltaTime);
            _endTime = DateTime.Now;

            if ((_endTime - _startTime).TotalSeconds >= 1)
                _isOpenClicked = false;
        }
    }

    private void ClickOpen()
    {
        _isOpenClicked = true;
        _startTime  = DateTime.Now;
        _rotation = new Vector3(0, 180, 0);

        // TODO: SFX abrindo livro
    }
}
