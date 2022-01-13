Shader "Custom/RimLight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0)
        _Power ("Power", float) = 3
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        
        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        struct Input
        {
            float3 viewDir;
        };

        fixed4 _Color;
        float _Power;

        void surf (Input IN, inout SurfaceOutput o)
        {
            half rim = 1.0 - saturate(dot( normalize(IN.viewDir), o.Normal));
            o.Emission = _Color.rgb * pow (rim, _Power);
            o.Alpha = pow (rim, _Power) * (sin(_Time.w) + 1);// (sin(_Time.w) + 1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
