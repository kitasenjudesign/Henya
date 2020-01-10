Shader "Custom/CubemapTest2"
{
    Properties {
        // キューブマップテクスチャのプロパティ
        _Cube ("Cube", CUBE) = "" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex: POSITION;
                half3 normal: NORMAL;
                half2 uv: TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                float3 pos2 : TEXCOORD1;
                half3 normal : TEXCOORD2;
            };

            // UNITY_SAMPLE_TEXCUBEで使用する変数を定義する
              UNITY_DECLARE_TEXCUBE(_Cube);

            v2f vert (appdata v)
            {
                //v.vertex.y += 0.02 * sin(_Time.y);

                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.pos2 = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;

            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);
                half3 viewDir = normalize(_WorldSpaceCameraPos - i.pos2);
                // 視点からのベクトルと法線から反射方向のベクトルを計算する
                half3 reflDir = reflect(-viewDir, i.normal);

                // キューブマップと反射方向のベクトルから反射先の色を取得する
                half4 refColor = UNITY_SAMPLE_TEXCUBE(_Cube, reflDir);

                return half4(refColor.rgb, 1);
            }
            ENDCG
        }
    }
}