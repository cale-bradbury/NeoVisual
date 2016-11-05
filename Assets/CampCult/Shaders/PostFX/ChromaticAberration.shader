Shader "Camp Cult/Chromatic Aberration" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Dist("Dist",Vector)=(0,0,0,0)
	_Center("Center",Vector)=(0,0,0,0)
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform float4 _Dist;
uniform float4 _Center;

fixed4 frag (v2f_img i) : COLOR
{
	float2 uv = i.uv;
	#if UNITY_UV_STARTS_AT_TOP
		uv.y = 1.0 - uv.y;
	#endif
	uv-=_Center.xy;
	float a = atan2(uv.y,uv.x);
	float d = length(uv);
	float4 c = tex2D(_MainTex,uv+_Center.xy);
	c.r = tex2D(_MainTex,float2(cos(a)*(d+d*_Dist.r), sin(a)*(d+d*_Dist.r))+_Center.xy).r;
	c.g = tex2D(_MainTex,float2(cos(a)*(d+d*_Dist.g), sin(a)*(d+d*_Dist.g))+_Center.xy).g;
	c.b = tex2D(_MainTex,float2(cos(a)*(d+d*_Dist.b), sin(a)*(d+d*_Dist.b))+_Center.xy).b;
	c.a = 1;
	return c;
}
ENDCG

	}
}

Fallback off

}