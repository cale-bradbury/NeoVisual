Shader "Unlit/DankParticle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Noise ("Noise", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Phase("Phase", Vector) = (0,0,0,0)
		_Soft("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		Cull off
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
				float4 projPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _Noise;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _Phase;
			float _Soft;
			sampler2D_float _CameraDepthTexture;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.projPos = ComputeScreenPos(o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//soft particles
				float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float fade = saturate(_Soft * (sceneZ - i.projPos.z));
				fade = min(fade, saturate(.3*i.projPos.z));

				fixed4 col = tex2D(_MainTex, i.uv).rgbr;
				col.a *= tex2D(_Noise,i.uv+_Phase.xy).r;
				col.a *= tex2D(_Noise,i.uv*2.+_Phase.zw).r;
				col.a *= tex2D(_Noise,i.uv*.5+_Phase.yz*1.1).r;
				_Color.a = col.a *fade;
				return _Color;
			}
			ENDCG
		}
	}
}
