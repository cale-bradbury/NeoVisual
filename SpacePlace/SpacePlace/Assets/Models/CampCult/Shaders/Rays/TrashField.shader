Shader "Camp Cult/Rays/TrashField" {
Properties {
	//_OffsetTex("Offset Tex",2D) "black" {}
	_Color1 ("Color 1",Color) = (0,0,0,1)
	_Color2 ("Color 2",Color) = (1,0,0,1)
	_Color3 ("Color 3",Color) = (1,1,1,1)
	_Fog("Fog",Float) = 10
	_Alpha("Alpha",Float) = 1
	_Camera ("XYZ-Camera Position W-FOV",Vector) = (0,0,0,60)
	_CameraAngle ("XYZ-Camera Target W-Fisheye",Vector) = (0,0,1,0)
	_Shape ("Z-Mod W-Size",Vector) = (0,0,1,.1)
 }
Category {
Blend SrcAlpha OneMinusSrcAlpha
Tags {"Queue"="Transparent"}
SubShader {
Pass {
CGPROGRAM
#include "UnityCG.cginc"
#include "ray.cginc"
#pragma multi_compile sphere 
#pragma vertex vert_img
#pragma fragment frag
#define count 1.
#define pi 3.14159

uniform sampler2D _OffsetTex;

uniform float4 _Color1;
uniform float4 _Color2;
uniform float4 _Color3;
uniform float _Fog;

uniform float4 _Camera;
uniform float4 _CameraAngle;
uniform float4 _Shape;
uniform float _Alpha;

float2 map( in float3 pos, float t )
{
   	float2 res = float2(1000.,0.);
   	//pos.xy = opKale(pos.xy,pi/6.,_Shape.x*pi,0.);
   	pos+=_Shape.z;
   	pos = abs(fmod(abs(pos),_Shape.z)-_Shape.z*.5);
   	//pos+=tex2Dlod(_OffsetTex,float4(pos.zx,1.,1.))*_Shape.y;
	//float s = sdCross(pos,_Shape.w,_Shape.y);
	float s = 100.0;
	#ifdef sphere
	s = sdSphere(pos,_Shape.w);
	#endif
	#ifdef cube
	s = sdBox(pos,float(_Shape.w).rrr);
	#endif
	#ifdef cross1
	s = sdCross(pos,_Shape.w,_Shape.w*.5);
	#endif
	#ifdef cross2
	s = sdCross(pos,_Shape.w*.1,_Shape.w);
	#endif
	#ifdef hex
	s = sdHexPrism(pos.zyx,float2(_Shape.w,.03));
	#endif
	#ifdef tri
	s = sdCone(pos,float3(_Shape.www));
	#endif
	#ifdef tie
	s = sdCone(pos.zxy,float3(_Shape.www));
	#endif
	//pos.yz = opSpin(pos.yz,pos.y*_Shape.y+_Shape.x);
	//s = sdTorus(pos,float2(_Shape.w,_Shape.w*.5));
    res = opU( res, float2( s, 1. ) );
    return res;
}

float2 castRay( in float3 ro, in float3 rd, float time )
{
    float tmin = 0.0001;
    float tmax = _Fog;
    
#if 0
    float tp1 = (0.0-ro.y)/rd.y; if( tp1>0.0 ) tmax = min( tmax, tp1 );
    float tp2 = (1.6-ro.y)/rd.y; if( tp2>0.0 ) { if( ro.y>1.6 ) tmin = max( tmin, tp2 );
                                                 else           tmax = min( tmax, tp2 ); }
#endif

	float precis = 0.00001;
    float t = tmin;
    float m = -1.0;
    for( int i=0; i<20; i++ )
    {
	    float2 res = map( ro+rd*t, time );
        if(t>tmax ) break;
        t += res.x;
	    m = res.y;
    }

    if( t>tmax ) m=m;
    return float2( t, m );
}

float3 calcNormal( in float3 pos, in float time )
{
	float3 eps = float3( 0.001, 0.00, 0.00 );
	float3 nor = float3(
	    map(pos+eps.xyz, time).x - map(pos-eps.xyz, time).x,
	    map(pos+eps.zxy, time).x - map(pos-eps.zxy, time).x,
	    map(pos+eps.zyx, time).x - map(pos-eps.zyx, time).x );
	return normalize(nor);
}

float3 render( in float3 ro, in float3 rd, float time )
{ 
    float3 col = float3(0.8, 0.9, 1.0);
    float2 res = castRay(ro,rd, time);
    float t = res.x;
	float m = res.y;
    if( m>-0.5 )
    {
        float3 pos = ro + t*rd;
        float3 nor = calcNormal( pos, time);

		col = nor.xyz;
		col.rgb *= min(1.0,pow(max(0.,1.0-t/_Fog),4.));
    }
	col*=pow(1.0-res.x*.05,1.);
	return col;
}

float3x3 setCamera( in float3 ro, in float3 ta, float cr )
{
	float3 cw = normalize(ta-ro);
	float3 cp = float3(sin(cr), cos(cr),0.0);
	float3 cu = normalize( cross(cw,cp) );
	float3 cv = normalize( cross(cu,cw) );
    return float3x3( cu, cv, cw );
}

fixed4 frag (v2f_img i) : COLOR
{
	float2 q = i.uv;
    float2 p = -1.0+2.0*q;
	float time = _Time.y;

	// camera	
	float3 ro = _Camera.xyz;
	float3 ta = _Camera.xyz+_CameraAngle.xyz;
	
	// camera-to-world transformation
    float3x3 ca = setCamera( ro, ta, 0.0 );
    // ray direction
	float velocity = _CameraAngle.w;
    
    float fov = _Camera.w;
    float mu = _Shape.x;
    float rayZ = tan ((90.0 - 0.5 * fov) * 0.01745329252);
	float3 rd = mul( normalize( float3(p.xy,-rayZ)*mu ),ca);
    rd = float3(rd.xy,rsqrt (1.0 - velocity * velocity) * (rd.z + velocity));
    rd = normalize(rd);
    
    // render	
    float3 col = render( ro, rd, time);
	col = pow( col, float(.8).rrr );
	
	float f = clamp(col.r,0.0,1.0);
	if(f<.5)
		col = lerp(_Color1,_Color2,f*2.);
	else
		col = lerp(_Color2,_Color3,(f-.5)*2.);

    return float4( col, _Alpha-(length(p)) );
}
ENDCG
}
}
}
FallBack "Unlit"
}
