Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_MainColor("Color", Color) = (0, 0, 0, 0)
		_speed("Speed", Float) = 1
	}
	SubShader
	{
		Tags {"Queue" = "Transparent"  "RenderType"="Transparent" }
		LOD 100
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};
#define hlsl_atan(x,y) atan2(x, y)
#define mod(x,y) ((x)-(y)*floor((x)/(y)))
inline float4 textureLod(sampler2D tex, float2 uv, float lod) {
    return tex2D(tex, uv);
}
inline float2 tofloat2(float x) {
    return float2(x, x);
}
inline float2 tofloat2(float x, float y) {
    return float2(x, y);
}
inline float3 tofloat3(float x) {
    return float3(x, x, x);
}
inline float3 tofloat3(float x, float y, float z) {
    return float3(x, y, z);
}
inline float3 tofloat3(float2 xy, float z) {
    return float3(xy.x, xy.y, z);
}
inline float3 tofloat3(float x, float2 yz) {
    return float3(x, yz.x, yz.y);
}
inline float4 tofloat4(float x, float y, float z, float w) {
    return float4(x, y, z, w);
}
inline float4 tofloat4(float x) {
    return float4(x, x, x, x);
}
inline float4 tofloat4(float x, float3 yzw) {
    return float4(x, yzw.x, yzw.y, yzw.z);
}
inline float4 tofloat4(float2 xy, float2 zw) {
    return float4(xy.x, xy.y, zw.x, zw.y);
}
inline float4 tofloat4(float3 xyz, float w) {
    return float4(xyz.x, xyz.y, xyz.z, w);
}
inline float4 tofloat4(float2 xy, float z, float w) {
    return float4(xy.x, xy.y, z, w);
}
inline float2x2 tofloat2x2(float2 v1, float2 v2) {
    return float2x2(v1.x, v1.y, v2.x, v2.y);
}
// EngineSpecificDefinitions
float rand(float2 x) {
    return frac(cos(mod(dot(x, tofloat2(13.9898, 8.141)), 3.14)) * 43758.5);
}
float2 rand2(float2 x) {
    return frac(cos(mod(tofloat2(dot(x, tofloat2(13.9898, 8.141)),
						      dot(x, tofloat2(3.4562, 17.398))), tofloat2(3.14))) * 43758.5);
}
float3 rand3(float2 x) {
    return frac(cos(mod(tofloat3(dot(x, tofloat2(13.9898, 8.141)),
							  dot(x, tofloat2(3.4562, 17.398)),
                              dot(x, tofloat2(13.254, 5.867))), tofloat3(3.14))) * 43758.5);
}
float param_rnd(float minimum, float maximum, float seed) {
	return minimum+(maximum-minimum)*rand(tofloat2(seed));
}
float param_rndi(float minimum, float maximum, float seed) {
	return floor(param_rnd(minimum, maximum + 1.0, seed));
}
float3 rgb2hsv(float3 c) {
	float4 K = tofloat4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
	float4 p = c.g < c.b ? tofloat4(c.bg, K.wz) : tofloat4(c.gb, K.xy);
	float4 q = c.r < p.x ? tofloat4(p.xyw, c.r) : tofloat4(c.r, p.yzx);
	float d = q.x - min(q.w, q.y);
	float e = 1.0e-10;
	return tofloat3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
float3 hsv2rgb(float3 c) {
	float4 K = tofloat4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
	float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
	return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
static const float p_o273938389109_translate_x = 0.000000000;
static const float p_o273938389109_translate_y = 0.000000000;
static const float p_o273938389109_rotate = 0.000000000;
static const float p_o273938389109_scale_x = 0.600000000;
static const float p_o273938389109_scale_y = 0.600000000;
static const float seed_o273871280243 = 0.335305810;
static const float p_o273871280243_count = 5.000000000;
static const float p_o273871280243_rings = 1.000000000;
static const float p_o273871280243_scale_x = 2.000000000;
static const float p_o273871280243_scale_y = 4.500000000;
static const float p_o273871280243_radius = 0.250000000;
static const float p_o273871280243_spiral = 0.000000000;
static const float p_o273871280243_i_rotate = 0.000000000;
static const float p_o273871280243_i_scale = 0.000000000;
static const float p_o273871280243_rotate = 0.000000000;
static const float p_o273871280243_scale = 0.000000000;
static const float p_o273871280243_value = 0.500000000;
static const float seed_o273602844772 = 0.820258379;
static const float p_o273602844772_rotate = 0.000000000;
static const float p_o273602844772_scale_x = 0.500000000;
static const float p_o273602844772_scale_y = 0.500000000;
static const float p_o272579434572_cx = 0.000000000;
static const float p_o272579434572_cy = 0.000000000;
static const float p_o272579434572_scale_x = 1.000000000;
static const float p_o272579434572_scale_y = 1.000000000;
static const float p_o273737062507_value = 0.000000000;
static const float p_o273737062507_width = 0.008000000;
static const float p_o273720285290_default_in1 = 0.000000000;
static const float p_o273720285290_default_in2 = 0.000000000;
static const float seed_o273686730856 = 0.000000000;
static const float p_o273686730856_in_min = 0.300000000;
static const float p_o273686730856_in_max = 0.600000000;
static const float p_o273686730856_out_max = 0.000000000;
static const float seed_o272596211789 = 0.603101492;
static const float p_o273753839724_default_in1 = 0.000000000;
static const float p_o273753839724_default_in2 = 0.000000000;
static const float p_o273787394158_default_in1 = 0.000000000;
static const float p_o273787394158_default_in2 = 0.000000000;
static const float seed_o273703508073 = 0.000000000;
static const float p_o273703508073_in_min = 0.300000000;
static const float p_o273703508073_in_max = 0.600000000;
static const float p_o273703508073_out_min = 0.000000000;
static const float p_o273770616941_min = 2.000000000;
static const float p_o273770616941_max = 0.000000000;
static const float p_o273770616941_step = 0.000000000;
static const float p_o273804171375_sides = 6.000000000;
static const float p_o273804171375_radius = 3.000000000;
static const float p_o273804171375_edge = 1.000000000;

CBUFFER_START(UnityPerMaterial)
            fixed4 _MainColor;
			float _speed;
CBUFFER_END

// #globals: invert (o36126144338772)
// #globals: transform2 (o273938389109)
float2 transform2_clamp(float2 uv) {
	return clamp(uv, tofloat2(0.0), tofloat2(1.0));
}
float2 transform2(float2 uv, float2 translate, float rotate, float2 scale) {
 	float2 rv;
	uv -= translate;
	uv -= tofloat2(0.5);
	rv.x = cos(rotate)*uv.x + sin(rotate)*uv.y;
	rv.y = -sin(rotate)*uv.x + cos(rotate)*uv.y;
	rv /= scale;
	rv += tofloat2(0.5);
	return rv;	
}
// #globals: scale_2 (o272579434572)
float2 scale(float2 uv, float2 center, float2 scale) {
	uv -= center;
	uv /= scale;
	uv += center;
	return uv;
}
// #globals: math_3 (o273720285290)
float pingpong(float a, float b)
{
  return (b != 0.0) ? abs(frac((a - b) / (b * 2.0)) * b * 2.0 - b) : 0.0;
}
// #globals: shape (o273804171375)
float shape_circle(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = max(edge, 1.0e-8);
	float distance = length(uv);
	return clamp((1.0-distance/size)/edge, 0.0, 1.0);
}
float shape_polygon(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = max(edge, 1.0e-8);
	float angle = hlsl_atan(uv.x, uv.y)+3.14159265359;
	float slice = 6.28318530718/sides;
	return clamp((1.0-(cos(floor(0.5+angle/slice)*slice-angle)*length(uv))/size)/edge, 0.0, 1.0);
}
float shape_star(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = max(edge, 1.0e-8);
	float angle = hlsl_atan(uv.x, uv.y);
	float slice = 6.28318530718/sides;
	return clamp((1.0-(cos(floor(angle*sides/6.28318530718-0.5+2.0*step(frac(angle*sides/6.28318530718), 0.5))*slice-angle)*length(uv))/size)/edge, 0.0, 1.0);
}
float shape_curved_star(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = max(edge, 1.0e-8);
	float angle = 2.0*(hlsl_atan(uv.x, uv.y)+3.14159265359);
	float slice = 6.28318530718/sides;
	return clamp((1.0-cos(floor(0.5+0.5*angle/slice)*2.0*slice-angle)*length(uv)/size)/edge, 0.0, 1.0);
}
float shape_rays(float2 uv, float sides, float size, float edge) {
	uv = 2.0*uv-1.0;
	edge = 0.5*max(edge, 1.0e-8)*size;
	float slice = 6.28318530718/sides;
	float angle = mod(hlsl_atan(uv.x, uv.y)+3.14159265359, slice)/slice;
	return clamp(min((size-angle)/edge, angle/edge), 0.0, 1.0);
}
// #globals: custom_uv
float2 get_from_tileset(float count, float seed, float2 uv) {
	return clamp((uv+floor(rand2(tofloat2(seed))*count))/count, tofloat2(0.0), tofloat2(1.0));
}
float2 custom_uv_transform(float2 uv, float2 cst_scale, float rnd_rotate, float rnd_scale, float2 seed) {
	seed = rand2(seed);
	uv -= tofloat2(0.5);
	float angle = (seed.x * 2.0 - 1.0) * rnd_rotate;
	float ca = cos(angle);
	float sa = sin(angle);
	uv = tofloat2(ca*uv.x+sa*uv.y, -sa*uv.x+ca*uv.y);
	uv *= (seed.y-0.5)*2.0*rnd_scale+1.0;
	uv /= cst_scale;
	uv += tofloat2(0.5);
	return uv;
}
float o273871280243_input_in(float2 uv, float _seed_variation_) {
// #output0: uniform_greyscale_5 (o272596211789)
float o272596211789_0_1_f = (frac(_Time.y*_speed*param_rnd(.5,.7, (seed_o272596211789+frac(_seed_variation_))+0.231714)+param_rnd(0,1, (seed_o272596211789+frac(_seed_variation_))+0.925452)));
// #output0: tones_map_2 (o273686730856)
float4 o273686730856_0_1_rgba = tofloat4(tofloat3((param_rnd(.02,.01, (seed_o273686730856+frac(_seed_variation_))+0.764541)))+(tofloat4(tofloat3(o272596211789_0_1_f), 1.0).rgb-tofloat3(p_o273686730856_in_min))*tofloat3((p_o273686730856_out_max-((param_rnd(.02,.01, (seed_o273686730856+frac(_seed_variation_))+0.764541))))/(p_o273686730856_in_max-(p_o273686730856_in_min))), tofloat4(tofloat3(o272596211789_0_1_f), 1.0).a);
// #output0: tones_map (o273703508073)
float4 o273703508073_0_1_rgba = tofloat4(tofloat3(p_o273703508073_out_min)+(tofloat4(tofloat3(o272596211789_0_1_f), 1.0).rgb-tofloat3(p_o273703508073_in_min))*tofloat3(((param_rnd(.5,.3, (seed_o273703508073+frac(_seed_variation_))+0.835115))-(p_o273703508073_out_min))/(p_o273703508073_in_max-(p_o273703508073_in_min))), tofloat4(tofloat3(o272596211789_0_1_f), 1.0).a);
// #output0: shape (o273804171375)
float o273804171375_0_1_f = shape_circle((scale((transform2(uv, tofloat2((param_rnd(-.2,.2, (seed_o273602844772+frac(_seed_variation_))+0.500000))*(2.0*1.0-1.0), (param_rnd(-.2,.2, (seed_o273602844772+frac(_seed_variation_))+0.019301))*(2.0*1.0-1.0)), p_o273602844772_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o273602844772_scale_x*(2.0*1.0-1.0), p_o273602844772_scale_y*(2.0*1.0-1.0)))), tofloat2(0.5+p_o272579434572_cx, 0.5+p_o272579434572_cy), tofloat2(p_o272579434572_scale_x, p_o272579434572_scale_y))), p_o273804171375_sides, p_o273804171375_radius*1.0, p_o273804171375_edge*1.0);
// #code: remap (o273770616941)
float o273770616941_0_x = o273804171375_0_1_f*(p_o273770616941_max-p_o273770616941_min);
// #output0: remap (o273770616941)
float o273770616941_0_1_f = p_o273770616941_min+o273770616941_0_x-mod(o273770616941_0_x, max(p_o273770616941_step, 0.00000001));
// #code: math (o273787394158)
float o273787394158_0_clamp_false = o273770616941_0_1_f-(dot((o273703508073_0_1_rgba).rgb, tofloat3(1.0))/3.0);
float o273787394158_0_clamp_true = clamp(o273787394158_0_clamp_false, 0.0, 1.0);
// #output0: math (o273787394158)
float o273787394158_0_1_f = o273787394158_0_clamp_false;
// #code: math_2 (o273753839724)
float o273753839724_0_clamp_false = abs(o273787394158_0_1_f);
float o273753839724_0_clamp_true = clamp(o273753839724_0_clamp_false, 0.0, 1.0);
// #output0: math_2 (o273753839724)
float o273753839724_0_1_f = o273753839724_0_clamp_false;
// #code: math_3 (o273720285290)
float o273720285290_0_clamp_false = o273753839724_0_1_f-(dot((o273686730856_0_1_rgba).rgb, tofloat3(1.0))/3.0);
float o273720285290_0_clamp_true = clamp(o273720285290_0_clamp_false, 0.0, 1.0);
// #output0: math_3 (o273720285290)
float o273720285290_0_1_f = o273720285290_0_clamp_false;
// #code: tones_step (o273737062507)
float3 o273737062507_0_false = clamp((tofloat4(tofloat3(o273720285290_0_1_f), 1.0).rgb-tofloat3(p_o273737062507_value))/max(0.0001, p_o273737062507_width)+tofloat3(0.5), tofloat3(0.0), tofloat3(1.0));
float3 o273737062507_0_true = tofloat3(1.0)-o273737062507_0_false;
// #output0: tones_step (o273737062507)
float4 o273737062507_0_1_rgba = tofloat4(o273737062507_0_true, tofloat4(tofloat3(o273720285290_0_1_f), 1.0).a);
// #output0: scale_2 (o272579434572)
float4 o272579434572_0_1_rgba = o273737062507_0_1_rgba;
// #output0: transform2_3 (o273602844772)
float4 o273602844772_0_1_rgba = o272579434572_0_1_rgba;
return (dot((o273602844772_0_1_rgba).rgb, tofloat3(1.0))/3.0);
}
// #instance: circle_splatter (o273871280243)
float4 splatter_o273871280243(float2 uv, int count, int rings, inout float3 instance_uv, float2 seed, float _seed_variation_) {
	float c = 0.0;
	float3 rc = tofloat3(0.0);
	float3 rc1;
	seed = rand2(seed);
	for (int i = 0; i < count; ++i) {
		float a = -1.57079632679+6.28318530718*float(i)*p_o273871280243_rings/float(count);
		float rings_distance = ceil(float(i+1)*float(rings)/float(count))/float(rings);
		float spiral_distance = float(i+1)/float(count);
		float2 pos = p_o273871280243_radius*lerp(rings_distance, spiral_distance, p_o273871280243_spiral)*tofloat2(cos(a), sin(a));
		float mask = 1.0;
		if (mask > 0.01) {
			float2 pv = uv-0.5-pos;
			rc1 = rand3(seed);
			seed = rand2(seed);
			float angle = (seed.x * 2.0 - 1.0) * p_o273871280243_rotate * 0.01745329251 + (a+1.57079632679) * p_o273871280243_i_rotate;
			float ca = cos(angle);
			float sa = sin(angle);
			pv = tofloat2(ca*pv.x+sa*pv.y, -sa*pv.x+ca*pv.y);
			pv /= lerp(1.0, float(i+1)/float(count+1), p_o273871280243_i_scale);
			pv /= tofloat2(p_o273871280243_scale_x, p_o273871280243_scale_y);
			pv *= (seed.y-0.5)*2.0*p_o273871280243_scale+1.0;
			pv += tofloat2(0.5);
			seed = rand2(seed);
			float2 test_value = clamp(pv, tofloat2(0.0), tofloat2(1.0));
			if (pv.x != test_value.x || pv.y != test_value.y) {
				continue;
			}
			float2 full_uv = pv;
			pv = get_from_tileset( 1.0, seed.x, pv);
			float c1 = o273871280243_input_in(pv, true ? seed.x : 0.0)*mask*(1.0-p_o273871280243_value*seed.x);
			c = max(c, c1);
			rc = lerp(rc, rc1, step(c, c1));
			instance_uv = lerp(instance_uv, tofloat3(full_uv, seed.x), step(c, c1));
		}
	}
	return tofloat4(rc, c);
}
		
			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target {
				float _seed_variation_ = 0.0;
				float2 uv = i.uv;

// #code: circle_splatter (o273871280243)
float3 o273871280243_0_instance_uv = tofloat3(0.0);
float4 o273871280243_0_rch = splatter_o273871280243((transform2((uv), tofloat2(p_o273938389109_translate_x*(2.0*1.0-1.0), p_o273938389109_translate_y*(2.0*1.0-1.0)), p_o273938389109_rotate*0.01745329251*(2.0*1.0-1.0), tofloat2(p_o273938389109_scale_x*(2.0*1.0-1.0), p_o273938389109_scale_y*(2.0*1.0-1.0)))), int(p_o273871280243_count), int(p_o273871280243_rings), o273871280243_0_instance_uv, tofloat2(float((seed_o273871280243+frac(_seed_variation_)))), _seed_variation_);
// #output0: circle_splatter (o273871280243)
float o273871280243_0_1_f = o273871280243_0_rch.a;

// #output0: transform2 (o273938389109)
float4 o273938389109_0_1_rgba = tofloat4(tofloat3(o273871280243_0_1_f), 1.0);

// #output0: invert (o36126144338772)
float4 o36126144338772_0_1_rgba = tofloat4(tofloat3(1.0)-o273938389109_0_1_rgba.rgb, o273938389109_0_1_rgba.a);

				// sample the generated texture
				fixed4 col = o36126144338772_0_1_rgba;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return fixed4(_MainColor.rgb,(col.r > .4 ? 0 : 1));
			}
			ENDCG
		}
	}
}



