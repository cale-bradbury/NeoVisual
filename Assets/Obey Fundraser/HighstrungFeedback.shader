Shader "Hidden/HighstrungFeedback"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

#define pi 3.1415
			sampler2D _MainTex;
			sampler2D _Audio;
			sampler2D _Feed;
			float2 _Center;
			float4 shape;
			float4 darken;
			float4 _MainTex_TexelSize;

			float4 _Freq;
			float _Amp;
			uniform float _Step = .0125;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv-.5;
				o.uv /= float2(1., 1+ _MainTex_TexelSize.x);
				o.uv += .5;
				return o;
			}

			float mic(float x, float y) {
				return tex2D(_Audio, abs(fmod(float2(x,y)+1, 2.) - 1.)).r;
			}


			fixed4 frag (v2f i) : SV_Target
			{

				float2 o = float2(0., sin(i.uv.x*3. + _Time.y)*.05);
				float2 uv = i.uv ;

#if UNITY_UV_STARTS_AT_TOP
				uv.y = 1.0 - uv.y;
#endif
				uv -= _Center;
				float d = length(uv);
				float a = atan2(uv.x,uv.y);
				a += sin(d*4. + _Time.y)*.001;
				d *= (sin(uv.x + uv.y + _Time.y)*.001 + .999);

				float m = mic(abs(a / 1.5707)+_Time.x, d*.5-.05 ) * darken.w;
				d -= d*_Amp;// m*d;
				uv.y = cos(a);
				uv.x = sin(a);
				uv *= d;
				uv += _Center;

				float4 c = tex2D(_MainTex,uv );
				float3 t = c.bbb* sin((d*5. + float3(0., .333,.666))*6.28 - fmod(_Time.y, 6.28))*.5 + .5;
				for (int j = 0; j < 10; j++) {
					float2 freq = _Freq.xy + sin(uv + a + _Time.y) * 10;
					float2 amp = _Freq.zw;
					float phase = m+shape.x;
					for (float i = 1.0; i > 0.0; i -= max(_Step, .1)) {
						uv -= _Center;
						uv += float2(cos(uv.x*freq.x + uv.y*freq.y  + phase), cos(uv.y*freq.x + uv.x*freq.y + phase))*amp*(1.00 - i);
						uv += _Center;
						phase += shape.w;// +sin(_Time.x + a)*.01;//5707;
					}
				}
				float4 f = tex2D(_Feed,uv);
				//f.b = 1.0-f.b;
				f.rgb -= darken.xxx;
				//f.rgb = 1.0-mod(1.0-f.rgb, 1.);
				f.rgb = c.bbb + (1.0 - length(c.ggg))*lerp(f.rgb,t,(c.b));
				//f.rgb = normalize(f.rgb);
				return  f;// m / darken.w;
			}
			ENDCG
		}
	}
}
