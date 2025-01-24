using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedCountText;
    [SerializeField] private TextMeshProUGUI eggsCountText;
    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject startScreenCanvas;
    public TextMeshProUGUI healthText;
    public bool gameOver;

    public static GameManager Instance { get; private set; }
    private int feedHealthPoints = 5;
    private int maxChickenHealth = 100;
    private float eggLayInterval = 20;
    private decimal eggSellPrice = 2.5m;
    private decimal playerCash = 0;

    public decimal Cash
    {
        get
        {
            return playerCash;
        }

        set
        {
            playerCash = value;
            cashText.SetText($"{playerCash:C}");
        }
    }

    private int eggsLaid = 0;

    public int Eggs
    {
        get
        {
            return eggsLaid;
        }

        set
        {
            eggsLaid = value;
            eggsCountText.SetText($"Eggs: {eggsLaid}");
        }
    }

    private int feedCount = 0;

    public int FeedCount
    {
        get
        {
            return feedCount;
        }

        set
        {
            feedCount = value;
            feedCountText.SetText($"Feed collected: {feedCount}");
        }
    }

    private int chickenHealth;

    public int ChickenHealth
    {
        get
        {
            return chickenHealth;
        }

        set
        {
            chickenHealth = value;
            healthText.SetText($"Chick health: {chickenHealth}");
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        ChickenHealth = 100;
        Eggs = 0;
        Cash = 0;
        startScreenCanvas.SetActive(true);
        InvokeRepeating(nameof(LayEgg), eggLayInterval, eggLayInterval);
    }


    void Update()
    {
        if (!gameOver && ChickenHealth <= 0)
        {
            GameOver();
        }
    }

    public void ReplenishChickenHealth()
    {
        int healthAmt = FeedCount * feedHealthPoints;
        int newHealth = healthAmt + ChickenHealth;
        int excessHealth = Math.Clamp(newHealth - maxChickenHealth, 0, 500);

        ChickenHealth = Mathf.Clamp(newHealth, 0, maxChickenHealth);
        FeedCount = excessHealth / feedHealthPoints;
    }

    void LayEgg()
    {
        if (!gameOver)
        {
            Eggs++;
        }
    }

    public void DamageChicken()
    {
        ChickenHealth = Mathf.Clamp(ChickenHealth - 10, 0, 100);
    }

    public void SellEggs()
    {
        if(gameOver) return;

        Cash += eggSellPrice * Eggs;
        Eggs = 0;
    }

    public void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over!");

        gameOverCanvas.SetActive(true);
    }

    public void HideStartScreen()
    {
        startScreenCanvas.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
