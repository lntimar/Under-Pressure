using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Configurações:")]

    [Header("Chave:")]
    [SerializeField] private DoorKeys.Key targetKey;

    [Header("Referências:")]
    [SerializeField] private Animator doorAnimator;

    [HideInInspector] public bool CanInteract = false;

    // Referências:
    private static Transform _playerTransform;

    private static bool[] _doorsWithKeyOpeneds = new bool[10];
    private static List<int> _doorsWithoutKeyOpeneds = new List<int>();
    #endregion

    #region Funções Unity
    private void Awake() => _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    private void Start() => VerifyAlreadyOpened();

    private void Update()
    {
        if (CanInteract && PlayerIsFacing())
        {
            if (targetKey != DoorKeys.Key.None)
            {
                if (Input.GetKeyDown(KeyCode.E) && HasKey())
                {
                    Open();
                    _doorsWithKeyOpeneds[(int)targetKey] = true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Open();
                    _doorsWithoutKeyOpeneds.Add(gameObject.GetInstanceID());
                }
            }
        }
    }
    #endregion

    #region Funções Próprias
    private void Open()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("door open");
        doorAnimator.enabled = true;
        Destroy(gameObject);
    }

    private void VerifyAlreadyOpened()
    {
        if (targetKey != DoorKeys.Key.None) // Porta com Chave
        {
            for (int i = 0; i < _doorsWithKeyOpeneds.Length; i++)
            {
                if (i == (int)targetKey)
                {
                    if (_doorsWithKeyOpeneds[i] == true)
                    {
                        Open();
                        break;
                    }
                }
            }
        }
        else // Porta sem Chave
        {
            for (int i = 0; i < _doorsWithoutKeyOpeneds.Count; i++)
            {
                if (_doorsWithoutKeyOpeneds[i] == gameObject.GetInstanceID())
                {
                    Open();
                    break;
                }
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
