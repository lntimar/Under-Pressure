using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    /* Esse Script serve pra basicamente relacionar o player com o inimigo
    Ou seja, É MUITO IMPORTANTE!!! */
    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject player;
}
