using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject chickenFeedPrefab;
    [SerializeField] GameObject startCollectionButton;
    [SerializeField] TextMeshProUGUI collectionTimerText;

    public static SpawnManager Instance;

    private int feedCollectionTime = 25;
    public bool startSpawningFeeds;
    private float chickenSpawnInterval = 3;
    ObjectPooler feedPooler;

    private void Awake() {
        Instance = this;
    }
    
    void Start()
    {
        feedPooler = ObjectPooler.Instance;
        InvokeRepeating(nameof(SpawnChickenFeed), 1, chickenSpawnInterval);
    }
    
    void Update()
    {
        if(GameManager.Instance.gameOver){
            CancelInvoke();
            StopCoroutine(CountDownFeedCollectionTime());
            startCollectionButton.SetActive(false);
            Destroy(this);
        }

        if(!GameManager.Instance.gameOver && Input.GetKeyDown(KeyCode.Space) && !startSpawningFeeds)
        {
            StartCollectingFeeds();
        }
    }

    void SpawnChickenFeed()
    {
        if(!startSpawningFeeds) return;

        GameObject item = feedPooler.GetItem();
        if(item != null){
            item.transform.position = GetFeedSpawnPosition();
            item.GetComponent<Feed>().isGrounded = false;
            item.SetActive(true);
        }
    }

    Vector3 GetFeedSpawnPosition()
    {
        float randomX = Random.Range(-1, 8.5f);
        float randomZ = Random.Range(-3.5f, 6.5f);

        return new Vector3(randomX, 15.0f, randomZ);
    }

    public void StartCollectingFeeds()
    {
        if(GameManager.Instance.gameOver) return;
        
        startSpawningFeeds = true;
        feedCollectionTime = Random.Range(25, 45);
        startCollectionButton.SetActive(false);

        StartCoroutine(CountDownFeedCollectionTime());
        collectionTimerText.SetText($"Collect Time: {feedCollectionTime} sec");
        collectionTimerText.enabled = true;
    }

    IEnumerator CountDownFeedCollectionTime()
    {
        while(startSpawningFeeds && !GameManager.Instance.gameOver)
        {
            yield return new WaitForSeconds(1.0f);
            feedCollectionTime --;
            collectionTimerText.SetText($"Collect Time: {feedCollectionTime} sec");

            if(feedCollectionTime == 0)
            {
                startSpawningFeeds = false;
                startCollectionButton.SetActive(true);
                collectionTimerText.enabled = false;
            }
        }
    }
}
