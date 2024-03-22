using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionLayersManager : MonoBehaviour
{
    [Header("Layers:")] 
    public SingleUnityLayer Player;
    public SingleUnityLayer CheckPoint;
    public SingleUnityLayer LoadPoint;
    public SingleUnityLayer Enemy;
    public SingleUnityLayer HealthPack;
    public SingleUnityLayer ConversationTrigger;
}
