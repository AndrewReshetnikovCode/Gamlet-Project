Shader "Projector/Multiply" {
   Properties {
       _MainTex ("Texture", 2D) = "" {  }
      _ShadowTex ("Cookie", 2D) = "gray" {  }
      _FalloffTex ("FallOff", 2D) = "white" {  }
   }

   SubShader {
      Tags { "RenderType"="Transparent-1" }
      Pass {
          CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        
        struct v2f {
            float4 pos : SV_POSITION;
            float3 uv : TEXCOORD0;
        };

        v2f vert (float4 v : POSITION)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v);

            // TexGen ObjectLinear:
            // use object space vertex position
            o.uv = v.xyz;
            return o;
        }

        sampler2D _MainTex;
        half4 frag (v2f i) : SV_Target
        {
            return tex2D(_MainTex, i.uv.xy);
        }
        ENDCG 
         ZWrite Off
         Fog { Color (1, 1, 1) }
         AlphaTest Greater 0
         ColorMask RGB
         Blend DstColor Zero
		 Offset -1, -1
         SetTexture [_ShadowTex] {
            combine texture, ONE - texture
            Matrix [_Projector]
         }
         SetTexture [_FalloffTex] {
            constantColor (1,1,1,0)
            combine previous lerp (texture) constant
            Matrix [_ProjectorClip]
         }
      }
   }
}

