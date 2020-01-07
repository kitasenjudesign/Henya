﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour
{


    private List<TubeBrush> _lines;
    [SerializeField] private TubeBrush _prefab;
    private TubeBrush _current;

    // Start is called before the first frame update
    void Start()
    {
        
        _lines = new List<TubeBrush>();
        _prefab.gameObject.SetActive(false);

    }

    private void _makeLine(){
        //line
        if(_current) _current.Hide();

        var line = Instantiate(_prefab, transform, false);
        line.Init();
        line.gameObject.SetActive(true);
        

        _current = line;

        _lines.Add(line);

        if(_lines.Count > 50){
            
            Destroy(_lines[0].gameObject);
            _lines.RemoveAt(0);
            
        }
        

    }

    void _removeAll(){

        for(int i=0;i<_lines.Count;i++){
            Destroy(_lines[i].gameObject);
        }
        _lines = new List<TubeBrush>();

    }


    // Update is called once per frame
    void Update()
    {



        if (Input.touchCount > 0){




            Touch t = Input.GetTouch(0);

            if( Input.touchCount == 1 ){
                if( t.phase == TouchPhase.Began ){

                    Debug.Log("touch!!!!");
                    _makeLine();

                }
            }else if(Input.touchCount == 2){

                _removeAll();

            }

        }else{

            if( Input.GetMouseButtonDown(0) ){

                Debug.Log("touch!!!!");                
                _makeLine();

            }
            if(Input.GetMouseButtonDown(1)){
                
                _removeAll();

            }

        }
        

    }


}
