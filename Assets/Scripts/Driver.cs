using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] float _steerSpeed = 100f;
    [SerializeField] float _normalSpeed = 20f;
    [SerializeField] float _minSpeed = 5f;
    [SerializeField] float _maxSpeed = 40f;
    [SerializeField] float _slowSpeed = -10f;
    [SerializeField] bool _log = false;
    [SerializeField] bool _slowOnCrash = false;
    [SerializeField] AnimationCurve _speedUp;
    [SerializeField] GameObject _speedUpTrail;

    public event Action<float> OnSpeedChanged;
    public event Action OnCrash;
    //public Event OnCrash;

    float _driveSpeed;
    AudioManager _audioManager;
    private bool _currentlySpeedingUp = false;
    private float _currentSpeedUpStartTime = 0f; // 1 is after the moment
    TrailRenderer _trailRenderer;



    // Start is called before the first frame update
    void Start()
    {
        _driveSpeed = _normalSpeed;
        _audioManager = AudioManager.Instance;

        _trailRenderer = _speedUpTrail.GetComponent<TrailRenderer>();
        _trailRenderer.emitting = false;

        AudioManager.Instance.PlaySound("Drive");


    }

    private void AlterSpeed(float speedAlteration)
    {
        var newDriveSpeed = _driveSpeed + speedAlteration;
        SetSpeed(newDriveSpeed);
    }

    private void SetSpeed(float newDriveSpeed)
    {
        newDriveSpeed = Mathf.Clamp(newDriveSpeed, _minSpeed, _maxSpeed);

        Log($"Set {nameof(_driveSpeed)} from {_driveSpeed} to {newDriveSpeed}");

        //if (OnSpeedChanged != null)
        //{

        //}

        _driveSpeed = newDriveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: check tag or layer
        CrashedIntoSomething(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SpeedUp")
        {
            SpeedUp();
        }
    }

    private void SpeedUp()
    {
        if (_currentlySpeedingUp)
            return;

        Log($"Speedup");
        // _speedUpTrail.SetActive(true);
        _trailRenderer.emitting = true;
        _currentSpeedUpStartTime = Time.time;
        _currentlySpeedingUp = true;


        //   _speedUp.
        //AlterSpeed(_boostSpeed);
        //_soundManager.PlaySpeedUpSound();
    }

    private void StopSpeedingUp()
    {
        //_speedUpTrail.SetActive(false);
        _trailRenderer.emitting = false;
        _currentlySpeedingUp = false;

    }

    private void CrashedIntoSomething(GameObject crashedWith)
    {
        Log($"Crash");
        _audioManager.PlaySound("Crash");
        StopSpeedingUp();
        OnCrash();
        if (_slowOnCrash)
        {
            SetSpeed(_normalSpeed);
            AlterSpeed(_slowSpeed);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float steerAmount = Input.GetAxis("Horizontal") * _steerSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, 0, -steerAmount);
        float driveAmount = Input.GetAxis("Vertical") * _driveSpeed * Time.fixedDeltaTime;
        if (_currentlySpeedingUp)
        {
            var timeInSpeedUpCurve = Time.time - _currentSpeedUpStartTime;
            //Debug.Log($"time in speed curve {timeInSpeedUpCurve}");
            var lastCurveTime = _speedUp.keys.Last().time;
            var doneSpeedingUp = (timeInSpeedUpCurve >= lastCurveTime);
            if (doneSpeedingUp)
            {
                StopSpeedingUp();
            }
            else
            {
                var curveValue = _speedUp.Evaluate(timeInSpeedUpCurve );
                var extraSpeed = curveValue; // * Input.GetAxis("Vertical"); // Less control when always boosted
               // Debug.Log($"curve value {curveValue} extra speed {extraSpeed}");
                driveAmount += extraSpeed;
            }

        }
        transform.Translate(0, driveAmount, 0);
        OnSpeedChanged(driveAmount);

    }

    private void Log(string message)
    {
        if (_log)
            Debug.Log(message);

    }
}
