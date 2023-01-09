Shader "Tropicats/VFX/Alpha/colorable + overexposure + gradientMask + FillUpDown "
{
  Properties
  {
    _MainTex ("Texture Mask (RGB)", 2D) = "white" {}
    _UV2Tiling ("Extra Tiling", float) = 1
    [PerRendererData] _Distance ("Distance set by code", float) = 1
    _AlphaState ("Alpha State", Range(0, 1)) = 0.5
    _CutOffSharpness ("Cutoff Sharpness", Range(0, 1)) = 0.5
    _CutOffSize ("Cutoff Size", Range(0.1, 20)) = 0.5
    _ScrollSize ("Size XY, Scrollamount XY", Vector) = (1,1,0,0)
    _FillInOffset ("Fill up Offset", Range(-1, 1)) = 0.5
    _AScale ("Alpha Scale", float) = 1
    _Multiply ("Overexposure", float) = 1
    _SSMin ("Smoothstep Mask Min", Range(0, 1)) = 0.1
    _SSMax ("Smoothstep Mask Max", Range(0, 1)) = 1
    _DebugTint ("Debug Tint", Color) = (1,1,1,1)
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
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
      uniform float4 _ScrollSize;
      uniform float _UV2Tiling;
      uniform float _CutOffSize;
      uniform float _CutOffSharpness;
      uniform float _FillInOffset;
      uniform float _Distance;
      uniform float4 _DebugTint;
      uniform float _Multiply;
      uniform float _AScale;
      uniform float _SSMin;
      uniform float _SSMax;
      uniform sampler2D _MainTex;
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
          float texcoord2 :TEXCOORD2;
          float texcoord3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float texcoord2 :TEXCOORD2;
          float texcoord3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat16_0;
      float4 u_xlat1;
      float u_xlat3;
      float2 u_xlat5;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          u_xlat16_0 = (in_v.color * _DebugTint);
          out_v.color = u_xlat16_0;
          u_xlat1.x = (_UV2Tiling * _Distance);
          u_xlat5.x = (u_xlat1.x * in_v.texcoord.x);
          u_xlat5.y = in_v.texcoord.y;
          u_xlat1.xy = float2((((-_Time.zz) * _ScrollSize.zw) + u_xlat5.xy));
          out_v.texcoord1.xy = float2((u_xlat1.xy * _ScrollSize.xy));
          out_v.texcoord.xy = float2(in_v.texcoord.xy);
          u_xlat1.x = (in_v.texcoord.x + 1);
          u_xlat1.x = (((-u_xlat16_0.w) * 2) + u_xlat1.x);
          u_xlat1.x = ((u_xlat1.x * 2) + (-1));
          u_xlat1.x = (((-u_xlat1.x) * u_xlat1.x) + 1);
          u_xlat1.x = log2(u_xlat1.x);
          u_xlat1.x = (u_xlat1.x * _CutOffSize);
          u_xlat1.x = exp2(u_xlat1.x);
          u_xlat1.x = (((-_CutOffSharpness) * 0.5) + u_xlat1.x);
          u_xlat3 = ((-_CutOffSharpness) + 1);
          u_xlat3 = (float(1) / u_xlat3);
          u_xlat1.x = (u_xlat3 * u_xlat1.x);
          u_xlat1.x = clamp(u_xlat1.x, 0, 1);
          u_xlat3 = ((u_xlat1.x * (-2)) + 3);
          u_xlat1.x = (u_xlat1.x * u_xlat1.x);
          u_xlat1.x = (u_xlat1.x * u_xlat3);
          u_xlat3 = ((-in_v.texcoord.x) + 1);
          u_xlat1.x = (u_xlat1.x * u_xlat3);
          u_xlat1.x = (u_xlat1.x * in_v.texcoord.x);
          out_v.texcoord2 = (u_xlat1.x * 4);
          u_xlat1.x = ((-_FillInOffset) + 1);
          u_xlat1.x = (((-u_xlat16_0.w) * u_xlat1.x) + (-_FillInOffset));
          u_xlat1.x = (u_xlat1.x + 1);
          u_xlat1.x = clamp(u_xlat1.x, 0, 1);
          out_v.texcoord3 = u_xlat1.x;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float2 u_xlat16_0_d;
      float u_xlat16_1;
      float2 u_xlat16_2;
      float u_xlat16_3;
      float u_xlat4;
      float u_xlat16_4;
      float2 u_xlat10_4;
      float u_xlat16_7;
      float u_xlat16_10;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat16_0_d.x = (in_f.texcoord3 + in_f.texcoord3);
          u_xlat16_0_d.x = clamp(u_xlat16_0_d.x, 0, 1);
          u_xlat16_1 = (u_xlat16_0_d.x + (-0.899999976));
          u_xlat16_1 = (u_xlat16_1 * 9.99999809);
          u_xlat16_1 = max(u_xlat16_1, 0);
          u_xlat16_4 = ((u_xlat16_1 * (-2)) + 3);
          u_xlat16_1 = (u_xlat16_1 * u_xlat16_1);
          u_xlat16_1 = (u_xlat16_1 * u_xlat16_4);
          u_xlat10_4.xy = tex2D(_MainTex, in_f.texcoord.xy).xy.xy;
          u_xlat16_7 = dot(u_xlat10_4.yy, u_xlat16_0_d.xx);
          u_xlat16_7 = clamp(u_xlat16_7, 0, 1);
          u_xlat16_10 = (in_f.texcoord3 + (-0.5));
          u_xlat16_10 = clamp(u_xlat16_10, 0, 1);
          u_xlat16_2.xy = float2((((-float2(u_xlat16_10, u_xlat16_10)) * float2(2, 2)) + float2(1, 0.100000024)));
          u_xlat16_10 = (u_xlat16_2.y * 9.99999809);
          u_xlat16_4 = dot(u_xlat10_4.xx, u_xlat16_2.xx);
          u_xlat16_4 = clamp(u_xlat16_4, 0, 1);
          u_xlat16_10 = max(u_xlat16_10, 0);
          u_xlat16_2.x = ((u_xlat16_10 * (-2)) + 3);
          u_xlat16_10 = (u_xlat16_10 * u_xlat16_10);
          u_xlat16_10 = (u_xlat16_10 * u_xlat16_2.x);
          u_xlat16_7 = (u_xlat16_10 * u_xlat16_7);
          u_xlat16_1 = ((u_xlat16_4 * u_xlat16_1) + u_xlat16_7);
          u_xlat10_4.x = tex2D(_MainTex, in_f.texcoord1.xy).z.x;
          u_xlat16_0_d.x = ((u_xlat10_4.x * in_f.texcoord2) + u_xlat16_1);
          u_xlat4 = (u_xlat10_4.x * in_f.texcoord2);
          u_xlat16_0_d.y = ((u_xlat4 * 2) + u_xlat16_1);
          u_xlat16_0_d.xy = float2((u_xlat16_0_d.xy * float2(_AScale, _Multiply)));
          out_f.color.xyz = float3(((u_xlat16_0_d.yyy * float3(0.5, 0.5, 0.5)) + in_f.color.xyz));
          u_xlat16_0_d.x = ((u_xlat16_0_d.x * 0.5) + (-_SSMin));
          u_xlat16_3 = ((-_SSMin) + _SSMax);
          u_xlat16_3 = (float(1) / u_xlat16_3);
          u_xlat16_0_d.x = (u_xlat16_3 * u_xlat16_0_d.x);
          u_xlat16_0_d.x = clamp(u_xlat16_0_d.x, 0, 1);
          u_xlat16_3 = ((u_xlat16_0_d.x * (-2)) + 3);
          u_xlat16_0_d.x = (u_xlat16_0_d.x * u_xlat16_0_d.x);
          out_f.color.w = (u_xlat16_0_d.x * u_xlat16_3);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
