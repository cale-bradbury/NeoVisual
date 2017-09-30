
float3 darken(float3 s, float3 d)
{
	return min(s, d);
}

float3 multiply(float3 s, float3 d)
{
	return s*d;
}

float3 colorBurn(float3 s, float3 d)
{
	return 1.0 - (1.0 - d) / s;
}

float3 linearBurn(float3 s, float3 d)
{
	return s + d - 1.0;
}

float3 darkerColor(float3 s, float3 d)
{
	return (s.x + s.y + s.z < d.x + d.y + d.z) ? s : d;
}

float3 lighten(float3 s, float3 d)
{
	return max(s, d);
}

float3 screen(float3 s, float3 d)
{
	return s + d - s * d;
}

float3 colorDodge(float3 s, float3 d)
{
	return d / (1.0 - s);
}

float3 linearDodge(float3 s, float3 d)
{
	return s + d;
}

float3 lighterColor(float3 s, float3 d)
{
	return (s.x + s.y + s.z > d.x + d.y + d.z) ? s : d;
}

float overlay(float s, float d)
{
	return (d < 0.5) ? 2.0 * s * d : 1.0 - 2.0 * (1.0 - s) * (1.0 - d);
}

float3 overlay(float3 s, float3 d)
{
	float3 c;
	c.x = overlay(s.x, d.x);
	c.y = overlay(s.y, d.y);
	c.z = overlay(s.z, d.z);
	return c;
}

float softLight(float s, float d)
{
	return (s < 0.5) ? d - (1.0 - 2.0 * s) * d * (1.0 - d)
		: (d < 0.25) ? d + (2.0 * s - 1.0) * d * ((16.0 * d - 12.0) * d + 3.0)
		: d + (2.0 * s - 1.0) * (sqrt(d) - d);
}

float3 softLight(float3 s, float3 d)
{
	float3 c;
	c.x = softLight(s.x, d.x);
	c.y = softLight(s.y, d.y);
	c.z = softLight(s.z, d.z);
	return c;
}

float hardLight(float s, float d)
{
	return (s < 0.5) ? 2.0 * s * d : 1.0 - 2.0 * (1.0 - s) * (1.0 - d);
}

float3 hardLight(float3 s, float3 d)
{
	float3 c;
	c.x = hardLight(s.x, d.x);
	c.y = hardLight(s.y, d.y);
	c.z = hardLight(s.z, d.z);
	return c;
}

float vividLight(float s, float d)
{
	return (s < 0.5) ? 1.0 - (1.0 - d) / (2.0 * s) : d / (2.0 * (1.0 - s));
}

float3 vividLight(float3 s, float3 d)
{
	float3 c;
	c.x = vividLight(s.x, d.x);
	c.y = vividLight(s.y, d.y);
	c.z = vividLight(s.z, d.z);
	return c;
}

float3 linearLight(float3 s, float3 d)
{
	return 2.0 * s + d - 1.0;
}

float pinLight(float s, float d)
{
	return (2.0 * s - 1.0 > d) ? 2.0 * s - 1.0 : (s < 0.5 * d) ? 2.0 * s : d;
}

float3 pinLight(float3 s, float3 d)
{
	float3 c;
	c.x = pinLight(s.x, d.x);
	c.y = pinLight(s.y, d.y);
	c.z = pinLight(s.z, d.z);
	return c;
}

float3 hardMix(float3 s, float3 d)
{
	return floor(s + d);
}

float3 difference(float3 s, float3 d)
{
	return abs(d - s);
}

float3 exclusion(float3 s, float3 d)
{
	return s + d - 2.0 * s * d;
}

float3 subtract(float3 s, float3 d)
{
	return frac(s - d);
}

float3 divide(float3 s, float3 d)
{
	return s / d;
}

//	rgb<-->hsv functions by Sam Hocevar
//	http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
float3 rgb2hsv(float3 c)
{
	float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
	float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

float3 hsv2rgb(float3 c)
{
	float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float3 hue(float3 s, float3 d)
{
	d = rgb2hsv(d);
	d.x = rgb2hsv(s).x;
	return hsv2rgb(d);
}

float3 color(float3 s, float3 d)
{
	s = rgb2hsv(s);
	s.z = rgb2hsv(d).z;
	return hsv2rgb(s);
}

float3 saturation(float3 s, float3 d)
{
	d = rgb2hsv(d);
	d.y = rgb2hsv(s).y;
	return hsv2rgb(d);
}

float3 luminosity(float3 s, float3 d)
{
	float dLum = dot(d, float3(0.3, 0.59, 0.11));
	float sLum = dot(s, float3(0.3, 0.59, 0.11));
	float lum = sLum - dLum;
	float3 c = d + lum;
	float minC = min(min(c.x, c.y), c.z);
	float maxC = max(max(c.x, c.y), c.z);
	if (minC < 0.0) return sLum + ((c - sLum) * sLum) / (sLum - minC);
	else if (maxC > 1.0) return sLum + ((c - sLum) * (1.0 - sLum)) / (maxC - sLum);
	else return c;
}