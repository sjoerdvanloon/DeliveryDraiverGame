using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip[] _pickupSounds;
    [SerializeField] AudioClip[] _deliverySounds;
    [SerializeField] AudioClip[] _wrongSounds;
    [SerializeField] AudioClip[] _inventoryFullSounds;
    [SerializeField] AudioClip[] _brakeSounds;
    [SerializeField] AudioClip[] _drivingSounds;
    [SerializeField] AudioClip[] _crashSounds;
     AudioSource _audioSource;

    private static SoundManager _instance;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();
            }

            return _instance;

        }
    }

    void Awake()
    {
        _instance = this;
    }

    public void PlayPickupSound()
    {
        PlayRandomSound(_pickupSounds);
    }

    public void PlayDeliveredSound()
    {
        PlayRandomSound(_deliverySounds);
    }

    public void PlayWrongSound()
    {
        PlayRandomSound(_wrongSounds);
    }
    public void PlayBrakeSound()
    {
        PlayRandomSound(_brakeSounds);
    }

    public void PlayCrashSound()
    {
        PlayRandomSound(_crashSounds);
    }

    public void PlayRandomSound(IEnumerable<AudioClip> audioClips)
    {
        if (!audioClips.Any())
        {
            throw new ArgumentException("Empty list of audio clips passed", nameof(audioClips));
        }

        var index = UnityEngine.Random.Range(0, audioClips.Count() - 1);
        var audioClip = audioClips.ToArray()[index];
        Debug.Log($"Try to play sound {index + 1} of {audioClips.Count()} {audioClip.name}");

        if (_audioSource.isPlaying)
        {
            Debug.Log($"Other sound was playing");

            return;
        }

        _audioSource.PlayOneShot(audioClip);
    }

}
