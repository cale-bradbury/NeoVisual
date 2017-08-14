Shader "Camp Cult/Color/Color Overlay" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color("Color", Color) = (1,1,1,.5)
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
uniform float4 _Color;

fixed4 frag (v2f_img i) : COLOR
{
	fixed4 orig = tex2D(_MainTex, i.uv);
	
	orig.rgb = lerp(orig.rgb,_Color.rgb, _Color.a);

	return orig;
}
ENDCG

	}
}

Fallback off

}