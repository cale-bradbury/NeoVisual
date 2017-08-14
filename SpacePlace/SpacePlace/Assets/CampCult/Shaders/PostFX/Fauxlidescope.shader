Shader "Camp Cult/Displacement/Fauxlidescope" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Taps ("Taps", float) = 1
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma target 3.0
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform float4 _MainTex_ST;
uniform float _Taps;

fixed4 frag (v2f_img i) : COLOR
{
	float2 uv = i.uv;
	uv-=0.5;
	float a = atan2(uv.y,uv.x);
	float d = length(uv);
	float4 c = float4(0.0,0.0,0.0,0.0);
	float p = _Taps/6.28318;
	for(float i= 0.0; i<6.28318;i+=p){
		float b = a+i;
		uv.x = cos(b)*d;
		uv.y = sin(b)*d;	
		uv = (uv+0.5)*_MainTex_ST.xy+_MainTex_ST.zw;
		c += tex2D(_MainTex, uv);
	}	
	return c/_Taps;
}
ENDCG

	}
}

Fallback off

}