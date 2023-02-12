Shader "Hole"
{
        SubShader
        {
            Tags{ "RenderType" = "Transparent" }
 
            Stencil{
            Ref 2
            Comp always
            Pass replace
        }
 
        CGPROGRAM
        #pragma surface surf Lambert alpha
 
        struct Input 
        {
            float2 uv_MainTex;
        };
 
        void surf(Input IN, inout SurfaceOutput o) 
        {
            o.Albedo = fixed3(1, 1, 1);
            o.Alpha = 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}