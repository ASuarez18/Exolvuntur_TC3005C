Shader "Unlit/PataDereMed"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //_MyColor ("Color", Color) = (1,1,1,1)
        //_MyVector ("The Vector", Vector) = (0,0,0,0)
        //_Myfloat ("The Float", Float) = 0


        _TypeTex ("TypeTex", 2D) = "white" {}
        _angle ("Angle", Float) = 0
        _angle2 ("Angle2", Float) = 0
        _posInit ("PosInit", Vector) = (0,0,0,0)
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
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _TypeTex;
            float _angle;
            float _angle2;
            float4 _MainTex_ST;
            Vector _posInit;

            v2f vert (appdata v)
            {
                v2f o;

                //if(v.vertex.x > 0) v.vertex.x *= 2;
                float rad = radians(_angle);
                float c = cos(rad);
                float s = sin(rad);
                float rad2 = radians(_angle2);
                float c2 = cos(rad2);
                float s2 = sin(rad2);
                float4x4 rotZM = {
                    c, -s, 0, 0,
                    s, c, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };

                float4x4 rotYM = {
                    c2, 0, s2, 0,
                    0, 1, 0, 0,
                    -s2, 0, c2, 0,
                    0, 0, 0, 1
                };

                float4x4 traM= {
                    1, 0, 0, 0.5,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };

                float4x4 traMInit= {
                    1, 0, 0, _posInit.x,
                    0, 1, 0, _posInit.y,
                    0, 0, 1, _posInit.z,
                    0, 0, 0, 1
                };

                float4x4 scaM = {
                    1, 0, 0, 0,
                    0, 0.5, 0, 0,
                    0, 0, 0.5, 0,
                    0, 0, 0, 1
                };

                float4x4 mat = mul(rotYM, traMInit);
                mat = mul(mat, traM);
                mat = mul(mat, rotZM);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float4 location = {o.uv[0], o.uv[1], 0, 0};
                float4 type = tex2Dlod(_TypeTex, location);

                if (type.x == 1 && type.y == 0 && type.z == 0)
                {
                    
                    mat = mul(mat, scaM);
                }
                else if (type.x == 0 && type.y == 1 && type.z == 0)
                {
                    mat = mul(mat, traM);
                    mat = mul(mat, rotZM);
                    mat = mul(mat, traM);
                    mat = mul(mat, scaM);
                }
                else if (type.x == 0 && type.y == 0 && type.z == 1)
                {
                    mat = mul(mat, traM);
                    mat = mul(mat, rotZM);
                    mat = mul(mat, traM);
                    mat = mul(mat, traM);
                    mat = mul(mat, rotZM);
                    mat = mul(mat, traM);
                    mat = mul(mat, scaM);
                }

                 
                v.vertex = mul(mat, v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                //col = float4(0, 0, 1, 1);
                //get the matrix 
                //float4 vpos = mul(UNITY_MATRIX_MV, mul(unity_ObjectToWorld, i.worldPos));
                //float4 vpos = mul(unity_ObjectToWorld, float4(i.worldPos,1));

                //if(vpos.x > 0) col = float4(1, 0, 0, 1);
                //if(i.uv.x > 0.5) col = float4(1, 0, 0, 1);
                //fixed4 col = tex2D(_TypeTex, i.uv);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
