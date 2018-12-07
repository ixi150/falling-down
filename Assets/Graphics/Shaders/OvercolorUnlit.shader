Shader "Unlit/Overcolor"
{
    Properties 
	{
        _Color ("Color", Color) = (1,1,1,1)
		_Multiply ("Multiply", Float) = 1
    }
    SubShader 
	{
        Tags { "RenderType"="Transpadent" }
       
        Pass 
		{
           Blend SrcAlpha OneMinusSrcAlpha
            Fog { Mode Off }
            ZWrite On
            ZTest LEqual
            Cull Back
            Lighting Off
   
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
               
                float4 _Color;
				float _Multiply;

                struct appdata {
                    float4 vertex : POSITION;
                };
               
                struct v2f {
                    float4 vertex : POSITION;
                };
               
                v2f vert (appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    return o;
                }
               
                fixed4 frag (v2f i) : SV_TARGET {
                    return _Color * _Multiply;
                }
            ENDCG
   
        }
    }
}