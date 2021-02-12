using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class playerAttack : MonoBehaviour
{
    public Transform primaryAttackPoint;
    public float attackLength = 0.75f; //1.55 for max length
    public Vector2 attackRange;
    public int attackDamage = 2;

    public PlayerGroundedCheck playerGroundedCheck;
    public Rigidbody2D player;
    public playerController playerController;
    public Transform secondaryAttackPoint;
    public GameObject activeSubWeapon;

    public int heartCounter = 5;
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
        if (morningStar >= 1)
        { 
            attackDamage = 4;
        }
        if(morningStar >= 2)
        {
            attackLength = 1.55f;
        }
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
        if (playerGroundedCheck.isGrounded == true)
        {
            if (playerController.isCrouching == true)
            {
                playerAnimator.Play("Crouch Attack " + (1 + morningStar).ToString());
            }
            else
                playerAnimator.Play("Idle Attack " + (1 + morningStar).ToString());
                player.velocity = Vector2.zero;
        }

        if (playerGroundedCheck.isGrounded == false && playerController.onStairs == false)
        {
            playerAnimator.Play("Jump Attack " + (1 + morningStar).ToString());
        }

        if (playerController.onStairs == true)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Upstairs"))
            {
                playerAnimator.Play("Upstairs Attack " + (1 + morningStar).ToString());
            }
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Downstairs"))
            {
                playerAnimator.Play("Downstairs Attack " + (1 + morningStar).ToString());
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
    }

    void SecondaryAttack()
    {
        if (playerGroundedCheck.isGrounded == true)
        {
            if (playerController.isCrouching == true)
            {
                playerAnimator.Play("Crouch Sub");
            }
            else
                playerAnimator.Play("Idle Sub");
                player.velocity = Vector2.zero;
        }
        else if (playerGroundedCheck.isGrounded == false && playerController.onStairs == false)
        {
            playerAnimator.Play("Jump Sub");
        }

        if (playerController.onStairs == true)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Upstairs"))
            {
                playerAnimator.Play("Upstairs Sub");
            }
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Downstairs"))
            {
                playerAnimator.Play("Downstairs Sub");
            }
        }

        Instantiate(activeSubWeapon, secondaryAttackPoint.position, secondaryAttackPoint.rotation);
        subWeaponsCount += 1;
    }
        void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(primaryAttackPoint.position, attackRange);
    }
}
