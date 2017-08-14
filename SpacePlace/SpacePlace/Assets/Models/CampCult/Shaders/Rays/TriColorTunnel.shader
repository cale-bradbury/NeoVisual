Shader "Camp Cult/Rays/Tri Color Tunnel" {
Properties {
	//_OffsetTex("Offset Tex",2D) = "black" {}
	_Color1 ("Color 1",Color) = (0,0,0,1)
	_Color2 ("Color 2",Color) = (1,0,0,1)
	_Color3 ("Color 3",Color) = (1,1,1,1)
	_Fog("Fog",Float) = 10
	_Camera ("Camera Position",Vector) = (0,0,0,0)
	_CameraAngle ("Camera Angle",Vector) = (0,0,1,0)
	_Cut ("X-Cut Distance, Y-Cut Thickness, Z-Phase",Vector) = (.05,.04,1,0)
	_Shape ("",Vector) = (1,1,1,1)
	_Box ("Box",Vector) = (.1,.1,.1,1)
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
uniform float4 _Cut;
uniform sampler2D _OffsetTex;
uniform float4 _OffsetTex_ST;

uniform float4 _Camera;
uniform float4 _CameraAngle;
uniform float4 _Shape;
uniform float4 _Box;

float2 map( in float3 pos, float t )
{
   float2 res = float2(1000.,0.);//float2( pos.y+.2+sin(pos.x*sin(1.+pos.x*4.+depth*3.)+cos(depth*2.+_Time.y*3.141*2.0)+_Time.y*3.141*2.0)*.03, 0. );
	float depth = pos.z;
	
	float a = atan2(pos.y,pos.x)/pi;
	a = a+(pos.z-_Camera.z)*_Cut.x;
    float d = length(pos.xy);
   	//d += d*_Camera.w*tex2Dlod (_OffsetTex,float4(abs(fmod(abs((a)*_OffsetTex_ST.x+_OffsetTex_ST.z),2.0)-1.0),.5,2.,2.));
    a*=pi;
    pos.y = sin(a)*d;
    pos.x = cos(a)*d;
    
   	pos.z = fmod(pos.z,1.0)-.5;
	float3 p = pos;
	
    float phase = (depth-t)*1.0-.7;
   // float s = sdTorus(p.xzy,float2(.5,.1));
   float s = sdBox(p.xyz,float3(2.,2.,100.));
    s = opS(s,sdCylinder(p.yzx,float2(.4,100.)));
    
    //p.xy = opSpin(p.xy,(length(p.xy))*4.-phase);
    
    
    p.xy = opKale(p.xy, pi/3,pi*1.33333,pi*.5);
    //p.y+=_Shape.w;
   // p.y*=sin(phase)*_Shape.x+_Shape.y;
    //s = smin(s,sdSphere(p,.1),.1);
    
    //gif1
    //s = smin(s,sdCone(p,float3(_Shape.xyz)),_Shape.w);
    float c = sdSphere(p,_Cut.w);
    float f = fmod((depth-t)*.1-.2,1.0);
    p.y -= sin(f*pi*2.+t*pi*2.)*_Shape.x-_Shape.y;
    //float a = _Cut.w;
    /*p.xyz = mul(p.xyz,float3x3(1.0,sin(_Cut.w), -sin(_Box.w), 
    						-sin(_Cut.w),1.0, sin(_Shape.w),
    						sin(_Box.w), -sin(_Shape.w), 1.0));*/
    p.xy = opSpin(p.xy,_Cut.w);
    p.zy = opSpin(p.zy,_Shape.w);
    p.zx = opSpin(p.zx,_Box.w);
    c = sdBox(p,float3(_Box.xyz));
    s = smin(s,c, _CameraAngle.w);
    
    //cuting
    //p.xyz = pos.xyz;
    //a = atan2(p.y,p.x);
    //float d = fmod(length(p.xy)+_Cut.z,_Cut.x);
    //p.y = sin(a)*d;
    //p.x = cos(a)*d;
    //s = opS(s,(sdTube(p.yzx,float3(_Cut.x,_Cut.x-_Cut.y,100.))));
    
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

	float precis = 0.000001;
    float t = tmin;
    float m = -1.0;
    for( int i=0; i<50; i++ )
    {
	    float2 res = map( ro+rd*t, time );
        if(t>tmax ) break;
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
    for( int i=0; i<68; i++ )
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
	float3 eps = float3( 0.01, 0.00, 0.00 );
	float3 nor = float3(
	    map(pos+eps.xyz, time).x - map(pos-eps.xyz, time).x,
	    map(pos+eps.zxy, time).x - map(pos-eps.zxy, time).x,
	    map(pos+eps.zyx, time).x - map(pos-eps.zyx, time).x );
	return normalize(nor);
}

float calcAO( in float3 pos, in float3 nor, in float time )
{
	float oCamp = 0.0;
    float sca = 0.0;//1.0;
    for( int i=0; i<1; i++ )
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
    float3 col = float3(0.8, 0.9, 1.0);
    float2 res = castRay(ro,rd, time);
    float t = res.x;
	float m = res.y;
    if( m>-0.5 )
    {
        float3 pos = ro + t*rd;
        float3 nor = calcNormal( pos, time);
        float3 ref = reflect( rd, nor );
        		

        // lighitng        
        float oCamp = calcAO( pos, nor , time);
		float3  lig = normalize( float3(1.0,1.0,1.0) );
		float amb = clamp( 1.0-0.5*nor.z, 0.0, 1.0 );
        float dif = clamp( dot( nor, lig ), 0.0, 1.0 );
        float bac = clamp( dot( nor, normalize(float3(-lig.x,0.0,-lig.z))), 0.0, 1.0 )*clamp( 1.0-pos.y,0.0,1.0);
        float fre = pow( clamp(1.0+dot(nor,rd),0.0,1.0), 1.0 );
        
        dif *= softshadow( pos, lig, 0.02, 2.5, time );

		float3 brdf = float(0.0).rrr;
        brdf += 1.20*dif*float(1.00).rrr;
        brdf += 0.30*amb*float(0.50).rrr*oCamp;
        brdf += 0.30*bac*float(.25).rrr*oCamp;
        brdf += 1.40*fre*float(1.00).rrr*oCamp;
		col = brdf;
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
	float3 rd = mul( normalize( float3(p.xy,2.5) ),ca);
    
    // render	
    float3 col = float3(0.0,0.0,0.0);
    
    float s = .002;
    for(float i = 0.;i<count;i++){
     col += render( ro, rd, time-s*i );
    }
	col/=count;
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
