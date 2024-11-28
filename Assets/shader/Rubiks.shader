Shader "Unlit/Rubiks"
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
        _posC ("Centro1", Vector) = (0,0,0,0)
        _posC2 ("Centro2", Vector) = (0,0,0,0)
        _bandera ("Bandera", int) = 0 
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
            Vector _posC;
            Vector _posC2;

            int _bandera;

            

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

                //Matrices comparten la misma estructura
                float4x4 rotXM = {
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

                float4x4 traMPivote1 = {
                    1, 0, 0, _posC.x,
                    0, 1, 0, _posC.y,
                    0, 0, 1, _posC.z,
                    0, 0, 0, 1
                };
                
                float4x4 traMPivote1menos = {
                    1, 0, 0, -_posC.x,
                    0, 1, 0, -_posC.y,
                    0, 0, 1, -_posC.z,
                    0, 0, 0, 1
                };

                float4x4 traMPivote2 = {
                    1, 0, 0, _posC2.x,
                    0, 1, 0, _posC2.y,
                    0, 0, 1, _posC2.z,
                    0, 0, 0, 1
                };

                float4x4 traMPivote2menos = {
                    1, 0, 0, -_posC2.x,
                    0, 1, 0, -_posC2.y,
                    0, 0, 1, -_posC2.z,
                    0, 0, 0, 1
                };

                float4x4 scaM = {
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };

                //Matriz de traslacion del cubo 1 superior izquierda
                float4x4 traCSTI = {
                    1, 0, 0, 1,
                    0, 1, 0, 1,
                    0, 0, 1, 1,
                    0, 0, 0, 1
                };

                //Matriz de traslacion del cubo 2 superior derecha
                float4x4 traCSTD = {
                    1, 0, 0, 1,
                    0, 1, 0, 1,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };

                //Matriz de traslacion del cubo 3 superiro delantero izquierda
                float4x4 traCSDI = {
                    1, 0, 0, 0,
                    0, 1, 0, 1,
                    0, 0, 1, 1,
                    0, 0, 0, 1
                };

                //Matriz de traslacion del cubo 4 superior delantero derecha
                float4x4 traCSDD = {
                    1, 0, 0, 0,
                    0, 1, 0, 1,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };

                //Matriz de traslacion del cubo 5 inferior trasero izquierda
                float4x4 traCITI = {
                    1, 0, 0, 1,
                    0, 1, 0, 0,
                    0, 0, 1, 1,
                    0, 0, 0, 1
                };

                //Matriz de traslacion del cubo 6 inferior trasero derecha
                float4x4 traCITD = {
                    1, 0, 0, 1,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };

                //Matriz de traslacion del cubo 7 inferior delantero izquierda
                float4x4 traCIDI = {
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 1,
                    0, 0, 0, 1
                };

                //Matriz de traslacion del cubo 8 inferior delantero derecha
                float4x4 traCIDDD = {
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };




                //Centro del cubo



                float4x4 mat ;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float4 location = {o.uv[0], o.uv[1], 0, 0};
                float4 type = tex2Dlod(_TypeTex, location);

                //cubo superior trasero izquierda rojo
                //TransCenter * Rot * TransCentermenos * TransOrigen * Escala

                if(type.x == 1 && type.y == 0 && type.z == 0){
                    mat = mul(traMPivote1, rotYM);
                    mat = mul(mat,traMPivote1menos);
                    mat = mul(mat,traCSTI);
                    mat = mul(mat,scaM);
                }

                //cubo superior trasero derecha azul

                //Revisamois la bandera del cubo azul sobre el eje y
                if(type.x == 0 && type.y == 0 && type.z == 1 && _bandera == -1){
                    mat = mul(traMPivote1, rotYM);
                    mat = mul(mat,traMPivote1menos);
                    mat = mul(mat,traCSTD);
                    mat = mul(mat,scaM);
                }

                //Revisamos la bandera del cubo azul sobre el eje x
                if(type.x == 0 && type.y == 0 && type.z == 1 && _bandera == 1){
                    mat = mul(traMPivote2, rotXM);
                    mat = mul(mat,traMPivote2menos);
                    mat = mul(mat,traCSTD);
                    mat = mul(mat,scaM);
                }

                //cubo superior delantero izquierda verde

                if(type.x == 0 && type.y == 1 && type.z == 0){
                    mat = mul(traMPivote1, rotYM);
                    mat = mul(mat,traMPivote1menos);
                    mat = mul(mat,traCSDI);
                    mat = mul(mat,scaM);
                }

                //cubo superior delantero derecha amarillo

                if(type.x == 1 && type.y == 1 && type.z == 0 && _bandera == -1){
                    mat = mul(traMPivote1, rotYM);
                    mat = mul(mat,traMPivote1menos);
                    mat = mul(mat,traCSDD);
                    mat = mul(mat,scaM);
                }

                if(type.x == 1 && type.y == 1 && type.z == 0 && _bandera == 1){
                    mat = mul(traMPivote2, rotXM);
                    mat = mul(mat,traMPivote2menos);
                    mat = mul(mat,traCSDD);
                    mat = mul(mat,scaM);
                }

                //cubo inferior trasero izquierda blanco
                
                if(type.x == 1 && type.y == 1 && type.z == 1){
                    mat = mul(traCITI, scaM);
                }

                //cubo inferior trasero derecha negro

                if(type.x == 0 && type.y == 0 && type.z == 0){
                    mat = mul(traMPivote2, rotXM);
                    mat = mul(mat,traMPivote2menos);
                    mat =  mul (mat,traCITD);
                    mat = mul(mat,scaM);
                }

                //cubo inferiro delantero izquieda cian

                if(type.x == 0 && type.y == 1 && type.z == 1){
                    mat =  mul (traCIDI,scaM);
                }

                //cubo inferiro delantero derecha magenta
                if(type.x == 1 && type.y == 0 && type.z == 1){
                    mat = mul(traMPivote2, rotXM);
                    mat = mul(mat,traMPivote2menos);
                    mat =  mul (mat,traCIDDD);
                    mat = mul(mat,scaM);
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

                fixed4 col = tex2D(_TypeTex, i.uv);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}