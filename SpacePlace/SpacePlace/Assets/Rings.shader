Shader "Unlit/Rings"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_ColorMin("Color", Color) = (1,1,1,1)
		_ColorMax("Color", Color) = (1,1,0,1)
		_Shape("Shape x/y ring min/max", Vector) = (0,1,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
			ZWrite off
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _AudioTex;
			float4 _Shape;
			float4 _ColorMax;
			float4 _ColorMin;
			
			v2f vert (appdata v)
			{
				v2f o;
				float f = (1. - abs(v.uv.x - .5)*2.);
				v.vertex.z += _Shape.w*pow(tex2Dlod(_AudioTex, float4(1.0-v.uv.y, f,1.,1. )).r, 2.);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = v.uv;
				o.uv.z = f;
				o.uv.w = 0.;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float f = tex2D(_AudioTex, float2((1. - i.uv.y)*.5, 0)).r;
			float g = tex2D(_AudioTex, float2((1. - i.uv.y)*.25, i.uv.z)).r;
				float4 col = lerp(_ColorMin, _ColorMax, g);
				UNITY_APPLY_FOG(i.fogCoord, col);
				col.a *= min(1.,f*2.);
				col.a *= smoothstep(0., .3, i.uv.y);
				return col;
			}
			ENDCG
		}
	}
}
