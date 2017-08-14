Shader "Camp Cult/Chromatic Aberration" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Dist("Dist",Vector)=(0,0,0,0)
	_Center("Center",Vector)=(0,0,0,0)
}

SubShader {
	Pass {
		Cull Off ZWrite Off ZTest Always
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
	uv-=_Center.xy;
	float4 c = 1.0;
	c.r = tex2D(_MainTex,float2(uv+uv*_Dist.r)+_Center.xy).r;
	c.g = tex2D(_MainTex,float2(uv+uv*_Dist.g)+_Center.xy).g;
	c.b = tex2D(_MainTex,float2(uv+uv*_Dist.b)+_Center.xy).b;
	return c;
}
ENDCG

	}
}

Fallback off

}