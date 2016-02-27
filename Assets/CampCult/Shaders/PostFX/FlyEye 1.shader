Shader "CampCult/Displacement/FlyEye1" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Strength("Strength",float)=0
	_Phase("Phase",float)=0
	_Taps("Taps",float) = 3
}
Category{
SubShader {
	Pass {
		ZTest Always Cull Off 
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {"Queue"="Transparent+1" "RenderType" = "Transparent+1"}
		
		
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#pragma target 3.0
#include "UnityCG.cginc"
#define TAU 6.283

uniform sampler2D _MainTex;
uniform float _Strength;
uniform float _Size;

fixed4 frag (v2f_img i) : COLOR
{
	float2 uv = i.uv;
	
	uv = fmod(uv,_Size)-_Size*.5;
	
	float a = atan2(uv.y,uv.x);
	float d = length(uv);
	d+=d*_Strength;
	uv.x = cos(a)*d;
	uv.y = sin(a)*d;
	uv+=i.uv-fmod(i.uv,_Size)+_Size*.5;
	
	float4 c = tex2D(_MainTex,uv);
	
	
	
	
	return c;
	
}
ENDCG

	}
}
}
Fallback off

}