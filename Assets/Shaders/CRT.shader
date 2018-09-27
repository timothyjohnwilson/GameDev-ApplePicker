// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/CRT"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "black" {}
        _Res ("Input Res XY / Display Res XY", Vector) = (320, 240, 320, 120)
        _FrameCount ("Frame Count", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
       
        // base pass - creates Luma signal
        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
 
            #define TEX2D(c) tex2D( _MainTex, (c) )
            #define PI 3.14159265
            #define PI_OVER_THREE PI / 3
 
 
            sampler2D _MainTex;
            float4 _Res;
            float _FrameCount;
 
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
            };
 
 
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.uv = v.texcoord;
                return o;
            }
 
 
            half4 frag (v2f i) : COLOR
            {
                float2 xy = i.uv.xy;
               
                float2 xyp = xy * _Res.zw * 4.0 * PI_OVER_THREE;
                xyp.y = xyp.y / 2.0 + 2.0 * PI_OVER_THREE * fmod( _FrameCount, 2 );
 
 
                float4 rgb = TEX2D( xy );
 
 
                float3x3 rgb2yuv = float3x3(0.299,-0.14713, 0.615,
                                            0.587,-0.28886,-0.51499,
                                            0.114, 0.436  ,-0.10001);
 
 
                float3 yuv;
                yuv = mul( rgb2yuv, rgb.rgb );
 
 
                float dx = PI_OVER_THREE;
                float c0 = yuv.x + yuv.y * sin(xyp.x+xyp.y) + yuv.z*cos(xyp.x+xyp.y);
                float c1 = yuv.x + yuv.y * sin(xyp.x+xyp.y+dx) + yuv.z * cos(xyp.x+xyp.y+dx);
                float c2 = yuv.x + yuv.y * sin(xyp.x+xyp.y+2.0*dx) + yuv.z * cos(xyp.x+xyp.y+2.0*dx);
                float c3 = yuv.x + yuv.y * sin(xyp.x+xyp.y+3.0*dx) + yuv.z * cos(xyp.x+xyp.y+3.0*dx);
 
 
                return (float4(c0,c1,c2,c3)+0.65)/2.3;
                //return float4(1,1,1,1);
            }
 
 
            ENDCG
        }
 
 
        // second pass - adds chroma signal and scanlines
        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
 
            #define TEX2D(c) tex2D( _MainTex, (c) )
            #define PI 3.14159265
            #define PI_OVER_THREE PI / 3
 
 
            sampler2D _MainTex;
            float4 _Res;
            float _FrameCount;
 
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
            };
 
 
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.uv = v.texcoord;
                return o;
            }
 
 
            half4 frag (v2f i) : COLOR
            {
                float scanlineIntensity = ( (int)( i.uv.y * _Res.y ) % 2 );// * ( (int)( i.uv.x * _Res.z ) % 2 );
                scanlineIntensity = clamp( scanlineIntensity, 0.75, 1 );
 
 
                float2 xy = i.uv.xy;
                float2 xyf = frac( xy * _Res.zw );
                float2 xyp = floor( xy * _Res.zw ) + float2( 0.5, 0.5 );
                xy = xyp / _Res.zw;
                float offs = fmod( _FrameCount, 2 )/2.0;
                float val1 = xyp.x + xyp.y / 2.0 + offs;
                float val2 = -1.0 + val1;
                float val3 = 1.0 + val1;
                float4 phases = (float4(0.0,0.25,0.5,0.75) + float4(val1,val1,val1,val1)) *4.0*PI/3.0;
                float4 phasesl = (float4(0.0,0.25,0.5,0.75) + float4(val2,val2,val2,val2)) *4.0*PI/3.0;
                float4 phasesr = (float4(0.0,0.25,0.5,0.75) + float4( val3,val3,val3,val3)) *4.0*PI/3.0;
                float4 phsin = sin(phases);
                float4 phcos = cos(phases);
                float4 phsinl= sin(phasesl);
                float4 phcosl= cos(phasesl);
                float4 phsinr= sin(phasesr);
                float4 phcosr= cos(phasesr);
                float4 phone = float4(1.0,1.0,1.0,1.0);
 
 
                float2 one = 1.0/_Res.zw;
 
                float4 c = TEX2D(xy)*2.3-0.65;
                float4 cl= TEX2D(xy + float2(-one.x,0.0))*2.3-0.65;
                float4 cr= TEX2D(xy + float2( one.x,0.0))*2.3-0.65;
 
 
                float3 yuva = float3((dot(cl.zw,phone.zw)+dot(c.xyz,phone.xyz)+0.5*(cl.y+c.w))/6.0, (dot(cl.zw,phsinl.zw)+dot(c.xyz,phsin.xyz)+0.5*(cl.y*phsinl.y+c.w*phsin.w))/3.0, (dot(cl.zw,phcosl.zw)+dot(c.xyz,phcos.xyz)+0.5*(cl.y*phcosl.y+c.w*phcos.w))/3.0);
                float3 yuvb = float3((cl.w*phone.w+dot(c.xyzw,phone.xyzw)+0.5*(cl.z+cr.x))/6.0, (cl.w*phsinl.w+dot(c.xyzw,phsin.xyzw)+0.5*(cl.z*phsinl.z+cr.x*phsinr.x))/3.0, (cl.w*phcosl.w+dot(c.xyzw,phcos.xyzw)+0.5*(cl.z*phcosl.z+cr.x*phcosr.x))/3.0);
                float3 yuvc = float3((cr.x*phone.x+dot(c.xyzw,phone.xyzw)+0.5*(cl.w+cr.y))/6.0, (cr.x*phsinr.x+dot(c.xyzw,phsin.xyzw)+0.5*(cl.w*phsinl.w+cr.y*phsinr.y))/3.0, (cr.x*phcosr.x+dot(c.xyzw,phcos.xyzw)+0.5*(cl.w*phcosl.w+cr.y*phcosr.y))/3.0);
                float3 yuvd = float3((dot(cr.xy,phone.xy)+dot(c.yzw,phone.yzw)+0.5*(c.x+cr.z))/6.0, (dot(cr.xy,phsinr.xy)+dot(c.yzw,phsin.yzw)+0.5*(c.x*phsin.x+cr.z*phsinr.z))/3.0, (dot(cr.xy,phcosr.xy)+dot(c.yzw,phcos.yzw)+0.5*(c.x*phcos.x+cr.z*phcosr.z))/3.0);
 
 
                float3x3 yuv2rgb = float3x3(1.0, 1.0, 1.0,
                                            0.0,-0.39465,2.03211,
                                            1.13983,-0.58060,0.0);
 
 
                if (xyf.x < 0.25)
                    return float4( mul( yuv2rgb, yuva ) * scanlineIntensity, 0.0);
                else if (xyf.x < 0.5)
                    return float4( mul( yuv2rgb, yuvb ) * scanlineIntensity, 0.0);
                else if (xyf.x < 0.75)
                    return float4( mul( yuv2rgb, yuvc ) * scanlineIntensity, 0.0);
                else
                    return float4( mul( yuv2rgb, yuvd ) * scanlineIntensity, 0.0);
            }
 
 
            ENDCG
        }
       
    }
    FallBack "Diffuse"
}
 