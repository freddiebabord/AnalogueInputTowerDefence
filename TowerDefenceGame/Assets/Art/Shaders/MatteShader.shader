// Shader created with Shader Forge v1.06 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.06;sub:START;pass:START;ps:flbk:Diffuse,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1392,x:32719,y:32710,varname:node_1392,prsc:2|custl-5795-OUT;n:type:ShaderForge.SFN_Multiply,id:3517,x:32354,y:33123,varname:node_3517,prsc:2|A-2532-OUT,B-8407-OUT,C-2761-RGB;n:type:ShaderForge.SFN_Color,id:6844,x:31362,y:33375,ptovrint:False,ptlb:Diff Color,ptin:_DiffColor,varname:node_6844,prsc:2,glob:False,c1:0.9191176,c2:0.762327,c3:0.527141,c4:1;n:type:ShaderForge.SFN_LightColor,id:2761,x:32111,y:33257,varname:node_2761,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:2532,x:31088,y:32972,varname:node_2532,prsc:2;n:type:ShaderForge.SFN_Power,id:4548,x:31530,y:33133,varname:node_4548,prsc:2|VAL-8077-OUT,EXP-9884-OUT;n:type:ShaderForge.SFN_Vector1,id:9884,x:31352,y:33261,varname:node_9884,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Add,id:8077,x:31352,y:33113,varname:node_8077,prsc:2|A-1609-OUT,B-3020-OUT;n:type:ShaderForge.SFN_Multiply,id:1609,x:31147,y:33113,varname:node_1609,prsc:2|A-3282-OUT,B-3020-OUT;n:type:ShaderForge.SFN_Vector1,id:3020,x:30940,y:33275,varname:node_3020,prsc:2,v1:0.6;n:type:ShaderForge.SFN_Dot,id:3282,x:30940,y:33113,varname:node_3282,prsc:2,dt:0|A-6468-OUT,B-6825-OUT;n:type:ShaderForge.SFN_LightVector,id:6468,x:30711,y:33113,varname:node_6468,prsc:2;n:type:ShaderForge.SFN_ViewVector,id:6825,x:30711,y:33275,varname:node_6825,prsc:2;n:type:ShaderForge.SFN_Blend,id:5795,x:32457,y:32745,varname:node_5795,prsc:2,blmd:10,clmp:True|SRC-6586-OUT,DST-3517-OUT;n:type:ShaderForge.SFN_Lerp,id:6586,x:32260,y:32637,varname:node_6586,prsc:2|A-6150-RGB,B-8839-OUT,T-722-OUT;n:type:ShaderForge.SFN_Color,id:6150,x:31476,y:32603,ptovrint:False,ptlb:Shadow Colour,ptin:_ShadowColour,varname:node_6150,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_OneMinus,id:7399,x:31526,y:32776,varname:node_7399,prsc:2|IN-2532-OUT;n:type:ShaderForge.SFN_Vector4,id:8839,x:31925,y:32684,varname:node_8839,prsc:2,v1:0.5,v2:0.5,v3:0.5,v4:1;n:type:ShaderForge.SFN_Vector1,id:3211,x:31526,y:32907,varname:node_3211,prsc:2,v1:10;n:type:ShaderForge.SFN_Multiply,id:4147,x:31732,y:32776,varname:node_4147,prsc:2|A-7399-OUT,B-3211-OUT;n:type:ShaderForge.SFN_OneMinus,id:722,x:31925,y:32776,varname:node_722,prsc:2|IN-4147-OUT;n:type:ShaderForge.SFN_Blend,id:8407,x:31942,y:33183,varname:node_8407,prsc:2,blmd:10,clmp:True|SRC-6150-RGB,DST-7119-OUT;n:type:ShaderForge.SFN_Multiply,id:7119,x:31729,y:33214,varname:node_7119,prsc:2|A-4548-OUT,B-6844-RGB;proporder:6844-6150;pass:END;sub:END;*/

Shader "Shader Forge/MatteShader" {
    Properties {
        _DiffColor ("Diff Color", Color) = (0.9191176,0.762327,0.527141,1)
        _ShadowColour ("Shadow Colour", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
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
            uniform float4 _DiffColor;
            uniform float4 _ShadowColour;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                LIGHTING_COORDS(1,2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_3020 = 0.6;
                float3 finalColor = saturate(( (attenuation*saturate(( (pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb) > 0.5 ? (1.0-(1.0-2.0*((pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)-0.5))*(1.0-_ShadowColour.rgb)) : (2.0*(pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)*_ShadowColour.rgb) ))*_LightColor0.rgb) > 0.5 ? (1.0-(1.0-2.0*((attenuation*saturate(( (pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb) > 0.5 ? (1.0-(1.0-2.0*((pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)-0.5))*(1.0-_ShadowColour.rgb)) : (2.0*(pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)*_ShadowColour.rgb) ))*_LightColor0.rgb)-0.5))*(1.0-lerp(float4(_ShadowColour.rgb,0.0),float4(0.5,0.5,0.5,1),(1.0 - ((1.0 - attenuation)*10.0))))) : (2.0*(attenuation*saturate(( (pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb) > 0.5 ? (1.0-(1.0-2.0*((pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)-0.5))*(1.0-_ShadowColour.rgb)) : (2.0*(pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)*_ShadowColour.rgb) ))*_LightColor0.rgb)*lerp(float4(_ShadowColour.rgb,0.0),float4(0.5,0.5,0.5,1),(1.0 - ((1.0 - attenuation)*10.0)))) )).rgb;
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
            uniform float4 _DiffColor;
            uniform float4 _ShadowColour;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                LIGHTING_COORDS(1,2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_3020 = 0.6;
                float3 finalColor = saturate(( (attenuation*saturate(( (pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb) > 0.5 ? (1.0-(1.0-2.0*((pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)-0.5))*(1.0-_ShadowColour.rgb)) : (2.0*(pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)*_ShadowColour.rgb) ))*_LightColor0.rgb) > 0.5 ? (1.0-(1.0-2.0*((attenuation*saturate(( (pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb) > 0.5 ? (1.0-(1.0-2.0*((pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)-0.5))*(1.0-_ShadowColour.rgb)) : (2.0*(pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)*_ShadowColour.rgb) ))*_LightColor0.rgb)-0.5))*(1.0-lerp(float4(_ShadowColour.rgb,0.0),float4(0.5,0.5,0.5,1),(1.0 - ((1.0 - attenuation)*10.0))))) : (2.0*(attenuation*saturate(( (pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb) > 0.5 ? (1.0-(1.0-2.0*((pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)-0.5))*(1.0-_ShadowColour.rgb)) : (2.0*(pow(((dot(lightDirection,viewDirection)*node_3020)+node_3020),0.5)*_DiffColor.rgb)*_ShadowColour.rgb) ))*_LightColor0.rgb)*lerp(float4(_ShadowColour.rgb,0.0),float4(0.5,0.5,0.5,1),(1.0 - ((1.0 - attenuation)*10.0)))) )).rgb;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
