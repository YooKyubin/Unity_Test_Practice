Shader "Unlit/PlotPointTest"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Scale ("Sclae", Range(0.001, 10)) = 0.005
        _Radius ("Radius", Range(0.1, 100)) = 100
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
                float4 pos : POSITION;
            };

            struct g2f
            {
                float4 position : SV_POSITION;
                // float4 center : CENTER;
                float2 uv : TEXCOORD;
            };

            fixed4 _Color;
            float _Scale;
            float _Radius;

            v2g vert (appdata v)
            {
                v2g o;
                // o.pos = UnityObjectToClipPos(v.pos);

                float4 positionOS = v.pos;
                float4 positionWS = mul(UNITY_MATRIX_M, float4(positionOS.xyz, 1));
                float4 positionVS = mul(UNITY_MATRIX_V, positionWS);
                // float4 positionCS = mul(UNITY_MATRIX_P, positionVS);

                o.pos = positionVS;
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

                // output[0].center = mul(UNITY_MATRIX_P, vtx.pos);
                // output[1].center = mul(UNITY_MATRIX_P, vtx.pos);
                // output[2].center = mul(UNITY_MATRIX_P, vtx.pos);
                // output[3].center = mul(UNITY_MATRIX_P, vtx.pos);

                output[0].uv = float2(0.f, 0.f);
                output[1].uv = float2(1.f, 0.f);
                output[2].uv = float2(1.f, 1.f);
                output[3].uv = float2(0.f, 1.f);

                outputStream.Append(output[0]);
                outputStream.Append(output[1]);
                outputStream.Append(output[2]);
                outputStream.RestartStrip();

                outputStream.Append(output[0]);
                outputStream.Append(output[2]);
                outputStream.Append(output[3]);
                outputStream.RestartStrip();
            }

            fixed4 frag (g2f input) : SV_Target
            {
                // float4 center = input.center;
                // center.y = -center.y; // ncd 좌표계가 위에서 아래 방향으로 -1 ~ 1 이기 때문에 screen space와 다르다. 이걸 이제 알았다.
                // float2 ndcCenter = center / center.w;
                // float2 screen = (ndcCenter + 1.0f) * _ScreenParams / 2;

                // float scaledRadius = 2 * _Scale / center.w;
                // if (length(input.position - screen) > scaledRadius)
                //     discard;

                if ( length(input.uv - float2(0.5f, 0.5f)) > 0.5 )
                    discard;


                return _Color;
            }
            ENDCG
        }
    }
}
