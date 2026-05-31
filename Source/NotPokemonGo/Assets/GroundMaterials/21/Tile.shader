Shader "Custom/TileWithDistortionFull"
{
    Properties
    {
        // ======== Основные текстуры ========
        [Header(Main Maps)]
        _MainTex ("Albedo (RGB) Cutout (A)", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)

        [NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
        _BumpScale ("Normal Strength", Range(0, 2)) = 1.0
        [NoScaleOffset] _ParallaxMap ("Height Map (R)", 2D) = "black" {}
        _ParallaxScale ("Parallax Strength", Range(0, 0.1)) = 0.02

        // ======== Глянец и отражения ========
        [Header(Specular and Reflections)]
        _Glossiness ("Smoothness", Range(0, 1)) = 0.5
        _SpecularStrength ("Specular Strength", Range(0, 1)) = 0.5
        _ReflectionIntensity ("Reflection Intensity", Range(0, 1)) = 0.0

        // ======== Искажения ========
        [Header(Distortion)]
        // Сила смещения UV. Небольшие значения дают естественную неровность.
        _DistortionStrength ("Distortion Strength", Range(0, 0.1)) = 0.02
        _DistortionTiling ("Distortion Tiling", Vector) = (2, 2, 0, 0)
        _DistortionScale ("Distortion Scale", Range(1, 500)) = 10

        // ======== Цветовой шум ========
        [Header(Color Noise)]
        [Toggle(_COLORNOISE_ON)] _ColorNoiseOn ("Enable Color Noise", Float) = 0
        [NoScaleOffset] _NoiseMask ("Noise Mask (R)", 2D) = "white" {}
        _NoiseTiling ("Noise Tiling", Vector) = (1, 1, 0, 0)
        _NoiseScale ("Noise Scale", Range(1, 500)) = 50
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.15
        [Toggle] _MultiplyMode ("Multiply Mode", Float) = 1

        // ======== Альфа‑клиппинг ========
        [Header(Alpha Clipping)]
        [Toggle(_ALPHATEST_ON)] _AlphaTestOn ("Enable Alpha Cutoff", Float) = 0
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5

        // ======== Свой тайлинг для Normal Map ========
        [Header(Normal Map Tiling)]
        _BumpMap_ST ("Normal Map Tiling/Offset", Vector) = (1, 1, 0, 0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 300

        // ----------------------------------------------------------------
        // Проход ForwardBase (основной направленный свет + GI)
        // ----------------------------------------------------------------
        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            Cull Back
            ZWrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #pragma multi_compile_fwdbase
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma shader_feature _COLORNOISE_ON
            #pragma shader_feature _ALPHATEST_ON

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 lightmapUV : TEXCOORD1;
                float2 dynamicLightmapUV : TEXCOORD2;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float3 tangentViewDir : TEXCOORD1;
                float3 tangentLightDir : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;
                float2 lightmapUV : TEXCOORD4;
                float2 dynamicLightmapUV : TEXCOORD5;
                SHADOW_COORDS(6)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _BumpScale;
            sampler2D _ParallaxMap;
            float _ParallaxScale;

            sampler2D _NoiseMask;
            float4 _NoiseTiling;
            float _NoiseScale;
            float _NoiseIntensity;
            float _MultiplyMode;

            float _DistortionStrength;
            float4 _DistortionTiling;
            float _DistortionScale;

            float _Glossiness;
            float _SpecularStrength;
            float _ReflectionIntensity;
            float _Cutoff;

            fixed4 _Color;

            inline float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float smoothNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));

                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }

            v2f vert (appdata v)
            {
                v2f o;

                o.pos = UnityObjectToClipPos(v.vertex);

                float2 baseUV = TRANSFORM_TEX(v.uv, _MainTex);

                float2 distortUV = v.uv * _DistortionTiling.xy * _DistortionScale;
                float2 noiseVec = float2(
                    smoothNoise(distortUV),
                    smoothNoise(distortUV + float2(31.7, 17.3))
                );
                float2 offset = (noiseVec - 0.5) * 2.0 * _DistortionStrength;
                float2 distortedUV = baseUV + offset;

                o.uv = float4(baseUV, distortedUV);

                TANGENT_SPACE_ROTATION;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                float3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                o.tangentLightDir = mul(rotation, lightDir);

                float3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                o.tangentViewDir = mul(rotation, viewDir);

                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                #ifdef LIGHTMAP_ON
                o.lightmapUV = v.lightmapUV * unity_LightmapST.xy + unity_LightmapST.zw;
                #else
                o.lightmapUV = 0;
                #endif

                #ifdef DYNAMICLIGHTMAP_ON
                o.dynamicLightmapUV = v.dynamicLightmapUV * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #else
                o.dynamicLightmapUV = 0;
                #endif

                TRANSFER_SHADOW(o);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 baseUV = i.uv.xy;
                float2 distortedUV = i.uv.zw;

                float3 tangentViewDir = normalize(i.tangentViewDir);
                #if !defined(UNITY_NO_PARALLAX)
                float height = tex2D(_ParallaxMap, distortedUV).r;
                float2 parallaxOffset = (height * _ParallaxScale - _ParallaxScale * 0.5) * tangentViewDir.xy / (tangentViewDir.z + 0.5);
                distortedUV += parallaxOffset;
                #endif

                fixed4 texColor = tex2D(_MainTex, distortedUV);
                fixed4 albedo = texColor * _Color;

                #if _ALPHATEST_ON
                clip(albedo.a - _Cutoff);
                #endif

                float2 bumpUV = baseUV * _BumpMap_ST.xy + _BumpMap_ST.zw;
                bumpUV += (distortedUV - baseUV);
                fixed3 tangentNormal = UnpackScaleNormal(tex2D(_BumpMap, bumpUV), _BumpScale);
                tangentNormal = normalize(tangentNormal);

                float3 lightDir = normalize(i.tangentLightDir);
                float3 viewDir = tangentViewDir;
                float3 halfDir = normalize(lightDir + viewDir);

                float NdotL = max(0.0, dot(tangentNormal, lightDir));
                float NdotH = max(0.0, dot(tangentNormal, halfDir));

                fixed shadowAtten = SHADOW_ATTENUATION(i);
                fixed3 lightColor = _LightColor0.rgb * NdotL * shadowAtten;

                float smoothness = _Glossiness * 0.98 + 0.02;
                float specPower = exp2(smoothness * 10.0 + 1.0);
                float spec = pow(NdotH, specPower) * _SpecularStrength;

                #ifdef LIGHTMAP_ON
                    #ifdef DYNAMICLIGHTMAP_ON
                    fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapUV);
                    fixed4 bakedColorTexDynamic = UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, i.dynamicLightmapUV);
                    fixed3 bakedGI = DecodeLightmap(bakedColorTex) + DecodeLightmap(bakedColorTexDynamic);
                    #else
                    fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapUV);
                    fixed3 bakedGI = DecodeLightmap(bakedColorTex);
                    #endif
                #else
                    fixed3 bakedGI = ShadeSH9(float4(i.worldNormal, 1.0));
                #endif

                fixed3 ambient = bakedGI * albedo.rgb;
                fixed3 specularGI = bakedGI * spec * _SpecularStrength;

                #if !defined(UNITY_NO_REFLECTION)
                float3 worldRefl = reflect(-viewDir, tangentNormal);
                float roughness = 1.0 - smoothness;
                float mipLevel = roughness * 6.0;
                half4 reflectionColor = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, worldRefl, mipLevel);
                half3 reflection = DecodeHDR(reflectionColor, unity_SpecCube0_HDR).rgb;
                fixed3 specular = lightColor * spec + specularGI + reflection * _ReflectionIntensity;
                #else
                fixed3 specular = lightColor * spec + specularGI;
                #endif

                fixed3 finalRGB = albedo.rgb * (ambient + lightColor * NdotL * shadowAtten) + specular;

                #if _COLORNOISE_ON
                float2 noiseUV = baseUV * _NoiseTiling.xy * _NoiseScale;
                float noiseVal = (smoothNoise(noiseUV) - 0.5) * 2.0 * _NoiseIntensity;
                float mask = tex2D(_NoiseMask, baseUV).r;
                noiseVal *= mask;
                if (_MultiplyMode > 0.5)
                    finalRGB *= (1.0 + noiseVal);
                else
                    finalRGB += noiseVal;
                #endif

                return fixed4(saturate(finalRGB), albedo.a);
            }
            ENDCG
        }

        // ----------------------------------------------------------------
        // Проход ForwardAdd (дополнительные источники света)
        // ----------------------------------------------------------------
        Pass
        {
            Tags { "LightMode"="ForwardAdd" }
            Blend One One
            ZWrite Off
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #pragma multi_compile_fwdadd_fullshadows
            #pragma shader_feature _ALPHATEST_ON

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                float3 tangentViewDir : TEXCOORD1;
                float3 tangentLightDir : TEXCOORD2;
                float3 worldPos : TEXCOORD3;  // для аттенюации
                SHADOW_COORDS(4)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _BumpScale;
            sampler2D _ParallaxMap;
            float _ParallaxScale;

            float _DistortionStrength;
            float4 _DistortionTiling;
            float _DistortionScale;
            float _Glossiness;
            float _SpecularStrength;
            float _Cutoff;
            fixed4 _Color;

            inline float hash(float2 p) { return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }
            float smoothNoise(float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float2 baseUV = TRANSFORM_TEX(v.uv, _MainTex);
                float2 distortUV = v.uv * _DistortionTiling.xy * _DistortionScale;
                float2 noiseVec = float2(
                    smoothNoise(distortUV),
                    smoothNoise(distortUV + float2(31.7, 17.3))
                );
                float2 offset = (noiseVec - 0.5) * 2.0 * _DistortionStrength;
                o.uv = float4(baseUV, baseUV + offset);

                TANGENT_SPACE_ROTATION;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldPos = worldPos;

                float3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
                o.tangentLightDir = mul(rotation, lightDir);
                float3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                o.tangentViewDir = mul(rotation, viewDir);

                TRANSFER_SHADOW(o);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 baseUV = i.uv.xy;
                float2 distortedUV = i.uv.zw;

                float3 tangentViewDir = normalize(i.tangentViewDir);

                // Параллакс
                float height = tex2D(_ParallaxMap, distortedUV).r;
                float2 parallaxOffset = (height * _ParallaxScale - _ParallaxScale * 0.5) * tangentViewDir.xy / (tangentViewDir.z + 0.5);
                distortedUV += parallaxOffset;

                fixed4 texColor = tex2D(_MainTex, distortedUV);
                fixed4 albedo = texColor * _Color;

                #if _ALPHATEST_ON
                clip(albedo.a - _Cutoff);
                #endif

                float2 bumpUV = baseUV * _BumpMap_ST.xy + _BumpMap_ST.zw;
                bumpUV += (distortedUV - baseUV);
                fixed3 tangentNormal = UnpackScaleNormal(tex2D(_BumpMap, bumpUV), _BumpScale);
                tangentNormal = normalize(tangentNormal);

                float3 lightDir = normalize(i.tangentLightDir);
                float3 viewDir = tangentViewDir;
                float3 halfDir = normalize(lightDir + viewDir);

                float NdotL = max(0.0, dot(tangentNormal, lightDir));
                float NdotH = max(0.0, dot(tangentNormal, halfDir));

                UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);

                float smoothness = _Glossiness * 0.98 + 0.02;
                float specPower = exp2(smoothness * 10.0 + 1.0);
                float spec = pow(NdotH, specPower) * _SpecularStrength;

                fixed3 lightColor = _LightColor0.rgb * NdotL * atten;
                fixed3 specular = lightColor * spec;

                fixed3 finalRGB = albedo.rgb * lightColor + specular;

                return fixed4(saturate(finalRGB), 1.0);
            }
            ENDCG
        }

        // ----------------------------------------------------------------
        // ShadowCaster
        // ----------------------------------------------------------------
        Pass
        {
            Tags { "LightMode"="ShadowCaster" }

            ZWrite On ZTest LEqual
            Cull Back

            CGPROGRAM
            #pragma vertex ShadowVert
            #pragma fragment ShadowFrag
            #pragma multi_compile_shadowcaster
            #pragma shader_feature _ALPHATEST_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;

            v2f ShadowVert(appdata v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 ShadowFrag(v2f i) : SV_Target
            {
                fixed4 albedo = tex2D(_MainTex, i.uv);
                #if _ALPHATEST_ON
                clip(albedo.a - _Cutoff);
                #endif
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }

        // ----------------------------------------------------------------
        // Meta Pass (запекание GI)
        // ----------------------------------------------------------------
        Pass
        {
            Tags { "LightMode"="Meta" }
            Cull Off

            CGPROGRAM
            #pragma vertex vert_meta
            #pragma fragment frag_meta
            #include "UnityCG.cginc"
            #include "UnityMetaPass.cginc"

            struct v2f_meta
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            v2f_meta vert_meta (appdata_full v)
            {
                v2f_meta o;
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag_meta (v2f_meta i) : SV_Target
            {
                fixed4 albedo = tex2D(_MainTex, i.uv) * _Color;
                UnityMetaInput meta;
                meta.Albedo = albedo.rgb;
                meta.Emission = half3(0,0,0);
                return UnityMetaFragment(meta);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}