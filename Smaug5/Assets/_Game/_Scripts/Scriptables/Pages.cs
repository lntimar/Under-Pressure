using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pages", menuName = "Scriptable Objects/Pages")]
public class Pages : ScriptableObject
{
    public string[] Texts = new string[2];
}
