Shader "Camp Cult/Vert/DiffuseVertGlitch" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Shape("Shape",Vector) = (1,0,0,0)
	}
	SubShader {
		Blend SrcAlpha OneMinusSrcAlpha
		Tags { "Queue"="Transparent+2000" "RenderType"="Transparent+2000" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard alpha fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _Shape;
		
		void vert (inout appdata_full v) {
			float2 u = float2(abs(atan2(v.vertex.x,v.vertex.z)/3.14),abs(fmod(abs(v.vertex.y)+1.,2.0)-1.0));
			u.x *=2.-1.;
			u.x = abs(u.x); 
			float f = tex2Dlod  (_MainTex, float4(u,1.,1.)).r;
	        v.vertex.xy += sin(v.normal*_Shape.x+_Shape.y)*_Shape.z*f;
	        v.vertex.yz += cos(v.normal*_Shape.x+_Shape.y)*_Shape.z*f;
	    }

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = _Color.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
