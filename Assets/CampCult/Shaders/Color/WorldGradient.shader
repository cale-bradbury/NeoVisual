// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Camp Cult/Color/Gradient - World Space Radial" {
	Properties {
		_color1 ("Color 1", Color) = (1,.5,.5,1)
		_color2 ("Color 2", Color) = (.5,1,1,1)
		_Position1("Position1",Vector) = (0,0,0,0)
		_Shape("X-Length Y-Offset, Z-Null W-WrapMode",Vector) = (0,1,0,0)
		_MainTex ("Main Texture - Multiply", 2D) = "white"{}
	}
	Category{
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaTest Greater 0
		Cull Off
		Tags {"Queue"="Transparent"}
		SubShader {
			Pass{
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#define pi 3.14159

				uniform float4 _color1;
				uniform float4 _color2;	
				uniform float4 _Position1;
				uniform float4 _Shape;
				uniform sampler2D _MainTex;
				uniform float4 _MainTex_ST;
				
				struct appdata {
					float4 vertex : POSITION;
					float4 uv : TEXCOORD0;
				};
				struct v2f {
					float2 uv : TEXCOORD;
					float4 pos : POSITION;
					float4 world : COLOR;
				};
				v2f vert(appdata v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.world = mul(unity_ObjectToWorld,v.vertex);
					o.uv = v.uv;
					return o;
				};
				
				float4 frag(v2f i) : COLOR {
					float2 uv = i.uv-.5;
					/*float a = atan2(uv.y,uv.x)+_Shape.x*pi;
					float d = length(uv);
					
					float f = sin(a)*d;
					f*=_Shape.y;
					f+=_Shape.z+.5;
					f = max(min(f,1.),0.);*/
					
					float f = distance(_Position1.xyz, i.world.xyz);
					f /= _Shape.x;
					f+=_Shape.y;
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
