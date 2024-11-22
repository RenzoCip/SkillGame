Shader "Universal Render Pipeline/LowPolyPBRShader" {
    Properties {
        _MainTex ("Color Scheme", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }
        LOD 200

        Pass {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPosition : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _Color;
            float _Glossiness;
            float _Metallic;

            Varyings vert(Attributes v) {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                o.worldPosition = TransformObjectToWorld(v.positionOS);
                return o;
            }

            half4 frag(Varyings i) : SV_Target {
                // Sample the texture
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // Combine the texture color with the tint color
                float4 finalColor = texColor * _Color;

                // Return the color with metallic and smoothness applied
                SurfaceData surf;
                InitializeStandardLitSurfaceData(surf);
                surf.albedo = finalColor.rgb;
                surf.metallic = _Metallic;
                surf.smoothness = _Glossiness;

                float3 viewDirWS = normalize(GetWorldSpaceViewDir(i.worldPosition));
                return UniversalFragmentBlinnPhong(surf, viewDirWS);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
