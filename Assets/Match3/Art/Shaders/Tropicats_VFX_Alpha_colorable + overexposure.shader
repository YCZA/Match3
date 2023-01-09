Shader "Tropicats/VFX/Alpha/colorable + overexposure"
{
  Properties
  {
    _MainTex ("Texture Mask (RGB)", 2D) = "white" {}
    _AScale ("Alpha Scale", float) = 1
    _Multiply ("Overexposure", float) = 1
    _SSMin ("Smoothstep Mask Min", Range(0, 1)) = 0.1
    _SSMax ("Smoothstep Mask Max", Range(0, 1)) = 1
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
      float u_xlat0_d;
      float4 u_xlat1_d;
      float u_xlat16_1;
      float u_xlat2;
      float u_xlat10_2;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = ((-_SSMin) + _SSMax);
          u_xlat0_d = (float(1) / u_xlat0_d);
          u_xlat10_2 = tex2D(_MainTex, in_f.texcoord.xy).x;
          u_xlat16_1 = (u_xlat10_2 * in_f.color.w);
          u_xlat2 = ((u_xlat16_1 * _AScale) + (-_SSMin));
          u_xlat1_d.xyz = float3(((float3(u_xlat16_1, u_xlat16_1, u_xlat16_1) * float3(_Multiply, _Multiply, _Multiply)) + in_f.color.xyz));
          u_xlat0_d = (u_xlat0_d * u_xlat2);
          u_xlat0_d = clamp(u_xlat0_d, 0, 1);
          u_xlat2 = ((u_xlat0_d * (-2)) + 3);
          u_xlat0_d = (u_xlat0_d * u_xlat0_d);
          u_xlat1_d.w = (u_xlat0_d * u_xlat2);
          out_f.color = u_xlat1_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
