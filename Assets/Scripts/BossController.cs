using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class BossController : MonoBehaviour
{
    public BoxCollider2D bossTrigger;
    public CapsuleCollider2D bossHead;
    public Transform bossTransform;
    public SpriteRenderer bossSprite;
    public Animator animator;
    public GameObject player;
    public playerAttack playerAttack;
    public int bossHealth = 16;
    public int facing;
    public bool startTrigger = false;

    public GameObject fireAttack;
    public Vector3 bossSpawnRange;

    void Start()
    {
        bossTransform = GetComponent<Transform>();
        bossHead = GetComponent<CapsuleCollider2D>();
        bossTrigger = GetComponent<BoxCollider2D>();
        bossSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAttack = player.GetComponent<playerAttack>();
        bossSpawnRange = bossTransform.position;
    }


    void Update()
    {
        if (startTrigger == true)
        {
            bossTransform.tag = "Boss";
            StartCoroutine(BossCombat());
        }
    }

    IEnumerator BossCombat()
    {
        startTrigger = false;
        while (bossHealth > 0)
        {
            animator.SetInteger("State", 0);
            for (int i = 0; i < 1; i++)
            {
                Vector3 spawnPosition = new Vector3((int)Random.Range(bossSpawnRange.x - 5, bossSpawnRange.x + 5), bossSpawnRange.y, bossSpawnRange.z);
                bossTransform.position = spawnPosition;
                if (spawnPosition.x < player.transform.position.x)
                {
                    facing = 1;
                    bossSprite.flipX = false;
                }
                else if (spawnPosition.x > player.transform.position.x)
                { 
                    facing = -1;
                bossSprite.flipX = true;
                }

                yield return new WaitForSeconds(2f);
                animator.SetInteger("State", 1);
                    Instantiate(fireAttack, bossTransform.position, bossTransform.rotation);
                yield return new WaitForSeconds(2f);
                bossSprite.enabled = false;
                bossHead.enabled = false;
                yield return new WaitForSeconds(2f);
            }
            animator.SetInteger("State", 0);
            bossSprite.enabled = true;
            bossHead.enabled = true;
        }
        
        if (bossHealth <= 0)
        {
            
        }
    }

    public void TakeDamage(int attackDamage)
    {
        bossHealth -= playerAttack.attackDamage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            bossTrigger.enabled = false;
            bossSprite.enabled = true;
            bossHead.enabled = true;
            startTrigger = true;
        }    
    }
}
