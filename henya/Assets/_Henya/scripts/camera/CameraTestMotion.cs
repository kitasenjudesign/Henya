using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTestMotion : MonoBehaviour
{
    #if UNITY_EDITOR

    private Vector3 _target;
    [SerializeField] private Camera _cam;

    void Start()
    {

    }

    void Update()
    {
        
        _cam.transform.rotation = Quaternion.Euler(
            10f*Mathf.Sin(Time.realtimeSinceStartup ),
            10f*Mathf.Cos(Time.realtimeSinceStartup + Mathf.PI*0.2f),
            0
        );

    }
    #endif

}
