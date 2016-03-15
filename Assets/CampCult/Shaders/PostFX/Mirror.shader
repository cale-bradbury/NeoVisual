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
#ifdef mirrorX
	i.uv.x = abs(fmod(i.uv.x+.5,1.) - .5);
#endif
#ifdef mirrorY
	i.uv.y =1.0- abs(fmod(i.uv.y+.5,1.)-.5 );
#endif
	return tex2D(_MainTex,i.uv);
}
ENDCG

	}
}

Fallback off

}