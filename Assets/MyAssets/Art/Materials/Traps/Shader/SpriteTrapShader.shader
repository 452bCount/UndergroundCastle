// Made with Amplify Shader Editor v1.9.3.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CS/2D/TrapShader"
{
	Properties
	{
		[SingleLineTexture]_Texture("Texture", 2D) = "white" {}
		_TextureColor("TextureColor", Color) = (1,1,1,1)
		_TextureScaleCenter("TextureScaleCenter", Range( 0 , 1)) = 1
		_TextureRotateCenter("TextureRotateCenter", Float) = 0
		_OffsetX("OffsetX", Float) = 0
		_OffsetY("OffsetY", Float) = 0
		[SingleLineTexture]_AlphaShapeTexture("AlphaShapeTexture", 2D) = "white" {}
		_AlphaScaleCenter("AlphaScaleCenter", Range( 0 , 1)) = 1
		[Toggle(_PIXELSWITCH_ON)] _PixelSwitch("PixelSwitch", Float) = 0
		_Pixel("Pixel", Int) = 14
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _PIXELSWITCH_ON
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _TextureScaleCenter;
		uniform float4 _TextureColor;
		uniform sampler2D _Texture;
		uniform int _Pixel;
		uniform float _TextureRotateCenter;
		uniform float _OffsetX;
		uniform float _OffsetY;
		uniform sampler2D _AlphaShapeTexture;
		uniform float _AlphaScaleCenter;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float temp_output_51_0 = ( 1.0 / _TextureScaleCenter );
			float2 temp_output_54_0 = (i.uv_texcoord*temp_output_51_0 + ( ( 1.0 - temp_output_51_0 ) / 2.0 ));
			float2 appendResult10_g6 = (float2(1.0 , 1.0));
			float2 temp_output_11_0_g6 = ( abs( (temp_output_54_0*2.0 + -1.0) ) - appendResult10_g6 );
			float2 break16_g6 = ( 1.0 - ( temp_output_11_0_g6 / fwidth( temp_output_11_0_g6 ) ) );
			#ifdef _PIXELSWITCH_ON
				float2 staticSwitch9 = ( floor( ( temp_output_54_0 * _Pixel ) ) / _Pixel );
			#else
				float2 staticSwitch9 = i.uv_texcoord;
			#endif
			float2 break2 = staticSwitch9;
			float2 appendResult1 = (float2(break2.x , break2.y));
			float cos24 = cos( _TextureRotateCenter );
			float sin24 = sin( _TextureRotateCenter );
			float2 rotator24 = mul( appendResult1 - float2( 0.5,0.5 ) , float2x2( cos24 , -sin24 , sin24 , cos24 )) + float2( 0.5,0.5 );
			float2 appendResult42 = (float2(_OffsetX , _OffsetY));
			float2 temp_output_10_0 = (rotator24*float2( 1,1 ) + appendResult42);
			float4 tex2DNode14 = tex2D( _Texture, temp_output_10_0 );
			float4 tex2DNode12 = tex2D( _Texture, temp_output_10_0 );
			#ifdef _PIXELSWITCH_ON
				float4 staticSwitch20 = tex2DNode12;
			#else
				float4 staticSwitch20 = tex2DNode14;
			#endif
			o.Albedo = saturate( ( saturate( min( break16_g6.x , break16_g6.y ) ) * ( _TextureColor * staticSwitch20 ) ) ).rgb;
			#ifdef _PIXELSWITCH_ON
				float staticSwitch21 = tex2DNode12.a;
			#else
				float staticSwitch21 = tex2DNode14.a;
			#endif
			float2 appendResult10_g5 = (float2(1.0 , 1.0));
			float2 temp_output_11_0_g5 = ( abs( (temp_output_54_0*2.0 + -1.0) ) - appendResult10_g5 );
			float2 break16_g5 = ( 1.0 - ( temp_output_11_0_g5 / fwidth( temp_output_11_0_g5 ) ) );
			float temp_output_36_0 = ( 1.0 / _AlphaScaleCenter );
			float2 temp_output_33_0 = (i.uv_texcoord*temp_output_36_0 + ( ( 1.0 - temp_output_36_0 ) / 2.0 ));
			float2 appendResult10_g3 = (float2(1.0 , 1.0));
			float2 temp_output_11_0_g3 = ( abs( (temp_output_33_0*2.0 + -1.0) ) - appendResult10_g3 );
			float2 break16_g3 = ( 1.0 - ( temp_output_11_0_g3 / fwidth( temp_output_11_0_g3 ) ) );
			o.Alpha = ( saturate( ( staticSwitch21 * saturate( min( break16_g5.x , break16_g5.y ) ) ) ) * saturate( ( tex2D( _AlphaShapeTexture, temp_output_33_0 ) * saturate( min( break16_g3.x , break16_g3.y ) ) ).a ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19302
Node;AmplifyShaderEditor.TexCoordVertexDataNode;18;-2979.564,59.39046;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;-4063.737,-450.3154;Inherit;False;Property;_TextureScaleCenter;TextureScaleCenter;2;0;Create;True;0;0;0;False;0;False;1;2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;51;-3342.441,-472.2784;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;57;-2881.141,-72.29697;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;52;-3185.352,-413.7334;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;56;-3524.028,-175.8624;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;53;-3036.757,-413.7334;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;55;-3570.743,-496.0817;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IntNode;6;-2643.229,6.924899;Inherit;False;Property;_Pixel;Pixel;9;0;Create;True;0;0;0;False;0;False;14;14;False;0;1;INT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;54;-2833.491,-479.5074;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-2477.681,-109.5571;Inherit;False;2;2;0;FLOAT2;0,0;False;1;INT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FloorOpNode;4;-2328.684,-109.5571;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;5;-2205.684,-109.5571;Inherit;True;2;0;FLOAT2;0,0;False;1;INT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1465.667,1140.528;Inherit;False;Property;_AlphaScaleCenter;AlphaScaleCenter;7;0;Create;True;0;0;0;False;0;False;1;2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;9;-1917.087,69.54116;Inherit;False;Property;_PixelSwitch;PixelSwitch;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;36;-1154.146,1068.844;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;2;-1649.449,40.78425;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;37;-997.0568,1127.389;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;34;-2483.798,729.6382;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;1;-1456.757,70.81085;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;25;-1490.423,235.6666;Inherit;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;41;-1255.001,490.3583;Inherit;False;Property;_OffsetY;OffsetY;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1255.001,421.3583;Inherit;False;Property;_OffsetX;OffsetX;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1547.334,369.6689;Inherit;False;Property;_TextureRotateCenter;TextureRotateCenter;3;0;Create;True;0;0;0;False;0;False;0;6.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;38;-848.4617,1127.389;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;58;-737.0308,834.0709;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;24;-1227.371,257.9677;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;-1100.001,425.3583;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;33;-645.1962,1061.615;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;10;-919.0441,255.4471;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;13;-891.6116,-12.06516;Inherit;True;Property;_Texture;Texture;0;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;7b53015e70ce8bb4580b2cf7837a5cbc;e86ba70d2363b9443beac28a6a370b87;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerStateNode;15;-870.8918,-180.0186;Inherit;False;0;0;0;0;-1;X8;1;0;SAMPLER2D;;False;1;SAMPLERSTATE;0
Node;AmplifyShaderEditor.WireNode;68;-1393.916,-305.245;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;31;-651.7944,754.1808;Inherit;True;Property;_AlphaShapeTexture;AlphaShapeTexture;6;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;9068448ecdd226340a582400f48bbe33;9068448ecdd226340a582400f48bbe33;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.WireNode;59;-384.6456,942.3078;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;12;-604.8918,-228.0186;Inherit;True;Property;_TextureSample6;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-622.2725,-19.37386;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;67;-1090.272,139.5301;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;32;-324.0264,752.5712;Inherit;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;46;-279.9963,976.3016;Inherit;True;Rectangle;-1;;3;6b23e0c975270fb4084c354b2c83366a;0;3;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;21;-282.2673,84.77368;Inherit;False;Property;_Keyword1;Keyword 0;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;9;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;63;-269.2119,200.5262;Inherit;True;Rectangle;-1;;5;6b23e0c975270fb4084c354b2c83366a;0;3;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;0.003705502,951.3016;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;20;-278.6579,-69.22373;Inherit;False;Property;_Keyword0;Keyword 0;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;9;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;23;-243.4408,-254.8654;Inherit;False;Property;_TextureColor;TextureColor;1;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;1.443399,148.5815;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;48;215.9305,949.2778;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;30.5592,-150.8654;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;60;22.28074,-518.7903;Inherit;True;Rectangle;-1;;6;6b23e0c975270fb4084c354b2c83366a;0;3;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;65;160.4436,147.5815;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;45;342.0911,954.4718;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;328.8752,-363.2682;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;66;347.3029,130.2093;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;61;487.8752,-364.2682;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;810.783,82.27035;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;964.486,-54.06638;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;CS/2D/TrapShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;51;1;50;0
WireConnection;57;0;18;0
WireConnection;52;1;51;0
WireConnection;56;0;57;0
WireConnection;53;0;52;0
WireConnection;55;0;56;0
WireConnection;54;0;55;0
WireConnection;54;1;51;0
WireConnection;54;2;53;0
WireConnection;3;0;54;0
WireConnection;3;1;6;0
WireConnection;4;0;3;0
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;9;1;18;0
WireConnection;9;0;5;0
WireConnection;36;1;35;0
WireConnection;2;0;9;0
WireConnection;37;1;36;0
WireConnection;34;0;18;0
WireConnection;1;0;2;0
WireConnection;1;1;2;1
WireConnection;38;0;37;0
WireConnection;58;0;34;0
WireConnection;24;0;1;0
WireConnection;24;1;25;0
WireConnection;24;2;27;0
WireConnection;42;0;40;0
WireConnection;42;1;41;0
WireConnection;33;0;58;0
WireConnection;33;1;36;0
WireConnection;33;2;38;0
WireConnection;10;0;24;0
WireConnection;10;2;42;0
WireConnection;68;0;54;0
WireConnection;59;0;33;0
WireConnection;12;0;13;0
WireConnection;12;1;10;0
WireConnection;12;7;15;0
WireConnection;14;0;13;0
WireConnection;14;1;10;0
WireConnection;14;7;13;1
WireConnection;67;0;68;0
WireConnection;32;0;31;0
WireConnection;32;1;59;0
WireConnection;32;7;31;1
WireConnection;46;1;33;0
WireConnection;21;1;14;4
WireConnection;21;0;12;4
WireConnection;63;1;67;0
WireConnection;47;0;32;0
WireConnection;47;1;46;0
WireConnection;20;1;14;0
WireConnection;20;0;12;0
WireConnection;64;0;21;0
WireConnection;64;1;63;0
WireConnection;48;0;47;0
WireConnection;22;0;23;0
WireConnection;22;1;20;0
WireConnection;60;1;54;0
WireConnection;65;0;64;0
WireConnection;45;0;48;3
WireConnection;62;0;60;0
WireConnection;62;1;22;0
WireConnection;66;0;65;0
WireConnection;61;0;62;0
WireConnection;30;0;66;0
WireConnection;30;1;45;0
WireConnection;0;0;61;0
WireConnection;0;9;30;0
ASEEND*/
//CHKSM=2AD32CB2CA6DEEA50F367A2DF4F554AB455D5F95