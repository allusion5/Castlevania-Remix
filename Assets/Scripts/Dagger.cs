using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public Transform attackPoint;
    private Transform daggerTransform;
    public GameObject player;
    public Rigidbody2D playerRigidbody;
    public playerAttack playerAttack;
    public float facing;
    private Vector3 targetPoint;
    public float speed;
    private bool destroyedByEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        daggerTransform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        facing = player.transform.localScale.x;
        playerAttack = player.GetComponent<playerAttack>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        attackPoint = player.transform.Find("subAttackPoint");
        daggerTransform.position = attackPoint.position;

        if (facing > 0)
        {
            targetPoint = new Vector3((daggerTransform.position.x + 50f), daggerTransform.position.y, 0);
        }
        else if (facing < 0)
        {
            targetPoint = new Vector3((daggerTransform.position.x - 50f), daggerTransform.position.y, 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        daggerTransform.position = Vector3.MoveTowards(daggerTransform.position, targetPoint, speed * Time.deltaTime);

        if (daggerTransform.position == targetPoint)
        {
            playerAttack.subWeaponsCount -= 1;
            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        if (destroyedByEnemy == false)
        {
            playerAttack.subWeaponsCount -= 1;
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //dealdamage
            playerAttack.subWeaponsCount -= 1;
            destroyedByEnemy = true;
            Destroy(gameObject);
        }
    }
}
