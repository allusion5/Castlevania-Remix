using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instanceGC;
    private GameObject player;
    private playerController playerController;
    private playerAttack playerAttack;

    public float timeRemaining = 90f;
    public bool gameActive;
    public int currentStage = 1;
    public int score;
    public Text timeRemainingText;
    public Text heartsText;
    public Text stageText;
    public Text scoreText;
    public Text statusText;
    public Image subWeaponSprite;
    public Image shotSprite;

    private void Awake()
    {
        if (instanceGC == null)
            instanceGC = this;
        else 
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<playerController>();
        playerAttack = player.GetComponent<playerAttack>();
        StartCoroutine(Timer());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        heartsText.text = " -" + playerAttack.heartCounter.ToString("00");
        stageText.text = "STAGE " + currentStage.ToString("00");
        scoreText.text = "SCORE-" + score.ToString("000000");
    }

    IEnumerator Timer()
    {
        while (timeRemaining >= 0)
        {
            timeRemainingText.text = "TIME " + timeRemaining.ToString("0000");
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }
        
        gameActive = false;
        playerController.Die();
        statusText.text = "GAME OVER \n R for Restart \n M for Menu";
    }
}
