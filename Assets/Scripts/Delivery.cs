using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private int _maxPackages = 1;
    [SerializeField] private float _packagePickupSpeed = 0.1f;
    [SerializeField] private Color32 _hasPackagesColor = new Color32(1,1,1, 1);
    [SerializeField] private Color32 _noPackagesColor = new Color32(1, 1, 1, 1);
    

    private int _packages = 0;

    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Auch");
        _soundManager.PlayCrashSound();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Package")
        {
            var isFull = (_packages == _maxPackages);

            if (isFull)
            {
                Debug.Log("Full");
                _soundManager.PlayInventoryFullSound();
            }
            else
            {
                Debug.Log("Pickup");
                _soundManager.PlayPickupSound();
                Destroy(collision.gameObject, _packagePickupSpeed);
                _packages++;

                _spriteRenderer.color = _hasPackagesColor;
            }
        }

        if (collision.tag == "Customer")
        {
            if (HasPackages())
            {
                Debug.Log("Package delivered");
                _soundManager.PlayDeliveredSound();
                _packages--;

                if (!HasPackages())
                {
                    _spriteRenderer.color = _noPackagesColor;
                }

            }
            else
            {
                Debug.Log("Empty");
                _soundManager.PlayWrongSound();
            }


        }
    }

    private bool HasPackages()
    {
        return _packages > 0;
    }
}
