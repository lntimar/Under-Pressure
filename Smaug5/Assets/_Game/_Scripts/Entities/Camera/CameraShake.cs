using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    #region Variáveis Globais
    [Header("Configurações:")]

    [SerializeField] private float animationSpeed;

    private Animator _camAnim;
    #endregion

    #region Funções Unity
    private void Awake()
    {
        _camAnim = gameObject.GetComponent<Animator>();
    }

    private void Start() => _camAnim.speed = animationSpeed;
    #endregion

    #region Funções Próprias
    public void ApplyScreenShake(int anim=0)
    {
        if (anim == 0) _camAnim.SetInteger("random", Random.Range(1, 3));
        else _camAnim.SetInteger("random", anim);

        _camAnim.SetTrigger("shake");
    }
    #endregion
}
