using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulScript : MonoBehaviour
{
    public int soulValue;
    SonarScript _sonar;

    private void Awake() => _sonar = GetComponent<SonarScript>();

    void Update()
    {


        if (_sonar != null)
        {
            
        }
    }
}
