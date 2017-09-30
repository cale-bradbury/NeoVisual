// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Text/Gradient" { 
Properties {
		_color1 ("Grad 1 Color 1", Color) = (1,.5,.5,1)
		_color2 ("Grad 1 Color 2", Color) = (.5,1,1,1)
		_range("range",Vector)=(0,0,0,0)
		_MainTex ("SelfIllum Color (RGB) Alpha (A)", 2D) = "white"{}
	}
	Category{
		SubShader {
			Tags { "Queue"="Transparent" "RenderType"="Transparent" } 
		   	Lighting Off Cull Off ZWrite Off Fog { Mode Off } 
		   	Blend SrcAlpha OneMinusSrcAlpha 
			Pass{
			
				CGPROGRAM
				// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members uv_MainTex)
				//#pragma exclude_renderers d3d11 xbox360
				#pragma vertex vert
				#pragma fragment frag

				float4 _color1;
				float4 _color2;
				float2 _range;	
				sampler2D _MainTex;
				
				struct appdata {
					float4 vertex : POSITION;
					float4 tex : TEXCOORD0;
				};
				struct v2f {
					float4 uv : TEXCOORD;
					float4 pos : POSITION;
				};
				v2f vert(appdata v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv.xy = v.tex.xy;
					o.uv.z = v.vertex.y*_range.y+_range.x;
					return o;
				};
				
				half4 frag(v2f i) : COLOR {
					float4 c = tex2D(_MainTex,i.uv.xy);
					c.rgb = lerp(_color1,_color2,i.uv.z).rgb;
					return c;
				};

				ENDCG
			}
		}
	} 
	Fallback "Text/Default"
}