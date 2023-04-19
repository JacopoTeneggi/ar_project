Shader "Unlit/PortalShader"
{
	Properties
	{
		[IntRange] _StencilID("Stencil ID", Range(1, 255)) = 1
	}
		
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Geometry"
			"RenderPipeline" = "UniversalPipeline"
		}

		Pass
		{
			Blend Zero One
			ZWrite Off

			Stencil
			{
				Ref[_StencilID]
				Comp Always
				Pass Replace
				Fail Keep
			}
		}
	}
}