using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VerletRope {

    private List<VerletPoint> _points;
    private List<VerletStick> _sticks;
    public List<Vector3> _positions;

    public void Init(Vector3[] positions){

        
        _points = new List<VerletPoint>();
        _sticks = new List<VerletStick>();
        _positions = new List<Vector3>();

        for(int i=0;i<positions.Length;i++){

            var p = new VerletPoint(positions[i]);
            _points.Add( p );
            _positions.Add( p.position );

        }

        for(int i=0;i<positions.Length-1;i++){
            
            var d = _points[ i+0 ].position - _points[ i+1 ].position;
            var s = new VerletStick(
                _points[ i+0 ],
                _points[ i+1 ],
                d.magnitude,
                0.1f
            );
            _sticks.Add( s );

            if(Random.value<0.5f){
                _points[i].AddCoord(
                    new Vector3(
                        0.4f * (Random.value - 0.5f),
                        0.4f * (Random.value - 0.5f),
                        0.4f * (Random.value - 0.5f)
                    )
                );
            }
            
        }

        
    }

    public void Update(){

        for(int i=0;i<_points.Count;i++){
            _points[i].Update();
            _positions[i] = _points[i].position;
        }
	
        for(int i=0;i<_sticks.Count;i++){
            _sticks[i].Update();
        }

    }


    



}
