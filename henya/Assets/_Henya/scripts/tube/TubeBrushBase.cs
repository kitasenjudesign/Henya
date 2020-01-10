using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubeBrushBase : BrushBase{

    private Mesh _mesh;
    [SerializeField] private Mesh _srcMesh;
    [SerializeField] private Vector3[] _vertices;
    [SerializeField] private List<Vector3> _positions;
    [SerializeField] private Vector3 _pos;
    [SerializeField] private float _debugLength = 1f;
    [SerializeField] private float _width = 1f;
    private Vector3[] _tangents;    
    private Vector3[] _normals;
    private Vector3[] _binormals;
    [SerializeField] private Camera _camera;
    private MousePosition _mouse;
    //private MouseUtil _mouse;

    private const int W = 150;//80;
    private const int H = 10;//20;
    private ProjObj _projObj;
    private bool _isHide = false;
    private bool _isInit = false;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Slider _sliderDist;
    [SerializeField] private Slider _sliderWidth;


    public void Init(){

        if(_isInit) return;
        _isInit=true;


        _mesh = MeshUtil.CloneMesh(_srcMesh,"test");

        //_width = _width*(0.1f + 0.9f*Random.value);

        var bounds = new Bounds();
        bounds.size = new Vector3(1000f,1000f,1000f);
        _mesh.bounds = bounds;
        //_mesh = MeshUtil.GetNewMesh(_mesh,MeshTopology.Lines);

        //GetComponent<MeshFilter>().mesh = _mesh;
        GetComponent<MeshFilter>().sharedMesh = _mesh;

        _renderer.enabled = false;


        _mouse = new MousePosition();
        _mouse.Init(W+5);
        //_mouse = new MouseUtil(0.1f + 0.1f * Random.value, 0.8f + 0.1f *Random.value, 3, null);

        _vertices   = _mesh.vertices;
        _binormals  = new Vector3[ _mesh.normals.Length ];

        for(int i=0;i<_vertices.Length;i++){

            int idxX = i % (W+1);
            int idxY = Mathf.FloorToInt((float)i/(float)(W+1));

            float rx = (float)idxX/(float)W;
            float ry = (float)idxY/(float)H;

            _vertices[i] = new Vector3(
                1f * Mathf.Cos( -rx * Mathf.PI * 2f + Mathf.PI*0.5f ),
                1f * Mathf.Sin( -rx * Mathf.PI * 2f + Mathf.PI*0.5f ),
                ry
            );
            
        }
        _mesh.vertices = _vertices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();

        _normals    = new Vector3[W+1];
        _tangents   = new Vector3[W+1];
        _binormals = new Vector3[W+1];
        //_tangents   = new Vector3[_mesh.tangents.Length]; 
        //var tangents = _mesh.tangents;

        Debug.Log("-------");
        Debug.Log(_vertices.Length);
        Debug.Log( (W+1) * (H+1) );

        //_projObj = GetComponent<ProjObj>();

        /* 
        for(int i=0;i<_normals.Length;i++){
            //_normals[i] = 
            var t = tangents[i];
            _tangents[i] = new Vector3(
                t.x,t.y,t.z
            );
            var tangent = tangents[i];
            var normal = _normals[i];

            Debug.Log(i + " _ " + _normals.Length);
            _binormals[i] = Vector3.Cross(tangent, normal);
        }*/

    }

    void OnGUI(){
		GUI.Label (new Rect (50, 50, 100, 100), _sliderDist.value+""); //テキスト表示
	}


    public void Hide(){
        _isHide=true;
    }


    //計算する
    void Update(){

        //if( _isFinish ) return;
        if( _isHide ) return;

        //http://karaagedigital.hatenablog.jp/entry/2016/09/22/201900

        
        _mouse.Upadate(_camera, _sliderDist.value);
        _positions = _mouse._positions;
        

        if(_mouse.isFinish) Hide();

        if(_positions.Count==0){
            
            //_renderer.enabled = false;
            return;

        }


        if(_mouse.updating){
            _renderer.enabled=true;
        }else{
            //_renderer.enabled=false;
        }

        for(int i=0;i<W+1;i++){
            
            var v = _positions[i+1] - _positions[i];
            _tangents[i] = v;
            _tangents[i].Normalize();

        }

        _vertices   = _mesh.vertices;
        int idx = 0;
        for(int i=0;i<W+1;i++){

            //tangent（接線）を取得
            var tangent = _tangents[i];
            var normal = new Vector3();
            var binormal = new Vector3();

            if( i == 0 ){

                //tangentを使って、ノーマルを計算準備
                var tx = Mathf.Abs(tangent.x);
                var ty = Mathf.Abs(tangent.y);
                var tz = Mathf.Abs(tangent.z);

                //ノーマルを計算準備
                var min = float.MaxValue;
                if (tx < min) {
                    min = tx;
                    normal.Set(1, 0, 0);
                }
                if (ty < min) {
                    min = ty;
                    normal.Set(0, 1, 0);
                }
                if (tz < min) {
                    normal.Set(0, 0, 1);
                }            

                //
                var vec = Vector3.Cross(tangent, normal).normalized;
                
                //最終的なノーマル計算
                normal = Vector3.Cross(tangent, vec);
                _normals[i] = normal;
                //バイノーマルは、接戦と法線のクロス
                _binormals[i] = Vector3.Cross(tangent, normal);
                
            }else{

                _normals[ i ] = _normals[ i - 1 ];
                _binormals[ i ] = _binormals[ i - 1 ];
                var vec = Vector3.Cross( _tangents[ i - 1 ], _tangents[ i ] );
                float epsilon = 0.0001f;
                if ( vec.magnitude > epsilon ) {
                    vec.Normalize();
                    float dot = Vector3.Dot(_tangents[i-1],_tangents[i]);
                    if(dot<-1)dot=-1;
                    if(dot>1)dot=1;
                    float theta = Mathf.Acos(dot);
                    // anselm normals[ i ].applyMatrix4( makeRotationAxis( vec, theta ) );
                   _normals[ i ] = makeRotationAxis(vec,theta).MultiplyVector ( _normals[i] );
                    
                }
                _binormals[i] = Vector3.Cross (_tangents[i],_normals[i]);
		
            }

            normal = _normals[i];
            binormal = _binormals[i];

            normal.Normalize();
            binormal.Normalize();


            var basePos = _positions[i];

            for(int j=0; j<H+1; j++){

                float rad = 1f * (float)j / (float)H * (Mathf.PI * 2f);
                float cos = Mathf.Cos( -rad + Mathf.PI * 0.5f );
                float sin = Mathf.Sin( -rad + Mathf.PI * 0.5f ); 

                //normal と binormal 方向に。
                float ww = i==0 ? 0 : _width;

                Vector3 v = ww * (
                    cos * normal + sin * binormal
                ).normalized;

                _vertices[idx] = basePos + v;
                idx++;
                //var v = (cos * N + sin * B).normalized;
                //p + radius * v

            }

            //debug用
            Debug.DrawLine (basePos, basePos + 0.1f * normal, Color.red);
            Debug.DrawLine (basePos, basePos + 0.1f * tangent, Color.green);
            Debug.DrawLine (basePos, basePos + 0.1f * binormal, Color.blue);

        }

        _mesh.vertices = _vertices;
        _mesh.RecalculateNormals();
        _mesh.RecalculateTangents();


        /* 
        //奥行き
            for(int i=0;i<_vertices.Length;i++){

                int idxX = i % 21;
                int idxY = Mathf.FloorToInt((float)i/21f);


                //copy previus
                _normals[i] = _normals[i - 1];
                _binormals[i] = _binormals[i - 1];

                //var rot = _mouse.rotations[i]

                //_mouse.positions[ i ];
                var axis = Vector3.Cross(tangents[i - 1], tangents[i]);
                
                //tangentを計算して、、ノーマルを計算する

            }
        */

    }

	Matrix4x4 makeRotationAxis(Vector3 axis, float angle) { // XXX ANSELM TODO - implication of NEW? also row/order?
	
		Matrix4x4 mat = new Matrix4x4();
	
		float c = Mathf.Cos( angle );
		float s = Mathf.Sin( angle );
		float t = 1 - c;
		float x = axis.x, y = axis.y, z = axis.z;
		float tx = t * x, ty = t * y;

		mat.SetRow (0,new Vector3(tx * x + c    , tx * y - s * z, tx * z + s * y ));
		mat.SetRow (1,new Vector3(tx * y + s * z, ty * y + c,     ty * z - s * x ));
		mat.SetRow (2,new Vector3(tx * z - s * y, ty * z + s * x, t * z * z + c  ));
		mat.SetRow (3,new Vector4(0,0,0,1));

		return mat;		
	}    


}