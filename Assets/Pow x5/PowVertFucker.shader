// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/PowVertFucker" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Vert("Vertex Displacement Tex", 2D) = "black"{}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_VertDisplacement("Vert Displacement Amnt", Float) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#define PI 3.14159

		sampler2D _MainTex;
		sampler2D _Vert;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _VertDisplacement;

		struct Input {
			float2 uv_MainTex;
		};


		void vert(inout appdata_full v) {
			float4 world = mul(unity_ObjectToWorld, v.vertex);
			float2 uv = float2(
				abs(fmod(atan2(world.x, world.z) / 3.1415 + 1., 2.) - 1.),
				0.// abs(fmod(abs(world.y*.5), 2.) - 1.)
			);
			float4 val = tex2Dlod(_Vert,  float4(uv.xy, 1., 1.));
			v.vertex.xyz += v.normal * val.r*_VertDisplacement;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
