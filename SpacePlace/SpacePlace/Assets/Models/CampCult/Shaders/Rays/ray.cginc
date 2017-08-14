//***************//
//OPERATIONS//
//***************//

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

float2 opSpin( float2 p, float ang ){
    float a = atan2(p.y,p.x)+ang;
    float d = length(p);
    return float2(cos(a)*d,sin(a)*d);
}

float2 opKale(float2 p, float a, float b, float s){
	float c = atan2(p.y,p.x)+s;
	float d = length(p);
	c = fmod(c+3.1415,a*2);
	c = abs(c-a)+b;
	p.x = cos(c)*d;
	p.y = sin(c)*d;
	return p;
}

float smin( float a, float b, float k ){
    float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
    return lerp( b, a, h ) - k*h*(1.0-h);
}
/*float smin( float a, float b, float k )
{
    float res = exp( -k*a ) + exp( -k*b );
    return -log( res )/k;
}*/
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

//*********//
//SHAPES//
//**********//

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

float sdCross(float3 pos, float a, float b){
	return opU(
		sdBox(pos, float3(a,a,b)), opU(
		sdBox(pos, float3(a,b,a)),
		sdBox(pos, float3(b,a,a))
	));
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

float sdTube( float3 p, float3 h )
{
  return opS(sdCylinder(p,h.xz),sdCylinder(p,h.yz));
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
