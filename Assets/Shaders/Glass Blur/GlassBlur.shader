Shader "Unlit/GlassBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
        _Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            float _BlurSize;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 blurOffset = _BlurSize / _ScreenParams.xy;

                fixed4 color = tex2D(_MainTex, uv) * 0.2;
                color += tex2D(_MainTex, uv + blurOffset) * 0.2;
                color += tex2D(_MainTex, uv - blurOffset) * 0.2;
                color += tex2D(_MainTex, uv + float2(blurOffset.x, -blurOffset.y)) * 0.2;
                color += tex2D(_MainTex, uv + float2(-blurOffset.x, blurOffset.y)) * 0.2;

                color *= _Color;

                UNITY_APPLY_FOG(i.fogCoord, color);
                return color;
            }
            ENDCG
        }
    }
}