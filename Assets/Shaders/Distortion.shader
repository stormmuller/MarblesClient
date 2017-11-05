Shader "Unlit/Aiming Shader"
{
	Properties
	{
		_TintCol("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_DistortionTex("Distortion Texture", 2D) = "white" {}
		_BumpAmt ("Distortion Amount", range(0,128)) = 0.5
		_BumpOffsetx("Distortion Direction x", range(-1,1)) = 0.5
		_BumpOffsety("Distortion Direction y", range(-1,1)) = 0.5
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 mainUv : TEXCOORD0;
				float2 distortionUv : TEXCOORD1;
			};

			struct v2f
			{
				float2 mainUv : TEXCOORD0;
				float2 distortionUv : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _DistortionTex;
			uniform float4 _DistortionTex_TexelSize;
			float4 _MainTex_ST;
			float4 _DistortionTex_ST;
			float4 _TintCol;
			float _BumpAmt;
			float _BumpOffsetx;
			float _BumpOffsety;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.mainUv = TRANSFORM_TEX(v.mainUv, _MainTex);
				o.distortionUv = TRANSFORM_TEX(v.distortionUv, _DistortionTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half2 bump = UnpackNormal(tex2D(_DistortionTex, i.distortionUv.xy + fixed2(_BumpOffsetx, _BumpOffsety))).rg; // we could optimize this by just reading the x & y without reconstructing the Z
				float2 grabOffset = bump * _BumpAmt * _DistortionTex_TexelSize.xy;

				i.mainUv.xy = grabOffset + i.mainUv.xy;

				fixed4 col = tex2D(_MainTex, i.mainUv);

				col *= _TintCol;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG

			ZWrite Off
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
		}
	}
}
