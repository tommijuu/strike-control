Shader "Outlined/Outline-Transparent" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0.0, 0.03)) = .005
	}

		CGINCLUDE
#include "UnityCG.cginc"

			struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f {
			float4 pos : POSITION;
			float4 color : COLOR;
		};

		uniform float _Outline;
		uniform float4 _OutlineColor;
		uniform float _Angle;

		v2f vert(appdata v) {
			appdata original = v;

			float3 scaleDir = normalize(v.vertex.xyz - float4(0, 0, 0, 1));
			if (degrees(acos(dot(scaleDir.xyz, v.normal.xyz))) > _Angle) {
				v.vertex.xyz += normalize(v.normal.xyz) * _Outline;
			}
			else {
				v.vertex.xyz += scaleDir * _Outline;
			}

			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.color = _OutlineColor;
			return o;
		}

		ENDCG

			SubShader{
				Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}

				/*
				Pass {
					Name "BASE"
					Cull Back
					Blend Zero One


				Offset - 8, -8

				SetTexture[_OutlineColor] {
					ConstantColor(0,0,0,0)
					Combine constant
				}
				}
				*/

				Pass {
					ColorMask 0
				}

				Pass {
					Name "OUTLINE"
					Tags { "LightMode" = "Always" }
					Cull Front


				Blend SrcAlpha OneMinusSrcAlpha


				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				half4 frag(v2f i) :COLOR {
					return i.color;
				}
				ENDCG
				}

				Pass {
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha
				ColorMask RGB
				Material {
					Diffuse[_Color]
					Ambient[_Color]
				}
				Lighting On

			}

		}

			Fallback "Diffuse"
}