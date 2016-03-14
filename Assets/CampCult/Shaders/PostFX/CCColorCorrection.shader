Shader "Camp Cult/Color/Color Correction" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_RampTex ("Base (RGB)", 2D) = "grayscaleRamp" {}
	_Off("Offset", Vector) = (0,0,0,0)
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
//#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _RampTex;
uniform float4 _Off; 

float4 frag (v2f_img i) : COLOR
{
	float4 orig = tex2D(_MainTex, i.uv);
	float2 rOff = fixed2(orig.r,1-_Off.r);
	float2 gOff = fixed2(orig.g,1-_Off.g);
	float2 bOff = fixed2(orig.b,1-_Off.b);
	float4 color = 1.0;
	color.r = tex2D(_RampTex, rOff.xy).r + 0.00001; // numbers to workaround Cg's bug at D3D code generation :(
	color.g = tex2D(_RampTex, gOff.xy).g + 0.00002;
	color.b = tex2D(_RampTex, bOff.xy).b + 0.00003;
	

	return lerp(orig,color, _Off.w);
}
ENDCG

	}
}

Fallback off

}