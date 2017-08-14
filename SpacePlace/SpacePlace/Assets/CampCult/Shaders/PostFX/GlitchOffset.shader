Shader "Camp Cult/Displacement/GlitchOffset" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Shape ("Shape", Vector) = (.1,.1,.1,0)
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
#define PI 3.14158

uniform sampler2D _MainTex;
uniform float4 _Shape;

fixed4 frag (v2f_img i) : COLOR
{
	float2 uv = i.uv;
    float4 c = tex2D(_MainTex,uv);
    uv.xy+=c.bg*_Shape.xy;
    uv-=.5;
    float a = atan2(uv.y,uv.x);
    float d = length(uv);
    a+=c.r*_Shape.z;
    uv.x = cos(a)*d;
    uv.y = sin(a)*d;
    uv+=.5;
    c = tex2D(_MainTex,uv);
	return c;
}
ENDCG

	}
}

Fallback off

}