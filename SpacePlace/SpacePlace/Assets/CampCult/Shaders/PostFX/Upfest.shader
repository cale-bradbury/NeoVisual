Shader "Camp Cult/Upfest" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_x("X",Vector) = (0,0,0,0)
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma target 3.0
//#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"
#define count 15.0
#define pi 3.14159

uniform sampler2D _MainTex;
uniform float4 _x;
uniform float4 _Color1;
uniform float4 _Color12;
uniform float4 _Color2;
uniform float4 _Color22;
uniform float _Amp;

float wave(float2 uv, float f){
	f *= (sin(uv.x*pi*.5+pi*.5));
	f+=uv.y;
	f += (f>=0)*1000000.0;
	f = 1.0-pow(f,.2);
	return max(f,0.);
}

float4 frag (v2f_img v) : COLOR{
	float t = fmod(_Time.x,200.0);
	float wrap = sin(min( min(t,200.0-t),.1)*10.0*pi*.5+pi);
	float2 uv = v.uv*2.0-1.0;
	uv.y-=.25;
	float p = length(uv)*.5+.5;
	uv = mul(uv,float2x2(p,0.0,0.0,p));
	//uv.x = -abs(uv.x);
	float f = 0.0;
	float am = _Amp*wrap;
	for(float i = 0.0; i<count; i++){
		t *= fmod(i,2.0)*2.0-1.0;
		p = sin(uv.x* (sin(t*.1+i*.3)+3.0) + t*(1.0+i*.1))*(sin(t+i)*.2+.4)*am;
		p +=      sin(uv.x* (sin(t*1.3+i*.5)*10.0+20.0) + t*(2.+i*.5))*(sin(t*1.4+i)*.1+.1)*am;
		p +=      sin(uv.x* (sin(t*.1+i*.5)*30.0) - t*(30.0+i*.5))*(sin(t*1.4+i)*.04)*am;
		f+=wave(uv,p);
	}
	f /= count;
	f*=2.0;
	float safe = max(0.0,.9-abs(uv.x));
	p = atan2(uv.y,uv.x);
	f *= safe*sin(t*3.0+(uv.x)*cos(t*2.1+sin(t+p)*(sin(t*.8+uv.x)*4.0))*30.0)*.1+.9;
	float4 c1 = lerp(_Color1,_Color12,v.uv.y);
	float4 c2 = lerp(_Color2,_Color22,v.uv.y);
	return lerp(c1,c2,f);
}
ENDCG

	}
}

Fallback off

}