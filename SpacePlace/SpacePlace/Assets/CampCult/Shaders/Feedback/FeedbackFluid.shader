Shader "Camp Cult/Feedback/FeedbackFluid" {
Properties {
_Feedback ("Base (RGB)", 2D) = "white" {}
}
Category{
	Blend SrcAlpha OneMinusSrcAlpha
	Tags{ "Queue" = "Transparent-1000" }
SubShader {
Pass {
	ZWrite Off
	ZTest Always
	Cull Off
	Fog{ Mode Off }
CGPROGRAM
#include "UnityCG.cginc"
#pragma vertex vert_img
#pragma fragment frag
uniform sampler2D _Feedback;
uniform sampler2D _MainTex;
uniform sampler2D _AudioTex;
uniform float4 _Mouse;
 uniform float4 _Feedback_ST;
 uniform float4 _Feedback_TexelSize;
 uniform float4 _Shape;
/*
    A fluid-like dynamical system
	see: https://www.shadertoy.com/view/XddSRX
*/


float2 normz(float2 x) {
	return x == float2(0.0, 0.0) ? float2(0.0, 0.0) : normalize(x);
}

// reverse adfloattion
float4 adfloatt(float2 ab, float2 vUv, float4 step, float sc) {
    
    float2 aUv = vUv - ab * sc * step;
    

	const float _G0 = 0.25; // center weight
	const float _G1 = 0.125; // edge-neighbors
	const float _G2 = 0.0625; // vertex-neighbors
    // 3x3 neighborhood coordinates
	
    float2 n  = float2(0.0, step.y);
    float2 ne = float2(step.x, step.y);
    float2 e  = float2(step.x, 0.0);
    float2 se = float2(step.x, step.w);
    float2 s  = float2(0.0, step.w);
    float2 sw = float2(step.z, step.w);
    float2 w  = float2(step.z, 0.0);
    float2 nw = float2(step.z, step.y);

    float4 uv =    tex2D(_Feedback, frac(aUv));
    float4 uv_n =  tex2D(_Feedback, frac(aUv+n));
    float4 uv_e =  tex2D(_Feedback, frac(aUv+e));
    float4 uv_s =  tex2D(_Feedback, frac(aUv+s));
    float4 uv_w =  tex2D(_Feedback, frac(aUv+w));
    float4 uv_nw = tex2D(_Feedback, frac(aUv+nw));
    float4 uv_sw = tex2D(_Feedback, frac(aUv+sw));
    float4 uv_ne = tex2D(_Feedback, frac(aUv+ne));
    float4 uv_se = tex2D(_Feedback, frac(aUv+se));
    
    return _G0*uv + _G1*(uv_n + uv_e + uv_w + uv_s) + _G2*(uv_nw + uv_sw + uv_ne + uv_se);
}
uniform int flip;
fixed4 frag (v2f_img i) : COLOR
{
if (flip==1) {
	i.uv.y = 1. - i.uv.y;
}

float2 vUv = i.uv.xy;// +_Feedback_TexelSize.xy*.5;
	vUv += _Feedback_TexelSize.xy*5.*(-(i.uv- _Mouse.xy))*_Mouse.z;
	float p = _Time.x;
	float vv = cos(i.uv.x*32. + p)+ sin(i.uv.y*32. + p);
	//abs(atan2(i.uv.x-.5, i.uv.y-.5)/3.1415)
	p+= tex2D(_AudioTex, float2(tex2D(_Feedback,vUv).r, 0.)).r*3.;
	p += vv;
	vUv += float2(sin(p), cos(p))*_Shape.y;


	const float _K0 = -20.0 / 6.0; // center weight
	const float _K1 = 4.0 / 6.0;   // edge-neighbors
	const float _K2 = 1.0 / 6.0;   // vertex-neighbors
	const float cs = -3.0;  // curl scale
	const float ls = 3.0;  // laplacian scale
	const float ps = 0.0;  // laplacian of divergence scale
	const float ds = -12.0; // divergence scale
	const float dp = -6.0; // divergence update scale
	const float pl = 0.3;   // divergence smoothing
	const float ad = 6.0;   // adfloattion distance scale
	const float pwr = 1.0;  // power when deriving rotation angle from curl
	const float amp = 1.0;  // self-amplification
	const float upd = .99;  // update smoothing
	const float sq2 = 0.6;  // diagonal weight

    float2 texel = _Feedback_TexelSize.xy*_Shape.zw;
   
    // 3x3 neighborhood coordinates
	float4 step = float4(texel.xy, -texel.xy);
	float2 n = float2(0.0, step.y);
	float2 ne = float2(step.x, step.y);
	float2 e = float2(step.x, 0.0);
	float2 se = float2(step.x, step.w);
	float2 s = float2(0.0, step.w);
	float2 sw = float2(step.z, step.w);
	float2 w = float2(step.z, 0.0);
	float2 nw = float2(step.z, step.y);

    float4 uv =    tex2D(_Feedback, frac(vUv));
    float4 uv_n =  tex2D(_Feedback, frac(vUv+n));
    float4 uv_e =  tex2D(_Feedback, frac(vUv+e));
    float4 uv_s =  tex2D(_Feedback, frac(vUv+s));
    float4 uv_w =  tex2D(_Feedback, frac(vUv+w));
    float4 uv_nw = tex2D(_Feedback, frac(vUv+nw));
    float4 uv_sw = tex2D(_Feedback, frac(vUv+sw));
    float4 uv_ne = tex2D(_Feedback, frac(vUv+ne));
    float4 uv_se = tex2D(_Feedback, frac(vUv+se));
    
    // uv.x and uv.y are the x and y components, uv.z and uv.w aCampumulate divergence 

    // laplacian of all components
    float4 lapl  = _K0*uv + _K1*(uv_n + uv_e + uv_w + uv_s) + _K2*(uv_nw + uv_sw + uv_ne + uv_se);
    
    // calculate curl
    // floattors point clockwise about the center point
    float curl = uv_n.x - uv_s.x - uv_e.y + uv_w.y + sq2 * (uv_nw.x + uv_nw.y + uv_ne.x - uv_ne.y + uv_sw.y - uv_sw.x - uv_se.y - uv_se.x);
    
    // compute angle of rotation from curl
    float sc = cs * sign(curl) * pow(abs(curl), pwr);
    // calculate divergence
    // floattors point inwards towards the center point
    float div  = uv_s.y - uv_n.y - uv_e.x + uv_w.x + sq2 * (uv_nw.x - uv_nw.y - uv_ne.x - uv_ne.y + uv_sw.x + uv_sw.y + uv_se.y - uv_se.x);
	
    float2 norm = normz(uv.xy);
    
    float sdx = uv.z + dp * uv.x * div + pl * lapl.z;
    float sdy = uv.w + dp * uv.y * div + pl * lapl.w;

    float2 ab = adfloatt(float2(uv.x, uv.y), vUv, step, ad).xy;
    
    // temp values for the update rule
    float ta = amp * ab.x + ls * lapl.x + norm.x * ps * lapl.z + ds * sdx;
    float tb = amp * ab.y + ls * lapl.y + norm.y * ps * lapl.w + ds * sdy;

    // rotate
    float a = ta * cos(sc) - tb * sin(sc);
    float b = ta * sin(sc) + tb * cos(sc);
    float4 abd = upd * uv + (1.0 - upd) * float4(a,b,sdx,sdy);
    
	return lerp(tex2D(_MainTex, i.uv), float4(abd), _Shape.x);
    
    //abd.xy = clamp(length(abd.xy) > 1.0 ? normz(abd.xy) : abd.xy, -1.0, 1.0);
	
    
}
ENDCG
}
}
}
FallBack "Unlit"
}
