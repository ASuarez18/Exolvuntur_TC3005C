Shader "Unlit/RayCasting"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //Obtenemos la ubicacion de cada uno de los vertices del objeto
            struct appdata
            {
                float4 vertex : POSITION;
            };

            //Obtenemos la posicion en pantalla de cada uno de los vertices del objeto
            struct v2f
            {
                float4 pos : SV_POSITION;
                float viewDepth : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                //Creamos una estructura de vertice to fragment y guardamos la pocision local al global
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                //Guardamos la profundidad del vertice
                o.viewDepth = -UnityObjectToViewPos(v.vertex).z;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Devolvemos la profundidad del vertice
                //Esta profundidad es la que usaremos para raytracing
                return i.viewDepth;
            }
            ENDCG
        }
    }
}
