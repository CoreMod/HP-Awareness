sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
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
float4 uSourceRect;
float2 uZoom;

float4 HPOverlay(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (coords.x >= 0.97 || coords.x <= 0.03 || coords.y <= 0.08 || coords.y >= 0.92)
    {
        color.r += uOpacity;
    }
    return color;
}

float4 HPOverlay2(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (coords.x >= 0.985 || coords.x <= 0.015 || coords.y <= 0.03 || coords.y >= 0.97)
    {
        color.r += (sin(uIntensity * uTime) + 1) * uOpacity;
    }
    return color;
}

technique Technique1
{
    pass HPOverlay
    {
        PixelShader = compile ps_2_0 HPOverlay();
    }
    pass HPOverlay2
    {
        PixelShader = compile ps_2_0 HPOverlay2();
    }
}