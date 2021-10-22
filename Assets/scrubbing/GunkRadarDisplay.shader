Shader "gunk/GunkRadarDisplay"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                fixed2 cor;


                col.rgb = (1 - col.r);
                col.g = col.r;
                col.r = 0;

                cor.x = i.vertex.x / 5.0;
                cor.y = (i.vertex.y + 5.0 * 1.5 * fmod(floor(cor.x), 2.0)) / (5.0*3.0);

                fixed2 ico = floor(cor);
                fixed2 fco = frac(cor);

                fixed3 pix = step(1.5, fmod(fixed3(0.0, 1.0, 2.0) + ico.x, 5.0));

                fixed3 color = pix*dot(pix, col);

                color *= step(abs(fco.x-0.5), 0.4);
                color *= step(abs(fco.y-0.5), 0.4);

                color *= 1.2;
                
                
                
                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
}
