Shader "SM/BM_Ocean" {
	Properties {
		_Color ("Color", Vector) = (0.5,0.5,0.5,1)
		_SpecColor ("SPColor", Vector) = (0.5,0.5,0.5,1)
		_Texture1 ("Tex1", 2D) = "white" {}
		_BumpMap1 ("Normalmap1", 2D) = "bump" {}
		_Texture2 ("Tex2", 2D) = "white" {}
		_BumpMap2 ("Normalmap2", 2D) = "bump" {}
		_MainTexSpeed ("_MainTexSpeed", Range(0, 1)) = 0
		_Bump1Speed ("_Bump1Speed", Range(0, 1)) = 0
		_Texture2Speed ("_Texture2Speed", Range(0, 1)) = 0
		_Bump2Speed ("_Bump2Speed", Range(0, 1)) = 0
		_NoizeMap ("NoizeMap", 2D) = "bump" {}
		_DistortionMap ("_DistortionMap", 2D) = "white" {}
		_DistortionSpeed ("_DistortionSpeed", Range(0, 1)) = 0
		_DistortionPower ("_DistortionPower", Range(0, 1)) = 0
		_Specular ("_Specular", Range(0, 1)) = 0
		_Gloss ("_Gloss", Range(0, 1)) = 0
		_Opacity ("_Opacity", Range(0, 1)) = 0
		_Reflection ("_Reflection", Vector) = (0,0,0,1)
		_ReflectPower ("_ReflectPower", Vector) = (0,0,0,1)
		_Pos ("_Pos", Vector) = (0,0,0,1)
		_Size ("_Size", Vector) = (0,0,0,1)
		_Angle ("_Angle", Vector) = (0,0,0,1)
		_CameraPosition ("_CameraPosition", Vector) = (0,0,0,1)
		_testparam1 ("_testparam1", Range(0, 1)) = 0
		_testparam2 ("_testparam2", Range(0, 1)) = 0
		_testparam3 ("_testparam3", Range(0, 1)) = 0
		_testparam4 ("_testparam4", Range(0, 1)) = 0
		_NoWaterTex ("NoWaterRenderTexture", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			float4 _Color;

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return _Color; // RGBA
			}

			ENDHLSL
		}
	}
	Fallback "Diffuse"
}