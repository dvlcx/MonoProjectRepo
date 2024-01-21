// Standard defines
#if OPENGL
	#define VS_SHADERMODEL vs_4_0
	#define PS_SHADERMODEL ps_4_0
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
    float4 Position		: POSITION0;
    
	float4 Color		: COLOR0;  
};

struct VertexShaderOutput
{
    float4 Position     : SV_POSITION;

    float4 Color		: COLOR0;
};

// Actual shaders
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    float4 worldPosition = mul(float4(input.Position.xyz, 1), World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.Color = input.Color;

    return output;
}

float4 MainPS(VertexShaderOutput input) : SV_TARGET
{    
    return input.Color;
}

VertexShaderOutput WireVS(in VertexShaderInput input)
{

    VertexShaderOutput output = (VertexShaderOutput)0;

    output.Position =  mul(mul(mul(input.Position, World), View), Projection);
    output.Color = input.Color;
    return output;
}

float4 WirePS(VertexShaderOutput input) : SV_TARGET
{    
    float4 invertedColor = float4(1.0 - input.Color.rgb, 1);
    return invertedColor;
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
        VertexShader = compile VS_SHADERMODEL WireVS();
        PixelShader = compile PS_SHADERMODEL WirePS();
        FillMode = WIREFRAME;
        CullMode = NONE;
    }
}