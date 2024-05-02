// Made with Amplify Shader Editor v1.9.3.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "CS/2D&3D/LevelShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _Foldout8MAINTEXTURE("[Foldout(8)] MAIN TEXTURE", Float) = 0
        _LineCoral4("[Line(Coral)]", Float) = 0
        [SingleLineTexture]_Texture("Texture", 2D) = "white" {}
        _NoteAutomaticSettingsgettheothermaterialsfromtheMainTexture("[Note] AutomaticSettings get the other materials from the MainTexture", Float) = 0
        _TextureColor("TextureColor", Color) = (1,1,1,1)
        _TextureScaleX("TextureScaleX", Float) = 1
        _TextureScaleY("TextureScaleY", Float) = 1
        _TextureOffsetX("TextureOffsetX", Float) = 0
        _TextureOffsetY("TextureOffsetY", Float) = 0
        _Foldout4NORMALMAP("[Foldout(4)] NORMAL MAP", Float) = 0
        _LineCoral2("[Line(Coral)]", Float) = 0
        _Foldout2METALLIC("[Foldout(2)] METALLIC", Float) = 0
        _LineCoral1("[Line(Coral)]", Float) = 0
        _Foldout2SMOOTHNESS("[Foldout(2)] SMOOTHNESS", Float) = 0
        _LineCoral("[Line(Coral)]", Float) = 0
        _Foldout2PIXEL("[Foldout(2)] PIXEL", Float) = 0
        _LineCoral3("[Line(Coral)]", Float) = 0
        [Toggle(_PIXELSWITCH_ON)] _PixelSwitch("PixelSwitch", Float) = 0
        _Pixel("Pixel", Int) = 14

    }

    SubShader
    {
		LOD 0

        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

        Stencil
        {
        	Ref [_Stencil]
        	ReadMask [_StencilReadMask]
        	WriteMask [_StencilWriteMask]
        	Comp [_StencilComp]
        	Pass [_StencilOp]
        }


        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        
        Pass
        {
            Name "Default"
        CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #pragma shader_feature_local _PIXELSWITCH_ON


            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4  mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
                float4 ase_texcoord3 : TEXCOORD3;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

            uniform float _LineCoral;
            uniform float _LineCoral1;
            uniform float _LineCoral2;
            uniform float _LineCoral3;
            uniform float _LineCoral4;
            uniform float _Foldout4NORMALMAP;
            uniform float _Foldout2METALLIC;
            uniform float _Foldout2SMOOTHNESS;
            uniform float _Foldout2PIXEL;
            uniform float _NoteAutomaticSettingsgettheothermaterialsfromtheMainTexture;
            uniform float _Foldout8MAINTEXTURE;
            uniform float4 _TextureColor;
            uniform sampler2D _Texture;
            uniform int _Pixel;
            uniform float _TextureScaleX;
            uniform float _TextureScaleY;
            uniform float _TextureOffsetX;
            uniform float _TextureOffsetY;

            
            v2f vert(appdata_t v )
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float3 ase_worldPos = mul(unity_ObjectToWorld, float4( (v.vertex).xyz, 1 )).xyz;
                OUT.ase_texcoord3.xyz = ase_worldPos;
                
                
                //setting value to unused interpolator channels and avoid initialization warnings
                OUT.ase_texcoord3.w = 0;

                v.vertex.xyz +=  float3( 0, 0, 0 ) ;

                float4 vPosition = UnityObjectToClipPos(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;

                float2 pixelSize = vPosition.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                OUT.texcoord = v.texcoord;
                OUT.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN ) : SV_Target
            {
                //Round up the alpha color coming from the interpolator (to 1.0/256.0 steps)
                //The incoming alpha could have numerical instability, which makes it very sensible to
                //HDR color transparency blend, when it blends with the world's texture.
                const half alphaPrecision = half(0xff);
                const half invAlphaPrecision = half(1.0/alphaPrecision);
                IN.color.a = round(IN.color.a * alphaPrecision)*invAlphaPrecision;

                float3 ase_worldPos = IN.ase_texcoord3.xyz;
                #ifdef _PIXELSWITCH_ON
                float3 staticSwitch110 = ( floor( ( ase_worldPos * _Pixel ) ) / _Pixel );
                #else
                float3 staticSwitch110 = ase_worldPos;
                #endif
                float3 break109 = staticSwitch110;
                float2 appendResult21 = (float2(break109.x , break109.y));
                float2 appendResult41 = (float2(_TextureScaleX , _TextureScaleY));
                float2 appendResult42 = (float2(_TextureOffsetX , _TextureOffsetY));
                float2 temp_output_22_0 = (appendResult21*appendResult41 + appendResult42);
                #ifdef _PIXELSWITCH_ON
                float4 staticSwitch123 = tex2D( _Texture, temp_output_22_0 );
                #else
                float4 staticSwitch123 = tex2D( _Texture, temp_output_22_0 );
                #endif
                float4 break27 = staticSwitch123;
                float4 appendResult28 = (float4(break27.r , break27.g , break27.b , 1.0));
                

                half4 color = ( _TextureColor * appendResult28 );

                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color.rgb *= color.a;

                return color;
            }
        ENDCG
        }
    }
    CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19302
