Shader "FX/Animated/MaskedGlowVertexOffset"
{
  Properties
  {
    _MainTex ("Texture (R = Gradient, G = Mask, A = Alpha)", 2D) = "white" {}
    _Noise ("Noise", 2D) = "white" {}
    _OffsetStrength ("Vertex Offset Strength", float) = 0
    _OffsetSpeed ("Vertex Offset Speed", float) = 0
    _GlowSpeed ("Glow Speed", float) = 2
    _GlowFrequency ("Glow Frequency", float) = 2
    _GlowIntensity ("Glow Intensity", float) = 1
    _MainColorA ("Gradient Color A", Color) = (0.5,0.5,0.5,0.5)
    _MainColorB ("Gradient Color B", Color) = (0.5,0.5,0.5,0.5)
    _GlowColor ("Glow Color", Color) = (0.5,0.5,0.5,0.5)
    _FadeIn ("Fade in Front-Back", Range(0, 1)) = 1
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "QUEUE" = "Transparent"
      }
      LOD 100
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile XAXIS
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _Time;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _Noise_ST;
      uniform float _OffsetStrength;
      uniform float _OffsetSpeed;
      uniform float _GlowSpeed;
      uniform float _GlowFrequency;
      uniform float _GlowIntensity;
      uniform float4 _MainColorA;
      uniform float4 _MainColorB;
      uniform float4 _GlowColor;
      uniform float _FadeIn;
      uniform sampler2D _MainTex;
      uniform sampler2D _Noise;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
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
      float u_xlat2;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.texcoord.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _MainTex));
          u_xlat0.xy = float2(TRANSFORM_TEX(in_v.texcoord.xy, _Noise));
          out_v.texcoord1.xy = float2(u_xlat0.xy);
          u_xlat2 = (_Time.x * _OffsetSpeed);
          u_xlat0.xy = float2(((u_xlat0.xx * float2(40, 50)) + float2(u_xlat2, u_xlat2)));
          u_xlat0.x = sin(u_xlat0.x);
          u_xlat2 = cos(u_xlat0.y);
          u_xlat0.x = (u_xlat2 * u_xlat0.x);
          u_xlat2 = (in_v.color.x * _OffsetStrength);
          u_xlat0.x = ((u_xlat0.x * u_xlat2) + in_v.vertex.x);
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat16_0;
      float2 u_xlat1_d;
      float u_xlat16_1;
      float u_xlat10_1;
      int u_xlati1;
      float4 u_xlat2_d;
      float4 u_xlat16_2;
      float u_xlat16_3;
      float3 u_xlat10_4;
      int u_xlati4;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat16_0.xyz = float3((_GlowColor.www * _GlowColor.xyz));
          u_xlat16_0.xyz = float3((u_xlat16_0.xyz + u_xlat16_0.xyz));
          u_xlat1_d.x = (_Time.x * _GlowSpeed);
          u_xlat1_d.x = ((in_f.texcoord1.x * _GlowFrequency) + u_xlat1_d.x);
          u_xlat1_d.y = in_f.texcoord1.y;
          u_xlat10_1 = tex2D(_Noise, u_xlat1_d.xy).x;
          u_xlat10_4.xyz = tex2D(_MainTex, in_f.texcoord.xy).xyw.xyz;
          u_xlat16_1 = (u_xlat10_4.y * u_xlat10_1);
          u_xlat16_2 = ((-_MainColorA) + _MainColorB);
          u_xlat16_2 = ((u_xlat10_4.xxxx * u_xlat16_2) + _MainColorA);
          u_xlat2_d = (u_xlat16_2 * float4(_GlowIntensity, _GlowIntensity, _GlowIntensity, _GlowIntensity));
          out_f.color.xyz = float3(((u_xlat16_0.xyz * float3(u_xlat16_1, u_xlat16_1, u_xlat16_1)) + u_xlat2_d.xyz));
          u_xlat16_0.x = (u_xlat10_4.z * u_xlat2_d.w);
          u_xlat16_3 = ((-_FadeIn) + 1);
          u_xlat1_d.x = ((-u_xlat16_3) + in_f.texcoord.x);
          if(int(((0<u_xlat1_d.x))?((-1)):(0)))
          {
              u_xlati4 = 1;
          }
          else
          {
              u_xlati4 = 0;
          }
          if(int(((u_xlat1_d.x<0))?((-1)):(0)))
          {
              u_xlati1 = 1;
          }
          else
          {
              u_xlati1 = 0;
          }
          u_xlati1 = ((-u_xlati4) + u_xlati1);
          u_xlat16_3 = float(u_xlati1);
          out_f.color.w = (u_xlat16_3 * u_xlat16_0.x);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
