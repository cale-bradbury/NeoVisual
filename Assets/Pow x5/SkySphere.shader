Shader "Camp Cult/Powx5/SkySphere" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Strength("Strength",float) = 0
		_Phase("Phase",float) = 0
		_Taps("Taps",float) = 3
		_Color1("Color1", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (0,1,1,1)
		_ColorData("Color Data", Vector) = (0,1,0,0)
	}
	//	Category{
		SubShader{
		Pass{
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		Tags{ "Queue" = "Transparent+1" "RenderType" = "Transparent+1" }


		Fog{ Mode off }

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#pragma target 3.0
#include "UnityCG.cginc"
#define TAU 6.283

	uniform sampler2D _MainTex;
	uniform float4 _MainTex_ST;
	uniform float _Strength;
	uniform float _Phase;
	uniform float _Taps;
	uniform float4 _ColorData;
	uniform float4 _Color1;
	uniform float4 _Color2;

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};
	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float4 world:TEXCOORD1;
		float taps:TEXCOORD2;
	};


	v2f vert(appdata v) {
		v2f o;
		o.world = mul(_Object2World, v.vertex);
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.taps = TAU / max(_Taps,.1);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}

	fixed4 frag(v2f j) : COLOR
	{

		float4 c = float(0.).xxxx;


		for (float i = 0.; i<TAU; i += 1.) {
			float4 f = tex2D(_MainTex, abs(fmod(j.uv + float2(cos(i + _Phase), sin(i + _Phase))*_Strength + 4., 2.) - 1.));

			c = max(c,f);
		}

		c = c*lerp(_Color1, _Color2, clamp((j.world.y - _ColorData.x) / (_ColorData.y - _ColorData.x),0.,1.));
		return c;

	}
		ENDCG

	}
	}
	//}

}