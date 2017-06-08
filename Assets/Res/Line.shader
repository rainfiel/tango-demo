Shader "Lines/Colored_Blended" {
	SubShader { Pass {
		BindChannels { Bind "Color",color }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite on Cull Off Fog { Mode Off }
} } }
