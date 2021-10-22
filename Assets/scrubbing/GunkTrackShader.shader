Shader "gunk/GunkTrackShader"
{
    Properties
    {
        _Color ("Gunk Color", Color) = (1,1,1,1)
        _Splat ("Splat Map", 2D) = "white" {}
        _BaseTex ("Base (RGB)", 2D) = "white" {}
        _GunkTex ("Gunk (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Standard

        #pragma target 4.6
        

        struct appdata {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
        };

        sampler2D _Splat;
        sampler2D _BaseTex, _GunkTex;

        fixed4 _Color;

        struct Input
        {
            float2 uv_Splat;
            float2 uv_BaseTex;
            float2 uv_GunkTex;
        };

        half _Glossiness;

        UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float3 splat = tex2D(_Splat, IN.uv_Splat);
            
            float4 c = tex2D(_BaseTex, IN.uv_BaseTex) * splat.r +
                tex2D(_GunkTex, IN.uv_GunkTex)* tex2D(_BaseTex, IN.uv_BaseTex) *
                    (1 - splat.r) * _Color * unity_ColorSpaceDouble;

            o.Albedo = c.rgb;
            o.Smoothness = saturate(splat.b + _Glossiness);
            o.Alpha = c.a;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
