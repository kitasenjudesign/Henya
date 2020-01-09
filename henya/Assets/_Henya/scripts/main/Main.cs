using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Main : MonoBehaviour
{

    [SerializeField] public ARCameraBackground _arBackground;
    [SerializeField] public AROcclusionManager _humanBodyManager;
    //public RenderTexture _camTex;
    


    //[SerializeField] private 

    // Start is called before the first frame update
    void Start()
    {
        var v = new Vector3(1f,1f,1f);
        Debug.Log("aaa" + v.magnitude);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
