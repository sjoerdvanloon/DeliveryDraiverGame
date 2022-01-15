using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    [SerializeField] private SoundManager _soundManager;

    private int _packages = 0;

    private void Start()
    {
       // _soundManager = GetComponent<SoundManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Auch");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Package")
        {
            Debug.Log("Package picked up");

            _packages++;

            _soundManager.PlayPickupSound();
        }

        if (collision.tag == "Customer")
        {
            var anyPackages = _packages > 0;
            if (anyPackages)
            {
                Debug.Log("Package delivered");
                _soundManager.PlayDeliveredSound();
                _packages--;

            }
            else
            {
                Debug.Log("Empty");
                _soundManager.PlayWrongSound();
            }


        }
    }



}
