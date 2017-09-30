Shader "Unlit/SimpleDisplacement"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DisplacementTex("Texture", 2D) = "grey" {}
		_Strength("Strength", Vector) = (1,1,0,0)
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

			sampler2D _MainTex;
		sampler2D _DisplacementTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float4 _Strength;
						
			float4 frag (v2f_img i) : SV_Target
			{
				// sample the texture
				float4 c = tex2D(_DisplacementTex, i.uv);
				i.uv += (c.xy*2. - 1.)*_Strength.xy*_MainTex_TexelSize.xy;
				float4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
