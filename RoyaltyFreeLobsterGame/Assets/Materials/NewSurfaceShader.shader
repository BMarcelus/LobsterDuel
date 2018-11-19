 
Shader "Custom/NewShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        //_transparent("transparent slider", Range(0,1) = 0.5
        _transparency ("transparency", Float) = 1
        _transparencyOscillation ("transparency oscillation", Float) = 1
    }
    SubShader {
        Tags { "Queue"= "Transparent" "RenderType"="Transparent" }
        LOD 200
        Zwrite off
        Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert alpha
        
 
        sampler2D _MainTex;
        float4 _MainTex_ST;

        float _transparency;
        float _transparencyOscillation;
 
        struct Input {
            float2 st_MainTex;
        };
 
        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
 
            o.st_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
 
            // add distortion
            // this is the part you need to modify, i  recomment to expose such
            // hard-coded values to the inspector for easier tweaking.
            o.st_MainTex.x += sin((o.st_MainTex.x+o.st_MainTex.y)*1 + _Time.g*.3)*0.02;
            o.st_MainTex.y += cos((o.st_MainTex.x-o.st_MainTex.y)*1 + _Time.g*.7)*0.02;
        }
 
        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.st_MainTex);
            o.Albedo = c.rgb; 
            float t = _transparency;
            t = (sin(_Time.g)+1)/2*(t*_transparencyOscillation) + t*(1-_transparencyOscillation);
            o.Alpha = t;
          
        }
        ENDCG
    }
    FallBack "Diffuse"
}
 
 