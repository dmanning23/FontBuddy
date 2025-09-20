#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

// Effect applies normalmapped lighting to a 2D sprite.

float3 AmbientColor = 0.35;
float4 ColorMask = 1.0;
float Rotation = 0.0;
bool HasNormal = false;
bool FlipHorizontal = false;
bool HasColorMask = false;

#define DIRECTIONLIGHTS 5

float3 DirectionLights[DIRECTIONLIGHTS];
float4 DirectionLightColors[DIRECTIONLIGHTS];
int NumberOfDirectionLights = 0;

#if OPENGL
#define POINTLIGHTS 32
#else
#define POINTLIGHTS 4
#endif

float3 PointLights[POINTLIGHTS];
float3 PointLightColors[POINTLIGHTS];
float PointLightBrightness[POINTLIGHTS];
int NumberOfPointLights = 0;

sampler TextureSampler : register(s0);
sampler NormalSampler : register(s1)
{
	Texture = (NormalTexture);
};
sampler ColorMaskSampler : register(s2)
{
	Texture = (ColorMaskTexture);
};

float4 PixelShaderFunction(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	//Look up the texture value
	float4 tex = tex2D(TextureSampler, texCoord);

	//the final color we are going to use, either primary or secondary
	float4 texColor = color;

	//If there is a palette swap, add it to the texture color
	if (HasColorMask == true)
	{
		//Get the color from the palette swap texture
		float4 paletteSwap = tex2D(ColorMaskSampler, texCoord);
		if (paletteSwap.a > 0.0)
		{
			texColor = (texColor * (1.0 - paletteSwap.a)) + (paletteSwap * ColorMask);
		}
	}

	//Dont do these calculations if there is no normal map
	if (HasNormal == true)
	{
		//the final light value that will be added to the texture color. Don't allow light level to go below the ambient light level
		float3 lightColor = AmbientColor;

		//Look up the normalmap value
		float4 normal = tex2D(NormalSampler, texCoord);
		if (FlipHorizontal == true)
		{
			//If we are drawing a flipped image, reverse the normal
			normal.x = 1 - normal.x;
		}
		normal = 2.0 * normal - 1.0;

		//Loop through all the directional lights. 
		[unroll(DIRECTIONLIGHTS)]
		for (int directionLightIndex = 0; directionLightIndex < NumberOfDirectionLights; directionLightIndex++)
		{
			//compute the rotated light direction
			float3 rotatedLight = DirectionLights[directionLightIndex];
			float cs = cos(-Rotation);
			float sn = sin(-Rotation);

			float px = rotatedLight.x * cs - rotatedLight.y * sn;
			float py = rotatedLight.x * sn + rotatedLight.y * cs;
			rotatedLight.x = px;
			rotatedLight.y = py;

			//Compute lighting.
			float lightAmount = max(dot(normal.xyz, rotatedLight), 0.0);
			lightColor += (lightAmount * (DirectionLightColors[directionLightIndex].xyz * DirectionLightColors[directionLightIndex].w));
		}

		//Loop through all the point lights
		[unroll(POINTLIGHTS)]
		for (int pointLightIndex = 0; pointLightIndex < NumberOfPointLights; pointLightIndex++)
		{
			//Get the vector from the point light to the pixel position
			float3 rotatedLight = { PointLights[pointLightIndex].x - position.x, -1 * (position.y - PointLights[pointLightIndex].y), PointLights[pointLightIndex].z };
			rotatedLight = normalize(rotatedLight);

			//compute the rotated light direction
			float cs = cos(-Rotation);
			float sn = sin(-Rotation);

			float px = rotatedLight.x * cs - rotatedLight.y * sn;
			float py = rotatedLight.x * sn + rotatedLight.y * cs;
			rotatedLight.x = px;
			rotatedLight.y = py;
			
			//Get the light attenuation
			float2 lengthVect = { PointLights[pointLightIndex].x - position.x, PointLights[pointLightIndex].y - position.y };
			float attenuation = 1 / pow(((length(lengthVect) / PointLightBrightness[pointLightIndex]) + 1), 2);

			//Compute lighting.
			float lightAmount = saturate(dot(normal.xyz, rotatedLight)) * (PointLightBrightness[pointLightIndex] * attenuation);
			lightColor += (lightAmount * PointLightColors[pointLightIndex]);

			//if (lightAmount > 0.0)
			//{

			//	// Sample the pixel from the specular map texture.
			//	float specularIntensity = 1;

			//	// Calculate the reflection vector based on the light intensity, normal vector, and light direction.
			//	float3 reflection = normalize(2 * lightAmount * normal - rotatedLight);

			//	// Determine the amount of specular light based on the reflection vector, viewing direction, and specular power.
			//	float3 viewDirection = { 0,0,-1 };
			//	float specularPower = 16;
			//	float4 specular = pow(saturate(dot(reflection, viewDirection)), specularPower);

			//	// Use the specular map to determine the intensity of specular light at this pixel.
			//	specular = specular * specularIntensity;

			//	// Add the specular component last to the output color.
			//	lightColor = saturate(lightColor + specular);
			//}
		}

		texColor.rgb *= lightColor;
	}

	return tex * texColor;
}

technique Normalmap
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}
