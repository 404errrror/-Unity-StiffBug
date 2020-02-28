Shader "Hidden/TintShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
			Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag

	#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float4 color : COLOR;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
			float4 color : COLOR;
		};


		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			o.color = v.color;
			return o;
		}

		sampler2D _MainTex;

		fixed4 frag(v2f i) : SV_Target
		{
	

		float4 color = tex2D( _MainTex, i.uv );
		float3 tintColor = i.color.rgb * i.color.a;

		color.rgb = (color * (1.0f - i.color.a) + tintColor);
		color.rgb *= color.a;
		return color;
		}
			ENDCG
		}
	}
}
