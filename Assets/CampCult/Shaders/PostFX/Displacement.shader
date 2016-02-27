Shader "Camp Cult/Displacement/Displacement" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Last ("last frame", 2D) = "white" {}
	_Flow ("flow control", 2D) = "white" {}
	_x("x-SampleX y-SampleY z-angle w-null",Vector) = (0,0,.1,0)
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
				
CGPROGRAM
#pragma multi_compile mlerp madd mmul
#pragma multi_compile radial nradial
#pragma multi_compile invert ninvert
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#pragma target 3.0
#include "UnityCG.cginc"
#define pi 3.14159265359

uniform sampler2D _MainTex;	//the screen texture
uniform sampler2D _Last;	//the last frames texture
uniform sampler2D _Flow;	//texture to control distance shited per channel
uniform float4 _Flow_ST;

uniform float4 _x;			//x/y-max flow distance		z-angle when not radial		w-frame/last frame lerp
uniform float4 _center;

float4 frag (v2f_img i) : COLOR
{
	float2 uv = i.uv;
	float4 c = tex2D(_MainTex,uv);
	#ifdef invert
		uv.y = 1.0-uv.y;
	#endif
	
	#ifdef radial
		float angle = atan2(uv.y-_center.y,uv.x-_center.x)+pi;
		float an = abs(fmod(angle/pi,1.0)-.5);
		angle+=_x.z*pi;
		float d = length(uv.xy-_center.xy);
		float2 u = (float2(an,d-.5));
		float4 f = tex2D(_Flow,fmod(u*_Flow_ST.xy+_Flow_ST.zw, float2(1.0,1.0)));
	#else
		float4 f = tex2D(_Flow,uv*_Flow_ST.xy+_Flow_ST.zw);
		float angle = _x.z;
	#endif
	
	float2 t = uv;
	t.x = cos(angle)*f.r*_x.x;
	t.y = sin(angle)*f.r*_x.y;
	float4 s = tex2D(_Last,uv-t);
	#ifdef madd
		return max(c,s-(1.0-_x.w));
	#endif
	#ifdef mmul
		return lerp(c,s,dot(c,s)*_x.w);
		//return lerp(c,s,float4(cross(c.rgb,s.rgb).rgb*_x.w,1.0));
		//return c/(s*_x.w);
		return smoothstep(c,s,_x.w);
		return min(c,s+(1.0-_x.w));
	#endif
	#ifdef mlerp
		return lerp(c,s,_x.w);
	#endif
	return c;
}
ENDCG

	}
}

Fallback off

}