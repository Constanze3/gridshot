Shader "PostProcessing/BasicOutline"
{
    // An Issue
    // when the default output color is solid and there are multiple cameras, 
    // then only one camrea has the outline color
    //
    // not a problem regarding the purpose of this shader,
    // maybe it can be solved by some special buffer that contains multiple camera's depth textures taking min values

    // Water Depth
    // wanted to also have water level as a parameter and check the world position of the texel (somehow reconstructed from depth)
    // and check if the y coordinate is below a certain level then don't show the border
    // 
    // Even after hours and hours of trying I did not manage to do this
    // Will be using an opaque water plane unterwater instead ig
    // ):

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0, 0, 0, 1)
        _Thickness ("Thickness", float) = 1
        _Threshold ("Threshold", float) = 0.01
    }

    SubShader
    {
        Tags { 
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipeline"
        }
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            sampler2D _CameraDepthTexture;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv= TRANSFORM_TEX(v.uv, _MainTex);
                o.screenSpace = ComputeScreenPos(o.vertex);
                
                return o;
            }

            struct depthData
            {
                sampler2D tex;
                float2 uv;
                float offsetMultiplier;
            };

            float getDepth(depthData data, float2 offset) 
            {
                return Linear01Depth(SAMPLE_DEPTH_TEXTURE(data.tex, data.uv + data.offsetMultiplier * offset));
            }

            float _Thickness, _Bias, _Threshold;
            float4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 screenSpaceUV = i.screenSpace.xy / i.screenSpace.w;

                depthData dd;
                dd.tex = _CameraDepthTexture;
                dd.uv = screenSpaceUV;
                dd.offsetMultiplier = _MainTex_TexelSize * _Thickness;

                float2 sobel = float2(0, 0);

                sobel.x += getDepth(dd, float2(-1, 1));
                sobel.x += 2 * getDepth(dd, float2(-1, 0));
                sobel.x += getDepth(dd, float2(-1, -1));

                sobel.x -= getDepth(dd, float2(1, 1));
                sobel.x -= 2 * getDepth(dd, float2(1, 0));
                sobel.x -= getDepth(dd, float2(1, -1));

                sobel.y += getDepth(dd, float2(-1, 1));
                sobel.y += 2 * getDepth(dd, float2(0, 1));
                sobel.y += getDepth(dd, float2(1, 1));

                sobel.y -= getDepth(dd, float2(-1, -1));
                sobel.y -= 2 * getDepth(dd, float2(0, -1));
                sobel.y -= getDepth(dd, float2(1, -1));

                float sobelDepth = saturate(length(sobel));
                bool outline = sobelDepth >= _Threshold;

                if (outline) {
                    return lerp(col, _Color, _Color.a);
                }
                
                return col;
            }

            ENDCG
        }
    }
}