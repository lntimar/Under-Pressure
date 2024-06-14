using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLayersManager : MonoBehaviour
{
    #region Variáveis Globais
    // Inspector:
    [Header("Layers:")]
    public SingleUnityLayer Player;
    public SingleUnityLayer CheckPoint;
    public SingleUnityLayer LoadPoint;
    public SingleUnityLayer Enemy;
    public SingleUnityLayer HealthPack;
    public SingleUnityLayer ConversationTrigger;
    public SingleUnityLayer Stairs;
    public SingleUnityLayer DoorTrigger;
    public SingleUnityLayer KeyTrigger;
    public SingleUnityLayer BookTrigger;
    public SingleUnityLayer WeaponCollectTrigger;
    public SingleUnityLayer StayCrouchTrigger;

    public static CollisionLayersManager Instance;
    #endregion

    #region Funções Unity
    private void Awake() => Instance = this;
    #endregion
}
