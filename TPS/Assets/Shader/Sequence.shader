Shader "Custom/Sequence" {
	Properties{
        _MainTex     ("RGB：颜色 ； A：透贴" , 2D)        = "white"{}
        _Opacity     ("不透明度" , Range(0.0 , 1.0))     = 1.0
        _Pass2Scale  ("Pass2顶点缩放" , Float)           = 0.0
        _SequenceTex ("序列帧图" , 2D)                   = "gray"{}
        _LineCount   ("行数" , Int)                      = 1
        _ColumnCount ("列数" , Int)                      = 1
        _Speed       ("速度" , Float)                    = 5.0
	}
	SubShader{
		Tags {
			"Queue" = "Transparent"              //渲染队列3000
            "RenderType" = "Transparent"         //把Shader归入到提前定义的组(这里是Transparent组)
                                                 //以指明该Shader使用了透明度测试
                                                 //RenderType标签通常被用于着色器替换功能
            "IgnoreProjector" = "True"           //shader不会受到投影器(Projectors)的影响
            "ForceNoShadowCasting" = "True"      //关闭阴影投射
		}
        //【SubShader LOD值（越大，配置越高）】
        LOD 500

        //【Pass1】 ：AB，MainTex
		Pass {
			Name "FORWARD_AB"
			Tags {
				"LightMode" = "ForwardBase"
			}
			ZWrite Off							//关闭深度写入
            Blend One OneMinusSrcAlpha          //修改混合方式


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase_fullshadows
			#pragma target 3.0

            uniform sampler2D _MainTex;
            uniform half      _Opacity;

			struct VertexInput {
				float4 vertex : POSITION;
                float2 uv0    : TEXCOORD0;
			};
			struct VertexOutput {
				float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
			};
			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv0 = v.uv0;
				return o;
			}

			half4 frag(VertexOutput i) : SV_TARGET {

                half4 var_MainTex = tex2D(_MainTex , i.uv0);	
                half  opacity = _Opacity * var_MainTex.a;
                half3 finalRGB = var_MainTex.rgb;
				return half4(finalRGB * opacity , opacity);	       
			}
				ENDCG
		}
        
        //【Pass2】：AD，帧序列动画
        Pass {
            Name "FORWARD_AD"
            Tags {
                "LightMode" = "ForwardBase"
            }
            ZWrite Off			   //关闭深度写入 
            Blend One One          //修改混合方式

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma target 3.0

            uniform half      _Opacity;
            uniform float     _Pass2Scale;
            uniform sampler2D _SequenceTex; uniform float4 _SequenceTex_ST;
            uniform half      _LineCount;           //行数，注意为Half
            uniform half      _ColumnCount;         //列数，注意为Half
            uniform float     _Speed;
            
            struct VertexInput {
                float4 vertex : POSITION;
                float2 uv0    : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert(VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                    v.vertex.xyz *=_Pass2Scale * 0.01;              //缩放顶点位置
                    o.pos = UnityObjectToClipPos(v.vertex);
                    //【帧序列uv】--------------
                    o.uv0 = v.uv0 * _SequenceTex_ST.xy + _SequenceTex_ST.zw;        //必须写在最前面，这样帧序列才能计算正确
                    //【进行到第几帧】
                    int sequId = floor(_Time.y * _Speed);           //定义第几帧(图)
                    //【定义行列步长】
                    float stepU = 1.0 / _ColumnCount;               //行步长
                    float stepV = 1.0 / _LineCount;                 //列步长
                    //【当前帧位置，第几行，第几列】
                    int idV = floor(sequId / _ColumnCount);     //第几行
                    int idU = fmod(sequId , _ColumnCount);      //第几列
                    //【设定其实位置，左上角】
                    o.uv0 *= float2(stepU , stepV);      //uv缩放,当前定位：左下角
                    o.uv0 += float2(0.0 , 1 - stepV);                //uv向上偏移，当前定位: 左上角
                    //【uv滚动】
                    o.uv0 += float2(stepU * idU , -stepV * idV);     //uv从左上角到右下角运动
                return o;
            }

            half4 frag(VertexOutput i) : SV_TARGET {
                half4 var_SequenceTex = tex2D(_SequenceTex , i.uv0);	
                half  opacity = _Opacity * var_SequenceTex.a;
                half3 finalRGB = var_SequenceTex.rgb;			
                return half4(finalRGB * opacity , opacity);	        
            }
                ENDCG
        }
	}					
}