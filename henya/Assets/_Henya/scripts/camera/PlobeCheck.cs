using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlobeCheck : MonoBehaviour
{
    [SerializeField] private AREnvironmentProbeManager _manager;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("_onHoge",2f);
    }

    void _onHoge(){
        
        _manager.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
