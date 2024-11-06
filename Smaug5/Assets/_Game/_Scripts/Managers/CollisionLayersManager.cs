using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLayersManager : MonoBehaviour
{
    #region Vari�veis Globais
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
    public SingleUnityLayer CollectTrigger;
    public SingleUnityLayer EnemyAttack;
    public SingleUnityLayer SoulOrb;
    public SingleUnityLayer Door;


    public static CollisionLayersManager Instance;
    #endregion

    #region Fun��es Unity
    private void Awake() => Instance = this;
    #endregion
}
