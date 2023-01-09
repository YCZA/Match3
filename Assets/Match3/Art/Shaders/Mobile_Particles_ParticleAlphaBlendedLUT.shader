Shader "Mobile/Particles/ParticleAlphaBlendedLUT"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _LUT ("LUT (RGB)", 2D) = "white" {}
    [PerRendererData] _ColorColumn ("Color Column", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZWrite Off
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
      uniform float4 _MainTex_ST;
      uniform float _ColorColumn;
      uniform sampler2D _MainTex;
      uniform sampler2D _LUT;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float2 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
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
          out_v.texcoord1 = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float3 u_xlat10_0;
      float3 u_xlat16_1;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = _ColorColumn;
          u_xlat0_d.yw = tex2D(_MainTex, in_f.texcoord.xy).xw.xy;
          u_xlat10_0.xyz = tex2D(_LUT, u_xlat0_d.xy).xyz.xyz;
          u_xlat16_1.xyz = float3((u_xlat0_d.www * u_xlat10_0.xyz));
          out_f.color.w = (u_xlat0_d.w * in_f.texcoord1.w);
          out_f.color.xyz = float3((u_xlat16_1.xyz * in_f.texcoord1.xyz));
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
