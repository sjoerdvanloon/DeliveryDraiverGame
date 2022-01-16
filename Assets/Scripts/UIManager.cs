using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _speedText;
    [SerializeField] GameObject _scoreText;
    [SerializeField] GameObject _player;
    [SerializeField] bool _log;

    private int _lastDisplayedSpeed;
    private int _score = 0;


    // Start is called before the first frame update
    void Start()
    {
        var driver = _player.GetComponent<Driver>();
        driver.OnSpeedChanged +=  SetSpeedText;
        driver.OnCrash += () => SetScore(-100);

        var delivery = _player.GetComponent<Delivery>();
        delivery.OnPickUp += () => SetScore(100);
        delivery.OnDeliver += (numberOfPackagesDroppedOff) => SetScore(1000 * numberOfPackagesDroppedOff);
    }

    // Update is called once per frame
    void Update()
    {
     //   StartCoroutine("SetSpeedText");
    }

    private void Log(string message)
    {
        if (_log)
            Debug.Log(message);

    }

    private void SetScore (int addScore)
    {
        Log($"Setting new score by adding {addScore} to {_score}");
        _score += addScore;

        var text = _scoreText.GetComponent<Text>();
        text.text = $"{_score}";
    }

    private void SetSpeedText(float newSpeed)
    {
        var newDisplaySpeed = Mathf.Abs(Mathf.FloorToInt(newSpeed * 100));
        if (newDisplaySpeed == _lastDisplayedSpeed)
            return;

        Log("Setting speed text");

        var text = _speedText.GetComponent<Text>();
        text.text = $"{newDisplaySpeed.ToString().PadLeft(2, ' ')} km/u";

    }
}
