using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feed : MonoBehaviour
{
    [SerializeField] float lifeTime = 3.0f; // seconds
    public bool isGrounded = false;

    private void Start() {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            Invoke(nameof(Deactivate), lifeTime);
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);        
    }

    private void OnDisable() {
        CancelInvoke(nameof(Deactivate));
    }
}
