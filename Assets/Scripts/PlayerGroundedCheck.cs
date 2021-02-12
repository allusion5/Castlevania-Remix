using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerGroundedCheck : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody2D player;
    public BoxCollider2D groundedCollider;
    public playerController playerController;
    public Animator playerAnimator;
    public bool isGrounded;
    public bool canExitAtStart;

    private void Start()
    {
        player = GetComponentInParent<Rigidbody2D>();
        playerTransform = GetComponentInParent<Transform>();
        groundedCollider = GetComponent<BoxCollider2D>();
        playerController = GetComponentInParent<playerController>();
        playerAnimator = GetComponentInParent<Animator>();
    }

    private void FixedUpdate()
    {
        if (playerController.onStairs == true)
        {
            if (playerController.goingUp == true)
            {
                if (playerTransform.position.x != playerController.stairsStartPoint.x)
                {
                    canExitAtStart = true;
                }
                if (canExitAtStart == true)
                {
                    if (playerTransform.position.x == playerController.stairsStartPoint.x)
                    {
                        playerController.onStairs = false;
                        groundedCollider.enabled = true;
                        playerController.goingUp = false;
                        canExitAtStart = false;
                    }
                }
                if (playerTransform.position == new Vector3(playerController.stairsEndPoint.x, playerController.stairsEndPoint.y, 0))
                {
                    playerController.onStairs = false;
                    playerController.playerCollider.isTrigger = false;
                    playerController.goingUp = false;
                }
            }
            if (playerController.goingDown == true)
            {
                if (playerTransform.position.x != playerController.stairsStartPoint.x)
                {
                    canExitAtStart = true;
                }
                if (canExitAtStart == true)
                {
                    if (playerTransform.position.x == playerController.stairsStartPoint.x)
                    {
                        playerController.onStairs = false;
                        groundedCollider.enabled = true;
                        playerController.goingDown = false;
                        canExitAtStart = false;   
                    }
                }
                if (playerTransform.position.y <= playerController.stairsStartPoint.y - 1)
                {
                    groundedCollider.enabled = true;
                }
                else
                    groundedCollider.enabled = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && player.velocity.y == 0)
        {
            if (playerController.goingDown == true)
            {
                isGrounded = true;
                playerController.onStairs = false;
                playerController.playerCollider.isTrigger = false;
                playerController.goingDown = false;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && player.velocity.y == 0)
        {
            if (playerController.goingUp == false)
            {
                isGrounded = true;
                playerController.onStairs = false;
                playerController.playerCollider.isTrigger = false;
                playerController.goingDown = false;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            playerAnimator.SetInteger("State", 2);
        }
    }
}
