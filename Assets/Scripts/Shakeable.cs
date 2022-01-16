using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class Shakeable : MonoBehaviour
{
    public enum ShakeType { UseCurve, UseRandom }
    [SerializeField] ShakeType _type = ShakeType.UseRandom;
    [SerializeField] AnimationCurve _shake;
    [SerializeField] float _shakeIntensity = 0.05f;
    [SerializeField] float _shakeTime = 2f;
    [SerializeField] float _shakeInterval = 0.05f;
    [SerializeField] float _shakeMaxOffset = 2f;
    
    [SerializeField] bool _log = false;

    bool _isShaking = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Log("Player hit me");

            if (_isShaking)
                return;

            StartShaking();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Shake")]
    public void StartShaking()
    {
        if (_isShaking)
            return;

        StartCoroutine("Shake");

    }

    IEnumerator Shake()
    {
        if (_isShaking)
            yield return null;

        //var collider = GetComponent<Collider2D>();
        //collider.enabled = false;

        Log("Shake");
         _isShaking = true;
        var timeElapsed = 0f;
        var timeToStopShaking =  (_type == ShakeType.UseCurve) ? _shake.keys.Last().time : _shakeTime;
        var originalPosition = Vector3.zero + transform.position;

        while (timeElapsed < timeToStopShaking)
        {
            timeElapsed += Time.deltaTime;

            //Log($"{timeElapsed}");

            if (_type == ShakeType.UseCurve)
            {
                var curveValue = _shake.Evaluate(timeElapsed);

                var offsetX = curveValue * _shakeIntensity;
                var offsetY = curveValue * _shakeIntensity;
                Log($"Curve value{curveValue}, so offset x: {offsetX} and  offset y: {offsetY}");
                transform.position += new Vector3(offsetX, offsetY, 0);
                yield return null;

            }
            else
            {
                var offsetX = (Random.Range(-_shakeMaxOffset, _shakeMaxOffset)  + originalPosition.x);
                var offsetY = (Random.Range(-_shakeMaxOffset, _shakeMaxOffset) + originalPosition.y);
                transform.position = new Vector3(offsetX, offsetY, 0);

               /// yield return null;

                yield return new WaitForSeconds(_shakeInterval);

                //transform.position = originalPosition;

                //yield return null;
            }

        }

        transform.position = originalPosition;

     //  collider.enabled = true;
        _isShaking = false;

        //yield return new WaitForSeconds()
    }

    private void Log(string message)
    {
        if (_log)
            Debug.Log(message);

    }
}
