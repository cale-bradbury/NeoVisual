Shader "Camp Cult/Feedback/FeedbackFracture" {
Properties {
_Feedback ("Base (RGB)", 2D) = "white" {}
}
Category{
Blend SrcAlpha OneMinusSrcAlpha
Tags {"Queue" = "Transparent-1000"}
SubShader {
Pass {
	ZWrite Off
	ZTest Always
	Cull Off
	Fog{Mode Off}
CGPROGRAM
#include "UnityCG.cginc"
#pragma vertex vert_img
#pragma fragment frag

uniform float4 _Mouse;
uniform sampler2D _MainTex;
uniform sampler2D _Feedback;
uniform float4 _Feedback_ST;
uniform float4 _Shape;
uniform float4 _Feedback_TexelSize;
/*
    A fracuring dynamical system
	see: https://www.shadertoy.com/view/MsyXRW
*/

#define _G0 0.25
#define _G1 0.125
#define _G2 0.0625
#define W0 -3.0
#define W1 0.5
#define TIMESTEP 0.1
#define ADVECT_DIST 2.0
#define DV 0.70710678

// nonlinearity
float nl(float x) {
    return 1.0 / (1.0 + exp(W0 * (W1 * x - 0.5))); 
}

float4 gaussian(float4 x, float4 x_nw, float4 x_n, float4 x_ne, float4 x_w, float4 x_e, float4 x_sw, float4 x_s, float4 x_se) {
    return _G0*x + _G1*(x_n + x_e + x_w + x_s) + _G2*(x_nw + x_sw + x_ne + x_se);
}

float2 normz(float2 x) {
	return x == float2(0.0, 0.0) ? float2(0.0, 0.0) : normalize(x);
}

float4 adfloatt(float2 ab, float2 vUv, float2 step) {
    
    float2 aUv = vUv - ab * ADVECT_DIST * step;
    float2 n  = float2(0.0, step.y);
    float2 ne = float2(step.x, step.y);
    float2 e  = float2(step.x, 0.0);
    float2 se = float2(step.x, -step.y);
    float2 s  = float2(0.0, -step.y);
    float2 sw = float2(-step.x, -step.y);
    float2 w  = float2(-step.x, 0.0);
    float2 nw = float2(-step.x, step.y);

    float4 u =    tex2D(_Feedback, frac(aUv));
    float4 u_n =  tex2D(_Feedback, frac(aUv+n));
    float4 u_e =  tex2D(_Feedback, frac(aUv+e));
    float4 u_s =  tex2D(_Feedback, frac(aUv+s));
    float4 u_w =  tex2D(_Feedback, frac(aUv+w));
    float4 u_nw = tex2D(_Feedback, frac(aUv+nw));
    float4 u_sw = tex2D(_Feedback, frac(aUv+sw));
    float4 u_ne = tex2D(_Feedback, frac(aUv+ne));
    float4 u_se = tex2D(_Feedback, frac(aUv+se));
    
    return gaussian(u, u_nw, u_n, u_ne, u_w, u_e, u_sw, u_s, u_se);
}

#define SQRT_3_OVER_2 0.86602540378
#define SQRT_3_OVER_2_INV 0.13397459621

float2 diagH(float2 x, float2 x_v, float2 x_h, float2 x_d) {
    return 0.5 * ((x + x_v) * SQRT_3_OVER_2_INV + (x_h + x_d) * SQRT_3_OVER_2);
}

float2 diagV(float2 x, float2 x_v, float2 x_h, float2 x_d) {
    return 0.5 * ((x + x_h) * SQRT_3_OVER_2_INV + (x_v + x_d) * SQRT_3_OVER_2);
}
uniform int flip;
fixed4 frag(v2f_img i) : COLOR
{
	if (flip==1) {
		i.uv.y = 1. - i.uv.y;
	}
float2 vUv = i.uv;// +_Feedback_TexelSize.xy*2.5;
    float2 texel = _Feedback_TexelSize.xy*_Shape.zw;
    
    float2 n  = float2(0.0, 1.0);
    float2 ne = float2(1.0, 1.0);
    float2 e  = float2(1.0, 0.0);
    float2 se = float2(1.0, -1.0);
    float2 s  = float2(0.0, -1.0);
    float2 sw = float2(-1.0, -1.0);
    float2 w  = float2(-1.0, 0.0);
    float2 nw = float2(-1.0, 1.0);

    float4 u =    tex2D(_Feedback, frac(vUv));
    float4 u_n =  tex2D(_Feedback, frac(vUv+texel*n));
    float4 u_e =  tex2D(_Feedback, frac(vUv+texel*e));
    float4 u_s =  tex2D(_Feedback, frac(vUv+texel*s));
    float4 u_w =  tex2D(_Feedback, frac(vUv+texel*w));
    float4 u_nw = tex2D(_Feedback, frac(vUv+texel*nw));
    float4 u_sw = tex2D(_Feedback, frac(vUv+texel*sw));
    float4 u_ne = tex2D(_Feedback, frac(vUv+texel*ne));
    float4 u_se = tex2D(_Feedback, frac(vUv+texel*se));
    
    const float vx = 0.5;
    const float vy = SQRT_3_OVER_2;
    const float hx = SQRT_3_OVER_2;
    const float hy = 0.5;

    float di_n  = nl(distance(u_n.xy + n, u.xy));
    float di_w  = nl(distance(u_w.xy + w, u.xy));
    float di_e  = nl(distance(u_e.xy + e, u.xy));
    float di_s  = nl(distance(u_s.xy + s, u.xy));
    
    float di_nne = nl(distance((diagV(u.xy, u_n.xy, u_e.xy, u_ne.xy) + float2(+ vx, + vy)), u.xy));
    float di_ene = nl(distance((diagH(u.xy, u_n.xy, u_e.xy, u_ne.xy) + float2(+ hx, + hy)), u.xy));
    float di_ese = nl(distance((diagH(u.xy, u_s.xy, u_e.xy, u_se.xy) + float2(+ hx, - hy)), u.xy));
    float di_sse = nl(distance((diagV(u.xy, u_s.xy, u_e.xy, u_se.xy) + float2(+ vx, - vy)), u.xy));    
    float di_ssw = nl(distance((diagV(u.xy, u_s.xy, u_w.xy, u_sw.xy) + float2(- vx, - vy)), u.xy));
    float di_wsw = nl(distance((diagH(u.xy, u_s.xy, u_w.xy, u_sw.xy) + float2(- hx, - hy)), u.xy));
    float di_wnw = nl(distance((diagH(u.xy, u_n.xy, u_w.xy, u_nw.xy) + float2(- hx, + hy)), u.xy));
    float di_nnw = nl(distance((diagV(u.xy, u_n.xy, u_w.xy, u_nw.xy) + float2(- vx, + vy)), u.xy));

    float2 xy_n  = u_n.xy + n - u.xy;
    float2 xy_w  = u_w.xy + w - u.xy;
    float2 xy_e  = u_e.xy + e - u.xy;
    float2 xy_s  = u_s.xy + s - u.xy;
    
    float2 xy_nne = (diagV(u.xy, u_n.xy, u_e.xy, u_ne.xy) + float2(+ vx, + vy)) - u.xy;
    float2 xy_ene = (diagH(u.xy, u_n.xy, u_e.xy, u_ne.xy) + float2(+ hx, + hy)) - u.xy;
    float2 xy_ese = (diagH(u.xy, u_s.xy, u_e.xy, u_se.xy) + float2(+ hx, - hy)) - u.xy;
    float2 xy_sse = (diagV(u.xy, u_s.xy, u_e.xy, u_se.xy) + float2(+ vx, - vy)) - u.xy;
    float2 xy_ssw = (diagV(u.xy, u_s.xy, u_w.xy, u_sw.xy) + float2(- vx, - vy)) - u.xy;
    float2 xy_wsw = (diagH(u.xy, u_s.xy, u_w.xy, u_sw.xy) + float2(- hx, - hy)) - u.xy;
    float2 xy_wnw = (diagH(u.xy, u_n.xy, u_w.xy, u_nw.xy) + float2(- hx, + hy)) - u.xy;
    float2 xy_nnw = (diagV(u.xy, u_n.xy, u_w.xy, u_nw.xy) + float2(- vx, + vy)) - u.xy;

    float2 ma = di_nne * xy_nne + di_ene * xy_ene + di_ese * xy_ese + di_sse * xy_sse + di_ssw * xy_ssw + di_wsw * xy_wsw + di_wnw * xy_wnw + di_nnw * xy_nnw + di_n * xy_n + di_w * xy_w + di_e * xy_e + di_s * xy_s;

    float4 u_blur = gaussian(u, u_nw, u_n, u_ne, u_w, u_e, u_sw, u_s, u_se);
    
    float4 au = adfloatt(u.xy, vUv, texel*2.);
    float4 av = adfloatt(u.zw, vUv, texel*2.);
    
    float2 dv = av.zw + TIMESTEP * ma;
    float2 du = au.xy + TIMESTEP * dv;

    float2 d = i.uv-_Mouse.xy;
    float m = exp(-length(d) / .20);
    du += 0.2 * m * normz(d)*_Mouse.z;

    du = length(du) > 1.0 ? normz(du) : du;
	return  lerp(tex2D(_MainTex, i.uv), float4(du, dv), _Shape.x);
    

}
ENDCG
}
}
}
FallBack "Unlit"
}
