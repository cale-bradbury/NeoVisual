Shader "Camp Cult/Displacement/Mirror" {
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
#pragma multi_compile mirrorX _
#pragma multi_compile mirrorY _
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _RampTex;
uniform float4 _Off; 

float4 frag (v2f_img i) : COLOR
{
	float2 uv = i.uv;
#if UNITY_UV_STARTS_AT_TOP
	uv.y = 1.0 - uv.y;
#endif
	uv.y = 1.0 - uv.y;
#ifdef mirrorX
	uv.x = abs(fmod(uv.x+.5,1.) - .5);
#endif
#ifdef mirrorY
	uv.y =1.0- abs(fmod(uv.y+.5,1.)-.5 );
#endif
	return tex2D(_MainTex,uv);
}
ENDCG

	}
}

Fallback off

}