Shader "ballShader"
{
    Properties
    {
        HighlightedColor("HighlightedColor", Color) = (1,0,0.81,1)
        NormalColor("NormalColor", Color) = (1,0.81,0,1)
        IsHighlighted("IsHighlighted", Int) = 0
    }
    SubShader
    {
            Pass{
            Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc" 

            
            int IsHighlighted;
            fixed4 HighlightedColor;
            fixed4 NormalColor;

            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };
            
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                if (IsHighlighted == 1)
                {
                    o.color.xyz = HighlightedColor;
                    //o.color.xyz = v.normal * 0.5 + 0.5;
                }
                else
                {
                   o.color.xyz = NormalColor;
                    //o.color.xyz = v.normal * 0.5 + 0.5;
                }
                o.color.w = 1.0;

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                
                half lightMod = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

                o.color =  lightMod * _LightColor0  * o.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target { return i.color; }
            ENDCG
                }
    }
}
