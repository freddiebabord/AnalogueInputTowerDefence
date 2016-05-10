// Shader created with Shader Forge v1.06 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.06;sub:START;pass:START;ps:flbk:Transparent/Cutout/Diffuse,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:3,bsrc:0,bdst:6,culm:0,dpts:2,wrdp:False,dith:0,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:9118,x:33180,y:32774,varname:node_9118,prsc:2|emission-6287-OUT,alpha-1383-OUT;n:type:ShaderForge.SFN_Vector1,id:1383,x:32886,y:33124,varname:node_1383,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Fresnel,id:1217,x:31997,y:32430,varname:node_1217,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3419,x:32177,y:32475,varname:node_3419,prsc:2|A-1217-OUT,B-7022-OUT;n:type:ShaderForge.SFN_Multiply,id:8385,x:32414,y:32433,varname:node_8385,prsc:2|A-6212-RGB,B-3419-OUT;n:type:ShaderForge.SFN_Tex2d,id:7924,x:32390,y:32681,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_7924,prsc:2,ntxv:0,isnm:False|UVIN-8677-OUT;n:type:ShaderForge.SFN_Multiply,id:9092,x:32604,y:32574,varname:node_9092,prsc:2|A-8385-OUT,B-7924-RGB;n:type:ShaderForge.SFN_Panner,id:1596,x:31784,y:32826,varname:node_1596,prsc:2,spu:0.5,spv:0.5|UVIN-2023-UVOUT;n:type:ShaderForge.SFN_ScreenPos,id:2023,x:31577,y:32768,varname:node_2023,prsc:2,sctp:0;n:type:ShaderForge.SFN_Multiply,id:6287,x:32796,y:32774,varname:node_6287,prsc:2|A-9092-OUT,B-5076-OUT;n:type:ShaderForge.SFN_Vector1,id:5076,x:32635,y:32896,varname:node_5076,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:8677,x:32185,y:32823,varname:node_8677,prsc:2|A-1596-UVOUT,B-1287-OUT;n:type:ShaderForge.SFN_Vector1,id:1287,x:31975,y:32914,varname:node_1287,prsc:2,v1:8;n:type:ShaderForge.SFN_Color,id:6212,x:31828,y:32239,ptovrint:False,ptlb:Colour,ptin:_Colour,varname:node_6212,prsc:2,glob:False,c1:0.3823529,c2:1,c3:0.9744422,c4:1;n:type:ShaderForge.SFN_Vector3,id:7022,x:31924,y:32618,varname:node_7022,prsc:2,v1:1,v2:1,v3:1;proporder:7924-6212;pass:END;sub:END;*/

Shader "Shader Forge/Holographic" {
    Properties {
        _Noise ("Noise", 2D) = "white" {}
        _Colour ("Colour", Color) = (0.3823529,1,0.9744422,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcColor
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float4 _Colour;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = o.pos;
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_5038 = _Time + _TimeEditor;
                float2 node_1596 = (i.screenPos.rg+node_5038.g*float2(0.5,0.5));
                float2 node_8677 = (node_1596*8.0);
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_8677, _Noise));
                float3 emissive = (((_Colour.rgb*((1.0-max(0,dot(normalDirection, viewDirection)))*float3(1,1,1)))*_Noise_var.rgb)*2.0);
                float3 finalColor = emissive;
                return fixed4(finalColor,0.5);
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
