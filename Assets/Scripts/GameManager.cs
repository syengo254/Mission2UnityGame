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
    public bool gameStarted;

    public static GameManager Instance { get; private set; }
    private int feedHealthPoints = 5;
    private int maxChickenHealth = 100;
    private int chickenDamageAmt = 10;
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
            feedCountText.SetText($"Feed collected: <b>{feedCount}</b>");
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
            if (chickenHealth < 40)
            {
                healthText.SetText($"Chick health: <color=#ee0000>{chickenHealth}</color>");
            }
            else
            {
                healthText.SetText($"Chick health: {chickenHealth}");
            }
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
        if (!gameOver && gameStarted)
        {
            Eggs++;
        }
    }

    public void DamageChicken()
    {
        ChickenHealth = Mathf.Clamp(ChickenHealth - chickenDamageAmt, 0, 100);
    }

    public void SellEggs()
    {
        if (gameOver) return;

        Cash += eggSellPrice * Eggs;
        Eggs = 0;
    }

    public void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over!");

        gameOverCanvas.SetActive(true);
    }

    public void StartTheGame()
    {
        startScreenCanvas.SetActive(false);
        gameStarted = true;
        AudioSource gameAudio = Camera.main.GetComponent<AudioSource>();
        Chicken chickenScript = GameObject.Find("Chicken").GetComponent<Chicken>();
        chickenScript.StartHealthDepletion();
        InvokeRepeating(nameof(LayEgg), eggLayInterval, eggLayInterval);
        gameAudio.enabled = true;
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        CancelInvoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
