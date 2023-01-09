Shader "UI/Field boost overlay mask"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    [PerRendererData] _CornerFill ("Corner Fill", Color) = (0,0,0,0)
    _CornerTex ("Corner Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", float) = 8
    _Stencil ("Stencil ID", float) = 0
    [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", float) = 0
    _StencilWriteMask ("Stencil Write Mask", float) = 255
    _StencilReadMask ("Stencil Read Mask", float) = 255
    _ColorMask ("Color Mask", float) = 15
    [Enum(UnityEngine.Rendering.BlendMode)] _BlendSource ("Blend Source", float) = 1
    [Enum(UnityEngine.Rendering.BlendMode)] _BlendDestination ("Blend Destination", float) = 0
    [Enum(UnityEngine.Rendering.BlendMode)] _BlendAlphaSource ("Blend Alpha Source", float) = 1
    [Enum(UnityEngine.Rendering.BlendMode)] _BlendAlphaDestination ("Blend Alpha Destination", float) = 0
    [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("Depth Test", float) = 8
    _ZWrite ("Depth Write", float) = 0
    [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", float) = 0
    [Toggle(UNITY_INVERT_TINT)] _UseInvertTint ("Use Tint As Glow", float) = 0
    [Toggle(UNITY_READ_NO_ALPHA)] _UseReadNoAlpha ("Don't Read Alpha", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      Blend Zero Zero
      ColorMask 0
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float4 _TextureSampleAdd;
      uniform float4 _CornerFill;
      uniform sampler2D _MainTex;
      uniform sampler2D _CornerTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 phase0_Output0_2;
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          u_xlat0.x = 0;
          u_xlat0.w = in_v.color.w;
          u_xlat0 = (u_xlat0.xxxw * _Color);
          out_v.color = u_xlat0;
          u_xlat0.xy = float2(((-in_v.color.xy) + in_v.texcoord.xy));
          u_xlat0.zw = (abs(u_xlat0.xy) * float2(4, 4));
          u_xlat0.xy = float2(in_v.texcoord.xy);
          phase0_Output0_2 = u_xlat0;
          out_v.texcoord = phase0_Output0_2.xy;
          out_v.texcoord1 = phase0_Output0_2.zw;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat0_d;
      float4 u_xlat16_0;
      float4 u_xlat10_0;
      float2 u_xlatb0;
      float4 u_xlat1_d;
      float u_xlat16_1;
      float u_xlat2;
      int u_xlatb2;
      float u_xlat16_3;
      float u_xlat16_5;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlatb0.xy = float2(bool4(float4(0.5, 0.5, 0, 0) >= in_f.texcoord1.xyxx).xy);
          u_xlat2 = (u_xlatb0.y)?(1):(float(0));
          u_xlat16_1 = (u_xlatb0.x)?(0):(u_xlat2);
          u_xlat10_0 = tex2D(_CornerTex, in_f.texcoord1.xy);
          u_xlat16_3 = dot(_CornerFill.xyz, u_xlat10_0.xyz);
          u_xlat16_5 = (u_xlat10_0.w * _CornerFill.w);
          u_xlat0_d = ((u_xlat16_5 * u_xlat16_1) + u_xlat16_3);
          if((0.100000001>=u_xlat0_d))
          {
              u_xlatb2 = 1;
          }
          else
          {
              u_xlatb2 = 0;
          }
          u_xlat16_1 = (u_xlatb2)?(0):(1);
          u_xlat2 = (u_xlatb2)?(1):(float(0));
          u_xlat0_d = (u_xlat0_d * u_xlat16_1);
          u_xlat1_d = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat1_d.w = ((u_xlat1_d.w * u_xlat2) + u_xlat0_d);
          u_xlat16_0 = (u_xlat1_d + _TextureSampleAdd);
          out_f.color = (u_xlat16_0 * in_f.color);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
