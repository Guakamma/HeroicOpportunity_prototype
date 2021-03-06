// Toony Colors Pro+Mobile 2
// (c) 2014-2019 Jean Moreno
Shader "Toony Colors Pro 2/User/ChararacterShader"
{
    Properties
    {
    [TCP2HeaderHelp(BASE, Base Properties)]
        //TOONY COLORS
        _Color ("Color", Color) = (1,1,1,1)
        _HColor ("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
        _SColor ("Shadow Color", Color) = (0.195,0.195,0.195,1.0)
        //DIFFUSE
        _MainTex ("Main Texture", 2D) = "white" {}
    [TCP2Separator]
        //TOONY COLORS RAMP
        [TCP2Header(RAMP SETTINGS)]
        [TCP2Gradient] _Ramp            ("Toon Ramp (RGB)", 2D) = "gray" {}
    [TCP2Separator]
    [TCP2HeaderHelp(MATCAP, MatCap)]
        //MATCAP
        _MatCapR ("MatCapR (RGB)", 2D) = "black" {}
        _MatCapG ("MatCapG (RGB)", 2D) = "black" {}
        _MatCapB ("MatCapB (RGB)", 2D) = "black" {}
        _MatCapMask ("MatCap Mask", 2D) = "black" {}
    [TCP2Separator]
       [TCP2HeaderHelp(EMISSION, Emission)]
		_EmissionColor ("Emission Color", Color) = (1,1,1,1.0)
	[TCP2Separator]
	   
	   
	   //Avoid compile error if the properties are ending with a drawer
        [HideInInspector] __dummy__ ("unused", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf ToonyColorsCustom noforwardadd vertex:vert exclude_path:deferred exclude_path:prepass
        #pragma target 2.5
        //================================================================
        // VARIABLES
        fixed4 _Color;
		fixed4 _EmissionColor;
        sampler2D _MainTex;
        sampler2D _MatCapR;
        sampler2D _MatCapG;
        sampler2D _MatCapB;
        sampler2D _MatCapMask;
        #define UV_MAINTEX uv_MainTex
        struct Input
        {
            half2 uv_MainTex;
            half2 matcap;
        };
        //================================================================
        // CUSTOM LIGHTING
        //Lighting-related variables
        fixed4 _HColor;
        fixed4 _SColor;
        sampler2D _Ramp;
        // Instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        //Custom SurfaceOutput
        struct SurfaceOutputCustom
        {
            half atten;
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 Emission;
            half Specular;
            fixed Gloss;
            fixed Alpha;
        };
        inline half4 LightingToonyColorsCustom (inout SurfaceOutputCustom s, half3 viewDir, UnityGI gi)
        {
        #define IN_NORMAL s.Normal
            half3 lightDir = gi.light.dir;
        #if defined(UNITY_PASS_FORWARDBASE)
            half3 lightColor = _LightColor0.rgb;
            half atten = s.atten;
        #else
            half3 lightColor = gi.light.color.rgb;
            half atten = 1;
        #endif
            IN_NORMAL = normalize(IN_NORMAL);
            fixed ndl = max(0, dot(IN_NORMAL, lightDir));
            #define NDL ndl
            #define     RAMP_TEXTURE    _Ramp
            fixed3 ramp = tex2D(RAMP_TEXTURE, fixed2(NDL,NDL)).rgb;
        #if !(POINT) && !(SPOT)
            ramp *= atten;
        #endif
        #if !defined(UNITY_PASS_FORWARDBASE)
            _SColor = fixed4(0,0,0,1);
        #endif
            _SColor = lerp(_HColor, _SColor, _SColor.a);    //Shadows intensity through alpha
            ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
            fixed4 c;
            c.rgb = s.Albedo * lightColor.rgb * ramp;
            c.a = s.Alpha;
        #ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
            c.rgb += s.Albedo * gi.indirect.diffuse;
        #endif
            return c;
        }
        void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom s, UnityGIInput data, inout UnityGI gi)
        {
            gi = UnityGlobalIllumination(data, 1.0, IN_NORMAL);
            s.atten = data.atten;   //transfer attenuation to lighting function
            gi.light.color = _LightColor0.rgb;  //remove attenuation
        }
        //Vertex input
        struct appdata_tcp2
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
        #if defined(LIGHTMAP_ON) && defined(DIRLIGHTMAP_COMBINED)
            float4 tangent : TANGENT;
        #endif
    #if UNITY_VERSION >= 550
            UNITY_VERTEX_INPUT_INSTANCE_ID
    #endif
        };
        //================================================================
        // VERTEX FUNCTION
        void vert(inout appdata_tcp2 v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            //MatCap
            float3 worldNorm = normalize(unity_WorldToObject[0].xyz * v.normal.x + unity_WorldToObject[1].xyz * v.normal.y + unity_WorldToObject[2].xyz * v.normal.z);
            worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
            o.matcap.xy = worldNorm.xy * 0.5 + 0.5;
        }
        //================================================================
        // SURFACE FUNCTION
        void surf(Input IN, inout SurfaceOutputCustom o)
        {
            fixed4 mainTex = tex2D(_MainTex, IN.UV_MAINTEX);
            o.Albedo = mainTex.rgb * _Color.rgb;
            o.Alpha = mainTex.a * _Color.a;
            //MatCap
            fixed4 matcapMask = tex2D(_MatCapMask, IN.UV_MAINTEX);
            fixed3 matcap = tex2D(_MatCapR, IN.matcap).rgb*matcapMask.r;
            matcap += tex2D(_MatCapG, IN.matcap).rgb*matcapMask.g;
            matcap += tex2D(_MatCapB, IN.matcap).rgb*matcapMask.b;
            o.Emission += matcap.rgb * mainTex.a;

			//Emission
			half3 emissiveColor = half3(1,1,1);
			emissiveColor *= _EmissionColor.rgb * _EmissionColor.a;
			o.Emission += emissiveColor;
        }
        ENDCG
    }
    Fallback "Diffuse"
    CustomEditor "TCP2_MaterialInspector_SG"
}