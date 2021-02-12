using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Animations;

public class playerController : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody2D player;
    public CapsuleCollider2D playerCollider;
    private PlayerGroundedCheck playerGroundedCheck;
    private playerAttack playerAttack;
    public Canvas UI;
    public int playerHealth = 16;
    public PlayerHealthBar healthBar;
    public int playerFacing = 1;
    public bool isInvulnerable;
    public float invulnerableUntil;
    public float jumpForce;
    public bool isHit = false;
    public bool isDead = false;
    private float speed = 5f;

    public Transform primaryAttackPoint;
    public Transform subAttackPoint;

    public bool nearStairs = false;
    public bool onStairs = false;
    public bool goingUp = false;
    public bool goingDown = false;
    public GameObject nearbyStairs;
    public Transform nearbyStairsTransform;
    public StairsInfo stairsInfo;
    public Vector2 stairsStartPoint;
    public Vector2 stairsEndPoint;
    public int stairsFacing;
    public int stairsSlope;
    public float maxVelocity;
    public float stepRate = 4f;

    private Animator playerAnimator;
    public bool isCrouching;
    void Start()
    {
        playerGroundedCheck = GetComponentInChildren<PlayerGroundedCheck>();
        playerTransform = GetComponent<Transform>();
        player = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerAttack = GetComponent<playerAttack>();
        UI = FindObjectOfType<Canvas>();
        //healthBar = UI.GetComponentInChildren<PlayerHealthBar>();
        primaryAttackPoint = transform.Find("primaryAttackPoint");
        subAttackPoint = transform.Find("subAttackPoint");
        playerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        /*if (playerGroundedCheck.isGrounded == false && onStairs == false && playerAttack.isAttacking == false)
        {
            playerAnimator.SetInteger("State", 2);
        }*/
        if (onStairs == true)
        {
            if(playerAttack.isAttacking == false)
            {
            if (stairsSlope == 1)
            {
                if (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") != 0)
                {
                    if (playerFacing == 1)
                    {
                        playerAnimator.SetInteger("State", 4);
                    }
                    else if (playerFacing == -1)
                    {
                        playerAnimator.SetInteger("State", 5);
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") == 0)
                {
                    if (playerFacing == 1)
                    {
                        playerAnimator.SetInteger("State", 28);
                    }
                    else if (playerFacing == -1)
                    {
                        playerAnimator.SetInteger("State", 29);
                    }
                }
            }
            else if (stairsSlope == -1)
            {
                if (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") != 0)
                {
                    if (playerFacing == 1)
                    {
                        playerAnimator.SetInteger("State", 5);
                    }
                    else if (playerFacing == -1)
                    {
                        playerAnimator.SetInteger("State", 4);
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical") == 0)
                {
                    if (playerFacing == 1)
                    {
                        playerAnimator.SetInteger("State", 29);
                    }
                    else if (playerFacing == -1)
                    {
                        playerAnimator.SetInteger("State", 28);
                    }
                }
            }
        }
            }
    }
    void FixedUpdate()
    {
        if (Time.time >= invulnerableUntil && isDead == false)
        {
            isInvulnerable = false;
        }

        if (Time.time >= playerAttack.nextMoveTime && isDead == false)
        {
            isHit = false;
        }

        if (onStairs == false)
        {
            player.gravityScale = 1f;
            playerCollider.isTrigger = false;
            playerGroundedCheck.groundedCollider.enabled = true;
            goingDown = false;
            goingUp = false;
            playerGroundedCheck.canExitAtStart = false;

            if (playerGroundedCheck.isGrounded == true && isHit == false && isDead == false)
            {
                if (player.velocity.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    playerFacing = 1;
                }
                else if (player.velocity.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    playerFacing = -1;
                }
                {
                    if (Input.GetAxisRaw("Horizontal") == 0)
                    {
                        playerAnimator.SetInteger("State", 0);
                        player.velocity = Vector2.zero;
                    }


                    else if (Time.time >= playerAttack.nextMoveTime)
                    {
                        float moveHorizontal = Input.GetAxisRaw("Horizontal");
                        playerAnimator.SetInteger("State", 1);
                        Vector2 movement = new Vector2(moveHorizontal, 0);

                        player.velocity = movement * speed;
                        player.velocity = Vector2.ClampMagnitude(player.velocity, maxVelocity);
                    }
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if (nearStairs == false || (nearStairs == true && (stairsFacing == stairsSlope)))
                        {
                            isCrouching = true;
                            playerAnimator.SetInteger("State", 3);
                            player.velocity = Vector2.zero;
                            playerCollider.offset = new Vector2(0, -0.5f);
                            playerCollider.size = new Vector2(1, 1);
                            primaryAttackPoint.localPosition = new Vector3 (0.7f, -0.375f, 0f);
                        }
                    }

                    else
                    {
                        isCrouching = false;
                        playerCollider.offset = new Vector2(0, 0);
                        playerCollider.size = new Vector2(1, 2);
                        primaryAttackPoint.localPosition = new Vector3(0.7f, -0.125f, 0f);
                    }
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        if (nearStairs == false || (nearStairs == true && (stairsFacing != stairsSlope)))
                        {
                            player.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                        }
                    }

                    if (player.velocity.y == 0 && nearStairs == true)
                    {
                        if (stairsFacing == 1 && stairsSlope == 1)
                        {
                            ApproachingStairsEntranceFromLeftPositiveSlope();
                        }
                        else if (stairsFacing == 1 && stairsSlope == -1)
                        {
                            ApproachingStairsEntranceFromLeftNegativeSlope();
                        }
                        else if (stairsFacing == -1 && stairsSlope == -1)
                        {
                            ApproachingStairsEntranceFromRightNegativeSlope();
                        }
                        else if (stairsFacing == -1 && stairsSlope == 1)
                        {
                            ApproachingStairsEntranceFromRightPositiveSlope();
                        }
                    }
                }
            }
        }
        if (onStairs == true)
        {
            if (stairsFacing == 1 && stairsSlope == 1)
            {
                PositiveSlopeStairsFromLeft();
            }
            else if(stairsFacing == 1 && stairsSlope == -1)
            {
                NegativeSlopeStairsFromLeft();
            }
            else if (stairsFacing == -1 && stairsSlope == -1)
            {
                NegativeSlopeStairsFromRight();
            }
            else if (stairsFacing == -1 && stairsSlope == 1)
            {
                PositiveSlopeStairsFromRight();
            }
        }
    }

    void ApproachingStairsEntranceFromLeftPositiveSlope()
    {
        if (Input.GetAxisRaw("Vertical") > 0 && playerTransform.position.x != nearbyStairsTransform.position.x)
        {
            if (playerTransform.position.x < nearbyStairsTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (playerTransform.position.x > nearbyStairsTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            player.velocity = Vector2.zero;
            player.position = Vector2.MoveTowards(player.position, new Vector2(nearbyStairsTransform.position.x, player.position.y), stepRate * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Vertical") > 0 && playerTransform.position.x == nearbyStairsTransform.position.x)
        {
            stairsStartPoint = playerTransform.position;
            stairsEndPoint = stairsInfo.endPoint.transform.position;
            transform.localScale = new Vector3(1, 1, 1);
            onStairs = true;
            player.velocity = Vector2.zero;
        }
    }

    void ApproachingStairsEntranceFromLeftNegativeSlope()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && playerTransform.position.x != nearbyStairsTransform.position.x)
        {
            if (playerTransform.position.x < nearbyStairsTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (playerTransform.position.x > nearbyStairsTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            player.velocity = Vector2.zero;
            player.position = Vector2.MoveTowards(player.position, new Vector2(nearbyStairsTransform.position.x, player.position.y), stepRate * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Vertical") < 0 && playerTransform.position.x == nearbyStairsTransform.position.x)
        {
            stairsStartPoint = playerTransform.position;
            stairsEndPoint = stairsInfo.endPoint.transform.position;
            transform.localScale = new Vector3(1, 1, 1);
            onStairs = true;
            playerGroundedCheck.groundedCollider.enabled = false;
            player.velocity = Vector2.zero;
        }
    }
    void ApproachingStairsEntranceFromRightPositiveSlope()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && playerTransform.position.x != nearbyStairsTransform.position.x)
        {
            if (playerTransform.position.x < nearbyStairsTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (playerTransform.position.x > nearbyStairsTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            player.velocity = Vector2.zero;
            player.position = Vector2.MoveTowards(player.position, new Vector2(nearbyStairsTransform.position.x, player.position.y), stepRate * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Vertical") < 0 && playerTransform.position.x == nearbyStairsTransform.position.x)
        {
            stairsStartPoint = playerTransform.position;
            stairsEndPoint = stairsInfo.endPoint.transform.position;
            transform.localScale = new Vector3(-1, 1, 1);
            onStairs = true;
            playerGroundedCheck.groundedCollider.enabled = false;
            player.velocity = Vector2.zero;
        }
    }
    void ApproachingStairsEntranceFromRightNegativeSlope()
    {
        if (Input.GetAxisRaw("Vertical") > 0 && playerTransform.position.x != nearbyStairsTransform.position.x)
        {
            if (playerTransform.position.x < nearbyStairsTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (playerTransform.position.x > nearbyStairsTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            player.velocity = Vector2.zero;
            player.position = Vector2.MoveTowards(player.position, new Vector2(nearbyStairsTransform.position.x, player.position.y), stepRate * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Vertical") > 0 && playerTransform.position.x == nearbyStairsTransform.position.x)
        {
            stairsStartPoint = playerTransform.position;
            stairsEndPoint = stairsInfo.endPoint.transform.position;
            transform.localScale = new Vector3(-1, 1, 1);
            onStairs = true;
            player.velocity = Vector2.zero;
        }
    }

    void PositiveSlopeStairsFromLeft()
    {
            goingUp = true;
            player.gravityScale = 0f;
            playerCollider.isTrigger = true;
            if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                playerFacing = 1;
                transform.localScale = new Vector3(1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsEndPoint, stepRate * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Horizontal") < 0)
            {
                playerFacing = -1;
                transform.localScale = new Vector3(-1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsStartPoint, stepRate * Time.deltaTime);
            }
    }

    void PositiveSlopeStairsFromRight()
    {
            goingDown = true;
            player.gravityScale = 0f;
            playerCollider.isTrigger = true;
            if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Horizontal") < 0)
            {
                playerFacing = -1;
                transform.localScale = new Vector3(-1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsEndPoint, stepRate * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                playerFacing = 1;
                transform.localScale = new Vector3(1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsStartPoint, stepRate * Time.deltaTime);
            }
    }
    void NegativeSlopeStairsFromLeft()
    {
            goingDown = true;
            player.gravityScale = 0f;
            playerCollider.isTrigger = true;
            if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                playerFacing = 1;
                transform.localScale = new Vector3(1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsEndPoint, stepRate * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") < 0)
            {
                playerFacing = -1;
                transform.localScale = new Vector3(-1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsStartPoint, stepRate * Time.deltaTime);
            }
    }
    void NegativeSlopeStairsFromRight()
    {
            goingUp = true;
            player.gravityScale = 0f;
            playerCollider.isTrigger = true;
            if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") < 0)
            {
                playerFacing = -1;
                transform.localScale = new Vector3(-1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsEndPoint, stepRate * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                playerFacing = 1;
                transform.localScale = new Vector3(1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsStartPoint, stepRate * Time.deltaTime);
            }
    }

    void TakeDamage()
    {
        if (playerHealth > 0)
        {
            isHit = true;
            playerAnimator.SetInteger("State", 6);
            playerAttack.nextMoveTime = Time.time + 0.75f;
            isInvulnerable = true;
            playerHealth -= 4;
            invulnerableUntil = 2.75f + Time.time;
            
            if (onStairs == false)
            {
                player.AddForce(new Vector2(playerFacing * -4f, 4f), ForceMode2D.Impulse);
            }
        }

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        playerAnimator.SetInteger("State", 7);
        //SceneManager.LoadScene()
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs") && playerGroundedCheck.isGrounded == true)
        {
            nearStairs = true;
            nearbyStairs = collision.gameObject;
            stairsInfo = nearbyStairs.GetComponent<StairsInfo>();
            nearbyStairsTransform = nearbyStairs.GetComponent<Transform>();
            stairsFacing = stairsInfo.facing;
            stairsSlope = stairsInfo.slope;
        }
        if (collision.gameObject.CompareTag("Enemy") && isInvulnerable == false && isHit == false)
        {
            TakeDamage();
            //knockback + take damage + invincibility
            //die if necessary
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            nearStairs = false;
            nearbyStairs = null;
            stairsInfo = null;
            nearbyStairsTransform = null;
        }
    }
}

