// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Camp Cult/Generators/Ring" {
	Properties {
		_Color("Color",Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Shape("Shape X-start angle, Y-null, Z-start rad, W-end rad",Vector)=(0,0,0,1)
		_Value("Value 0-2pi",Float) = 0
	}
	
	Category{
		Blend SrcAlpha OneMinusSrcAlpha
		//ZTest Always
		Tags {"Queue" = "Overlay" "RenderType"="Transparent"}
		SubShader {
		Pass{
			CGPROGRAM
			#pragma vertex vert
	        #pragma fragment frag
			#pragma target 3.0
	        #include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Shape;
			float _Value;
			float4 _Color;
			
			struct appdata
	        {
	                float4 vertex : POSITION;
	                float2 texcoord : TEXCOORD0;
	        };

	        struct v2f
	        {
	                float4 vertex : SV_POSITION;
	                half2 uv : TEXCOORD0;
	        };
			
	        v2f vert (appdata v)
	        {
	                v2f o;
	                o.vertex = UnityObjectToClipPos(v.vertex);
	                o.uv = v.texcoord;
	                return o;
	        };
			
			float4 frag(v2f i):COLOR{
				const float TAU = 6.2831853;
				float dist = distance(i.uv,float2(.5,.5));
				float angle = fmod(atan2(i.uv.y-.5, i.uv.x-.5)+TAU,TAU)+TAU;
				float val = 0;
				float2 r = _Shape.zw*.1;
				float s = fmod(_Shape.x,TAU)+TAU;
				if(
					dist<r.x+r.y&&
					dist>r.x-r.y&&
					((angle<s+_Value && angle>s)||
					(angle+TAU<s+_Value && angle+TAU>s)))val = 1;
				
				return val*tex2D (_MainTex, float2((angle*_MainTex_ST.x/TAU+_MainTex_ST.z),(dist*_MainTex_ST.y+_MainTex_ST.w)))*_Color;
			};
			ENDCG
		}
		} 
	}
	FallBack "Diffuse"
}
