// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "GUI/Blend/Invert" 
{
	Properties { _MainTex ("Texture", 2D) = "white" {} } 

	SubShader {

		Tags {  "RenderType"="Overlay" "Queue" = "Transparent" }
		
		Pass
        {
           //ZWrite On
           //ColorMask 0
        }
        Lighting Off 
		Blend   OneMinusDstAlpha Zero
		Cull Off 
		ZWrite On 
		Fog { Mode Off } 
		ZTest Always 
		
		Pass {	
		
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;

			uniform float4 _MainTex_ST;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float4 c = tex2D(_MainTex, i.texcoord) ;//* i.color;
				return  c;
			}
			ENDCG 
		}
	} 
	
	Fallback off 
}
