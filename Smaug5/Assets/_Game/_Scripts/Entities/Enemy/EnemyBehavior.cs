using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;

public class EnemyBehavior : MonoBehaviour
{
    public float lookRadius = 10f;

    void Start()
    {
       
    }
    
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
