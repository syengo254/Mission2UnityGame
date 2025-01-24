using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{   
    GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        StartHealthDepletion();
    }

    
    void Update()
    {
        if(gameManager.gameOver)
        {
            Destroy(this);
        }
    }

    void StartHealthDepletion()
    {
        // StopCoroutine(DissipateHealth());
        StartCoroutine(DissipateHealth());
    }

    IEnumerator DissipateHealth()
    {
        while(!gameManager.gameOver)
        {
            yield return new WaitForSeconds(7.5f);
            gameManager.DamageChicken();
        }
    }
}
