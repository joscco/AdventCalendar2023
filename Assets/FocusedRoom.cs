using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedRoom : MonoBehaviour
{
    [SerializeField] private GameObject virtualCamera;

    private void Start()
    {
        virtualCamera.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCamera.SetActive(false);
        }
    }
}
