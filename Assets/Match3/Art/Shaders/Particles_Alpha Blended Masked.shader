Shader "Particles/Alpha Blended Masked"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _Mask ("Mask", 2D) = "white" {}
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _Intensity ("Intensity", float) = 1
  }
  SubShader
  {
    Tags
    { 
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZTest Always
      ZWrite Off
      Cull Off
      Blend One One
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
      uniform float4 _Mask_ST;
      uniform float4 _TintColor;
      uniform float _Intensity;
      uniform sampler2D _MainTex;
      uniform sampler2D _Mask;
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
          float4 texcoord2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
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
          out_v.texcoord1.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _Mask));
          out_v.texcoord2 = in_v.color;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat0_d;
      float4 u_xlat10_0;
      float3 u_xlat16_1;
      float u_xlat10_2;
      float3 u_xlat3;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_1.xyz = float3((u_xlat10_0.xyz * _TintColor.xyz));
          u_xlat0_d = (u_xlat10_0.w * _Intensity);
          u_xlat3.xyz = float3((u_xlat16_1.xyz * float3(_Intensity, _Intensity, _Intensity)));
          u_xlat10_2 = tex2D(_Mask, in_f.texcoord1.xy).x;
          u_xlat16_1.x = (u_xlat10_2 * in_f.texcoord2.x);
          u_xlat16_1.x = (u_xlat0_d * u_xlat16_1.x);
          out_f.color.xyz = float3((u_xlat3.xyz * u_xlat16_1.xxx));
          out_f.color.w = u_xlat16_1.x;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
