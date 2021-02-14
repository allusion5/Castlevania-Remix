using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack2 : MonoBehaviour
{
    public Transform attack;
    public Rigidbody2D attackBody;
    public playerAttack playerAttack;
    public int facing;
    public int health = 1;

    void Start()
    {

        facing = GameObject.Find("Boss").GetComponent<BossController>().facing;
        playerAttack = GameObject.Find("player").GetComponent<playerAttack>();
        attack = GetComponent<Transform>();
        attackBody = GetComponent<Rigidbody2D>();
        if (facing == 1)
        { attack.rotation = new Quaternion(0, 0, 90, 0); }
        else attack.rotation = new Quaternion(0, 0, 270, 0);
        attackBody.velocity = new Vector2(facing * 4f, 0f);
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        { 
            Destroy(gameObject);
        }
    }
    public void TakeDamage(int attackDamage)
    {
        health -= playerAttack.attackDamage;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
