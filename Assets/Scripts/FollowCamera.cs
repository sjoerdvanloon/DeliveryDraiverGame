using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject _thingToFollow;
    [SerializeField] int _cameraDistance = 10;

    void LateUpdate()
    {
        transform.position = _thingToFollow.transform.position + new Vector3(0, 0, -_cameraDistance);
    }
}
