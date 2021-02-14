using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubweaponPickup : MonoBehaviour
{
    public GameController gameController;
    public GameObject player;
    public playerAttack playerAttack;
    public GameObject subDrop;
    private Rigidbody2D subRB;

    void Start()
    {
        subDrop = GetComponent<GameObject>();
        gameController = GameObject.Find("UI").GetComponentInChildren<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = player.GetComponent<playerAttack>();
        //heartTransform = GetComponent<Transform>();
        subRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //potentially add Lerp movement back and forth on float down.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAttack.activeSubWeapon = subDrop;
            gameController.score += 50;
            Destroy(gameObject);
        }
        if (collision.CompareTag("Ground"))
        {
            subRB.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(gameObject, 4f);
        }
    }
}
