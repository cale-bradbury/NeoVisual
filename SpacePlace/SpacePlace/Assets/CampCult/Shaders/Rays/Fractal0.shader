Shader "Camp Cult/Rays/Fractal0" {
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
#include "ray.cginc"
#pragma vertex vert_img
#pragma fragment frag
#define count 1.
#define pi 3.14159
uniform float4 _Color1;
uniform float4 _Color2;

// Hacked apart by cale bradbury - netgrind.net/2015
// Based on work by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// More info here: http://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm

float fr(float3 p, float v){
	return sdSphere(p,v);
}

float2 map( in float3 pos, float t )
{
   	float2 res = float2(pos.y>-100.,1.1);//float2( pos.y+.2+sin(pos.x*sin(1.+pos.x*4.+pos.z*3.)+cos(pos.z*2.+_Time.y*3.141*2.0)+_Time.y*3.141*2.0)*.03, 0. );
	           
    float d = (length(pos.xy)*.3);
    float a = atan2(pos.y,pos.x);
    float3 p = pos;
    //p.xy = opSpin(p.xy,sin(length(p.xy)+t*pi)*.1);
    
    float2 s = float2(100.,0.);
    for(float i = 1.; i>=0.13;i*=.75){
    	float r = i+(sin(t*pi*4.-d*6.)*.07+.07)*d;
    	s = opU(s,float2(fr(p,r),i));
    	p.xy = opKale(p,pi/2,pi*.9,0.);
    	//p.z-=.1;
    	p.x+=i*2.9;
    }
    res = opU( res, s );
    return res;
}

float2 castRay( in float3 ro, in float3 rd, float time )
{
    float tmin = 0.0001;
    float tmax = 20.0;
    
#if 0
    float tp1 = (0.0-ro.y)/rd.y; if( tp1>0.0 ) tmax = min( tmax, tp1 );
    float tp2 = (1.6-ro.y)/rd.y; if( tp2>0.0 ) { if( ro.y>1.6 ) tmin = max( tmin, tp2 );
                                                 else           tmax = min( tmax, tp2 ); }
#endif

	float precis = 0.0000000001;
    float t = tmin;
    float m = -1.0;
    for( int i=0; i<100; i++ )
    {
	    float2 res = map( ro+rd*t, time );
        if( res.x<precis || t>tmax ) break;
        t += res.x;
	    m = res.y;
    }

    if( t>tmax ) m=m;
    return float2( t, m );
}

float3 calcNormal( in float3 pos, in float time )
{
	float3 eps = float3( 0.1, 0.0,0.0 );
	float3 nor = float3(
	    map(pos+eps.xyy, time).x - map(pos-eps.xyy, time).x,
	    map(pos+eps.yxy, time).x - map(pos-eps.yxy, time).x,
	    map(pos+eps.yyx, time).x - map(pos-eps.yyx, time).x );
	return normalize(nor);
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
        //float3 ref = reflect( rd, nor );
        
        // material        
		col = float3(1.0,1.0,1.0);
		col.rgb = max(m,.05)*(nor.z*.5+.55).rrr;
		col.rgb *= min(1.0,20.-distance(ro,rd*t));
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
	float3 ro = float3( 0.0,-7.0,-7.5 );
	float3 ta = float3(0., 0.0, 0. );
	
	// camera-to-world transformation
    float3x3 ca = setCamera( ro, ta, 0.0 );
    // ray direction
	float3 rd = mul( normalize( float3(p.xy,2.5) ),ca);
    
    // render	
    float3 col = float3(0.,0.,0.);
    
    float s = .001;
    for(float i = 0.;i<count;i++){
     col += render( ro, rd, time+s*i );
    }
	col/=count;
	col.rgb = max(0.,min(1.,col));
	
	col = pow( col, float(0.4545).rrr );

    return lerp(_Color1,_Color2,float4( col, 1.0 ));
}
ENDCG
}
}
}
FallBack "Unlit"
}
