using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private Rigidbody2D axe;
    public float throwForce;
    public GameObject player;
    public playerAttack playerAttack;
    public float facing;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = player.GetComponent<playerAttack>();
        axe = GetComponent<Rigidbody2D>();
        facing = player.transform.localScale.x;

        if (facing > 0)
        {
            axe.AddForce(new Vector2(2f, throwForce), ForceMode2D.Impulse);
        }
        else if (facing < 0)
        {
            axe.AddForce(new Vector2(-2f, throwForce), ForceMode2D.Impulse);
        }
    }

        void OnBecameInvisible()
        {
        playerAttack.subWeaponsCount -= 1;
            Destroy(gameObject);
        }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //deal damage
        }
    }  
}
