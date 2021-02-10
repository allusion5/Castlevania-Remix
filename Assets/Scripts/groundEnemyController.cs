using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class groundEnemyController : MonoBehaviour
{
    public GameController gameController;
    public Transform groundEnemyTransform;
    public Rigidbody2D groundEnemyRB;
    public int groundEnemyHealth = 1;
    public GameObject player;
    public playerAttack playerAttack;
    public GameObject enemyDrop;
    public float facing;
    public bool isGrounded;

    void Start()
    {
       gameController = GameObject.Find("UI").GetComponent<GameController>();
       player = GameObject.FindGameObjectWithTag("Player");
       playerAttack = player.GetComponent<playerAttack>();
       groundEnemyTransform = GetComponent<Transform>();
       groundEnemyRB = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        facing = groundEnemyTransform.localScale.x;

        if(groundEnemyHealth <= 0)
        {
            //Instantiate(enemyDrop, groundEnemyRB.position, groundEnemyTransform.rotation);
            gameController.score += 100;
            Destroy(gameObject);
        }
        
        if (isGrounded == true)
        {
            groundEnemyRB.velocity = new Vector2(facing * 10f, 0f);
        }
        else
        {
            groundEnemyRB.velocity = new Vector2(0f, groundEnemyRB.velocity.y);
        }
    }
    // Update is called once per frame
    public void TakeDamage(int attackDamage)
    {
        groundEnemyHealth -= playerAttack.attackDamage;
    }
    private void OnBecameVisible()
    {
        gameObject.SetActive(true);
        groundEnemyRB.velocity = new Vector2(facing * 10f, 0f);
    }

    private void OnBecameInvisible()
    {
        {
            Destroy(gameObject, 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
     if (collision.gameObject.CompareTag("Weapon"))
        {
            groundEnemyHealth -= 1;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
