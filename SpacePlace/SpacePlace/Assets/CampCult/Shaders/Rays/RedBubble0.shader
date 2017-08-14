Shader "Camp Cult/Rays/RedBubble0" {
Properties {
	_Color1("Color 1", Color) = (1,1,1,1)
	_Color2("Color 2", Color) = (1,0,0,1)
 }
Category {
Blend SrcAlpha OneMinusSrcAlpha
Tags {"Queue"="Transparent"}
SubShader {
Pass {
CGPROGRAM
#include "UnityCG.cginc"
#pragma vertex vert_img
#pragma fragment frag
#define count 20.
#define pi 3.14159
uniform float4 _Color1;
uniform float4 _Color2;

// Created by inigo quilez - iq/2013
// Hacked apart by cale bradbury - netgrind.net/2015
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

// More info here: http://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm

float sdPlane( float3 p )
{
	return p.y;
}

float sdSphere( float3 p, float s )
{
    return length(p)-s;
}

float sdBox( float3 p, float3 b )
{
  float3 d = abs(p) - b;
  return min(max(d.x,max(d.y,d.z)),0.0) + length(max(d,0.0));
}

float udRoundBox( float3 p, float3 b, float r )
{
  return length(max(abs(p)-b,0.0))-r;
}

float sdTorus( float3 p, float2 t )
{
  return length( float2(length(p.xz)-t.x,p.y) )-t.y;
}

float sdHexPrism( float3 p, float2 h )
{
    float3 q = abs(p);
#if 0
    return max(q.z-h.y,max((q.x*0.866025+q.y*0.5),q.y)-h.x);
#else
    float d1 = q.z-h.y;
    float d2 = max((q.x*0.866025+q.y*0.5),q.y)-h.x;
    return length(max(float2(d1,d2),0.0)) + min(max(d1,d2), 0.);
#endif
}

float sdCapsule( float3 p, float3 a, float3 b, float r )
{
	float3 pa = p-a, ba = b-a;
	float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
	return length( pa - ba*h ) - r;
}

float sdTriPrism( float3 p, float2 h )
{
    float3 q = abs(p);
#if 0
    return max(q.z-h.y,max(q.x*0.866025+p.y*0.5,-p.y)-h.x*0.5);
#else
    float d1 = q.z-h.y;
    float d2 = max(q.x*0.866025+p.y*0.5,-p.y)-h.x*0.5;
    return length(max(float2(d1,d2),0.0)) + min(max(d1,d2), 0.);
#endif
}

float sdCylinder( float3 p, float2 h )
{
  float2 d = abs(float2(length(p.xz),p.y)) - h;
  return min(max(d.x,d.y),0.0) + length(max(d,0.0));
}

float sdCone( in float3 p, in float3 c )
{
    float2 q = float2( length(p.xz), p.y );
#if 0
	return max( max( dot(q,c.xy), p.y), -p.y-c.z );
#else
    float d1 = -p.y-c.z;
    float d2 = max( dot(q,c.xy), p.y);
    return length(max(float2(d1,d2),0.0)) + min(max(d1,d2), 0.);
#endif    
}


float sdPyramid( in float3 p, in float3 c )
{
    float2 q = float2( length(p.xz), p.y );
#if 0
	return max( max( dot(q,c.xy), p.y), -p.y-c.z );
#else
    float d1 = -p.y-c.z;
    float d2 = max( dot(q,c.xy), p.y);
    return length(max(float2(d1,d2),0.0)) + min(max(d1,d2), 0.);
#endif    
}

float length2( float2 p )
{
	return sqrt( p.x*p.x + p.y*p.y );
}

float length6( float2 p )
{
	p = p*p*p; p = p*p;
	return pow( p.x + p.y, 1.0/6.0 );
}

float length8( float2 p )
{
	p = p*p; p = p*p; p = p*p;
	return pow( p.x + p.y, 1.0/8.0 );
}

float sdTorus82( float3 p, float2 t )
{
  float2 q = float2(length2(p.xz)-t.x,p.y);
  return length8(q)-t.y;
}

float sdTorus88( float3 p, float2 t )
{
  float2 q = float2(length8(p.xz)-t.x,p.y);
  return length8(q)-t.y;
}

float sdCylinder6( float3 p, float2 h )
{
  return max( length6(p.xz)-h.x, abs(p.y)-h.y );
}

//----------------------------------------------------------------------

float opS( float d1, float d2 )
{
    return max(-d2,d1);
}

float2 opU( float2 d1, float2 d2 ){
	return (d1.x<d2.x) ? d1 : d2;
}

float3 opRep( float3 p, float3 c ){
    return fmod(p,c)-0.5*c;
}

float3 opTwist( float3 p, float freq, float phase ){
    float  c = cos(freq*p.y+phase);
    float  s = sin(freq*p.y+phase);
    float2x2   m = float2x2(c,-s,s,c);
    return float3(mul(p.xz,m),p.y);
}
/*float smin( float a, float b, float k ){
    float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
    return lerp( b, a, h ) - k*h*(1.0-h);
}*/
float smin( float a, float b, float k )
{
    float res = exp( -k*a ) + exp( -k*b );
    return -log( res )/k;
}
float smin3( float a, float b, float c, float k )
{
    float res = exp( -k*a ) + exp( -k*b )+ exp( -k*c );
    return -log( res )/k;
}
float smin4( float a, float b, float c, float d, float k )
{
    float res = exp( -k*a ) + exp( -k*b )+ exp( -k*c )+ exp( -k*d );
    return -log( res )/k;
}

//----------------------------------------------------------------------

float sphereRing(float3 pos, float amp, float phase, float size){
	float b = sdSphere(pos+float3(sin(phase)*amp,cos(phase)*amp,0),size);
    b = opU(b,sdSphere(pos+float3(sin(phase+pi)*amp,cos(phase+pi)*amp,0),size));
    b = opU(b,sdSphere(pos+float3(sin(phase+pi*.5)*amp,cos(phase+pi*.5)*amp,0),size));
    b = opU(b,sdSphere(pos+float3(sin(phase-pi*.5)*amp,cos(phase-pi*.5)*amp,0),size));							
	return b;
}

float sdCross(float3 pos, float a, float b){
	return opU(
		sdBox(pos, float3(a,a,b)), opU(
		sdBox(pos, float3(a,b,a)),
		sdBox(pos, float3(b,a,a))
	));
}

float2 map( in float3 pos, float t )
{
   	float2 res = float2(pos.y>-100.,0.);//float2( pos.y+.2+sin(pos.x*sin(1.+pos.x*4.+pos.z*3.)+cos(pos.z*2.+_Time.y*3.141*2.0)+_Time.y*3.141*2.0)*.03, 0. );
	           
   	//pos.z-=10.;
    float d = length(pos);
    float a = t;
    //pos.xy = 
    float3 p = pos;
    p.x+=sin(p.y*2.+t*pi+length(pos.xy))*.1;
    p.y+=cos(cos(pos.x*1.)*3.+t*pi)*.14;
    float s = sdSphere(p,float3(1.,1.,1.));
   		
   	
   	
    pos = abs(pos.xyz)-2.;
    s = opU(s,sdCross(pos,.015,.2));
    
    //pos.xy = mul(pos.zy,float2x2(1.,sin(a),-sin(a),1.));
    
    res = opU( res, float2( s, 1. ) );
    return res;
}

float2 castRay( in float3 ro, in float3 rd, float time )
{
    float tmin = 0.01;
    float tmax = 20.0;
    
#if 0
    float tp1 = (0.0-ro.y)/rd.y; if( tp1>0.0 ) tmax = min( tmax, tp1 );
    float tp2 = (1.6-ro.y)/rd.y; if( tp2>0.0 ) { if( ro.y>1.6 ) tmin = max( tmin, tp2 );
                                                 else           tmax = min( tmax, tp2 ); }
#endif

	float precis = 0.000001;
    float t = tmin;
    float m = -1.0;
    for( int i=0; i<200; i++ )
    {
	    float2 res = map( ro+rd*t, time );
        if( res.x<precis || t>tmax ) break;
        t += res.x;
	    m = res.y;
    }

    if( t>tmax ) m=m;
    return float2( t, m );
}


float softshadow( in float3 ro, in float3 rd, in float mint, in float tmax, in float time)
{
	float res = 1.0;
    float t = mint;
    for( int i=0; i<126; i++ )
    {
		float h = map( ro + rd*t, time ).x;
        res = min( res, 8.0*h/t );
        t += clamp( h, 0.02, 0.10 );
        if( h<0.0001 || t>tmax ) break;
    }
    return clamp( res, 0.0, 1.0 );

}

float3 calcNormal( in float3 pos, in float time )
{
	float3 eps = float3( 0.00001, 0.0, 0. );
	float3 nor = float3(
	    map(pos+eps.xyy, time).x - map(pos-eps.xyy, time).x,
	    map(pos+eps.yxy, time).x - map(pos-eps.yxy, time).x,
	    map(pos+eps.yyx, time).x - map(pos-eps.yyx, time).x );
	return normalize(nor);
}

float calcAO( in float3 pos, in float3 nor, in float time )
{
	float oCamp = 0.0;
    float sca = 1.0;
    for( int i=0; i<5; i++ )
    {
        float hr = 0.01 + 0.12*float(i)/4.0;
        float3 aopos =  nor * hr + pos;
        float dd = map( aopos , time).x;
        oCamp += -(dd-hr)*sca;
        sca *= 0.95;
    }
    return clamp( 1.0 - 3.0*oCamp, 0.0, 1.0 );    
}




float3 render( in float3 ro, in float3 rd, float time )
{ 
    float3 col = float3(1.0,1.0, 1.0);
    float2 res = castRay(ro,rd, time);
    float t = res.x;
	float m = res.y;
    if( m>-0.5 )
    {
        float3 pos = ro + t*rd;
        float3 nor = calcNormal( pos, time);
        float3 ref = reflect( rd, nor );
        
        // material        
		col = float3(1.0,1.0,1.0);
		

        // lighitng        
       /* float oCamp = calcAO( pos, nor , time);
		float3  lig = normalize( float3(1.0,1.0,1.0) );
		float amb = clamp( 0.5+0.5*nor.y, 0.0, 1.0 );
        float dif = clamp( dot( nor, lig ), 0.0, 1.0 );
        float bac = clamp( dot( nor, normalize(float3(-lig.x,0.0,-lig.z))), 0.0, 1.0 )*clamp( 1.0-pos.y,0.0,1.0);
        float fre = pow( clamp(1.0+dot(nor,rd),0.0,1.0), 2.0 );
        
        dif *= softshadow( pos, lig, 0.02, 2.5, time );

		float3 brdf = float(0.0).rrr;
        brdf += 1.20*dif*float(1.00).rrr;
        brdf += 0.30*amb*float(0.50).rrr*oCamp;
        brdf += 0.30*bac*float(.25).rrr*oCamp;
        brdf += 1.40*fre*float(1.00).rrr*oCamp;
		col = col*brdf;*/
		col.rgb = res.yyy*pow(-nor.y*.5+.6,1.);
    }
	//col*=pow(1.0-res.x*.01,8.);
	return float3( clamp(col,0.0,1.0) );
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
	float3 ro = float3( 0.0,0.0,-8.5 );
	float3 ta = float3(0., 0.0, 0. );
	
	// camera-to-world transformation
    float3x3 ca = setCamera( ro, ta, 0.0 );
    // ray direction
	float3 rd = mul( normalize( float3(p.xy,2.5) ),ca);
    
    // render	
    float3 col = render( ro, rd, time );
    
    float s = .01;
    for(float i = 0.;i<count;i++){
     col += render( ro, rd, time+s*i );
    }
	col/=count+1.0;
	col.rgb = max(0.,min(1.,col.r)).rrr;
	
	col = pow( col, float(0.4545).rrr );

    return lerp(_Color1,_Color2,float4( col, 1.0 ));
}
ENDCG
}
}
}
FallBack "Unlit"
}
