using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public BossController bossController;
    //public BossController bossController;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    private void Start()
    {
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = 16;
    }
    public void FixedUpdate()
    {
        healthSlider.value = bossController.bossHealth;
    }

    public void OnSceneLoad(Scene Level2, LoadSceneMode load)
    {
        bossController = GameObject.Find("Boss").GetComponent<BossController>();
    }
}
