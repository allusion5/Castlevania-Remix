using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack3 : MonoBehaviour
{
    public int health = 1;
    public Transform attack;
    public Rigidbody2D attackBody;
    public playerAttack playerAttack;
    public int facing;

    void Start()
    {
        health = 1;
        facing = GameObject.Find("Boss").GetComponent<BossController>().facing;
        playerAttack = GameObject.Find("player").GetComponent<playerAttack>();
        attack = GetComponent<Transform>();
        attackBody = GetComponent<Rigidbody2D>();
        if (facing == 1)
        { attack.rotation = new Quaternion(0, 0, 65, 0); }
        else attack.rotation = new Quaternion(0, 0, 295, 0);
        attackBody.velocity = new Vector2(facing * 4f, -4f);
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
