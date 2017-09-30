// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Camp Cult/Color/Vertical Gradient" {
	Properties{
		_Color1("Color 1", Color) = (1,.5,.5,1)
		_Color2("Color 2", Color) = (.5,1,1,1)
		_Shape("X-min Y-max",Vector) = (0,1,0,0)
	}
		Category{
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		uniform float4 _Color1;
	uniform float4 _Color2;
	uniform float4 _Shape;

	struct appdata {
		float4 vertex : POSITION;
	};
	struct v2f {
		float f : TEXCOORD;
		float4 pos : POSITION;
	};
	v2f vert(appdata v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.f = mul(unity_ObjectToWorld, v.vertex).y;
		o.f = (o.f-_Shape.x)/(_Shape.y-_Shape.x);
		return o;
	};

	float4 frag(v2f i) : COLOR{
		return lerp(_Color1,_Color2,i.f);
	};

	ENDCG
	}
	}
	}
		FallBack "Diffuse"
}
