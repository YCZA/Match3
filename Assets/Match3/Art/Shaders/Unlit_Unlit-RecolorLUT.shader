Shader "Unlit/Unlit-RecolorLUT"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _Mask ("Color Mask", 2D) = "white" {}
    _LUT ("LUT (RGB)", 2D) = "white" {}
    _ColorValue ("Color Value", Range(0, 1)) = 0
    _NextColorValue ("Next Color Value", Range(0, 1)) = 0
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
      uniform float4 _MainTex_ST;
      uniform float _ColorValue;
      uniform float _NextColorValue;
      uniform sampler2D _MainTex;
      uniform sampler2D _Mask;
      uniform sampler2D _LUT;
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
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat10_0;
      float4 u_xlat10_1;
      float3 u_xlat16_2;
      float3 u_xlat10_3;
      float3 u_xlat16_4;
      float u_xlat16_15;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xz = float2(_ColorValue, _NextColorValue);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_2.x = max(u_xlat10_1.x, 0.0199999996);
          u_xlat16_15 = min(u_xlat16_2.x, 0.980000019);
          u_xlat0_d.yw = float2(u_xlat16_15, u_xlat16_15);
          u_xlat10_3.xyz = tex2D(_LUT, u_xlat0_d.xy).xyz.xyz;
          u_xlat10_0.xzw = tex2D(_LUT, u_xlat0_d.zw).xyz.xyz;
          u_xlat16_2.xyz = float3(((-u_xlat10_0.xzw) + u_xlat10_3.xyz));
          u_xlat10_3.xy = tex2D(_Mask, in_f.texcoord.xy).xy.xy;
          u_xlat16_2.xyz = float3(((u_xlat10_3.xxx * u_xlat16_2.xyz) + u_xlat10_0.xzw));
          u_xlat16_4.x = (-u_xlat0_d.y);
          u_xlat16_4.yz = (-u_xlat10_1.yz);
          u_xlat16_2.xyz = float3((u_xlat16_2.xyz + u_xlat16_4.xyz));
          u_xlat16_4.x = ((u_xlat10_3.y * u_xlat16_2.x) + u_xlat0_d.y);
          u_xlat16_4.yz = ((u_xlat10_3.yy * u_xlat16_2.yz) + u_xlat10_1.yz);
          out_f.color.xyz = float3((u_xlat10_1.www * u_xlat16_4.xyz));
          out_f.color.w = u_xlat10_1.w;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
