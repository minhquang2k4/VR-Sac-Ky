Shader "Custom/CylinderText"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Texture của Text
        _Radius ("Radius of Cylinder", Float) = 1.0 // Bán kính của hình trụ
        _Height ("Height Offset", Float) = 0.0      // Độ cao offset
        _AlphaCutoff ("Alpha Cutoff", Float) = 0.5   // Ngưỡng alpha để hiển thị
    }
    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;  // Text Texture
            float _Radius;       // Bán kính của hình trụ
            float _Height;       // Độ cao offset
            float _AlphaCutoff;  // Ngưỡng alpha

            struct appdata_t
            {
                float4 vertex : POSITION; // Vị trí đỉnh
                float2 uv : TEXCOORD0;    // Tọa độ UV
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;       // Tọa độ UV
                float4 vertex : SV_POSITION; // Vị trí đỉnh sau khi chỉnh sửa
            };

            // Vertex Shader: Bẻ cong Text theo hình trụ
            v2f vert (appdata_t v)
            {
                v2f o;

                // Tọa độ đỉnh gốc
                float3 pos = v.vertex.xyz;

                // Uốn cong theo bán kính hình trụ
                float angle = pos.x / _Radius; // Góc cong dựa trên vị trí X
                float x = sin(angle) * _Radius; // X sau khi cong
                float z = cos(angle) * _Radius; // Z sau khi cong

                pos.x = x;
                pos.z = z;
                pos.y += _Height; // Thêm độ cao nếu cần

                // Gắn vị trí đỉnh mới
                o.vertex = UnityObjectToClipPos(float4(pos, 1.0));
                o.uv = v.uv;

                return o;
            }

            // Fragment Shader: Hiển thị Texture và áp dụng AlphaTest
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv); // Lấy màu của Text từ Texture

                // Kiểm tra alpha của pixel, nếu nhỏ hơn ngưỡng thì bỏ qua
                if (color.a < _AlphaCutoff)
                {
                    discard; // Loại bỏ pixel nếu alpha < _AlphaCutoff
                }

                return color; // Trả về màu của Text
            }
            ENDCG
        }
    }
}
