sampler uImage0 : register(s0); // The contents of the screen.
sampler uImage1 : register(s1); // Up to three extra textures you can use for various purposes (for instance as an overlay).
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition; // The position of the camera.
float2 uTargetPosition; // The "target" of the shader, what this actually means tends to vary per shader.
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect; // Doesn't seem to be used, but included for parity.
float2 uZoom;

float4 FilterSpawnFlash(float2 coords : TEXCOORD0) : COLOR0
{
    float4 col = tex2D(uImage0, coords);
    if (uProgress >= 1.0f) {
        return col;
    } else if (uProgress >= 0.5f) {
        return lerp(col, float4(1.0f, 1.0f, 1.0f, 1.0f), (1.0f - uProgress) * 2.0f);
    } else {
        return lerp(col, float4(1.0f, 1.0f, 1.0f, 1.0f), uProgress * 2.0f);
    }
}

technique Technique1
{
    pass FilterSpawnFlash
    {
        PixelShader = compile ps_2_0 FilterSpawnFlash();
    }
}