using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject boss;
    //public BossController bossController;

    private void Start()
    {
        //boss = GameObject.FindGameObjectWithTag("Boss");
        //bossController = boss.GetComponent<BossController>();
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = 16;
    }
    public void FixedUpdate()
    {
        //healthSlider.value = BossController.bossHealth;
        //if boss spawn get component
    }
}