Node;AmplifyShaderEditor.WorldPosInputsNode;108;-1876.1,298.5168;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.IntNode;107;-1660.888,246.7852;Inherit;False;Property;_Pixel;Pixel;29;0;Create;True;0;0;0;False;0;False;14;14;False;0;1;INT;0
Node;AmplifyShaderEditor.WireNode;112;-1702.392,215.4236;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-1495.341,130.3032;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;INT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FloorOpNode;105;-1346.341,130.3032;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;106;-1223.341,130.3032;Inherit;True;2;0;FLOAT3;0,0,0;False;1;INT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;110;-934.7425,309.4015;Inherit;False;Property;_PixelSwitch;PixelSwitch;28;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-924.26,423.386;Inherit;False;Property;_TextureScaleX;TextureScaleX;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-922.26,502.386;Inherit;False;Property;_TextureScaleY;TextureScaleY;7;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-920.26,591.386;Inherit;False;Property;_TextureOffsetX;TextureOffsetX;8;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-918.26,670.386;Inherit;False;Property;_TextureOffsetY;TextureOffsetY;9;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;109;-667.105,280.6445;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;21;-474.4133,310.6711;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;41;-731.26,446.386;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;-727.26,613.386;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;22;-311.8095,422.8449;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;127;-160.8129,73.224;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;16;-366.5327,116.1774;Inherit;True;Property;_Texture;Texture;2;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerStateNode;126;-345.8129,-51.776;Inherit;False;0;0;0;0;-1;X8;1;0;SAMPLER2D;;False;1;SAMPLERSTATE;0
Node;AmplifyShaderEditor.SamplerNode;125;-79.81287,-99.776;Inherit;True;Property;_TextureSample6;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-75.19355,113.8687;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;123;222.4431,-46.87878;Inherit;False;Property;_Keyword2;Keyword 2;28;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;110;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;27;495.3184,70.851;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ColorNode;122;1859.974,-62.63615;Inherit;False;Property;_TextureColor;TextureColor;5;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;28;610.7065,70.6984;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;15;232.9708,383.9037;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;14;381.3592,381.7511;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;26;4.115544,384.9455;Inherit;False;NormalCreate;12;;2;e12f7ae19d416b942820e3932b56220f;0;4;1;SAMPLER2D;;False;2;FLOAT2;0,0;False;3;FLOAT;0.5;False;4;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-513.3243,665.0276;Inherit;False;Property;_NormalOffsetX;NormalOffsetX;15;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-511.3243,741.0276;Inherit;False;Property;_NormalOffsetY;NormalOffsetY;16;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;64;-334.3243,691.0276;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-405.4634,864.1738;Inherit;False;Property;_NormalStrength;Normal Strength;17;0;Create;True;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;65;259.7058,893.2573;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;59;-14.40113,724.0211;Inherit;True;Property;_NormalTexture;NormalTexture;14;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;None;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.WireNode;66;629.3246,235.2974;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;60;355.4074,732.6241;Inherit;True;Property;_TextureSample3;Texture Sample 3;15;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;67;669.6414,395.321;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;68;687.6096,257.3916;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;74;423.7493,926.3392;Inherit;True;Property;_SmoothnessTexture;SmoothnessTexture;24;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;75;784.614,792.7127;Inherit;True;Property;_TextureSample4;Texture Sample 4;19;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;70;786.1628,987.4532;Inherit;False;Property;_Smoothness;Smoothness;25;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;73;1119.332,939.4999;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;80;716.9393,490.6528;Inherit;True;Property;_MetallicTexture;MetallicTexture;20;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;81;1077.804,357.0263;Inherit;True;Property;_TextureSample5;Texture Sample 4;19;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;83;1392.522,421.8135;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;69;1078.918,569.8121;Inherit;False;Property;_Metallic;Metallic;21;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;91;1703.493,286.8858;Inherit;False;Property;_Keyword0;Keyword 0;3;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;51;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;100;1104.379,700.717;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;1277.048,676.7242;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;101;1001.813,610.9634;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;102;1026.904,58.95031;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;76;585.6934,597.1065;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;51;766.1682,179.2871;Inherit;False;Property;_AutomaticSettings;AutomaticSettings;3;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;2;None;All;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1060.435,-436.0226;Inherit;False;Property;_LineCoral;[Line(Coral)];23;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1061.511,-359.0085;Inherit;False;Property;_LineCoral1;[Line(Coral)];19;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1064.199,-283.8355;Inherit;False;Property;_LineCoral2;[Line(Coral)];11;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-1064.029,-207.0554;Inherit;False;Property;_LineCoral3;[Line(Coral)];27;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-1064.139,-128.2311;Inherit;False;Property;_LineCoral4;[Line(Coral)];1;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;115;-780.9968,-356.0498;Inherit;False;Property;_Foldout4NORMALMAP;[Foldout(4)] NORMAL MAP;10;0;Create;True;1;;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;117;-772.5438,-280.2058;Inherit;False;Property;_Foldout2METALLIC;[Foldout(2)] METALLIC;18;0;Create;True;1;;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;119;-772.0942,-202.7633;Inherit;False;Property;_Foldout2SMOOTHNESS;[Foldout(2)] SMOOTHNESS;22;0;Create;True;1;;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-772.681,-121.2811;Inherit;False;Property;_Foldout2PIXEL;[Foldout(2)] PIXEL;26;0;Create;True;1;;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-1472.192,-439.3731;Inherit;False;Property;_NoteAutomaticSettingsgettheothermaterialsfromtheMainTexture;[Note] AutomaticSettings get the other materials from the MainTexture;4;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;2144.12,80.85255;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;1413.123,-124.5799;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-784.1858,-436.5407;Inherit;False;Property;_Foldout8MAINTEXTURE;[Foldout(8)] MAIN TEXTURE;0;0;Create;True;1;;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;98;1695.951,635.0786;Inherit;False;Property;_Keyword1;Keyword 0;3;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;51;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCGrayscale;95;509.1475,-150.8604;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;128;2311.399,144.1511;Float;False;True;-1;2;ASEMaterialInspector;0;3;CS/2D&3D/LevelShader;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;True;_ColorMask;False;False;False;False;False;False;False;True;True;0;True;_Stencil;255;True;_StencilReadMask;255;True;_StencilWriteMask;0;True;_StencilComp;0;True;_StencilOp;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;0;True;unity_GUIZTestMode;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;112;0;108;0
WireConnection;104;0;112;0
WireConnection;104;1;107;0
WireConnection;105;0;104;0
WireConnection;106;0;105;0
WireConnection;106;1;107;0
WireConnection;110;1;108;0
WireConnection;110;0;106;0
WireConnection;109;0;110;0
WireConnection;21;0;109;0
WireConnection;21;1;109;1
WireConnection;41;0;40;0
WireConnection;41;1;39;0
WireConnection;42;0;43;0
WireConnection;42;1;44;0
WireConnection;22;0;21;0
WireConnection;22;1;41;0
WireConnection;22;2;42;0
WireConnection;127;0;22;0
WireConnection;125;0;16;0
WireConnection;125;1;127;0
WireConnection;125;7;126;0
WireConnection;17;0;16;0
WireConnection;17;1;22;0
WireConnection;17;7;16;1
WireConnection;123;1;17;0
WireConnection;123;0;125;0
WireConnection;27;0;123;0
WireConnection;28;0;27;0
WireConnection;28;1;27;1
WireConnection;28;2;27;2
WireConnection;15;0;26;0
WireConnection;14;0;15;0
WireConnection;14;1;15;1
WireConnection;14;2;15;2
WireConnection;26;1;16;0
WireConnection;26;2;22;0
WireConnection;26;3;64;0
WireConnection;26;4;38;0
WireConnection;64;0;62;0
WireConnection;64;1;63;0
WireConnection;65;0;38;0
WireConnection;66;0;60;0
WireConnection;60;0;59;0
WireConnection;60;1;22;0
WireConnection;60;5;65;0
WireConnection;60;7;59;1
WireConnection;67;0;14;0
WireConnection;68;0;67;0
WireConnection;75;0;74;0
WireConnection;75;1;76;0
WireConnection;75;7;74;1
WireConnection;73;0;75;0
WireConnection;73;2;70;0
WireConnection;81;0;80;0
WireConnection;81;7;80;1
WireConnection;83;1;81;0
WireConnection;83;2;69;0
WireConnection;91;1;83;0
WireConnection;91;0;97;0
WireConnection;100;0;70;0
WireConnection;99;0;101;0
WireConnection;99;1;100;0
WireConnection;101;0;102;0
WireConnection;102;0;95;0
WireConnection;76;0;22;0
WireConnection;51;1;66;0
WireConnection;51;0;68;0
WireConnection;121;0;122;0
WireConnection;121;1;28;0
WireConnection;97;0;95;0
WireConnection;97;1;69;0
WireConnection;98;1;73;0
WireConnection;98;0;99;0
WireConnection;95;0;123;0
WireConnection;128;0;121;0
ASEEND*/
//CHKSM=AFC04D762F1A1409F6FB12F5CD3EA020D5719801