Shader "Tropicats/VFX/Alpha/colorable + overexposure + alpha + FillUpDown "
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
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _ScrollSize;
      uniform float _CutOffSize;
      uniform float _CutOffSharpness;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float texcoord1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float u_xlat2;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          u_xlat0.x = (in_v.texcoord.x + (-0.5));
          u_xlat0.x = dot(u_xlat0.xx, u_xlat0.xx);
          u_xlat0.x = ((-u_xlat0.x) + 1);
          u_xlat0.x = log2(u_xlat0.x);
          u_xlat0.x = (u_xlat0.x * _CutOffSize);
          u_xlat0.x = exp2(u_xlat0.x);
          u_xlat0.x = (((-_CutOffSharpness) * 0.5) + u_xlat0.x);
          u_xlat2 = ((-_CutOffSharpness) + 1);
          u_xlat2 = (float(1) / u_xlat2);
          u_xlat0.x = (u_xlat2 * u_xlat0.x);
          u_xlat0.x = clamp(u_xlat0.x, 0, 1);
          u_xlat2 = ((u_xlat0.x * (-2)) + 3);
          u_xlat0.x = (u_xlat0.x * u_xlat0.x);
          out_v.texcoord1 = (u_xlat0.x * u_xlat2);
          u_xlat0.x = ((in_v.color.w * 2) + (-1));
          u_xlat0.xy = float2((((-u_xlat0.xx) * _ScrollSize.zw) + in_v.texcoord.xy));
          out_v.texcoord.xy = float2((u_xlat0.xy * _ScrollSize.xy));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat0_d.w = (u_xlat0_d.w * in_f.texcoord1);
          out_f.color = u_xlat0_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
