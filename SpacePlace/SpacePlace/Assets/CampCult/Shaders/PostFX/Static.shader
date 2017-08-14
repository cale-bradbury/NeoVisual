Shader "Camp Cult/Displacement/Static" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_StaticTex ("Base (RGB)", 2D) = "white" {}
	_x("x-SampleX y-SampleY z-strength w-SampleYPerSecond",Vector) = (0,0,.1,0)
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
uniform sampler2D _StaticTex;
uniform float4 _x;

fixed4 frag (v2f_img i) : COLOR
{
	float2 uv = i.uv;
	float4 s = tex2D(_StaticTex,float2(_x.x,fmod(uv.y+_x.y+_Time.y*_x.w,1)));
	float4 c = 1.0;
	c.r = tex2D(_MainTex,float2(i.uv.x+(s.r-0.5)*_x.z,uv.y)).r;
	c.g = tex2D(_MainTex,float2(i.uv.x+(s.g-0.5)*_x.z,uv.y)).g;
	c.b = tex2D(_MainTex,float2(i.uv.x+(s.b-0.5)*_x.z,uv.y)).b;
	c.a = tex2D(_MainTex,float2(i.uv.x+(s.a-0.5)*_x.z,uv.y)).a;
	return c;
}
ENDCG

	}
}

Fallback off

}