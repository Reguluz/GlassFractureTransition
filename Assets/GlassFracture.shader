Shader "Unlit/GlassFracture"
{
    Properties
    {
        //_ScreenCopyTexture("grab tex", 2D) = "bump" {}
        [HDR]_Color("Color", Color) = (1,1,1,1)
        _Fresnel("Fresnel", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue" = "Transparent"
        }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            //Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD2;
                float3 normal : TEXCOORD3;
            };

            uniform sampler2D _ScreenCopyTexture;
            float4 _Color;

            inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		    {
			    #if UNITY_UV_STARTS_AT_TOP
			    float scale = -1.0;
			    #else
			    float scale = 1.0;
			    #endif
			    float4 o = pos;
			    o.y = pos.w * 0.5f;
			    o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			    return o;
		    }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos  = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                // float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
                // float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
                // float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
                fixed4 col = tex2D(_ScreenCopyTexture, i.uv/*ase_grabScreenPosNorm.xy*/);
                //col.xyz -= float3(0.3, 0.3, 0.3);
                // apply fog
                float3 dots = dot(float3(0,0,1), i.normal) * _Color.rgb;
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                col.rgb = max(col.rgb, dots);
                return col;
            }
            ENDCG
        }
    }
}
