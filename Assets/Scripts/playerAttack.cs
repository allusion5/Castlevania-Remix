using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public Transform primaryAttackPoint;
    public int attackLength;
    private Vector2 attackRange; //need to make this accessible to other script to increase whip range
    public int attackDamage = 1;

    public PlayerGroundedCheck playerGroundedCheck;
    public Rigidbody2D player;
    public Transform secondaryAttackPoint;
    public GameObject activeSubWeapon;

    public int heartCounter = 5;
    public float attackRate = 0.25f;
    public float moveDelay;
    public int subWeaponsCount = 0;
    public int subWeaponShot = 1;
    public float nextMoveTime;
    private float nextAttackTime;

    public LayerMask destructableLayers;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerGroundedCheck = player.GetComponentInChildren<PlayerGroundedCheck>();
    }

    void FixedUpdate()
    {
        attackRange = new Vector2(attackLength, 0.6f);

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            PrimaryAttack();
            nextAttackTime = attackRate + Time.time;
            nextMoveTime = moveDelay + Time.time;
        }
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Time.time >= nextAttackTime && subWeaponsCount < subWeaponShot && heartCounter > 0)
        {
            SecondaryAttack();
            nextAttackTime = attackRate + Time.time;
            nextMoveTime = moveDelay + Time.time;
            heartCounter -= 1;
        }
    }

    void PrimaryAttack()
    {
        if (playerGroundedCheck.isGrounded == true)
        {
            player.velocity = Vector2.zero;
        }
        //play animation

        //detect destructables
        Collider2D[] hitDestructables = Physics2D.OverlapBoxAll(primaryAttackPoint.position, attackRange, 0f, destructableLayers);

        //deal damage
        foreach(Collider2D destructable in hitDestructables)
        {
            if (destructable.CompareTag("Secret"))
            {
                destructable.GetComponent<Secret>().TakeDamage(attackDamage);
            }
            else if (destructable.CompareTag("GroundEnemy"))
            {
                destructable.GetComponent<groundEnemyController>().TakeDamage(attackDamage);
            }
            else if (destructable.CompareTag("FlyingEnemy"))
            {
                destructable.GetComponent<FlyingEnemyController>().TakeDamage(attackDamage);
            }
        }
    }

    void SecondaryAttack()
    {
        if (playerGroundedCheck.isGrounded == true)
        {
            player.velocity = Vector2.zero;
        }
        subWeaponsCount += 1;
        Instantiate(activeSubWeapon, secondaryAttackPoint.position, secondaryAttackPoint.rotation);
    }    
    
    void OnDrawGizmosSelected()
    {
        if (primaryAttackPoint == null)
            return;
            Gizmos.DrawWireCube(primaryAttackPoint.position, attackRange);   
    }
}
