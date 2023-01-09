Shader "Custom/Unlit/BombGemOverlay"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _Tint ("Tint", Color) = (1,1,1,1)
    _ScrollSpeed ("Scroll Speed", float) = 0.5
    _ScrollOffset ("Scroll Offset", float) = 1
    _ScrollSize ("Scroll Size", float) = 0.1
    _HighlightSpeed ("Highlight Speed", float) = 0.5
    _HighlightTiling ("Highlight Tilling", float) = 1
    _StencilComp ("Stencil Comparison", float) = 0
    _Stencil ("Stencil ID", float) = 2
    _StencilOp ("Stencil Operation", float) = 2
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent+1"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent+1"
        "RenderType" = "Transparent"
      }
      ZTest Always
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 255
        WriteMask 255
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
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _Time;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float _ScrollSpeed;
      uniform float _ScrollSize;
      uniform float _ScrollOffset;
      uniform float _HighlightSpeed;
      uniform float _HighlightTiling;
      uniform float4 _Tint;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float2 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float3 u_xlat16_2;
      float u_xlat3;
      float2 u_xlat6;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.x = (_Time.y * _HighlightSpeed);
          u_xlat1.x = cos(u_xlat0.x);
          u_xlat0.x = sin(u_xlat0.x);
          u_xlat0.x = (u_xlat0.x / u_xlat1.x);
          u_xlat6.y = ((u_xlat0.x * (-_HighlightTiling)) + in_v.texcoord.y);
          u_xlat6.x = in_v.texcoord.x;
          u_xlat0.xy = float2((u_xlat6.xy + float2(0, (-0.5))));
          u_xlat0.xy = float2(clamp(u_xlat0.xy, 0, 1));
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(u_xlat0.xy, _MainTex));
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          u_xlat0.x = ((in_v.color.z * _ScrollOffset) + _Time.y);
          u_xlat0.x = (u_xlat0.x * _ScrollSpeed);
          u_xlat0.x = sin(u_xlat0.x);
          u_xlat0.x = (u_xlat0.x + 1);
          u_xlat0.x = (u_xlat0.x * 0.5);
          u_xlat3 = (u_xlat0.x * u_xlat0.x);
          u_xlat3 = (u_xlat3 * u_xlat3);
          u_xlat0.x = (u_xlat3 * u_xlat0.x);
          u_xlat0.x = (u_xlat0.x * _ScrollSize);
          u_xlat16_2.xy = float2(((in_v.color.xy * float2(2, 2)) + float2(-1, (-1))));
          u_xlat16_2.z = 0;
          u_xlat0.xyz = float3(((u_xlat0.xxx * u_xlat16_2.xyz) + in_v.vertex.xyz));
          out_v.vertex = UnityObjectToClipPos(u_xlat0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat10_0;
      float u_xlat16_1;
      float3 u_xlat2;
      float u_xlat3_d;
      float2 u_xlat10_3;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord1.xy).y.x;
          u_xlat10_3.xy = tex2D(_MainTex, in_f.texcoord.xy).xz.xy;
          u_xlat3_d = (u_xlat10_3.x * _Tint.w);
          u_xlat3_d = (u_xlat10_3.y * u_xlat3_d);
          u_xlat16_1 = ((-u_xlat10_3.y) + 1);
          u_xlat16_1 = (u_xlat16_1 * 0.200000003);
          u_xlat2.xyz = float3(((float3(u_xlat16_1, u_xlat16_1, u_xlat16_1) * (-_Tint.xyz)) + _Tint.xyz));
          out_f.color.xyz = float3(u_xlat2.xyz);
          out_f.color.w = ((u_xlat10_0 * u_xlat3_d) + u_xlat3_d);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
