Shader "Camp Cult/Feedback/WaterFeedback" {
Properties {
_MainTex ("Base (RGB)", 2D) = "grey" {}
 }
Category {
SubShader {
Pass {
CGPROGRAM
#include "UnityCG.cginc"
#pragma vertex vert_img
#pragma fragment frag
uniform sampler2D _MainTex;
 uniform sampler2D _AudioTex;
uniform float4 _MainTex_TexelSize;
uniform float4 _MainTex_ST;
uniform float4 _Mouse;
fixed4 frag (v2f_img i) : COLOR
{
    float2 ps = _MainTex_TexelSize.xy;
   	float2 uv = i.uv;
    
    float h = tex2D(_MainTex,uv).x-0.5;
    float v = (tex2D(_MainTex,uv).y-0.5);
    
    float hu = (tex2D(_MainTex,uv+float2(0.0,-ps.y)).x-0.5)-h;
    float hd = (tex2D(_MainTex,uv+float2(0.0,ps.y)).x-0.5)-h;
    float hl = (tex2D(_MainTex,uv+float2(-ps.x,0.0)).x-0.5)-h;
    float hr = (tex2D(_MainTex,uv+float2(+ps.x,0.0)).x-0.5)-h;
    
    v -= 0.1*h;
    v += (hu+hd+hl+hr)*.25;
    v *= .9;
    h += v;

		uv -= .5;
		float d = length(uv);
		float a =abs(atan2(uv.y, uv.x)/3.1415)*.7+.3;
        h+= max(0.,pow(tex2D(_AudioTex,float2(a, 1.).x-.1)*(1.0-abs(d-.2)*20.), 5.));
    
    v = clamp(v,-0.5,0.5);
    return float4(h+.5,v+.5,ceil(a-.1),1.);
}
ENDCG
}
}
}
FallBack "Unlit"
}
