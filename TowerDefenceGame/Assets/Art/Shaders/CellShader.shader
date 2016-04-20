// Shader created with Shader Forge v1.06 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.06;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:5021,x:32719,y:32712,varname:node_5021,prsc:2|custl-8451-OUT;n:type:ShaderForge.SFN_Tex2d,id:6039,x:32145,y:32565,ptovrint:False,ptlb:Diffuse Tex,ptin:_DiffuseTex,varname:node_6039,prsc:2,tex:d895447653b6e1c4ebe0c65a3bc926b6,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8451,x:32460,y:32736,varname:node_8451,prsc:2|A-6039-RGB,B-6467-RGB,C-2657-OUT;n:type:ShaderForge.SFN_Color,id:6467,x:32145,y:32740,ptovrint:False,ptlb:Diffuse Colour,ptin:_DiffuseColour,varname:node_6467,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_NormalVector,id:6833,x:31584,y:33022,prsc:2,pt:False;n:type:ShaderForge.SFN_LightVector,id:981,x:31584,y:32890,varname:node_981,prsc:2;n:type:ShaderForge.SFN_Dot,id:9293,x:31772,y:32963,varname:node_9293,prsc:2,dt:1|A-981-OUT,B-6833-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:562,x:31987,y:33092,varname:node_562,prsc:2;n:type:ShaderForge.SFN_Vector1,id:4392,x:31987,y:33220,varname:node_4392,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:2657,x:32352,y:32946,varname:node_2657,prsc:2|A-1476-OUT,B-562-OUT,C-4392-OUT;n:type:ShaderForge.SFN_Posterize,id:1476,x:31990,y:32963,varname:node_1476,prsc:2|IN-9293-OUT,STPS-2731-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2731,x:31772,y:33126,ptovrint:False,ptlb:Shade Steps,ptin:_ShadeSteps,varname:node_2731,prsc:2,glob:False,v1:4;proporder:6039-6467-2731;pass:END;sub:END;*/

Shader "Custom/CellShader" {
    Properties {
        _DiffuseTex ("Diffuse Tex", 2D) = "white" {}
        _DiffuseColour ("Diffuse Colour", Color) = (1,1,1,1)
        _ShadeSteps ("Shade Steps", Float ) = 4
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _DiffuseTex; uniform float4 _DiffuseTex_ST;
            uniform float4 _DiffuseColour;
            uniform float _ShadeSteps;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _DiffuseTex_var = tex2D(_DiffuseTex,TRANSFORM_TEX(i.uv0, _DiffuseTex));
                float node_9293 = max(0,dot(lightDirection,i.normalDir));
                float node_1476 = floor(node_9293 * _ShadeSteps) / (_ShadeSteps - 1);
                float3 finalColor = (_DiffuseTex_var.rgb*_DiffuseColour.rgb*(node_1476*attenuation*2.0));
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _DiffuseTex; uniform float4 _DiffuseTex_ST;
            uniform float4 _DiffuseColour;
            uniform float _ShadeSteps;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(_Object2World, float4(v.normal,0)).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _DiffuseTex_var = tex2D(_DiffuseTex,TRANSFORM_TEX(i.uv0, _DiffuseTex));
                float node_9293 = max(0,dot(lightDirection,i.normalDir));
                float node_1476 = floor(node_9293 * _ShadeSteps) / (_ShadeSteps - 1);
                float3 finalColor = (_DiffuseTex_var.rgb*_DiffuseColour.rgb*(node_1476*attenuation*2.0));
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
