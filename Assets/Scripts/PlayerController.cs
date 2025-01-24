using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float rotateSpeed = 720;
    [SerializeField] ParticleSystem feedParticles;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] GameObject feedAndEggsControlHint;
    [SerializeField] GameObject shopControlHint;
    AudioSource playerSound;

    float horizontalInput;
    float verticalInput;
    GameManager gameManager;
    Animator playerAnimator;

    bool chickenTriggerActive = false;
    bool shopTriggerActive = false;

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerSound = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
    }


    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        MoveAndRotate();

        if(!gameManager.gameOver)
        {
            if(chickenTriggerActive){
                if(Input.GetKeyDown(KeyCode.E))
                {
                    gameManager.ReplenishChickenHealth();
                }
            }
            else if(shopTriggerActive)
            {
                if(Input.GetKeyDown(KeyCode.X))
                {
                    gameManager.SellEggs();
                }
            }
        }
    }

    private void MoveAndRotate()
    {
        if (!gameManager.gameOver)
        {
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
            movement.Normalize();

            transform.Translate(moveSpeed * Time.deltaTime * movement, Space.World);

            if(movement != Vector3.zero){
                Quaternion rotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    rotation,
                    rotateSpeed * Time.deltaTime
                );
            }

            // animate
            playerAnimator.SetFloat("Speed_f", movement.sqrMagnitude * 1.2f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Feed"))
        {
            other.gameObject.SetActive(false);
            feedParticles.transform.position = other.gameObject.transform.position;
            gameManager.FeedCount += 1;

            // sounds & effects
            feedParticles.Play();
            playerSound.PlayOneShot(pickupSound);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(SpawnManager.Instance.startSpawningFeeds) return;

        if (other.CompareTag("Chicken"))
        {
            chickenTriggerActive = true;
            ShowFeedInteractionControls();
        }
        else if (other.CompareTag("Shop"))
        {
            shopTriggerActive = true;
            ShowShopInteractionControls();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(SpawnManager.Instance.startSpawningFeeds) return;
        
        if (other.CompareTag("Chicken"))
        {
            chickenTriggerActive = false;
            Invoke(nameof(HideFeedInteractionControls), 1.0f);
        }
        else if (other.CompareTag("Shop"))
        {
            shopTriggerActive = false;
            Invoke(nameof(HideShopInteractionControls), 1.0f);
        }
    }

    private void ShowShopInteractionControls()
    {
        shopControlHint.SetActive(true);
    }

    private void HideShopInteractionControls()
    {
        shopControlHint.SetActive(false);
    }

    private void ShowFeedInteractionControls()
    {
        feedAndEggsControlHint.SetActive(true);
    }

    private void HideFeedInteractionControls()
    {
        feedAndEggsControlHint.SetActive(false);
    }
}
