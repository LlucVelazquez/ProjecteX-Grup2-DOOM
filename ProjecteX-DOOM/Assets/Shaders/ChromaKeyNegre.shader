Shader "Custom/ChromaKeyNegre" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Threshold", Range(0,1)) = 0.1
        _Transparency ("Transparency", Range(0,1)) = 0.0
        _KeyColor ("KeyColor", Color) = (0,0,0,1)
    }
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };
            
            sampler2D _MainTex;
            float _Threshold;
            float _Transparency;
            fixed4 _KeyColor;
            
            v2f vert (appdata v) { 
                v2f o; 
                o.vertex = UnityObjectToClipPos(v.vertex); 
                o.uv = v.uv; 
                return o; 
            }
            
            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                float dist = distance(col.rgb, _KeyColor.rgb);
                
                float alpha = lerp(_Transparency, 1.0, step(_Threshold, dist));
                
                return fixed4(col.rgb, alpha);
            }
            ENDCG
        }
    }
}
