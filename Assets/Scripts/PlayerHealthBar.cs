using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject player;
    public playerController playerController;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<playerController>();
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = playerController.playerHealth;
    }
    public void FixedUpdate()
    {
        healthSlider.value = playerController.playerHealth;
    }
}