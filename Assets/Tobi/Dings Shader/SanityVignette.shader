Shader "Custom/SanityVignette"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        
        [Header(Vignette)]
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.5
        _VignetteRadius ("Vignette Radius", Range(0, 1)) = 0.5
        _VignetteSoftness ("Vignette Softness", Range(0.01, 1)) = 0.3
        
        [Header(Rays)]
        _RayCount ("Ray Count", Range(8, 128)) = 48
        _RayIntensity ("Ray Intensity", Range(0, 1)) = 0.5
        _RayLength ("Ray Length", Range(0, 0.5)) = 0.2
        _RaySharpness ("Ray Sharpness", Range(1, 20)) = 8
        _RayWidth ("Ray Width", Range(0.1, 2)) = 1
        
        [Header(Animation)]
        _PulseSpeed ("Pulse Speed", Range(0, 5)) = 1
        _PulseAmount ("Pulse Amount", Range(0, 0.5)) = 0.15
        _WaveSpeed ("Wave Speed", Range(0, 10)) = 3
        _WaveFrequency ("Wave Frequency", Range(1, 20)) = 5
        _WaveAmplitude ("Wave Amplitude (Distortion)", Range(0, 0.3)) = 0.08
        
        [Header(Per Ray Variation)]
        _RayVariation ("Ray Size Variation", Range(0, 1)) = 0.3
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipeline"
        }
        
        LOD 100
        ZWrite Off
        Cull Off
        ZTest Always
        
        Pass
        {
            Name "SanityVignettePass"
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            float _VignetteIntensity;
            float _VignetteRadius;
            float _VignetteSoftness;
            
            float _RayCount;
            float _RayIntensity;
            float _RayLength;
            float _RaySharpness;
            float _RayWidth;
            
            float _PulseSpeed;
            float _PulseAmount;
            float _WaveSpeed;
            float _WaveFrequency;
            float _WaveAmplitude;
            
            float _RayVariation;
            
            float Hash(float n)
            {
                return frac(sin(n) * 43758.5453);
            }
            
            float GetRays(float2 uv, float time)
            {
                float2 centeredUV = uv - 0.5;
                float angle = atan2(centeredUV.y, centeredUV.x);
                float dist = length(centeredUV);
                
                float normalizedAngle = (angle + PI) / (2.0 * PI);
                
                float rayAngle = normalizedAngle * _RayCount;
                float rayIndex = floor(rayAngle);
                float rayFrac = frac(rayAngle);
                
                float rayHash = Hash(rayIndex);
                float rayPhaseOffset = rayHash * 6.28318;
                float raySizeVariation = 1.0 + (rayHash - 0.5) * _RayVariation;
                
                float rayShape = pow(abs(sin(rayFrac * PI * _RayWidth)), _RaySharpness);
                
                float pulse = sin(time * _PulseSpeed + rayPhaseOffset) * 0.5 + 0.5;
                float pulseOffset = pulse * _PulseAmount;
                
                float wave1 = sin(dist * _WaveFrequency * 10.0 - time * _WaveSpeed + rayPhaseOffset);
                float wave2 = sin(dist * _WaveFrequency * 6.0 + time * _WaveSpeed * 0.7 + rayPhaseOffset * 2.0) * 0.5;
                float wave3 = sin(rayFrac * PI * 4.0 + time * _WaveSpeed * 1.3) * 0.3;
                float combinedWave = (wave1 + wave2 + wave3) / 1.8;
                float distortion = combinedWave * _WaveAmplitude * (dist * 3.0 + 0.5);
                
                rayShape *= 1.0 + distortion;
                
                float rayStart = 1.0 - _VignetteRadius - _RayLength * raySizeVariation - pulseOffset;
                float rayEnd = 1.0 - _VignetteRadius + 0.05;
                float rayMask = smoothstep(rayStart, rayEnd, dist * 2.0);
                
                float rays = rayShape * rayMask * _RayIntensity;
                
                return rays;
            }
            
            float GetVignette(float2 uv)
            {
                float2 centeredUV = uv - 0.5;
                float dist = length(centeredUV) * 2.0;
                
                float vignette = smoothstep(_VignetteRadius, _VignetteRadius + _VignetteSoftness, dist);
                return vignette * _VignetteIntensity;
            }
            
            float4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.texcoord;
                float time = _Time.y;
                
                float4 sceneColor = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);
                
                float vignette = GetVignette(uv);
                float rays = GetRays(uv, time);
                
                float darkness = saturate(vignette + rays);
                
                float3 finalColor = sceneColor.rgb * (1.0 - darkness);
                
                return float4(finalColor, sceneColor.a);
            }
            
            ENDHLSL
        }
    }
}
