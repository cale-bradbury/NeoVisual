Shader "Camp Cult/Displacement/HeatWave" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Strength("Strength",float)=0
	_Phase("Phase",float)=0
	_Freq ("Freq",float)=1
	_Taps("Taps",float) = 3
}
Category{
SubShader {
	Pass {
		 Cull Off 
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {"Queue"="Transparent+1" "RenderType" = "Transparent+1"}
		
		
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#pragma target 3.0
#pragma multi_compile HEATWAVE_RADIAL _
#include "UnityCG.cginc"
#define PI 3.14158

uniform sampler2D _MainTex;
uniform float _Strength;
uniform float _Phase;
uniform float _Freq;
uniform float _Taps;

fixed4 frag (v2f_img i) : COLOR
{
	int k = 0;
	float4 c = 0.0;
	float t = (max(1.0,_Taps));
	float p = _Phase/(6.28318/t);
	#ifdef HEATWAVE_RADIAL
		float2 uv = i.uv-.5;
		float d = length(uv);
		float a = atan2(uv.y,uv.x);
		float ang =  a*_Freq+p;
		float adis = d*_Strength;
		for(float j = 0.0; j<(PI*2.0);j+=t){
			k++;
			uv.x = cos(a)*(d+adis*sin(ang+j));
			uv.y = sin(a)*(d+adis*cos(ang+j));
			uv+=1.5;
			uv = abs(fmod(uv,2.0)-1.0);
			c+=tex2Dlod(_MainTex,float4(uv.x,uv.y,0.0,1.0));
		}
	#else
		for(float j = 0.0; j<PI*2.0;j+=t){
			k++;
			c+=tex2Dlod(_MainTex,float4(i.uv.x+sin(i.uv.y*_Freq+p+j)*_Strength,i.uv.y,0.0,1.0));
		}
	#endif
	return c/float(k);
	
}
ENDCG

	}
}
}
Fallback off

}