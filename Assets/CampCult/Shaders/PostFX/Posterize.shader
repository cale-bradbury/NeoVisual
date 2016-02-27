Shader "Camp Cult/Color/Posterize" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Steps("Phase",float)=0
	_Gamma("Phase",float)=.6
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
//#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform float _Steps;
uniform float _Gamma;

fixed4 frag (v2f_img i) : COLOR
{
	float4 c = tex2D(_MainTex, i.uv);
	c.rgb = pow(c.rgb,_Gamma.rrr);
	c.rbg*=_Steps;
	c.rgb=floor(c.rgb);
	c.rgb/=_Steps;
 	c.rgb = pow(c.rgb, float3(1.0/_Gamma));
	return c;
}
ENDCG

	}
}

Fallback off

}