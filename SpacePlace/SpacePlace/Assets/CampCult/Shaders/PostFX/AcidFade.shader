Shader "Camp Cult/Feedback/AcidFade" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Last ("last frame", 2D) = "white" {}
	_x("x-SampleDistance y-null z-null w-blending",Vector) = (.01,.01,0,.5)
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
uniform sampler2D _Last;	//the last frames texture
uniform float4 _x;			//x/y-max flow distance		z-angle when not radial		w-frame/last frame lerp
uniform float4 _Last_ST;
float4 v(float x, float y){
	return tex2D(_Last,float2(x,y));
}

float4 frag (v2f_img i) : COLOR
{
	float2 uv = i.uv;
	float4 c = tex2D(_MainTex,uv);
	#if UNITY_UV_STARTS_AT_TOP
		uv.y = 1.0-uv.y;

	#endif
		uv.y = 1.0 - uv.y;
	/*
	#ifdef light
		float4 s = v(uv.x+_x.x,uv.y+_x.x);
		s = max(s,v(uv.x+_x.x,uv.y-_x.x));
		s = max(s,v(uv.x-_x.x,uv.y+_x.x));
		s = max(s,v(uv.x-_x.x,uv.y-_x.x));
	#else
		float4 s = v(uv.x+_x.x,uv.y+_x.x);
		s = min(s,v(uv.x+_x.x,uv.y-_x.x));
		s = min(s,v(uv.x-_x.x,uv.y+_x.x));
		s = min(s,v(uv.x-_x.x,uv.y-_x.x));
	#endif
	*/
	float2 u = uv;
	u*=_Last_ST.xy;
	u+=_Last_ST.zw;
	float ti = _Time.y*3.141592;
	float an = sin((u.x)*_x.y+ti)+_x.z;
	an += cos((u.y)*_x.y+ti)+_x.z;
	float4 s = v(uv.x+sin(an)*_x.x,uv.y+cos(an)*_x.x);
	
	
	return (lerp(c,s,_x.w));
}
ENDCG

	}
}

Fallback off

}