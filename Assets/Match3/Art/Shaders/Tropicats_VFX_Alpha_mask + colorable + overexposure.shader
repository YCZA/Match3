Shader "Tropicats/VFX/Alpha/mask + colorable + overexposure"
{
  Properties
  {
    _MainTex ("Texture Mask (RGB)", 2D) = "white" {}
    _Multiply ("Overexposure", float) = 1
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
      uniform float _Multiply;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 color :COLOR0;
          float3 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 color :COLOR0;
          float3 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 color :COLOR0;
          float3 texcoord :TEXCOORD0;
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
          out_v.texcoord.xyz = float3(in_v.texcoord.xyz);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat0_d;
      float3 u_xlat10_0;
      float4 u_xlat1_d;
      float u_xlat4;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_MainTex, in_f.texcoord.xy).xyz.xyz;
          u_xlat4 = (u_xlat10_0.z + in_f.texcoord.z);
          u_xlat4 = floor(u_xlat4);
          u_xlat1_d.w = ((in_f.color.w * u_xlat10_0.y) + (-u_xlat4));
          u_xlat1_d.w = clamp(u_xlat1_d.w, 0, 1);
          u_xlat0_d = (u_xlat10_0.x * _Multiply);
          u_xlat1_d.xyz = float3((float3(u_xlat0_d, u_xlat0_d, u_xlat0_d) * in_f.color.xyz));
          out_f.color = u_xlat1_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
