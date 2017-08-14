Shader "Camp Cult/Rays/HYPRGLTCH" {
Properties {
	_Color1("Color 1", Color) = (1,1,1,1)
	_Color2("Color 2", Color) = (1,0,0,1)
	_Shape("X-curve Y-shift Z-mul",Vector)=(.75,1.5,.75,0)
	_x("X-curve Y-shift Z-mul",Vector)=(.75,1.5,.75,0)
	_Fog("Fog",Float) = 10
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
uniform float4 _x;
uniform float4 _Shape;
uniform float _Fog;

// Hacked apart by cale bradbury - netgrind.net/2015
// Based on work by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// More info here: http://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm

float fr(float3 p, float v){
	return sdBox(p,v.rrr);
}

float2 map( in float3 pos, float t )
{
	float space = _Shape.y;
	float size = _Shape.z+sin(length(pos)*.2-_Time.y*pi*2.+pos.x*.2)*.1+.1;
   	float2 res = float2(pos.y>-100.,1.1);//float2( pos.y+.2+sin(pos.x*sin(1.+pos.x*4.+pos.z*3.)+cos(pos.z*2.+_Time.y*3.141*2.0)+_Time.y*3.141*2.0)*.03, 0. );	

    float3 p = pos;
    p.xy = opSpin(pos.xy,_Time.y*pi*_x.z + pos.z*_x.x*pi);
    p.x = fmod(abs(p.x),space)-space*.5;
    p.z = fmod(abs(p.z),space)-space*.5;
    p.y = fmod(abs(p.y),space)-space*.5;
    
    
    float vert = fr(float3(p.x,0.0,p.z),size);
    float hori = fr(float3(0.0,p.y,p.z),size);
    float dept = fr(float3(p.x,p.y,0.0),size);
    
    float2 s = float2(smin3(vert,hori,dept,_x.w),1.);
    /*
	pos.xy = opSpin(pos.xy,pi*.25);
	pos.xz = opSpin(pos.xz,pi*.25);
	pos.yz = opSpin(pos.yz,_Time.y*pi*.5);
    float2 s = float2(sdBox(pos,float(1.0).rrr),1.);*/
    
    res = opU( res, s );
    return res;
}

float2 castRay( in float3 ro, in float3 rd, float time )
{
    float tmin = 0.01;
    float tmax = 110.0;
    
#if 0
    float tp1 = (0.0-ro.y)/rd.y; if( tp1>0.0 ) tmax = min( tmax, tp1 );
    float tp2 = (1.6-ro.y)/rd.y; if( tp2>0.0 ) { if( ro.y>1.6 ) tmin = max( tmin, tp2 );
                                                 else           tmax = min( tmax, tp2 ); }
#endif

	float precis = 0.00000001;
    float t = tmin;
    float m = -1.0;
    float3 glitch = float3(0.,0.,0.);
    for( int i=0; i<100; i++ )
    {
	    float2 res = map( ro+rd*t+glitch, time );
	    glitch.x = floor(cos(floor(rd.z*t*343.36234)/1324.36234+_Time.y*pi*2.)*4.)/4.;
	    glitch.y = floor(cos(floor(rd.z*t*134.36)/1428.36234+floor(_Time.y*pi*4.)*pi*2.)*4.)/4.;
    	glitch = fmod(glitch,_Shape.yyy);
    	res = opU(res,float2(map( ro+rd*t+glitch, time ).x,.5));
	    glitch.x = floor(sin(floor(rd.z*t*134.36234)/14.36234+floor(_Time.y*pi*4.)*pi*2.)*4.)/4.;
	    glitch.y = floor(cos(floor(rd.z*t*268.234)/28.36234+(_Time.y*pi*6.)*pi*2.)*4.)/4.;
    	glitch = fmod(glitch,_Shape.yyy);
    	res = opU(res,float2(map( ro+rd*t+glitch, time ).x,0.));
        //if( res.x<precis || t>tmax ) break;
        if(t>tmax)break;
        t += res.x;
	    m = res.y;
	    glitch.x = floor(sin(floor(rd.z*t*344.234)/324.36234+floor(_Time.y*pi*6.)*pi*2.)*4.)/4.;
	    glitch.y = floor(sin(floor(rd.z*t*856.3234)/428.36234+floor(_Time.y*pi*4.)*pi*2.)*4.)/4.;
    	glitch = fmod(glitch,_Shape.yyy);
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
		//m = max(m,.05)*(nor.z*.5+.55);
		col.r = floor(m+.1);
		col.g = 1.0-(abs(m-.5)*2.);
		col.b = 1.0-ceil(m);
		col = abs(floor(fmod(abs(pos*.2),2.0.rrr))-col);
		//col.rgb = sin(m*float3(0.,.33,.66)*pi*2.)*.5+.5;
		//col.rgb = m.rrr;
		col.rgb *= min(1.0,pow(max(0.,1.0-t/_Fog),2.));
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
	float3 ro = float3( 0.0,0.,10.+_Shape.w);
	float3 ta = float3( 0, -3., 10.+_Shape.w+ 10.);
	
	// camera-to-world transformation
    float3x3 ca = setCamera( ro, ta, 0.0 );
    // ray direction
    float velocity = _Shape.x;//sin(_Time.y*pi*.25)*.48+.48;
    
    float fov = 100.0;
    float mu = 500.;
    float rayZ = tan ((90.0 - 0.5 * fov) * 0.01745329252);
	float3 rd = mul( normalize( float3(p.xy,-rayZ)*mu ),ca);
    rd = float3(rd.xy,rsqrt (1.0 - velocity * velocity) * (rd.z + velocity));
    rd = normalize(rd);
    
    
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
