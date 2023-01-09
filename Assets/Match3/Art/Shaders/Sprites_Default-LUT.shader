Shader "Sprites/Default-LUT"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture (R, A)", 2D) = "white" {}
    _LUT ("LUT (RGB)", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    [MaterialToggle] PixelSnap ("Pixel snap", float) = 0
    [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
    [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
    [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", float) = 0
    [PerRendererData] _Tint ("Tint", Color) = (1,1,1,1)
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
      Blend One OneMinusSrcAlpha
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
      uniform float4 _Tint;
      uniform sampler2D _MainTex;
      uniform sampler2D _LUT;
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
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 color :COLOR0;
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
          out_v.color = in_v.color;
          out_v.texcoord.xy = float2(in_v.texcoord.xy);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float3 u_xlat10_0;
      float4 u_xlat16_1;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = in_f.color.x;
          u_xlat0_d.yw = tex2D(_MainTex, in_f.texcoord.xy).xw.xy;
          u_xlat16_1.w = (u_xlat0_d.w + (-in_f.color.w));
          u_xlat10_0.xyz = tex2D(_LUT, u_xlat0_d.xy).xyz.xyz;
          u_xlat16_1.xyz = float3((u_xlat16_1.www * u_xlat10_0.xyz));
          u_xlat0_d = (_Color * _Tint);
          u_xlat0_d = (u_xlat0_d * u_xlat16_1);
          out_f.color = u_xlat0_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
