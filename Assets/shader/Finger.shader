Shader "Unlit/Finger"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _angle ("angle", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _angle;


            v2f vert (appdata v)
            {
                v2f o;
                float _x = 0;
                float _y= 0;
                float _z= 0;
                float rad = radians(_angle);
                float c = cos(rad);
                float s = sin(rad);

                float4x4 translationM = {
                    1, 0, 0, _x,
                    0, 1, 0, _y,
                    0, 0, 1, _z,
                    0, 0, 0, 1
                };


                float4x4 rotZM = {
                    c, -s, 0, 0,
                    s, c, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };

                



                v.vertex = mul(rotZM, v.vertex);  
                v.vertex = mul(translationM, v.vertex);             
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
