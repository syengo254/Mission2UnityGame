using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{   
    GameManager gameManager;
    public float healthDamageRate = 8;
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    
    void Update()
    {
        if(gameManager.gameOver)
        {
            Destroy(this);
        }
    }

    public void StartHealthDepletion()
    {
        // StopCoroutine(DissipateHealth());
        StartCoroutine(DissipateHealth());
    }

    IEnumerator DissipateHealth()
    {
        while(!gameManager.gameOver)
        {
            yield return new WaitForSeconds(healthDamageRate);
            gameManager.DamageChicken();
        }
    }
}
