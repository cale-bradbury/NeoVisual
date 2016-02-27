Shader "Camp Cult/Color/Gradient" {
	Properties {
		_color1 ("Color 1", Color) = (1,.5,.5,1)
		_color2 ("Color 2", Color) = (.5,1,1,1)
		_Shape ("X-Angle Y-Freq Z-Phase W-WrapMode",Vector) = (0,1,0,0)
		_MainTex ("Main Texture - Multiply", 2D) = "white"{}
	}
	Category{
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaTest Greater 0
		Tags {"Queue"="Transparent"}
		SubShader {
			Pass{
			
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				 #pragma target 3.0
				#include "UnityCG.cginc"
				#define pi 3.14159

				uniform float4 _color1;
				uniform float4 _color2;	
				uniform float4 _Shape;
				uniform sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				
				float4 frag(v2f_img i) : COLOR {
					float2 uv = i.uv-.5;
					float a = atan2(uv.y,uv.x)+_Shape.x*pi;
					float d = length(uv);
					
					float f = sin(a)*d;
					f*=_Shape.y;
					f+=_Shape.z+.5;
					f = max(min(f,1.),0.);
					uv = i.uv.xy*_MainTex_ST.xy+_MainTex_ST.zw;
					if(_Shape.w<1.){
					
					}else if(_Shape.w<2.){
						uv = fmod(abs(uv),1.0);
					}else if(_Shape.w<3.){
						uv = abs(fmod(uv,2.0)-1.0);
					}
					return tex2D(_MainTex,uv)*lerp(_color1,_color2,f);
				};

				ENDCG
			}
		}
	} 
	FallBack "Diffuse"
}
