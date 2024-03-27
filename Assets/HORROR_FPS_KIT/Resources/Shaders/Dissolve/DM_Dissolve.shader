Shader "Dizzy Media/Dissolve" {

    Properties
    {
    
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
        
        [Space]
        
        _BumpMap ("Normal", 2D) = "bump" {}
        _NormalStrength ("Normal Strength", Range(0, 2)) = 0.17
        
        [Space]
        
        _OcclusionMap ("Occlusion", 2D) = "white" {}
        _OcclusionStrength ("Occlusion Strength", Range(0, 1)) = 0

        [Space]
        
        _NoiseTex ("Dissolve", 2D) = "white" {}

		[Header(Edge Color)]
        
        [Space]
        
		[Toggle(EDGE_COLOR)]_UseEdgeColor("Use Edge Color?", Float) = 1
        [HideIfDisabled(EDGE_COLOR)][HDR] _EdgeColor ("Edge Color", Color) = (1,1,1,1)
        [HideIfDisabled(EDGE_COLOR)]_EdgeWidth ("Edge Width", Range(0, 1)) = 0.05
        
        [Space]
        
        _Cutoff ("Amount", Range(0, 1)) = 0.25
        
        [Space]
        
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 2
        
    }//Properties
    
    SubShader
    {
    
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull [_Cull]

        CGPROGRAM
        #pragma surface surf Standard addshadow fullforwardshadows
        #pragma shader_feature EDGE_COLOR

        #ifndef SHADER_API_D3D11
        
            #pragma target 3.0
            
        #else
        
            #pragma target 4.0
            
        #endif

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;
        sampler2D _NoiseTex;
        
        half _Cutoff;
        half _EdgeWidth;

        fixed4 _Color;
        fixed4 _EdgeColor;

        struct Input
        {
        
            float2 uv_MainTex;
            float2 uv_BumpMap;
            //float2 uv_AOTex;
            float2 uv_NoiseTex;
            
        };

		half _Glossiness;
		half _Metallic;
        half _NormalStrength;
        half _OcclusionStrength;
        
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        fixed4 uvDissolve, uvMainTex, uvAO;
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
        
            uvMainTex = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //uvAO = tex2D (_OcclusionMap, IN.uv_MainTex);

            o.Albedo = uvMainTex.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
            o.Normal = UnpackScaleNormal (tex2D (_BumpMap, IN.uv_BumpMap), _NormalStrength);
            o.Occlusion = (1 - _OcclusionStrength) + tex2D(_OcclusionMap, IN.uv_MainTex).g, _OcclusionStrength;
			o.Alpha = uvMainTex.a;

            uvDissolve = tex2D (_NoiseTex, IN.uv_NoiseTex);

            clip(uvDissolve.r >= _Cutoff ? 1 : -1);
            
            #ifdef EDGE_COLOR
                
                if(_Cutoff > 0){
            
                    o.Emission = uvDissolve.r >= (_Cutoff * (_EdgeWidth + 1.0)) ? 0 : _EdgeColor;
            
                }//_Cutoff > 0
                
            #endif
            
        }//surf
        
        ENDCG
        
    }//SubShader

	FallBack "Diffuse"

}//Shader
