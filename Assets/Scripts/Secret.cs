using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Secret : MonoBehaviour
{
    public int health = 1;
    private Rigidbody2D secretRB;
    private Transform secretTransform;
    public GameObject secretItem;

    public GameObject player;
    public playerAttack playerAttack;
    //public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        //gameController = GameObject.Find("UI").GetComponentInChildren<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = player.GetComponent<playerAttack>();
        secretRB = GetComponent<Rigidbody2D>();
        secretTransform = GetComponent<Transform>();
    }

    // Update is called once per frame

    public void TakeDamage(int attackDamage)
    {
        health -= playerAttack.attackDamage;
    }
    void FixedUpdate()
    {
        if(health <= 0)
        {
            //explosion animation
            Instantiate(secretItem, secretTransform.position, secretTransform.rotation);
            //gameController.score += 50;
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            health -= 1;
        }
    }
}
