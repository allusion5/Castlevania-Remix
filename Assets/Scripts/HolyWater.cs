using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HolyWater : MonoBehaviour
{
    public Transform holyWaterTransform;
    public Rigidbody2D holyWater;
    public GameObject player;
    public GameObject fire;
    public SpriteRenderer holyWaterSprite;
    public Rigidbody2D playerRigidbody;
    public playerAttack playerAttack;
    public Animator animator;
    public float facing;
    public float throwForce;
    public int attackDamage =1;

    // Start is called before the first frame update
    void Start()
    {
        holyWaterTransform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = player.GetComponent<playerAttack>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        holyWater = GetComponent<Rigidbody2D>();
        holyWaterSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        facing = player.transform.localScale.x;

        if (facing > 0)
        {
            holyWater.AddForce(new Vector2(4f, throwForce), ForceMode2D.Impulse);
        }
        else if (facing < 0)
        {
            holyWater.AddForce(new Vector2(-4f, throwForce), ForceMode2D.Impulse);
        }    
    }

    void FixedUpdate()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Secret") || collision.gameObject.CompareTag("Enemy"))
        {
            Explosion();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Secret") || collision.gameObject.CompareTag("Enemy"))
        {
            Explosion();
        }
    }

    void Explosion()
    {
        animator.SetInteger("State", 1);
        holyWater.gravityScale = 0;
        holyWater.velocity = Vector2.zero;
        holyWater.constraints = RigidbodyConstraints2D.FreezeAll;
        playerAttack.subWeaponsCount -= 1;
        Destroy(gameObject, 1f);
    }
}
