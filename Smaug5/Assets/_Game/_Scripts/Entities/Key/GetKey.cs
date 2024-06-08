using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class GetKey : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Configurações:")]
    [SerializeField] private DoorKeys.Key key;

    [Header("Referências:")]
    [SerializeField] private Outline outlineEffect;
    
    [HideInInspector] public bool CanInteract = false;

    // Referências:
    private static Transform _playerTransform;

    private static bool[] _keysCaughts = new bool[10];
    #endregion

    #region Funções Unity
    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        VerifyAlreadyCaught();
    }

    private void Update()
    {
        if (CanInteract)
        {
            if (PlayerIsFacing())
            {
                print("Foi!");
                outlineEffect.eraseRenderer = false;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    print("Foi 2");
                    Get();
                    _keysCaughts[(int)key] = true;
                }
            }
        }
        else
        {
            outlineEffect.eraseRenderer = true;
        }
    }
    #endregion

    #region Funções Próprias
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

    private bool PlayerIsFacing()
    {
        var triggerDirection = (transform.position - _playerTransform.position).normalized;
        var playerDirection = _playerTransform.forward;

        float scalar = Vector3.Dot(triggerDirection, playerDirection);

        float angulo = Mathf.Acos(scalar) * Mathf.Rad2Deg;

        if (angulo <= 45f)
            return true;

        return false;
    }
    #endregion
}
