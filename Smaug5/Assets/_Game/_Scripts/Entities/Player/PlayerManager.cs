using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //N�o me pergunte, tamb�m n�o entendi nada
    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject player;
}
