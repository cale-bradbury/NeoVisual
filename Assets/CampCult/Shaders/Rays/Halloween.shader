Shader "Camp Cult/Rays/Halloween" {
Properties {
	_OffsetTex("Offset Tex",2D) = "black" {}
	_Color1 ("Color 1",Color) = (0,0,0,1)
	_Color2 ("Color 2",Color) = (1,0,0,1)
	_Color3 ("Color 3",Color) = (1,1,1,1)
	_Fog("Fog",Float) = 10
	_Camera ("Camera Position",Vector) = (0,0,0,0)
	_CameraAngle ("Camera Angle",Vector) = (0,0,1,0)
	_TerainHeight ("TerainHeight",float) = .1
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
#pragma target 3.0
#define count 1.

#define pi 3.14159

uniform float4 _Color1;
uniform float4 _Color2;
uniform float4 _Color3;
uniform float _Fog;
uniform sampler2D _OffsetTex;
uniform float4 _OffsetTex_ST;

uniform float4 _Camera;
uniform float4 _CameraAngle;
uniform float _TerainHeight;

float2 map( in float3 pos, float t )
{
   	float2 res = float2(1000.,0.);//float2( pos.y+.2+sin(pos.x*sin(1.+pos.x*4.+depth*3.)+cos(depth*2.+_Time.y*3.141*2.0)+_Time.y*3.141*2.0)*.03, 0. );
	float depth = pos.z;
	//float2 uv = abs(fmod(abs(pos.xz*_OffsetTex_ST.xy+_OffsetTex_ST.zw),2.0)-1.);
	float2 uv = fmod(abs(pos.xz*_OffsetTex_ST.xy+_OffsetTex_ST.zw),1.0);
	float s = pos.y-tex2Dlod(_OffsetTex, float4(uv,1.,1.))*_TerainHeight;
   	
   	
   	
   	//cross
   	float3 p = pos - float3(_Camera.xyz+float3(0.,-1.,-2.));
   	float c = sdBox(p,float3(.2,.8,.1)*2.);
   	c = opU(c, sdBox(float3(p.x,p.y+.3,p.z),float3(.55,.2,.1)*2.));
	//s = opU(s, c);
     
   	//gates
   	p = pos;
   	p.z = fmod(p.z,5.0);
   	//p.x = abs(p.x);
   	p.y += 1.;
   	p.xy = opKale(p.xy,pi*.5,1.75*pi,pi*.25);
   	p.x -= 3.;
   	float g = sdBox(p,float3(.1,20.,.5));
   	//s = opU(s, g);
   	
   	//penta
   	p = pos;
   	p.z = fmod(p.z,10.0);
   	p.y -= _Camera.y+(1.0-distance(_Camera.z,pos.z)*.5);
   	p.xy = opKale(p.xy,pi*.2,pi*.2,pi*.1);
   	p.x -= 3.1;
   	float pen = sdBox(p,float3(.3,20.,.5));
   	float a = atan2(p.y,p.x);
   	a-=pi*.4;
   	float d = length(p.xy);
   	p.x = cos(a)*d-2.1;
   	p.y = sin(a)*d;
   	pen = opU(pen,sdBox(p,float3(.3,20.,.5)));
   	//s = smin(s,pen,_Shape.w);
   
   	res = opU( res, float2( s, 1. ) );
    

   	return res;
}

float2 castRay( in float3 ro, in float3 rd, float time )
{
    float tmin = 0.001;
    float tmax = _Fog;
    
#if 0
    float tp1 = (0.0-ro.y)/rd.y; if( tp1>0.0 ) tmax = min( tmax, tp1 );
    float tp2 = (1.6-ro.y)/rd.y; if( tp2>0.0 ) { if( ro.y>1.6 ) tmin = max( tmin, tp2 );
                                                 else           tmax = min( tmax, tp2 ); }
#endif

	float precis = 0.000001;
    float t = tmin;
    float m = -1.0;
    for( int i=0; i<20; i++ )
    {
	    float2 res = map( ro+rd*t, time );
        if(res.y<precis||t>tmax ) break;
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
    for( int i=0; i<168; i++ )
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
	float3 eps = float3( 0.001, 0.0, 0. );
	float3 nor = float3(
	    map(pos+eps.xyy, time).x - map(pos-eps.xyy, time).x,
	    map(pos+eps.yxy, time).x - map(pos-eps.yxy, time).x,
	    map(pos+eps.yyx, time).x - map(pos-eps.yyx, time).x );
	return normalize(nor);
}

float calcAO( in float3 pos, in float3 nor, in float time )
{
	float occ = 0.0;
    float sca = 1.0;
    for( int i=0; i<5; i++ )
    {
        float hr = 0.01 + 0.12*float(i)/4.0;
        float3 aopos =  nor * hr + pos;
        float dd = map( aopos , time).x;
        occ += -(dd-hr)*sca;
        sca *= 0.95;
    }
    return clamp( 1.0 - 3.0*occ, 0.0, 1.0 );    
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
        float3 ref = reflect( rd, nor );
        
        // material        
		col = float3(1.0,1.0,1.0);
		
        // lighitng        
        float occ = calcAO( pos, nor , time);
		float3  lig = normalize( float3(1.0,1.0,1.0) );
		float amb = clamp( 0.5+0.5*nor.y, 0.0, 1.0 );
        float dif = clamp( dot( nor, lig ), 0.0, 1.0 );
        float bac = clamp( dot( nor, normalize(float3(-lig.x,0.0,-lig.z))), 0.0, 1.0 )*clamp( 1.0-pos.y,0.0,1.0);
        float fre = pow( clamp(1.0+dot(nor,rd),0.0,1.0), 2.0 );
        
        dif *= softshadow( pos, lig, 0.02, 2.5, time );

		float3 brdf = float(0.0).rrr;
        brdf += 1.20*dif*float(1.00).rrr;
        brdf += 0.30*amb*float(0.50).rrr*occ;
        brdf += 0.30*bac*float(.25).rrr*occ;
        brdf += 1.40*fre*float(1.00).rrr*occ;
		col = col*brdf;
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
    float mu = 1.0;
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

    return float4( col, 1.0 );
}
ENDCG
}
}
}
FallBack "Unlit"
}
