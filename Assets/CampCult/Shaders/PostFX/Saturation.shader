Shader "Camp Cult/Color/Saturation" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Data("X-hue Y-sat Z-val W-null",Vector)=(0,1,1,0)
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform float4 _Data;

fixed3 rgb2hsv(float3 c){
	float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = c.g < c.b ? float4(c.bg, K.wz) : float4(c.gb, K.xy);
    float4 q = c.r < p.x ? float4(p.xyw, c.r) : float4(c.r, p.yzx);

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

float3 hsv2rgb(float3 c){
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

fixed4 frag (v2f_img i) : COLOR{

	float2 uv = i.uv;
	#if !UNITY_UV_STARTS_AT_TOP
	//	uv.y = 1.0-uv.y;
	#endif
	
	float4 c = tex2D(_MainTex,uv);
	c.rgb = rgb2hsv(c.rgb);
	c.r +=_Data.x;
	c.r = fmod(c.r,1.0);
	c.gb *= _Data.yz;
	c.rgb = hsv2rgb(c.rgb);
	return c;
}
ENDCG

	}
}

Fallback off

}