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

//Basic Overlays
float4 HPOverlay(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (coords.x >= 0.97 || coords.x <= 0.03 || coords.y <= 0.08 || coords.y >= 0.92)
    {
        color.r += clamp(uColor.r * uOpacity, 0, uColor.r);
        color.g += clamp(uColor.g * uOpacity, 0, uColor.g);
        color.b += clamp(uColor.b * uOpacity, 0, uColor.b);
    }
    return color;
}

float4 HPOverlayLow(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (coords.x >= 0.985 || coords.x <= 0.015 || coords.y <= 0.03 || coords.y >= 0.97)
    {
        color.r += clamp((sin(uIntensity * uTime) + 1) * uColor.r * uOpacity, 0, uColor.r);
        color.g += clamp((sin(uIntensity * uTime) + 1) * uColor.g * uOpacity, 0, uColor.g);
        color.b += clamp((sin(uIntensity * uTime) + 1) * uColor.b * uOpacity, 0, uColor.b);
    }
    return color;
}

//New Overlays
float4 NewHPOverlay(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float1 powerFormula = (pow(2 * coords.x - 1, 6) + pow(2 * coords.y - 1, 6));
    color.r += clamp(powerFormula * uColor.r * uOpacity, 0, uColor.r);
    color.g += clamp(powerFormula * uColor.g * uOpacity, 0, uColor.g);
    color.b += clamp(powerFormula * uColor.b * uOpacity, 0, uColor.b);
    //color.a has no effect
    return color;
}

float4 NewHPOverlayLow(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float1 powerFormula = (pow(2 * coords.x - 1, 40) + pow(2 * coords.y - 1, 40));
    color.r += clamp(powerFormula * (sin(uIntensity * uTime) + 1) * uColor.r * uOpacity, 0, uColor.r);
    color.g += clamp(powerFormula * (sin(uIntensity * uTime) + 1) * uColor.g * uOpacity, 0, uColor.g);
    color.b += clamp(powerFormula * (sin(uIntensity * uTime) + 1) * uColor.b * uOpacity, 0, uColor.b);
    return color;
}

//Flat Overlay
float4 HPOverlayFlat(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    color.r += clamp(uColor.r * uOpacity, 0, uColor.r);
    color.g += clamp(uColor.g * uOpacity, 0, uColor.g);
    color.b += clamp(uColor.b * uOpacity, 0, uColor.b);
    return color;
}

//Grayscale
float4 HPOverlayFlatGrayScale(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    //Calculate grayscale color (not the only way but this does the job)
    float1 avg = (color.r + color.g + color.b) / 3;
    float4 finalColor = color;
    finalColor.r = lerp(color.r, avg, uIntensity);
    finalColor.g = lerp(color.g, avg, uIntensity);
    finalColor.b = lerp(color.b, avg, uIntensity);
    return finalColor;
}

technique Technique1
{
    //Basic Overlays
    pass HPOverlay
    {
        PixelShader = compile ps_2_0 HPOverlay();
    }
    pass HPOverlayLow
    {
        PixelShader = compile ps_2_0 HPOverlayLow();
    }

    //New Overlays
    pass NewHPOverlay
    {
        PixelShader = compile ps_2_0 NewHPOverlay();
    }
    pass NewHPOverlayLow
    {
        PixelShader = compile ps_2_0 NewHPOverlayLow();
    }

    //Flat Overlays
    pass HPOverlayFlat
    {
        PixelShader = compile ps_2_0 HPOverlayFlat();
    }
    pass HPOverlayFlatGrayScale
    {
        PixelShader = compile ps_2_0 HPOverlayFlatGrayScale();
    }
}