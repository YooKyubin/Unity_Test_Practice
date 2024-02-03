Shader "Unlit/UnlitShaderTest"
{
    Properties
    {
        _Density ("Density", Range(2,50)) = 30
        _Factor ("Factor", Range(0, 1.0)) = 0.5
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc" // _LightColor()

            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "AutoLight.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed4 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
            };

            float _Density;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));

                o.diff = nl * _LightColor0;
                o.ambient = ShadeSH9(half4(worldNormal,1));
                // compute shadows data
                TRANSFER_SHADOW(o);
                return o;
            }

            sampler2D _MainTex;
            float _Factor;
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col;
                col = tex2D(_MainTex, i.uv);
                float2 c = i.uv * _Density;
                c = floor(c) / 2;
                float checker = frac(c.x + c.y) * 2;
                col += float4(checker, checker, checker, 1);
                col *= _Factor;

                fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
                return col;


                // fixed4 col = tex2D(_MainTex, i.uv);
                // // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                // fixed shadow = SHADOW_ATTENUATION(i);
                // // darken light's illumination with shadow, keep ambient intact
                // fixed3 lighting = i.diff * shadow + i.ambient;
                // col.rgb *= lighting;
                // return col;
            }
            ENDCG
        }

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        // Pass
        // {
        //     Tags {"LightMode"="ShadowCaster"}

        //     CGPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag
        //     #pragma multi_compile_shadowcaster
        //     #include "UnityCG.cginc"

        //     struct v2f { 
        //         V2F_SHADOW_CASTER;
        //     };

        //     v2f vert(appdata_base v)
        //     {
        //         v2f o;
        //         TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
        //         return o;
        //     }

        //     float4 frag(v2f i) : SV_Target
        //     {
        //         SHADOW_CASTER_FRAGMENT(i)
        //     }
        //     ENDCG
        // }
    }
}