Shader "Unlit/FluidBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 0.1
        _AlphaFalloff ("Alpha Falloff", Range(0.0, 0.05)) = 0.01

        _Fill ("Filling status", Range(0.0, 1.0)) = 0.5
        _Amp ("Wave Amplitude", Range(0.0, 0.1)) = 0.1
        _Wavelen ("Wavelength", Range(0.0, 30.0)) = 0.3
        _Speed ("Wavespeed", Range(0.0, 2.0)) = 1.0

        _layerCount ("Layer Count", Int) = 5
        _layerAmpScale ("Layer Amplitude Scale", Range(0.0, 1.0)) = 1.13235243
        _layerSpeedScale ("Layer Speed Scale", Range(1.0, 2.0)) = 1.13235243
        _layerWavelenScale ("Layer Wavelength Scale", Range(1.0, 2.0)) = 1.232235

        _XScale ("X Scale Factor", Float) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;
            float _AlphaFalloff;

            float _Fill;
            float _Amp;
            float _Wavelen;
            float _Speed;

            int _layerCount;
            float _layerAmpScale;
            float _layerSpeedScale;
            float _layerWavelenScale;

            //float _XScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);// sample the texture
                clip(col.a - _Cutoff);

                float rescaledFill = _Fill * (1 + 2*_Amp) - _Amp;
                float wavePos = i.uv.y - rescaledFill;

                float amp = _Amp;
                float speed = _Speed;
                float wavelen = _Wavelen;// * _XScale;
                for(int l = 0; l < _layerCount; l++){
                    wavePos += sin(i.uv.x * wavelen + _Time.g * speed) * amp;
                    amp *= _layerAmpScale;
                    speed *= -_layerSpeedScale;//reverse the sign -> no clear flowing direction
                    wavelen *= _layerWavelenScale;
                }
                clip(-wavePos);
                col.a *= saturate(-wavePos / _AlphaFalloff);

                return float4(col.rgb, col.a);
            }
            ENDCG
        }
    }
}
