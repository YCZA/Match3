Shader "UI/Image Glow"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _GlowColor ("Glow Color", Color) = (1,1,1,1)
    _GlowSpread ("Glow Spread", Range(0, 0.1)) = 0.01
    _GlowBlur ("Glow Blur", Range(0, 5)) = 4
    _GlowIntensity ("Glow Intensity", Range(0, 10)) = 1.25
    [Toggle(USE_DIAGONAL_BLUR)] _UseDiagonalBlur ("Advanced Blur", float) = 0
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
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _GlowColor;
      uniform float _GlowSpread;
      uniform float _GlowBlur;
      uniform float _GlowIntensity;
      uniform float4 _MainTex_TexelSize;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.texcoord.xy = float2(in_v.texcoord.xy);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float u_xlat16_0;
      float u_xlat10_0;
      float4 u_xlat1_d;
      float4 u_xlat16_1;
      float4 u_xlat2;
      float4 u_xlat16_2;
      float4 u_xlat3;
      float u_xlat10_3;
      float u_xlat10_4;
      float u_xlat10_5;
      float u_xlat16_6;
      float u_xlat10_7;
      float u_xlat10_10;
      float u_xlat10_11;
      float u_xlat16_14;
      float u_xlat10_14;
      float u_xlat10_18;
      float u_xlat10_21;
      float u_xlat10_25;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xy = float2((_MainTex_TexelSize.xy * float2(256, 256)));
          u_xlat0_d.xw = (u_xlat0_d.xy * float2(_GlowSpread, _GlowSpread));
          u_xlat0_d.y = float(0);
          u_xlat0_d.z = float(0);
          u_xlat0_d = (u_xlat0_d + in_f.texcoord.xyxy);
          u_xlat10_0 = tex2Dlod(_MainTex, float4(float3(u_xlat0_d.xy, 0), _GlowBlur)).w;
          u_xlat10_7 = tex2Dlod(_MainTex, float4(float3(u_xlat0_d.zw, 0), _GlowBlur)).w;
          u_xlat16_1.xw = float2(_GlowSpread, _GlowSpread);
          u_xlat16_1.y = float(0);
          u_xlat16_1.z = float(0);
          u_xlat16_2 = (u_xlat16_1.wzzw * float4(-3, (-3), (-3), (-3)));
          u_xlat3 = (_MainTex_TexelSize.xyxy * float4(256, 256, 256, 256));
          u_xlat2 = ((u_xlat16_2 * u_xlat3) + in_f.texcoord.xyxy);
          u_xlat10_14 = tex2Dlod(_MainTex, float4(float3(u_xlat2.xy, 0), _GlowBlur)).w;
          u_xlat10_21 = tex2Dlod(_MainTex, float4(float3(u_xlat2.zw, 0), _GlowBlur)).w;
          u_xlat16_14 = (u_xlat10_14 * 0.0900000036);
          u_xlat16_2 = (u_xlat16_1.wzzw * float4(-4, (-4), (-4), (-4)));
          u_xlat2 = ((u_xlat16_2 * u_xlat3) + in_f.texcoord.xyxy);
          u_xlat10_4 = tex2Dlod(_MainTex, float4(float3(u_xlat2.xy, 0), _GlowBlur)).w;
          u_xlat10_11 = tex2Dlod(_MainTex, float4(float3(u_xlat2.zw, 0), _GlowBlur)).w;
          u_xlat16_14 = ((u_xlat10_4 * 0.0500000007) + u_xlat16_14);
          u_xlat16_2 = (u_xlat16_1.wzzw * float4(-2, (-2), (-2), (-2)));
          u_xlat2 = ((u_xlat16_2 * u_xlat3) + in_f.texcoord.xyxy);
          u_xlat10_4 = tex2Dlod(_MainTex, float4(float3(u_xlat2.xy, 0), _GlowBlur)).w;
          u_xlat10_18 = tex2Dlod(_MainTex, float4(float3(u_xlat2.zw, 0), _GlowBlur)).w;
          u_xlat16_14 = ((u_xlat10_4 * 0.119999997) + u_xlat16_14);
          u_xlat2 = (((-u_xlat16_1.wzzw) * u_xlat3) + in_f.texcoord.xyxy);
          u_xlat10_4 = tex2Dlod(_MainTex, float4(float3(u_xlat2.xy, 0), _GlowBlur)).w;
          u_xlat10_25 = tex2Dlod(_MainTex, float4(float3(u_xlat2.zw, 0), _GlowBlur)).w;
          u_xlat16_14 = ((u_xlat10_4 * 0.150000006) + u_xlat16_14);
          u_xlat10_4 = tex2Dlod(_MainTex, float4(float3(in_f.texcoord.xy, 0), _GlowBlur)).w;
          u_xlat16_14 = ((u_xlat10_4 * 0.180000007) + u_xlat16_14);
          u_xlat16_0 = ((u_xlat10_0 * 0.150000006) + u_xlat16_14);
          u_xlat16_2 = (u_xlat16_1.wzzw + u_xlat16_1.wzzw);
          u_xlat2 = ((u_xlat16_2 * u_xlat3) + in_f.texcoord.xyxy);
          u_xlat10_14 = tex2Dlod(_MainTex, float4(float3(u_xlat2.xy, 0), _GlowBlur)).w;
          u_xlat10_5 = tex2Dlod(_MainTex, float4(float3(u_xlat2.zw, 0), _GlowBlur)).w;
          u_xlat16_0 = ((u_xlat10_14 * 0.119999997) + u_xlat16_0);
          u_xlat16_2 = (u_xlat16_1.wzzw * float4(3, 3, 3, 3));
          u_xlat16_1 = (u_xlat16_1 * float4(4, 4, 4, 4));
          u_xlat1_d = ((u_xlat16_1 * u_xlat3) + in_f.texcoord.xyxy);
          u_xlat2 = ((u_xlat16_2 * u_xlat3) + in_f.texcoord.xyxy);
          u_xlat10_14 = tex2Dlod(_MainTex, float4(float3(u_xlat2.xy, 0), _GlowBlur)).w;
          u_xlat10_3 = tex2Dlod(_MainTex, float4(float3(u_xlat2.zw, 0), _GlowBlur)).w;
          u_xlat16_0 = ((u_xlat10_14 * 0.0900000036) + u_xlat16_0);
          u_xlat10_14 = tex2Dlod(_MainTex, float4(float3(u_xlat1_d.xy, 0), _GlowBlur)).w;
          u_xlat10_10 = tex2Dlod(_MainTex, float4(float3(u_xlat1_d.zw, 0), _GlowBlur)).w;
          u_xlat16_0 = ((u_xlat10_14 * 0.0500000007) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_11 * 0.0500000007) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_21 * 0.0900000036) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_18 * 0.119999997) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_25 * 0.150000006) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_4 * 0.180000007) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_7 * 0.150000006) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_5 * 0.119999997) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_3 * 0.0900000036) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_10 * 0.0500000007) + u_xlat16_0);
          u_xlat16_6 = (u_xlat16_0 * _GlowColor.w);
          out_f.color.w = (u_xlat16_6 * _GlowIntensity);
          out_f.color.xyz = float3(_GlowColor.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: 
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
      Blend SrcAlpha OneMinusSrcAlpha
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
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.texcoord.xy = float2(in_v.texcoord.xy);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat16_0;
      float4 u_xlat10_0;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2Dlod(_MainTex, float4(float3(in_f.texcoord.xy, 0), 0));
          u_xlat16_0 = (u_xlat10_0 * _Color);
          out_f.color = u_xlat16_0;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
