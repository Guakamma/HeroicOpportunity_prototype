//версия шейдера использует Instansing доступный с версии  GLES 3.0
//для девайсов ниже GLES 2.0 используем откат на шейдер  SynergyLineNoneInstance
Shader "Custom/SynergyLine" {
   Properties {
    [PerRendererData]_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
    Blend SrcAlpha OneMinusSrcAlpha
    ColorMask RGB
    Cull Off Lighting Off ZWrite Off  Fog { Mode Off }

    SubShader {
        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            
            fixed4 _TintColor;

            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float4 _MainTex_ST;

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _TintColor)
            
            
            UNITY_INSTANCING_BUFFER_END(Props)
            

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
               
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                o.texcoord = v.texcoord.xy ;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                fixed4 col = 2.0f * i.color * UNITY_ACCESS_INSTANCED_PROP(Props, _TintColor);
                return col; 
            }
            ENDCG
        }
    }
    Fallback "Custom/SynergyLineNoneInstance"
}
}