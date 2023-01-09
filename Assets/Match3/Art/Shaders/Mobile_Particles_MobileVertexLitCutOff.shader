Shader "Mobile/Particles/MobileVertexLitCutOff"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _CutOff ("Cut Off", Range(0, 1)) = 0.5
  }
  SubShader
  {
    Tags
    { 
      "RenderType" = "Opaque"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "RenderType" = "Opaque"
      }
      LOD 100
      Cull Off
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
      uniform float _CutOff;
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
      float4 u_xlat10_0;
      int u_xlatb1;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          if((u_xlat10_0.w<_CutOff))
          {
              u_xlatb1 = 1;
          }
          else
          {
              u_xlatb1 = 0;
          }
          if(((int(u_xlatb1) * (-1))!=0))
          {
              discard;
          }
          out_f.color = (u_xlat10_0 * in_f.texcoord1);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
