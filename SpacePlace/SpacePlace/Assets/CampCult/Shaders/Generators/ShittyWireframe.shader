// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Camp Cult/Generators/Shitty Wireframe" {
    Properties {
	    _Color("Color",Color)=(1.0,1.0,1.0,1.0)
	    _FaceColor("FaceColor",Color)=(0.0,0.0,0.0,0.0)
	 	_EdgeSize("EdgeSize",Range (0.0, 1.0))=0.1
	 	_FogColor("FogColor",Color) = (0.0,0.0,0.0,1.0)
	 	_FogEnd("FogEnd",float)=100.0
    }
    Category{
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {"Queue" = "Transparent"}
	    SubShader {
	        Pass {   
	    		//Lighting Off  
	    		Fog {
	    		
		     	}     
	    		Cull Off  
	            CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#include "UnityCG.cginc"
				float4 _Color;
				float4 _FaceColor;
				float _EdgeSize;
				float4 _FogColor;
				float _FogEnd;
				
				struct appdata {
					float4 vertex : POSITION;
					float4 tex:TEXCOORD0;
				};
				struct v2f {
					float4 pos : POSITION;	
					float4 vert: TEXCOORD0;	
					float fog:TEXCOORD1;
					//float4 world;	
				};
				v2f vert(appdata v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.vert = v.tex;//v.vertex;
					float4 world = mul(unity_ObjectToWorld, v.vertex);
					o.fog = world.z/_FogEnd;
					return o;
				};
				half4 frag(v2f i) : COLOR {
				float s = min(i.vert.x,min(i.vert.y,min(1.0-i.vert.x,1.0-i.vert.y)));
					return lerp(lerp(_Color,_FaceColor,smoothstep(0.0,_EdgeSize,s)), _FogColor,i.fog);
				};
				ENDCG
	        }
	    }
    }
    Fallback "VertexLit"
}