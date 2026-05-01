Shader "Custom/PhaseWall"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _alpha("alpha",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"
            
            #define TAU 6.28318530718

            struct MeshData
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

            float _alpha;

            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //fixed4 texCol = tex2D(_MainTex, i.uv);

                float3 wallCol = float3(0.8,0,0.8);
                
                float2 centerUvs = i.uv * 2 - 1;
                float radialDist = length(centerUvs);
                
                float t = cos((radialDist - _Time.y * 0.1f) * TAU * 5) * 0.5f + 1;
                t *= 1 - radialDist;
       
                
                return float4(wallCol.xyz * t,_alpha);
            }
            ENDCG
        }
    }
}
