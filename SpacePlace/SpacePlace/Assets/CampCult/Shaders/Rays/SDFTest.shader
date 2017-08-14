Shader "Unlit/SDFTest"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Camera("Camera Position W-FOV",Vector) = (0,0,0,0)
		_CameraAngle("Camera Angle W-fisheye",Vector) = (0,0,1,0)
		_Fog("X-Fog Y-Steps", Vector) = (100, 100, 0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "mecury.cginc"
			#include "ray.cginc"

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

			sampler2D _MainTex;
			float4 _MainTex_ST;

			uniform float4 _Camera;
			uniform float4 _CameraAngle;	
			uniform float4 _Fog;

			float2 map(in float3 pos, float t)
			{
				float2 res = float2(1000., 0.);
				float a = atan2(pos.y, pos.x);
				float d = floor((pos.z+3.) / 6.)+ floor((pos.y + 3.) / 6.)+ floor((pos.x + 3.)  / 6.);
				pMod3(pos, 6.);
				float s = fVenus(pos, .25);
				res.x = s;
				return res;
			}

			float2 castRay(in float3 ro, in float3 rd, float time)
			{
				float tmin = .001;
				float tmax = _Fog.x;

#if 0
				float tp1 = (0.0 - ro.y) / rd.y; if (tp1>0.0) tmax = min(tmax, tp1);
				float tp2 = (1.6 - ro.y) / rd.y; if (tp2>0.0) {
					if (ro.y>1.6) tmin = max(tmin, tp2);
					else           tmax = min(tmax, tp2);
				}
#endif

				float precis = 0.00001;
				float t = tmin;
				float m = -1.0;
				for (int i = 0; i<int(_Fog.y); i++)
				{
					float2 res = map(ro + rd*t, time);
					if (t>tmax) break;
					t += res.x;
					m = res.y;
				}

				if (t>tmax) m = m;
				return float2(t, m);
			}

			float3 calcNormal(in float3 pos, in float time)
			{
				float3 eps = float3(0.001, 0.00, 0.00);
				float3 nor = float3(
					map(pos + eps.xyz, time).x - map(pos - eps.xyz, time).x,
					map(pos + eps.zxy, time).x - map(pos - eps.zxy, time).x,
					map(pos + eps.zyx, time).x - map(pos - eps.zyx, time).x);
				return normalize(nor);
			}

			float3 render(in float3 ro, in float3 rd, float time)
			{
				float3 col = float3(0.8, 0.9, 1.0);
				float2 res = castRay(ro, rd, time);
				float t = res.x;
				float m = res.y;
				if (m>-0.5)
				{
					float3 pos = ro + t*rd;
					float3 nor = calcNormal(pos, time);

					col = (-nor.zzz)*.5 + .5;
					col.rgb *= min(1.0, pow(max(0., 1.0 - (t / _Fog.x)), 2.));
				}
				//col*=pow(1.0-res.x*.05,1.);
				return col;
			}

			float3x3 setCamera(in float3 ro, in float3 ta, float cr)
			{
				float3 cw = normalize(ta - ro);
				float3 cp = float3(sin(cr), cos(cr), 0.0);
				float3 cu = normalize(cross(cw, cp));
				float3 cv = normalize(cross(cu, cw));
				return float3x3(cu, cv, cw);
			}

			fixed4 frag(v2f_img i) : COLOR
			{
				float2 q = i.uv;
				float2 p = -1.0 + 2.0*q;

				// camera  
				float3 ro = _Camera.xyz;
				float3 ta = _Camera.xyz + _CameraAngle.xyz;

				// camera-to-world transformation
				float3x3 ca = setCamera(ro, ta, 0.0);
				float fov = _Camera.w;
				float rayZ = tan((90.0 - 0.5 * fov) * 0.01745329252);
				float3 rd = mul(normalize(float3(p.xy,rayZ)),ca);
				rd = normalize(rd);

				// render  
				float3 col = render(ro, rd, _Time.y);
				return float4(col, 1.0);
			}
			ENDCG
		}
	}
}
