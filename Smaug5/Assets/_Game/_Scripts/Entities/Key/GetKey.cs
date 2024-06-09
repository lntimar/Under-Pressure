using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class GetKey : MonoBehaviour
{
    #region Vari�veis Globais
    // Inspector:
    [Header("Configura��es:")]
    [SerializeField] private DoorKeys.Key key;

    [Header("Refer�ncias:")]
    [SerializeField] private Outline outlineEffect;
    
    [HideInInspector] public bool CanInteract = false;

    // Refer�ncias:
    private static Transform _playerTransform;

    private static bool[] _keysCaughts = new bool[10];
    #endregion

    #region Fun��es Unity
    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        VerifyAlreadyCaught();
    }

    private void Update()
    {
        if (CanInteract)
        {
            outlineEffect.eraseRenderer = false;
            if (Input.GetKeyDown(KeyCode.E))
            { 
                Get();
                _keysCaughts[(int)key] = true;
            }
        }
        else
        {
            outlineEffect.eraseRenderer = true;
        }
    }
    #endregion

    #region Fun��es Pr�prias
    private void Get()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("key catch");
        PlayerProgress.KeysCollected.Add(key);
        Destroy(transform.parent.gameObject);
    }

    private void VerifyAlreadyCaught()
    {
        for (int i = 0; i < _keysCaughts.Length; i++)
        {
            if (i == (int)key)
            {
                if (_keysCaughts[i] == true)
                {
                    Destroy(transform.parent.gameObject);
                    break;
                }
            }
        }
    }
    #endregion
}
