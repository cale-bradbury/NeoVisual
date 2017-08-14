Shader "CampCult/Displacement/FlyEye2" {
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
#define PI 3.141

uniform sampler2D _MainTex;
uniform float _Strength;
uniform float _Size;

float smin( float a, float b, float k ){
    float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
    return lerp( b, a, h ) - k*h*(1.0-h);
}

fixed4 frag (v2f_img i) : COLOR
{
	float2 uv = i.uv;
	
	float2 m = fmod(uv,_Size)-_Size*.5;
	
	float a = atan2(m.y,m.x);
	float d = 1.0-max(abs(m.x),abs(m.y))/(_Size/2.);
	
	uv = lerp(uv,uv+m*d*sin(d*PI),_Strength);
	
	
	float4 c = tex2D(_MainTex,uv);
	
	return c;
	
}
ENDCG

	}
}
}
Fallback off

}