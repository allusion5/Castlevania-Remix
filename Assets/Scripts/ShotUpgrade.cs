using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotUpgrade : MonoBehaviour
{
    public GameController gameController;
    public GameObject player;
    public playerAttack playerAttack;
    private Rigidbody2D shotRB;

    void Start()
    {
        gameController = GameObject.Find("UI").GetComponentInChildren<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = player.GetComponent<playerAttack>();
        shotRB = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAttack.subWeaponShot += 1;
            gameController.score += 50;
            Destroy(gameObject);
        }
        if (collision.CompareTag("Ground"))
        {
            shotRB.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(gameObject, 4f);
        }
    }
}
