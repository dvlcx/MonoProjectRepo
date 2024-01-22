// Standard defines
#if OPENGL
	#define VS_SHADERMODEL vs_4_0
	#define PS_SHADERMODEL ps_4_0
    #define GS_SHADERMODEL gs_4_0
#else
	#define VS_SHADERMODEL vs_5_0
	#define PS_SHADERMODEL ps_5_0
#endif

// Properties you can use from C# code
float4x4 World;
float4x4 View;
float4x4 Projection;
float4 Color;

// Required attributes of the input vertices
struct VertexShaderInput
{
    float4 Position		: POSITION;
    
	float4 Color		: COLOR0;  
};

struct VertexShaderOutput
{
    float4 Position     : SV_POSITION;

    float4 Color		: COLOR0;
};

struct GeometryShaderOutput
{
    float4 Position     : SV_POSITION;

    float4 Color		: COLOR0;
};

// Actual shaders
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position =  mul(mul(mul(input.Position, World), View), Projection);
    output.Color = input.Color;

    return output;
}

VertexShaderOutput PointVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position = input.Position;

    //idk why it just doesnt work other way (i hate writing shaders btw)
    output.Position /= float4(1, 1, 2, 1);
    output.Position += float4(0, 0, 0.5f, 0);

    output.Color = input.Color;

    return output;
}

float4 MainPS(VertexShaderOutput input) : SV_TARGET
{    
    return input.Color;
}


float4 InvertPS(GeometryShaderOutput input) : SV_TARGET
{    
    float4 invertedColor = float4(1.0 - input.Color.rgb, 1);
    return invertedColor;
}

[maxvertexcount(6)]
void MainGS(point VertexShaderOutput IN[1], inout TriangleStream<GeometryShaderOutput> triStream)
{
    GeometryShaderOutput OUT = (GeometryShaderOutput)0;
    
    float3 offsetX = float3(0.1f, 0.0f, 0.0f);
    float3 offsetY = float3(0.0f, 0.1f, 0.0f);
    float3 offsetZ = float3(0.0f, 0.0f, 0.1f);

    float4 vertices[4];
    vertices[0] = float4(IN[0].Position.xyz - offsetX - offsetY, 1.0f);
    // lower-left 
    vertices[1] = float4(IN[0].Position.xyz - offsetX + offsetY, 1.0f);
    // upper-left 
    vertices[2] = float4(IN[0].Position.xyz + offsetX + offsetY, 1.0f);
    // upper-right 
    vertices[3] = float4(IN[0].Position.xyz + offsetX - offsetY, 1.0f);
    // lower-right 
    
    float4x4 WorldViewProjection = mul(mul(World, View), Projection);
    OUT.Color = IN[0].Color;
    // tri: 0, 1, 2
    OUT.Position = mul(vertices[0], WorldViewProjection);
    triStream.Append(OUT);
    OUT.Position = mul(vertices[1], WorldViewProjection);
    triStream.Append(OUT);
    OUT.Position = mul(vertices[2], WorldViewProjection);
    triStream.Append(OUT);
    triStream.RestartStrip();
    // tri: 0, 2, 3 
    OUT.Position = mul(vertices[0], WorldViewProjection);
    triStream.Append(OUT);
    OUT.Position = mul(vertices[2], WorldViewProjection);
    triStream.Append(OUT);
    OUT.Position = mul(vertices[3], WorldViewProjection);
    triStream.Append(OUT);
}
 
// Technique and passes within the technique
technique ColorEffect
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
        FillMode = SOLID;
        CullMode = NONE;
    }
	pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL InvertPS();
        FillMode = WIREFRAME;
        CullMode = NONE;
    }
    pass Pass2
    {
        VertexShader = compile VS_SHADERMODEL PointVS();
        GeometryShader = compile GS_SHADERMODEL MainGS();
        PixelShader = compile PS_SHADERMODEL InvertPS();
        FillMode = SOLID;
        CullMode = NONE;
    }
}