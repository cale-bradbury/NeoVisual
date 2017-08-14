Shader "Camp Cult/Color/RGBRotate" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Freq("Freq", float) = 1
	_Phase("Phase", float) = 0
	_Intensity("Intensity", float) = 1
}

SubShader {
	Pass {
		 Cull Off 
		Fog { Mode off }

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
//#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
	uniform float4 _MainTex_TexelSize;
uniform float _Phase;
uniform float _Freq; 
uniform float _Intensity;

fixed4 frag (v2f_img i) : COLOR
{
#if UNITY_UV_STARTS_AT_TOP
	if (_MainTex_TexelSize.y < 0)
		i.uv.y = 1.0 - i.uv.y;
#endif
	fixed4 orig = tex2D(_MainTex, i.uv);
	
	orig.rgb = lerp(orig.rgb,sin(orig.rgb*_Freq+_Phase)*.5+.5, _Intensity);
	orig.a = 1.;
	return orig;
}
ENDCG

	}
}

Fallback off

}