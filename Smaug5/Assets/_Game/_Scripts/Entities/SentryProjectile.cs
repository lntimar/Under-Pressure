using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryProjectile : MonoBehaviour
{
    public LayerMask wallAndGround;
    public LayerMask player;
    PlayerStats playerStats;

    private void Update()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == wallAndGround)
        {
            Destroy(gameObject);
        }

        //l�gica de colis�o com jogador t� no PlayerCollision

        /*if (collision.gameObject.layer == player)
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.ChangeHealthPoints(-10);
            }
            Destroy(gameObject);
        }*/
    }
}
