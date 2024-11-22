Shader "Universal Render Pipeline/LowPolyUnlitShader" {
    Properties {
        _MainTex ("Color Scheme", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }
        LOD 200

        Pass {
            Name "UnlitPass"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Uniforms
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _Color;

            struct Attributes {
                float4 positionOS : POSITION;  // Object space position
                float2 uv : TEXCOORD0;        // Texture coordinates
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION; // Homogeneous clip space position
                float2 uv : TEXCOORD0;           // Texture coordinates
            };

            Varyings vert(Attributes v) {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS); // Transform to clip space
                o.uv = v.uv; // Pass UV coordinates
                return o;
            }

            half4 frag(Varyings i) : SV_Target {
                // Sample the texture
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // Multiply by the tint color
                return texColor * _Color;
            }
            ENDHLSL
        }
    }
    FallBack "Unlit/Texture"
}
