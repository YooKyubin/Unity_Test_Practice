Shader "Unlit/PlotPointTest"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Scale ("Sclae", Range(0.001, 10)) = 0.005
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                
            };

            struct v2g
            {
                float4 pos : SV_POSITION;
                float pointSize : PSIZE;
            };

            struct g2f
            {
                float4 position : SV_POSITION;
                float pointSize : PSIZE;
            };

            fixed4 _Color;
            float _Scale;

            v2g vert (appdata v)
            {
                v2g o;
                // o.pos = UnityObjectToClipPos(v.pos);

                float4 positionOS = v.pos;
                float4 positionWS = mul(UNITY_MATRIX_M, float4(positionOS.xyz, 1));
                float4 positionVS = mul(UNITY_MATRIX_V, positionWS);
                // float4 positionCS = mul(UNITY_MATRIX_P, positionVS);

                // o.pointSize = _Scale * (300.0 / -positionVS.z);
                o.pos = positionVS;
                o.pointSize = 10.0;
                return o;
            }

            [maxvertexcount(6)]
            void geom (point v2g input[1], inout TriangleStream<g2f> outputStream)
            {
                g2f output[4] = 
                {
                    (g2f)0, (g2f)0, (g2f)0, (g2f)0
                };

                v2g vtx = input[0];

                // float scale = _Scale * (300 / -vtx.pos.z);
                // float scale = 0.5f;
                float scale = _Scale;

                // View Space
                output[0].position = vtx.pos + float4(-scale, scale, 0.f, 0.f);
                output[1].position = vtx.pos + float4(scale, scale, 0.f, 0.f);
                output[2].position = vtx.pos + float4(scale, -scale, 0.f, 0.f);
                output[3].position = vtx.pos + float4(-scale, -scale, 0.f, 0.f);

                // Projection Space
                output[0].position = mul(UNITY_MATRIX_P, output[0].position);
                output[1].position = mul(UNITY_MATRIX_P, output[1].position);
                output[2].position = mul(UNITY_MATRIX_P, output[2].position);
                output[3].position = mul(UNITY_MATRIX_P, output[3].position);

                outputStream.Append(output[0]);
                outputStream.Append(output[1]);
                outputStream.Append(output[2]);
                outputStream.RestartStrip();

                outputStream.Append(output[0]);
                outputStream.Append(output[2]);
                outputStream.Append(output[3]);
                outputStream.RestartStrip();

            }

            fixed4 frag (g2f i) : SV_Target
            {
                float4 col = _Color;    

                return col;
            }
            ENDCG
        }
    }
}
