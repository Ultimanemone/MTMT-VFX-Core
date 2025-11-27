Shader "SM/SM_Alpha_Smoke" {
    Properties
    {
        _Color1 ("Smoke", Color) = (1,0.5,0.2,1)
        _Color2 ("Smoke Secondary", Color) = (1,0.5,0.2,1)
        _SubColor1 ("Glow", Color) = (1,0.7,0.2,1)
        _SubColor2 ("Glow Initial", Color) = (1,0.7,0.2,1)

        _MainTex ("Main Texture (sheet)", 2D) = "white" {}
        _AlphaTex ("Alpha / Noise", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}

        _AlphaStrength ("Alpha Strength", Range(0,2)) = 1.0
        _NormalStrength ("Normal Strength", Range(0,2)) = 1.0
        _InvFade ("Soft Particles Factor (1 = off)", Range(0.01,3)) = 1
        _HighlightLife ("Hightlight Life", Range(0,1)) = 0.3

        _TilesX ("Tiles X (fallback)", int) = 1
        _TilesY ("Tiles Y (fallback)", int) = 1
        _Emission ("Emission Strength", Range(0,5)) = 0.6
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            sampler2D _NormalMap;
            sampler2D _CameraDepthTexture;

            float4 _MainTex_ST;
            float4 _AlphaTex_ST;
            float4 _NormalMap_ST;

            float4 _Color1;
            float4 _Color2;
            float4 _SubColor1;
            float4 _SubColor2;
            float _AlphaStrength;
            float _NormalStrength;
            float _InvFade;
            int _TilesX;
            int _TilesY;
            float _Emission;
            float _HighlightLife;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;    // xy = uv, z = AgePercent (common)
                float4 uv2 : TEXCOORD1;   // possible: frameIndex (x), tilesX (z), tilesY (w)
                float4 uv3 : TEXCOORD2;   // fallback if uv2 not filled by PS
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uvMain : TEXCOORD0;
                float2 uvAlpha : TEXCOORD1;
                float2 uvNormal : TEXCOORD2;
                float normalizedAge : TEXCOORD3;
                float4 particleColor : COLOR;
                float4 screenPos : TEXCOORD4;
                float3 viewDir : TEXCOORD5;
            };

            // Helper: read frame/tiles from vertex streams if present
            static inline void GetSheetInfo(appdata v, out float frameIndex, out float tilesX, out float tilesY)
            {
                // 1) Try uv2.x / uv2.zw (common Unity encoding for particle sheet)
                frameIndex = v.uv2.x;
                tilesX = v.uv2.z;
                tilesY = v.uv2.w;

                // 2) fallback to uv3 if uv2 empty
                if (tilesX <= 0.0 || tilesY <= 0.0)
                {
                    // uv3 might contain frame / tiles when using different vertex streams
                    if (v.uv3.z > 0.0 && v.uv3.w > 0.0)
                    {
                        frameIndex = v.uv3.x;
                        tilesX = v.uv3.z;
                        tilesY = v.uv3.w;
                    }
                }

                // 3) final fallback to shader properties
                if (tilesX <= 0.0) tilesX = max(1.0, (float)_TilesX);
                if (tilesY <= 0.0) tilesY = max(1.0, (float)_TilesY);
                if (frameIndex < 0.0) frameIndex = 0.0;
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // screen pos for soft particles depth sampling
                o.screenPos = ComputeScreenPos(o.pos);

                // transform UVs
                float2 baseUV = v.uv.xy;
                o.uvMain = TRANSFORM_TEX(baseUV, _MainTex);
                o.uvAlpha = TRANSFORM_TEX(baseUV, _AlphaTex);
                o.uvNormal = TRANSFORM_TEX(baseUV, _NormalMap);

                // particle color multiplier (start color)
                o.particleColor = v.color;

                // normalized age usually in TEXCOORD0.z (AgePercent)
                o.normalizedAge = v.uv.z;

                // view dir for optional shading/distortion (camera space)
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);

                return o;
            }

            // Soft-particle helper: returns fade in [0..1] (1 = no fade)
            float SoftParticleFade(float4 screenPos)
            {
                if (_InvFade <= 0.0001) return 1.0;

                #if defined(UNITY_NEVER_DEFINED)
                return 1.0; // placeholder if depth sampling disabled
                #else
                // sample scene depth at particle pixel
                float sceneZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, screenPos);
                float particleZ = screenPos.z / screenPos.w;
                // linearize depths
                float linearScene = LinearEyeDepth(sceneZ);
                float linearPart = LinearEyeDepth(particleZ);
                float diff = linearScene - linearPart; // positive when scene closer than particle
                // convert using factor (bigger _InvFade => softer)
                float fade = saturate(diff * _InvFade);
                // when scene closer (diff < 0) we want 0 -> fully hidden; else 1
                // clamp to [0..1]
                return fade;
                #endif
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // --- Animation sheet UV remap ---
                float frameIndex, tilesX, tilesY;
                // We cannot call GetSheetInfo here (it's inline expecting appdata), replicate minimal logic:
                // Attempt to read frame/tiles from TEXCOORDs passed via particle system:
                // Unity commonly uses TEXCOORD1.x as frame, TEXCOORD1.z/w as tiles, but we passed those as uv2/uv3 earlier.
                // If none present, fall back to properties.
                // For simplicity here, assume single tile unless sheet info is placed in particle streams.
                frameIndex = 0.0; tilesX = _TilesX; tilesY = _TilesY;
                // If uv2 was used we would have remapped in vert; but to keep frag robust simply support single-tile if frameIndex == 0.

                float2 tileSize = float2(1.0 / tilesX, 1.0 / tilesY);
                float frameX = fmod(frameIndex, tilesX);
                float frameY = floor(frameIndex / tilesX);

                // remap UV to specific frame
                float2 uvMain_frame = i.uvMain * tileSize + float2(frameX * tileSize.x, (tilesY - 1.0 - frameY) * tileSize.y);
                float2 uvAlpha_frame = i.uvAlpha * tileSize + float2(frameX * tileSize.x, (tilesY - 1.0 - frameY) * tileSize.y);
                float2 uvNormal_frame = i.uvNormal * tileSize + float2(frameX * tileSize.x, (tilesY - 1.0 - frameY) * tileSize.y);

                // --- Sample textures ---
                fixed4 mainS = tex2D(_MainTex, uvMain_frame);
                float alphaNoise = tex2D(_AlphaTex, uvAlpha_frame).r;
                float3 normalTex = UnpackNormal(tex2D(_NormalMap, uvNormal_frame)).xyz;

                // --- Use main texture channels as data ---
                // Interpretation:
                // mainS.r -> density / base shape
                // mainS.g -> secondary detail / sub mix
                // mainS.b -> inner highlight / emission mask
                float secondary = mainS.r;
                float tetriary  = mainS.g;
                float primary    = mainS.b;

                // compute color from channels and tint/subcolor
                // // heavily influenced by density, with detail mixing to SubColor
                float3 baseColor = _Color1.rgb * secondary;
                

                // emission contribution from core
                baseColor += _SubColor1.rgb * mainS.g * saturate(1 - i.normalizedAge / _HighlightLife) * _Emission;
                baseColor += _SubColor2.rgb * mainS.b * saturate(1 - i.normalizedAge / _HighlightLife) * _Emission;
                
                // alpha: composed from texture alpha (if present), alpha noise, and density
                float baseAlpha = mainS.a; // interpret density as alpha base
                // if main texture contains alpha channel, mix it:
                baseAlpha *= mainS.a;
                // multiply by external alpha noise and strength
                float alpha = baseAlpha * alphaNoise * _AlphaStrength;

                // lifetime fade: as normalizedAge -> 1, fade out
                float lifeFade = 1.0 - saturate(i.normalizedAge);
                alpha *= lifeFade;

                fixed4 outCol;
                outCol.rgb = baseColor * i.particleColor.rgb; // multiply by particle start color
                outCol.a = saturate(alpha);

                // Premultiply alpha? we use standard alpha blend; leave as-is
                return outCol;
            }
            ENDCG
        }
    }

    FallBack "Transparent/Diffuse"
}