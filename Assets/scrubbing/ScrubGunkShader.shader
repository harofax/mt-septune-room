Shader "gunk/ScrubGunkShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Coordinate ("Coordinate", Vector) = (0,0,0,0)
        _Color("Draw Color", Color) = (1,0,0,0)
        _PolishColor("Polish Color", Color) = (0,0,1,0)
        _Size("Size", Range(1, 500)) = 1
        _Strength("Strength", Range(0,1)) = 1
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                //float4 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Coordinate, _Color, _PolishColor;
            half _Size, _Strength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                float draw = pow(saturate(1 - distance(i.uv, _Coordinate.xy)), 4000 / _Size);
                float4 drawcol = _Color * (draw * _Strength*0.7) +
                    _PolishColor * (draw * _Strength * 0.19);
                
                return saturate(col + drawcol);
            }
            
            ENDCG
        }
    }
}
