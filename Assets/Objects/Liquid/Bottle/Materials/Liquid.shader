Shader "Custom/waterliquid"
{
    Properties
    {
        _Color("Colour",color) = (1,1,1,1)
        _Bumpmap("NormalMap", 2D) = "bump" {}
        _Cube("Cube",Cube) = ""{}
        _SPColor("Specular Color", color) = (1,1,1,1)
        _SPPower("Specular Power",Range(50,300)) = 150
        _SPMulti("Specular Multiply",Range(1,10)) = 3

        _FillAmount("FillAmount", Range(-100,100)) = 0.0
        [HideInInspector] _WobbleX("WobbleX", Range(-1,1)) = 0.0
        [HideInInspector] _WobbleZ("WobbleZ", Range(-1,1)) = 0.0


    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

            cull off

            CGPROGRAM
            #pragma surface surf WaterSpecular alpha:fade

            samplerCUBE _Cube;
            sampler2D _Bumpmap;

            float _FillAmount, _WobbleX, _WobbleZ;

            float4 _Color;
            float4 _SPColor;
            float _SPPower;
            float _SPMulti;
            float _LiquidY;

            struct Input
            {
                float2 uv_Bumpmap;
                float3 worldRefl;
                float3 viewDir;
                float3 worldPos;
                float3 worldNormal;
                float3 screenPos;
                INTERNAL_DATA
            };

            float4 RotateAroundYInDegrees(float4 vertex, float degrees)
            {
                float alpha = degrees * UNITY_PI / 180;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, sina, -sina, cosa);
                return float4(vertex.yz, mul(m, vertex.xz)).xzyw;
            }

            void surf(Input IN, inout SurfaceOutput o)
            {
                float3 normal1 = UnpackNormal(tex2D(_Bumpmap, IN.uv_Bumpmap + _Time.x * 0.01));
                float3 normal2 = UnpackNormal(tex2D(_Bumpmap, IN.uv_Bumpmap - _Time.z * 0.01));
                o.Normal = (normal1 + normal2) / 2;

                float3 refcolor = _Color.rgb * texCUBE(_Cube, WorldReflectionVector(IN, o.Normal));

                float rim = saturate(dot(o.Normal, IN.viewDir));
                rim = pow(1.5 - rim, 1.5);


                float3 worldPos = IN.worldPos;
                // rotate it around XY
                float3 worldPosX = RotateAroundYInDegrees(float4(worldPos, 0), 360);
                // rotate around XZ

                float3 worldPosZ = float3(worldPosX.y, worldPosX.z, worldPosX.x);
                // combine rotations with worldPos, based on sine wave from script
                float3 worldPosAdjusted = worldPos + (worldPosX * _WobbleX) + (worldPosZ * _WobbleZ);
                // how high up the liquid is
                float fillEdge = worldPosAdjusted.y + (-1 * _FillAmount) + 0.5f ;

                // Calculate the dot product between view direction and surface normal
                float facing = dot(normalize(IN.worldPos - worldPosAdjusted), o.Normal);

                // foam edge
                float foam = (step(fillEdge, 0.5) - step(1-rim,1.25));
                float4 foamColored = foam * (_Color * 0.3);

                // rest of the liquid
                float result = step(fillEdge, 0.5) - foam;
                float4 resultColored = result * _Color;
                resultColored.a = 1;

                // both together
                float4 finalResult = resultColored + foamColored;

                o.Emission = finalResult.rgb * refcolor;
                o.Alpha = saturate(pow(saturate(pow(saturate(finalResult).a,10)),2));
            }

            float4 LightingWaterSpecular(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {
                float3 H = normalize(lightDir + viewDir);
                float spec = saturate(dot(H, s.Normal));
                spec = pow(spec, _SPPower);

                float4 finalColor;
                finalColor.rgb = spec * _SPColor.rgb * _SPMulti;
                finalColor.a = s.Alpha;

                return finalColor;
            }
            ENDCG
        }
            
}
