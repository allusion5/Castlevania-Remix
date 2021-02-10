using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public Transform attackPoint;
    private Transform crossTransform;
    public GameObject player;
    public Secret secret;
    public Rigidbody2D playerRigidbody;
    public playerAttack playerAttack;
    public float facing;
    public float throwDirection;
    private Vector3 startPoint;
    private Vector3 targetPoint;
    public float range;
    public float speed;
    public int attackDamage = 1;
    private bool isReturning = false;
    private bool destroyedByPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        crossTransform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        facing = player.transform.localScale.x;
        playerAttack = player.GetComponent<playerAttack>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        attackPoint = player.transform.Find("subAttackPoint");
        crossTransform.position = attackPoint.position;
        startPoint = attackPoint.position;

        if (facing > 0)
        {
            targetPoint = new Vector3((crossTransform.position.x + range), crossTransform.position.y, 0);
            throwDirection = 1;
        }
        else if (facing < 0)
        {
            targetPoint = new Vector3((crossTransform.position.x - range), crossTransform.position.y, 0);
            throwDirection = -1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        crossTransform.position = Vector3.MoveTowards(crossTransform.position, targetPoint, speed * Time.deltaTime);

       if(crossTransform.position == targetPoint)
        {
            if (throwDirection == -1)
            {
                targetPoint = startPoint + new Vector3(50, 0, 0);
                isReturning = true;
            }
            else if(throwDirection == 1)
            {
                targetPoint = startPoint - new Vector3(50, 0, 0);
                isReturning = true;
            }
        }
       if(isReturning == true && crossTransform.position == targetPoint)
        {
            playerAttack.subWeaponsCount -= 1;
            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        if (isReturning == true && destroyedByPlayer == false)
        {
            playerAttack.subWeaponsCount -= 1;
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && isReturning == true)
        {
            playerAttack.subWeaponsCount -= 1;
            destroyedByPlayer = true;
            Destroy(gameObject);
        }
    }
}
