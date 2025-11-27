Shader "SM/SM_Add_UVDist" {
	Properties {
		_TintColor ("Tint Color", Vector) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_NoizeTex ("Noize Texture", 2D) = "white" {}
		_NoizeScroll_U ("Noize Scroll U", Float) = 0
		_NoizeScroll_V ("Noize Scroll V", Float) = 0
		_time ("time", Float) = 0
		_InvFade ("Soft Particles Factor", Range(0.01, 3)) = 1
		_LightBoostPow ("Light Boost Pow", Float) = 2
		_LightBoostScale ("Light Boost Scale", Float) = 1
	}

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv); // return texture exactly as-is
            }
            ENDCG
        }
    }
}