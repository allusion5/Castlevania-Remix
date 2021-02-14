using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaggerPickUp : MonoBehaviour
{
    public GameController gameController;
    public GameObject player;
    public playerAttack playerAttack;
    //private Transform heartTransform;
    private Rigidbody2D daggerPickUpRB;
    //private bool isGrounded;

    public GameObject Dagger;

    void Start()
    {
        gameController = GameObject.Find("UI").GetComponentInChildren<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = player.GetComponent<playerAttack>();
        //heartTransform = GetComponent<Transform>();
        daggerPickUpRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //potentially add Lerp movement back and forth on float down.
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Player"))
            {
                playerAttack.activeSubWeapon = Dagger;
                Destroy(gameObject);
            }
            if (collision.CompareTag("Ground"))
        {
            daggerPickUpRB.constraints = RigidbodyConstraints2D.FreezeAll;
            //isGrounded = true;
            Destroy(gameObject, 4f);
        }
    }
}
