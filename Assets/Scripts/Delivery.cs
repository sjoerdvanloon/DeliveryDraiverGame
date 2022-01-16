using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    /// [SerializeField] private SoundManager _soundManager = null;
    [SerializeField] private int _maxPackages = 1;
    [SerializeField] private float _packagePickupSpeed = 0.1f;
    [SerializeField] private Color32 _hasPackagesColor = new Color32(1, 1, 1, 1);
    [SerializeField] private Color32 _noPackagesColor = new Color32(1, 1, 1, 1);
    [SerializeField] private bool _log = false;


    private int _packages = 0;
    SpriteRenderer _spriteRenderer;
    AudioManager _audioManager;

    public event Action OnPickUp;
    public event Action<int> OnDeliver;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioManager = AudioManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Package")
        {
            var isFull = (_packages == _maxPackages);

            if (isFull)
            {
                _audioManager.PlaySound("TooManyPackages");
            }
            else
            {
                _audioManager.PlaySound("PickUp");
                Destroy(collision.gameObject, _packagePickupSpeed);
                _packages++;

                _spriteRenderer.color = _hasPackagesColor;
                OnPickUp();
            }
        }

        if (collision.tag == "Customer")
        {
            if (HasPackages())
            {
                _audioManager.PlaySound("Deliver");
                _packages -= 1;  // One package dropped off

                OnDeliver(1); // One package

                if (!HasPackages())
                {
                    _spriteRenderer.color = _noPackagesColor;
                }

            }
            else
            {
                Log("empty");
                _audioManager.PlaySound("NoPackages");
            }
        }
    }

    private bool HasPackages()
    {
        return _packages > 0;
    }

    private void Log(string message)
    {
        if (_log)
            Debug.Log(message);

    }
}
