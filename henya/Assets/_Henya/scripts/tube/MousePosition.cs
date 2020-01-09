using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition {

    public List<Vector3> _positions;
    private int NUM = 150;
    private Vector3 _oldPos;
    public bool updating = false;
    private int _index = 0;
    public bool isFinish=false;
    
    public void Init(int nn){

        NUM = nn;
        _oldPos = Vector3.zero;
        _positions = new List<Vector3>();
        //for(int i=0;i<NUM;i++){
        //    _positions.Add(Vector3.zero);
        //}

    }

    public void Upadate(Camera camera, float dist){

        if ( Input.GetMouseButton (0) || Input.touchCount > 0) {
            
            //Debug.Log ("スクリーン座標" + Input.mousePosition);
            #if UNITY_EDITOR

                Vector3 screen_point = Input.mousePosition;

            #else
                
                Touch touch = Input.GetTouch(0);
                Vector3 screen_point = new Vector3(
                    touch.position.x,
                    touch.position.y,
                    0
                );

            #endif

            
            screen_point.z = dist;//1.0f;

                
            var pos = camera.ScreenToWorldPoint (screen_point);
            var sub = pos - _oldPos;     

            if( sub.magnitude > 0.02f){

                _oldPos = pos;
                updating=true;

                if(_positions.Count==0){

                    for(int i=0;i<NUM;i++){
                        _positions.Insert(0,pos);
                    }

                }

                //追加してる。最初に追加しちゃおう
                //if(_positions.Count>=NUM){
                    //_positions.RemoveAt(_positions.Count-1);
                    //_positions.RemoveAt(0);
                //}
                //_positions.Insert(0,pos);

                var currentIndex = _index%_positions.Count;
                _positions[ currentIndex ] = pos;
                for(int i=currentIndex+1;i<NUM;i++){
                    _positions[ i ] = pos;
                }
                _index++;

                if(_index>=NUM){
                    isFinish=true;
                }

            }

            //Debug.Log ("ワールド座標" + camera.ScreenToWorldPoint (screen_point));

        }

    }

}