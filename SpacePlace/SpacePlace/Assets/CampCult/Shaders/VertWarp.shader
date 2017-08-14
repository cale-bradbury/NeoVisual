Shader "Camp Cult/Vert/DiffuseVertWarp" {
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
		//cull off
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert

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
			float3 p = v.vertex.xyz;
			float a = atan2(p.z,p.y)+sin(_Shape.x+p.x*_Shape.y)*_Shape.z;
			float d = length(p.zy);
			v.vertex.yz = float2(cos(a),sin(a))*d;
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
