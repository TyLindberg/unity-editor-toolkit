Shader "Unity Editor Toolkit/Depth to Color" {
	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass {
			CGPROGRAM

			#pragma target 3.5
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float4 frag(v2f i) : SV_Target {
				float depth = i.pos.z;
				#if (SHADER_API_D3D11 || SHADER_API_METAL)
				depth = 1 - depth;
				#endif
				return float4(depth, depth, depth, 1);
			}
			ENDCG
		}
	}
	Fallback Off
}
