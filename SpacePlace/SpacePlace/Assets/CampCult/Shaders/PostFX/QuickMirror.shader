Shader "Camp Cult/Displacement/QuickMirror" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Angle("Angle",float) = 0
}

SubShader {
	Pass {
		Tags{"PreviewType"="Plane" "Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Fog { Mode off }

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
//#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform float4 _MainTex_ST;
uniform float _Angle;

fixed4 frag (v2f_img i) : COLOR{
	float2 uv = i.uv;
	uv*=_MainTex_ST.xy;
	/*float a = atan2(uv.y,uv.x);
	a+=_Angle;
	float d = length(uv);
	uv.y = sin(a)*d;
	uv.x = cos(a)*d;
	uv.x = abs(uv.x);
	a = atan2(uv.y,uv.x)-_Angle;
	uv.y = sin(a)*d;
	uv.x = cos(a)*d;
	uv+=.5;*/
	uv = fmod(uv,2.0)-1.0;
	uv = abs(uv);
	return tex2D(_MainTex,uv+_MainTex_ST.zw );
}
ENDCG

	}
}

Fallback "Unlit/Texture"

}