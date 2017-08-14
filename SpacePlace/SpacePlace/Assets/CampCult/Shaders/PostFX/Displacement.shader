Shader "Camp Cult/Displacement/Displacement" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Last("last frame", 2D) = "white" {}
		_Flow("flow control", 2D) = "white" {}
		_x("x-SampleX y-SampleY z-angle w-null",Vector) = (0,0,.1,0)
		_Offset("sample xy min/max", Vector) = (-.1,-.1, .1, .1)
	}

		SubShader{
			Pass {
				ZTest Always Cull Off ZWrite Off
				Fog { Mode off }

		CGPROGRAM
		#pragma multi_compile radial nradial
		#pragma multi_compile invert ninvert
#pragma multi_compile Lerp Add Mul Wtf ColorDodge LighterColor VividLight HardMix Difference Subtract Divide
		#pragma vertex vert_img
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		#pragma target 3.0
		#include "UnityCG.cginc"
		#include "blend.cginc"
		#define pi 3.14159265359

		uniform sampler2D _MainTex;	//the screen texture
		uniform sampler2D _Last;	//the last frames texture
		uniform sampler2D _Flow;	//texture to control distance shited per channel
		uniform float4 _Flow_ST;

		uniform float4 _x;			//x/y-max flow distance		z-angle when not radial		w-frame/last frame lerp
		uniform float4 _Offset;
		uniform float4 _center;

		float4 frag(v2f_img i) : COLOR
		{
			float2 uv = i.uv;
			float4 c = tex2D(_MainTex,uv);
			#ifdef invert
				uv.y = 1.0 - uv.y;
			#endif

			#ifdef radial
				float angle = atan2(uv.y - _center.y,uv.x - _center.x) + pi;
				float an = abs(fmod(angle / pi,1.0) - .5);
				angle += _x.z*pi;
				float d = length(uv.xy - _center.xy);
				float2 u = (float2(an,1.));//d - .5));
				u = abs(fmod(u*_Flow_ST.xy + _Flow_ST.zw, 2.)-1.);
				float4 f = tex2D(_Flow,u);
				float2 t = uv;
				t.x = cos(angle);
				t.y = sin(angle);
				t *= lerp(_Offset.xy, _Offset.zw, f.r) *saturate(d*3.);
			#else
				float4 f = tex2D(_Flow,abs(fmod(uv*_Flow_ST.xy + _Flow_ST.zw, 2.)-1.))*2.-1.;
				float2 t = f.rg;
				t *= f.b;
				t *= lerp(_Offset.xy, _Offset.zw, f.r);
			#endif

				float4 s = tex2D(_Last, uv - t);

#ifdef ColorDodge
			return lerp(c, float4(colorDodge(c, s), 1.), _x.w);
#endif
#ifdef Divide
			return lerp(c, float4(divide(c.rgb, s.rgb), 1.), _x.w);  
#endif
#ifdef Subtract
			return lerp(c, float4(subtract(c.rgb, s.rgb), 1.), _x.w);  
#endif
#ifdef Difference
			return lerp(c, float4(difference(c.rgb, s.rgb), 1.), _x.w);  
#endif
#ifdef HardMix
			return lerp(c, float4(hardMix(c.rgb, s.rgb), 1.), _x.w);  
#endif
#ifdef VividLight
			return lerp(c, float4(vividLight(c.rgb, s.rgb), 1.), _x.w);  
#endif
#ifdef LighterColor
			return lerp(c, float4(lighterColor(c, s), 1.), _x.w);  
#endif
			#ifdef Add
				return max(c,max((sign(-_x.w)*.5 + .5)*((-s-(_x.w))),s - (1.0 - _x.w)));
			#endif
			#ifdef Mul
					return lerp(c, s, dot(c, s)*_x.w);
			#endif
			#ifdef Wtf
					return max(c, _x.w-s);
			#endif
			#ifdef Lerp
				return lerp(c,s,_x.w);
			#endif
			return c;
		}
		ENDCG

			}
		}

			Fallback off
}