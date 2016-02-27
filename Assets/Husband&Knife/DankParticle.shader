Shader "Unlit/DankParticle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Noise ("Noise", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Phase("Phase", Vector) = (0,0,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		
		ZWrite Off
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha 
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
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _Noise;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _Phase;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv).rgbr;
				col.a *= tex2D(_Noise,i.uv+_Phase.xy).r*3;
				col.a *= tex2D(_Noise,i.uv*2.+_Phase.zw).r*2;
				col.a *= tex2D(_Noise,i.uv*.5+_Phase.yz*1.1).r*2.;
				_Color.a = col.a;
				return _Color;
			}
			ENDCG
		}
	}
}
