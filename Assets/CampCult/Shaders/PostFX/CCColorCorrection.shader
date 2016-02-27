Shader "Camp Cult/Color/Color Correction" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_RampTex ("Base (RGB)", 2D) = "grayscaleRamp" {}
	_Off("Offset", Vector) = (0,0,0,0)
	_Intensity("Intensity", float) = 1
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
uniform float _Intensity;
uniform float4 _Off; 

fixed4 frag (v2f_img i) : COLOR
{
	fixed4 orig = tex2D(_MainTex, i.uv);
	fixed2 rOff = fixed2(orig.r,1-_Off.r);
	fixed2 gOff = fixed2(orig.g,1-_Off.g);
	fixed2 bOff = fixed2(orig.b,1-_Off.b);
	fixed rr = tex2D(_RampTex, rOff.xy).r + 0.00001; // numbers to workaround Cg's bug at D3D code generation :(
	fixed gg = tex2D(_RampTex, gOff.xy).g + 0.00002;
	fixed bb = tex2D(_RampTex, bOff.xy).b + 0.00003;
	
	fixed4 color = fixed4(rr, gg, bb, orig.a);

	return lerp(orig,color, _Intensity);
}
ENDCG

	}
}

Fallback off

}