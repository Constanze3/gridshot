Shader "Custom/DepthTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorOne ("Color One", color) = (1, 1, 1, 1)
        _ColorTwo ("Color Two", color) = (1, 1, 1, 1)
        _Ramp ("Frensel Ramp", float) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
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
                float2 uv: TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenSpace : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _CameraDepthTexture;
            float4 _ColorOne, _ColorTwo;
            float _Ramp;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.x += frac(_Time.y);
                o.screenSpace = ComputeScreenPos(o.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 screenSpaceUV = i.screenSpace.xy / i.screenSpace.w;

                float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenSpaceUV));
                float3 mixedColor = lerp(_ColorOne, _ColorTwo, depth);
                float frensel = pow(1 - dot(i.viewDir, i.normal), _Ramp);
                float3 finalColor = lerp(mixedColor, col, frensel);
                return fixed4(finalColor, 1);
            }

            ENDCG
        }
    }
}