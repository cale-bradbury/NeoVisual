Shader "Camp Cult/SignedDistance/SMinCircle" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_ShapeCount("Shape Count", int) = 3
		_ShapeSize("Shape Size", float) = 0.2
		_Speed("Speed Multiplier", float) = 1.0
		_Shape("Shape", float) = 0.0
		_SMinVal("Soft Min", float) = 0.5
		_Gradient("Gradient", float) = 0.0
	}

		SubShader{
			Pass {
				ZTest Always Cull Off ZWrite Off
				Fog { Mode off }

		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		#pragma target 3.0
		#include "UnityCG.cginc"
		#include "../Rays/ray.cginc"
		#define pi 3.14159265359

		uniform sampler2D _MainTex;	//the screen texture
		uniform float _Shape;
		uniform float _SMinVal;
		uniform float _ShapeSize;
		uniform float _Gradient;
		uniform float _Speed;
		uniform int _ShapeCount;

		uniform float4 _x;			//x/y-max flow distance		z-angle when not radial		w-frame/last frame lerp
		uniform float4 _center;

		float hash( float n )
		{
		    return frac(sin(n)*43758.5453123);
		}

		float noise( in float2 x )
		{
		    float2 p = floor(x);
		    float2 f = frac(x);

		    f = f*f*(3.0-2.0*f);

		    float n = p.x + p.y*157.0;

		    return lerp(lerp( hash(n+  0.0), hash(n+  1.0),f.x),
		               lerp( hash(n+157.0), hash(n+158.0),f.x),f.y);
		}

		float sdCircle(float2 pos, float radius) {
			return length(pos) - radius;
		}

		float sdTri( float2 p, float2 h )
		{
		    float2 q = abs(p);
  			return max(q.x*0.866025+p.y*0.5,-p.y)-h.x*0.5;
		}

		float shape(float2 p) {
			float radius = _ShapeSize;
			return lerp(sdTri(p, radius), sdCircle(p, radius), _Shape);
		}

		float2 rotate(float2 v, float angle){
			float t = atan2(v.y,v.x)+angle;
		    float d = length(v);
		    v.x = cos(t)*d;
		    v.y = sin(t)*d;
		    return v;
		}

		float displacement(float seed) {
			return (noise(seed)*2.0-1.0) * _Speed;
		}

		float map(float2 p) {
			float2 fpos = rotate(p, -_Time.x);
			float2 seed = float2(_Time.x, -_Time.x);
			fpos.x += displacement(seed);
			fpos.y += displacement(seed);

			float res = shape(fpos);

			for (int i = 1; i < _ShapeCount; i++) {

				float2 spos = rotate(p, _Time.x + i*20.0);

				spos.x += displacement(seed);
				spos.y += displacement(seed);

				float sd = shape(spos);

				res = smin(res, sd, _SMinVal);
			}

			return res;
		}

		float4 frag(v2f_img i) : COLOR
		{
			float2 uv = i.uv;
			float2 p = -1.0 + 2.0 * uv;
			p.x *= _ScreenParams.x / _ScreenParams.y;
			float4 c = tex2D(_MainTex,uv);

			if (_ShapeSize <= 0.0) {
				return c;
			}

			float res = map(p);

			if (res < 0.0) {
				c = lerp(c, 1.0-c, min(abs(res/_Gradient), 1.0));
			}
			return c;
		}
		ENDCG

			}
		}

			Fallback off
}