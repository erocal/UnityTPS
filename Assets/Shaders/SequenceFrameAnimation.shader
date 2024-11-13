Shader "Custom/SequenceFrameAnimation"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
        [PerRendererData]_MainTex("Image Sequence", 2D) = "white" {}
        _HorizontalAmount("Horizontal Amount", Float) = 2
        _VerticalAmount("VerticalAmount", Float) = 2
        _Speed("Speed", Range(1, 10)) = 30
    }
    SubShader
    {
        LOD 200
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}     
        PASS
        {
            Tags { "LightMode" = "ForwardBase" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
   
            CGPROGRAM
            #pragma vertex vert             
            #pragma fragment frag           
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
   
            fixed4 _Color;
            float _Speed;
            float _HorizontalAmount;
            float _VerticalAmount;
            sampler2D _MainTex;

            float4 _MainTex_ST;
            struct _2vert
            {
                float4 vertex: POSITION;
                float4 texcoord: TEXCOORD0;
            };
            struct vert2frag
            {
                float4 pos: SV_POSITION;
                float2 uv: TEXCOORD0;
            };
   
            vert2frag vert(_2vert v)
            {
                vert2frag o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
            fixed4 frag(vert2frag i) : SV_Target
            {
            	// 獲取時間，向下取整，以_Speed做調整
                float time = floor(_Time.y * _Speed);
                
                //獲取當前圖片ID
                float row = time % (_HorizontalAmount * _VerticalAmount);
				
				//獲取處於哪一行
                float posx = floor(row % _HorizontalAmount);
                //獲取處於哪一列
                //Unity的紋理座標垂直方向的順序和序列幀紋理中垂直方向上的順序是相反的
                //因此要倒過來取
                float posy = _VerticalAmount - floor(row / _HorizontalAmount) - 1;
				
				//添加到uv座標上
                half2 uv = i.uv + half2(posx, posy);
                //縮小範圍
                uv.x /= _HorizontalAmount;
                uv.y /= _VerticalAmount;
                
                fixed4 color = tex2D(_MainTex, uv);
                color.rgb *= _Color;
                return color;

            }
            ENDCG
        }
    }
    FallBack "Transparent/VertexLit"
}
