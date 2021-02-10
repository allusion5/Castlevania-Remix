using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedCheck : MonoBehaviour
{
    public BoxCollider2D groundedCollider;
    public playerController playerController;
    public bool isGrounded;

    private void Start()
    {
        groundedCollider = GetComponent<BoxCollider2D>();
        playerController = GetComponentInParent<playerController>();
    }

    private void FixedUpdate()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        { 
            isGrounded = true;
            playerController.onStairs = false;
            playerController.playerCollider.isTrigger = false;
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
