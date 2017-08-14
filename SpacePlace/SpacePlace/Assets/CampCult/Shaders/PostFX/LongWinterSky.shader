Shader "Camp Cult/Generators/LongWinterSky" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Strength("Strength",float)=0
}
Category{
SubShader {
	Pass {
		Cull Off 
		Tags {"Queue"="Opaque-1" "RenderType" = "Opaque-1"}
		
		
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma target 3.0
#include "UnityCG.cginc"
#define TAU 6.283
#define PI 3.1415
#define count 12.

uniform sampler2D _MainTex;
uniform float _Strength;


float wave(float2 uv, float f){
	f *= (sin(uv.x*PI*.5+PI*.5));
	f+=uv.y;
	//f += (f>=0)*1000000.0;
	f = 1.0-pow(fmod(f,.1),.2);
	return max(f,0.);
}

float mic(float f, float y){
	return tex2D(_MainTex,float2(f,y));
}

fixed4 frag (v2f_img j) : COLOR
{
	float phase = _Time.y;
	float2 uv = j.uv;
	
	uv.x*=2.;
	uv.y*=2.;
	
	uv = abs(fmod(uv,2.0)-1.0);
	
	float4 c = tex2D(_MainTex,uv);
	
	float v = 0.;
	uv.x*=4.;
	for(float i = 0; i<count;i++){
		phase *= -1.5;
		//
		float amp = mic(sin((phase+uv.x*TAU+i+cos(uv.y*_Strength+phase+i)))*.5+.5,0.)*.2;
		float freq = 1.+mic(sin(uv.x*_Strength)*.5+.5,uv.y);
		float f = j.uv.y+sin(uv.x*freq+phase+i)*amp-.5;
		
		f = abs(f);
		
		f = max(1.0-f*50.,0.);
		v+=f;
	}
	v/=count;
	
	c.r = v;
	c.gb = c.rr;
	
	return c;
}
ENDCG

	}
}
}
Fallback off

}