Shader "Unlit/Ping Pong" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	Category{
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaTest Greater 0
		Tags {"Queue"="Transparent"}
		SubShader {
			Pass{
				CGPROGRAM
				#include "UnityCG.cginc"
				#pragma vertex vert_img
				#pragma fragment frag
				sampler2D _MainTex;
				float4 _MainTex_ST;

				fixed4 frag (v2f_img i) : COLOR{
					return tex2D(_MainTex,abs(fmod(abs(i.uv*_MainTex_ST.xy+_MainTex_ST.zw),2.0)-1.0));
				}
				ENDCG
			}
		}
	} 
	FallBack "Unlit"
}
