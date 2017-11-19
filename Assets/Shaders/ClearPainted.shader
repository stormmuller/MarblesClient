// Per pixel bumped refraction.
// Uses a normal map to distort the image behind, and
// an additional texture to tint the color.

Shader "Custom/Marbles/ClearPainted" {
	Properties{
		_BumpAmt("Distortion", range(0,128)) = 10
		_RimColor("RimColor", Color) = (1, 1, 1, 1)
		_RimStrength("RimStrength", Float) = 0.5
		_MainTex("Tint Color (RGB)", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
		_PaintTex("Paint Map", 2D) = "white" {}
		_PaintBumpAmt("Paint Bump Amount", range(0,1)) = 10
	}

		Category{

		// We must be transparent, so other objects are drawn before this one.
		Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }


		SubShader {

		// This pass grabs the screen behind the object into a texture.
		// We can access the result in the next pass as _GrabTexture
		GrabPass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
		}

		// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
		// on to the screen
		Pass {
			Name "BASE"
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord: TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

	struct v2f {
		float4 vertex : SV_POSITION;
		float4 uvgrab : TEXCOORD0;
		float2 uvbump : TEXCOORD1;
		float2 uvmain : TEXCOORD2;
		float2 uvPaint : TEXCOORD3;
		float4 rimColor : COLOR;
		UNITY_FOG_COORDS(3)
	};

			uniform float _BumpAmt;
			uniform float _PaintBumpAmt;
			uniform float4 _BumpMap_ST;
			uniform float4 _MainTex_ST;
			uniform float4 _PaintTex_ST;
			uniform float4 _RimColor;
			uniform float _RimStrength;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uvgrab = ComputeGrabScreenPos(o.vertex);
				o.uvbump = TRANSFORM_TEX(v.texcoord, _BumpMap);
				o.uvmain = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uvPaint = TRANSFORM_TEX(v.texcoord, _PaintTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				// RimLighting code
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				float dotProduct = 1 - dot(v.normal, viewDir);

				o.rimColor = smoothstep(1 - _RimStrength, 1.0, dotProduct);

				o.rimColor *= _RimColor;

				return o;
			}

			uniform sampler2D _GrabTexture;
			uniform float4 _GrabTexture_TexelSize;
			uniform sampler2D _BumpMap;
			uniform sampler2D _MainTex;
			uniform sampler2D _PaintTex;
			uniform float4 _PaintTex_TexelSize;

			fixed4 frag(v2f i) : SV_Target
			{
				#if UNITY_SINGLE_PASS_STEREO
				i.uvgrab.xy = TransformStereoScreenSpaceTex(i.uvgrab.xy, i.uvgrab.w);
				#endif

				// calculate perturbed coordinates
				half2 bump = UnpackNormal(tex2D(_BumpMap, i.uvbump)).rg; // we could optimize this by just reading the x & y without reconstructing the Z
				float2 grabOffset = bump * _BumpAmt * _GrabTexture_TexelSize.xy;
				#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE //to handle recent standard asset package on older version of unity (before 5.5)
					i.uvgrab.xy = grabOffset * UNITY_Z_0_FAR_FROM_CLIPSPACE(i.uvgrab.z) + i.uvgrab.xy;
				#else
					i.uvgrab.xy = grabOffset * i.uvgrab.z + i.uvgrab.xy;
				#endif

				half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				half4 tint = tex2D(_MainTex, i.uvmain);
				half4 paint = tex2D(_PaintTex, i.uvPaint);
				col *= tint;

				col = lerp(col.rgba, paint.rgba, paint.a);
				col = lerp(col.rgba, i.rimColor.rgba, i.rimColor.a);

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
		ENDCG
	}
}

// ------------------------------------------------------------------
// Fallback for older cards and Unity non-Pro

SubShader {
	Blend DstColor Zero
	Pass {
		Name "BASE"
		SetTexture[_MainTex] {	combine texture }
	}
}
	}
}
