using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int _initialNumberOfPackages;

    // Start is called before the first frame update
    void Start()
    {
        var packages = GameObject.FindGameObjectsWithTag("Package");
        _initialNumberOfPackages = packages.Count();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
