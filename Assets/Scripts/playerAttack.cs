using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class playerAttack : MonoBehaviour
{
    public Transform primaryAttackPoint;
    public float attackLength = 0.75f; //1.55 for max length
    public Vector2 attackRange;
    public int attackDamage = 1;

    public PlayerGroundedCheck playerGroundedCheck;
    public Rigidbody2D player;
    public playerController playerController;
    public Transform secondaryAttackPoint;
    public GameObject activeSubWeapon;

    public int heartCounter = 5;
    public bool isAttacking = false;
    public float attackRate;
    public float moveDelay;
    public int subWeaponsCount = 0;
    public int subWeaponShot = 1;
    public int morningStar = 0;
    public float nextMoveTime;
    private float nextAttackTime;

    public Animator playerAnimator;

    public LayerMask destructableLayers;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = GetComponent<playerController>();
        playerGroundedCheck = player.GetComponentInChildren<PlayerGroundedCheck>();
        playerAnimator = player.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        attackRange = new Vector2(attackLength, 0.4f);

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
        isAttacking = true;
        if (playerGroundedCheck.isGrounded == true)
        {
            if(playerController.isCrouching == true)
            {
                playerAnimator.SetInteger("State", 16 + morningStar);
            }
            else
            playerAnimator.SetInteger("State", 8 + morningStar);
            player.velocity = Vector2.zero;
        }
        
        if (playerGroundedCheck.isGrounded == false && playerController.onStairs == false)
        {
            playerAnimator.SetInteger("State", 12 + morningStar);
        }

        if (playerController.onStairs == true)
        {
            if (playerController.playerFacing == 1)
            {
                playerAnimator.SetInteger("State", 20 + morningStar);
            }
            else if (playerController.playerFacing == -1)
            {
                playerAnimator.SetInteger("State", 24 + morningStar);
            }
        }

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
        isAttacking = false;
    }

    void SecondaryAttack()
    {
        isAttacking = true;
        if (playerGroundedCheck.isGrounded == true)
        {
            if(playerController.isCrouching == true)
            {
                playerAnimator.SetInteger("State", 19);
            }
            else
            playerAnimator.SetInteger("State", 11);
            player.velocity = Vector2.zero;
        }
        else if (playerGroundedCheck.isGrounded == false && playerController.onStairs == false)
        {
            playerAnimator.SetInteger("State", 15);
        }

        if (playerController.onStairs == true)
        {
            playerAnimator.SetInteger("State", 23);
        }

        Instantiate(activeSubWeapon, secondaryAttackPoint.position, secondaryAttackPoint.rotation);
        subWeaponsCount += 1;
        isAttacking = false;
    }
        void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(primaryAttackPoint.position, attackRange);
    }
}
