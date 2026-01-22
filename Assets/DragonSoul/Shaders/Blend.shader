Shader "UI/BlendRawImage"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SubTex ("Sub Texture", 2D) = "black" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _SubTex;
            sampler2D _MaskTex;
            float4 _MainTex_ST;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col1 = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_SubTex, i.uv);
                fixed4 mask = tex2D(_MaskTex, i.uv);
                return lerp(col1, col2, mask.r);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}