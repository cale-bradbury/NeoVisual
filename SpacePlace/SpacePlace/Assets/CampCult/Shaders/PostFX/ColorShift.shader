Shader "Camp Cult/Feedback/ColorShift" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	iChannel0 ("last frame", 2D) = "white" {}
	_Strength("strength", float) = .9
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
#define pi 3.14159265359

uniform sampler2D _MainTex;	//the screen texture
uniform sampler2D iChannel0;	//the last frames texture
uniform float4 _x;			//x/y-max flow distance		z-angle when not radial		w-frame/last frame lerp
uniform float _Strength;

float4 frag (v2f_img i) : COLOR{
	float2 uv = i.uv;
	#if UNITY_UV_STARTS_AT_TOP
		uv.y = 1.0-uv.y;
	#endif
    
    float3 fog = tex2D(_MainTex,uv).rgb;
    
    float3 old = tex2D(iChannel0,uv).rgb;
    uv-=.5;
    float d = length(uv);
    float a = atan2(uv.y,uv.x)+(old.r*sin(_Time.y*.1+d*8.)*.03);
    old.gb = old.gb*.01-.005;
    uv.x = cos(a)*(d-old.g);
    uv.y = sin(a)*(d+old.b);
    old = tex2D(iChannel0,uv+.5).rgb*_Strength;
    fog = max(old,fog);
    fog = pow(fog,1.01);
    return float4(fog, 1.0);
}
ENDCG

	}
}

Fallback off

}