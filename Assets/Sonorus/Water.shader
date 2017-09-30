Shader "Unlit/Water"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_RippleTex("Ripple Texture", 2D) = "grey" {}
		_Difraction("Difraction", float) = 1
			_Color("Color", Color) = (1,1,1,1)
			_Color2("Color2", Color) = (1,1,1,1)
		_Height("Height", float) = 8
	}
		Category{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		SubShader
	{
		LOD 100

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float3 n:TEXCOORD1;
	};
	sampler2D _MainTex;
	sampler2D _RippleTex;
	float4 _RippleTex_TexelSize;
	float4 _RippleTex_ST;
	float _Difraction;
	float4 _Color;
	float4 _Color2;
	float _Height;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

		o.vertex.y -= (tex2Dlod(_RippleTex, float4(v.uv, 1., 1.)).r-.5)*_Height;

		float3 e = float3(_RippleTex_TexelSize.xy, 0.);
		float p10 = tex2Dlod(_RippleTex, float4(v.uv - e.zy, 1., 1.)).x;
		float p01 = tex2Dlod(_RippleTex, float4(v.uv - e.xz, 1., 1.)).x;
		float p21 = tex2Dlod(_RippleTex, float4(v.uv + e.xz, 1., 1.)).x;
		float p12 = tex2Dlod(_RippleTex, float4(v.uv + e.zy, 1., 1.)).x;

		// Totally fake displacement and shading:
		o.n = normalize(float3(p21 - p01, p12 - p10,1.));

		o.uv = TRANSFORM_TEX(v.uv, _RippleTex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture
		fixed4 col = tex2D(_MainTex, i.uv);
	
	col.rgb = i.n*.5+.5;
	col.rgb *= (1. + .2*(i.n*2. - 1.));
	
	//col = depth;
	//col = _Color;
	float3 highlight = lerp(float3(1., 0., 1.), float3(1., 1., 0.), sin(_Time.y*3.)*.5 + .5);
	col.rgb = lerp(_Color, _Color2,(-(i.n.y))*_Difraction);// lerp(col.rgb, highlight, (i.n.y)*_Difraction);
	return col;// tex2D(_RippleTex, i.uv);// col;
	}
		ENDCG
	}
	}
	}
}