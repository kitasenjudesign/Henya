using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ProbeCheck : MonoBehaviour
{

    [SerializeField] private AREnvironmentProbeManager _manager;
    
    void Start()
    {
        Invoke("_onHoge",2f);
    }

    void _onHoge(){
        
        _manager.enabled = false;
        _manager.automaticPlacement=false;

    }

}
