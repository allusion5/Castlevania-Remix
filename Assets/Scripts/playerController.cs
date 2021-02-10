using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    public bool nearStairs = false;
    public bool onStairs = false;
    public GameObject nearbyStairs;
    public Transform nearbyStairsTransform;
    public StairsInfo stairsInfo;
    public Vector2 stairsStartPoint;
    public Vector2 stairsEndPoint;
    public int stairsFacing;
    public int stairsSlope;
    public float maxVelocity;
    public float stepRate = 4f;



    // Start is called before the first frame update
    void Start()
    {
        playerGroundedCheck = GetComponentInChildren<PlayerGroundedCheck>();
        playerTransform = GetComponent<Transform>();
        player = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerAttack = GetComponent<playerAttack>();
        UI = FindObjectOfType<Canvas>();
        healthBar = UI.GetComponentInChildren<PlayerHealthBar>();
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

            if (playerGroundedCheck.isGrounded == true && onStairs == false && isHit == false && isDead == false)
            {
                player.gravityScale = 1f;

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
                        player.velocity = Vector2.zero;
                    }


                    else if (Time.time >= playerAttack.nextMoveTime)
                    {
                        float moveHorizontal = Input.GetAxisRaw("Horizontal");

                        Vector2 movement = new Vector2(moveHorizontal, 0);

                        player.velocity = movement * speed;
                        player.velocity = Vector2.ClampMagnitude(player.velocity, 5);
                    }
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        if (nearStairs == false || (nearStairs == true && (stairsFacing == stairsSlope)))
                        {
                            player.velocity = Vector2.zero;
                            playerCollider.offset = new Vector2(0, -0.5f);
                            playerCollider.size = new Vector2(1, 1);
                        }
                    }

                    else
                    {
                        playerCollider.offset = new Vector2(0, 0);
                        playerCollider.size = new Vector2(1, 2);
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

        if (onStairs == true)
        {
            if (stairsSlope == 1)
            {
                PositiveSlopeStairs();
            }
            else if (stairsSlope == -1)
            {
                NegativeSlopeStairs();
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

    void PositiveSlopeStairs()
    {
        if (onStairs == true)
        {
            player.gravityScale = 0f;
            playerCollider.isTrigger = true;
            if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsEndPoint, stepRate * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsStartPoint, stepRate * Time.deltaTime);
            }
        }
    }
    void NegativeSlopeStairs()
    {
        if (onStairs == true)
        {
            player.gravityScale = 0f;
            playerCollider.isTrigger = true;
            if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsEndPoint, stepRate * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                player.position = Vector2.MoveTowards(player.position, stairsStartPoint, stepRate * Time.deltaTime);
            }
        }
    }

    void TakeDamage()
    {
        if (playerHealth > 0)
        {
            isHit = true;
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
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
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

