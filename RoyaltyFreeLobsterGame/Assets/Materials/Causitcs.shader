Shader "Custom/Caustics" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _noiseTexture("noise texture", 2D) = "white"{}
        _transparency ("transparency", Range(0,1)) = .5
        _distortStrength("strength of the distortion", range(0,.1))=.025
        _distortionSpeed("speed of the distortion movement",range(0,.1))=.05
    }
    SubShader {
        Tags { "Queue"= "Transparent" "RenderType"="Transparent" }
        LOD 200
        Zwrite off
        Cull Off
        Lighting Off
        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency
        Pass{
        
        CGPROGRAM
        
        #pragma vertex vert
        #pragma fragment frag
        
        #include "UnityCG.cginc"
        sampler2D _MainTex;
        float4 _MainTex_ST;

        float _transparency;
        float _transparencyOscillation;

 
        struct appdata {
            float4 vertex: POSITION;
            float2 uv: TEXCOORD0;
        };
        struct v2f{
            float4 vertex: SV_POSITION;
            float2 uv:TEXCOORD0;
        };
 
        v2f vert (appdata v)
        {
            v2f o;   
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }
 
        sampler2D _noiseTexture;
        float4 _noiseTexture_st;
        float _distortStrength,_distortionSpeed;
        float _cutoff;
        float3 _Tint;

        fixed4 frag(v2f i) : SV_TARGET{
            float2 uv = i.uv;
        
            float4 noise = tex2D(_noiseTexture,i.uv + _Time*_distortionSpeed);
            uv.x += noise.r*_distortStrength;
            uv.y += noise.r*_distortStrength;
            half4 c = tex2D (_MainTex,uv);
            c.a *= _transparency * noise.r;
            return c;
          
        }
        ENDCG
    }
  }
    FallBack "Diffuse"
}
 
 