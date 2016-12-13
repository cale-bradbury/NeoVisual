Shader "CampCult/Displacement/FlyEye" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Strength("Strength",float)=0
	_Phase("Phase",float)=0
	_Taps("Taps",float) = 3
}
Category{
SubShader {
	Pass {
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {"Queue"="Transparent+1" "RenderType" = "Transparent+1"}
		
		
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#pragma multi_compile FLYEYE_MIN _
#pragma target 3.0
#include "UnityCG.cginc"
#define TAU 6.283

uniform sampler2D _MainTex;
uniform float4 _MainTex_ST;
uniform float _Strength;
uniform float _Phase;
uniform float _Taps;

fixed4 frag (v2f_img j) : COLOR
{
#if UNITY_UV_STARTS_AT_TOP
	j.uv.y = 1.0 - j.uv.y;
#endif
	#ifdef FLYEYE_MIN
	float4 c = float(1.).xxxx;
	#else
	float4 c = float(0.).xxxx;
	#endif
	float k = 0.;
	j.uv = 1.0 - j.uv;
	float t = max(_Taps, .01);
	for(float i = 0.; i<TAU;i+=t){
		k++;
		float4 f = tex2D(_MainTex, abs(fmod(j.uv + float2(cos(i + _Phase), sin(i + _Phase))*_Strength + 4., 2.) - 1.));
		#ifdef FLYEYE_MIN
		c = min(c,f);
		#else
		c = max(c,f);
		#endif
	}
	
	return c;
	
}
ENDCG

	}
}
}
Fallback off

}