// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Projector/ViewCone" {
	Properties {		
		_Angle ("Angle", Float) = 30
		_MaxHeight ("MaxHeight", Float) = 3
		_Color ("Color", Color) = (0,0,0,0)
	}
	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZWrite Off
			ColorMask RGB
			Blend DstColor Zero
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uvShadow : TEXCOORD0;			
				UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
				float ignore : COLOR;								
			};
			
			float4x4 unity_Projector;
			float _Angle;
			float _MaxHeight;
			float4 _Color;
						
			v2f vert (appdata_base v)
			{
				v2f o;				
				o.pos = UnityObjectToClipPos (v.vertex);							
				o.uvShadow = mul (unity_Projector, v.vertex);				
				UNITY_TRANSFER_FOG(o,o.pos);
				
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);								
				o.ignore = (v.normal.y <= 0) || (worldPos.y > _MaxHeight);
				return o;
			}
			
			inline float angleBetween(fixed2 vector1, fixed2 vector2) {
			     return acos(dot(vector1, vector2)/(length(vector1)*length(vector2)));
			 }
			
			fixed4 frag (v2f i) : SV_Target
			{				
				fixed4 res = fixed4(1,0,0,0);
				
				UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(1,1,1,1));				
				
				const float PI = 3.14159;
				fixed2 forward = fixed2(0,0.5);			
				fixed2 target = i.uvShadow - fixed2(0.5,0.5);
				float angle = angleBetween(forward,target);				
				float targetAngle = _Angle * PI / 180;
				if(i.ignore || angle > targetAngle || angle < -targetAngle) { 					
					return fixed4(1,1,1,1);
				}				
				
				float2 center = fixed2(0.5,0.5);
				float dist = distance(center,fixed2(i.uvShadow.x,i.uvShadow.y));				
				if(dist > 0.5){
					return fixed4(1,1,1,1);
				}
				
				return _Color;
			}		
			
			
			ENDCG
		}
	}
}
