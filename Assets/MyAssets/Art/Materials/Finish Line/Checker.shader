// Made with Amplify Shader Editor v1.9.3.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Checker"
{
	Properties
	{
		_ScaleX("ScaleX", Float) = 1
		_ScaleY("ScaleY", Float) = 1

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float _ScaleX;
			uniform float _ScaleY;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float4 color10 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
				float4 color9 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
				float2 appendResult8 = (float2(_ScaleX , _ScaleY));
				float2 FinalUV13_g1 = ( appendResult8 * ( 0.5 + i.ase_texcoord1.xy ) );
				float2 temp_cast_0 = (0.5).xx;
				float2 temp_cast_1 = (1.0).xx;
				float4 appendResult16_g1 = (float4(ddx( FinalUV13_g1 ) , ddy( FinalUV13_g1 )));
				float4 UVDerivatives17_g1 = appendResult16_g1;
				float4 break28_g1 = UVDerivatives17_g1;
				float2 appendResult19_g1 = (float2(break28_g1.x , break28_g1.z));
				float2 appendResult20_g1 = (float2(break28_g1.x , break28_g1.z));
				float dotResult24_g1 = dot( appendResult19_g1 , appendResult20_g1 );
				float2 appendResult21_g1 = (float2(break28_g1.y , break28_g1.w));
				float2 appendResult22_g1 = (float2(break28_g1.y , break28_g1.w));
				float dotResult23_g1 = dot( appendResult21_g1 , appendResult22_g1 );
				float2 appendResult25_g1 = (float2(dotResult24_g1 , dotResult23_g1));
				float2 derivativesLength29_g1 = sqrt( appendResult25_g1 );
				float2 temp_cast_2 = (-1.0).xx;
				float2 temp_cast_3 = (1.0).xx;
				float2 clampResult57_g1 = clamp( ( ( ( abs( ( frac( ( FinalUV13_g1 + 0.25 ) ) - temp_cast_0 ) ) * 4.0 ) - temp_cast_1 ) * ( 0.35 / derivativesLength29_g1 ) ) , temp_cast_2 , temp_cast_3 );
				float2 break71_g1 = clampResult57_g1;
				float2 break55_g1 = derivativesLength29_g1;
				float4 lerpResult73_g1 = lerp( color10 , color9 , saturate( ( 0.5 + ( 0.5 * break71_g1.x * break71_g1.y * sqrt( saturate( ( 1.1 - max( break55_g1.x , break55_g1.y ) ) ) ) ) ) ));
				
				
				finalColor = lerpResult73_g1;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19302
Node;AmplifyShaderEditor.RangedFloatNode;3;-718.3149,150.2016;Inherit;False;Property;_ScaleX;ScaleX;0;0;Create;True;0;0;0;False;0;False;1;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-716.3149,238.2016;Inherit;False;Property;_ScaleY;ScaleY;1;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-558.3149,175.2016;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;9;-567.3149,538.2015;Inherit;False;Constant;_Color2;Color 2;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-571.3149,356.2015;Inherit;False;Constant;_Color3;Color 2;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;6;-280.3149,230.2016;Inherit;True;Checkerboard;-1;;1;43dad715d66e03a4c8ad5f9564018081;0;4;1;FLOAT2;0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;12;95,153;Float;False;True;-1;2;ASEMaterialInspector;100;5;Checker;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;;0;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
WireConnection;8;0;3;0
WireConnection;8;1;7;0
WireConnection;6;2;10;0
WireConnection;6;3;9;0
WireConnection;6;4;8;0
WireConnection;12;0;6;0
ASEEND*/
//CHKSM=0BFAD1D81B28E709C84898AF2A76255CB116F124