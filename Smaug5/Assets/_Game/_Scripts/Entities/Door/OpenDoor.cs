using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    #region Vari�veis Globais
    // Inspector:
    [Header("Configura��es:")]

    [Header("Chave:")]
    [SerializeField] private DoorKeys.Key targetKey;

    [Header("Refer�ncias:")]
    [SerializeField] private Animator doorAnimator;

    [HideInInspector] public bool CanInteract = false;

    private static bool[] _doorsOpened = new bool[10];
    #endregion

    #region Fun��es Unity
    private void Start() => VerifyAlreadyOpened();

    private void Update()
    {
        if (CanInteract)
        {
            if (Input.GetKeyDown(KeyCode.E) && HasKey())
            {
                Open();
                _doorsOpened[(int)targetKey] = true;
            }
        }
    }
    #endregion

    #region Fun��es Pr�prias
    private void Open()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("door open");
        doorAnimator.enabled = false;
        Destroy(gameObject);
    }

    private void VerifyAlreadyOpened()
    {
        for (int i = 0; i < _doorsOpened.Length; i++)
        {
            if (i == (int)targetKey)
            {
                if (_doorsOpened[i] == true)
                    Open();
            }
        }
    }

    private bool HasKey()
    {
        for (int i = 0; i < PlayerProgress.KeysCollected.Count; i++)
        {
            if (PlayerProgress.KeysCollected[i] == targetKey)
                return true;
        }

        return false;
    }
    #endregion
}
