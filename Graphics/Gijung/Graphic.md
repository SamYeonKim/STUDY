# Graphics


## Name

+ 쉐이더의 이름 설정
+ 구성

```
Shader "Unlit/SpecialFX/Cool Hologram" { ... }
```

## Peroperties

+ 외부에서 설정 할 수 있는 값.
+ 구성

```
Properties {
    // "내부에서 사용하는 이름" ("외부에서 보여지는 이름", "타입") = "기본값"
    _MainTex ("Albedo Texture", 2D) = "white" {}
}
```

## SubShader

+ 하드웨어의 다양성으로 인해 `여러개의 SubShader를 선언` 할 수 있고, 그 중에서 `적합한 하나의 SubShader를 실행`하게 된다.
+ 적합한 SubShader가 `존재하지 않을 경우 Fallback을 실행`하게 된다.
+ 구성

```
SubShader {
    [Tags] 
    [CommonState] 
    [Pass ...] 
    [Fallback]
}
```

## Pass

+ 구성

```
Pass { 
  [Name and Tags]
  [RenderSetup] 
    ex) ZWrite Off

  [fixed-function style commands] 
    ex) Lighting On
}
```

## cgprogram

+ 시작과 끝을 `CGPROGRAM ~ ENDCG` 로 표현.
+ `#pragma` 는 지시자로서 아래에서는 vertext와 fragment에 사용될 함수를 지정하고 있다.

```
ex)

CGPROGRAM

#pragma vertex vert
#pragma fragment frag

v2f vert (appdata v) {
    ...
}

fixed4 frag (v2f i) : SV_Target {
    ...
}

ENDCG
```

## Tint

+ Fragment Shader를 이용한 `색상 적용`.
+ Perperty에 사용한 이름, Pass 내부에 사용한 변수 이름, 함수 내부에서 선언된 `변수 이름이 같아야 제대로 적용`된다.

```
...

Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _TintColor("Tint Color", Color) = (1,1,1,1)
}

...

Pass {
    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    ...

    struct v2f {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
    };

    sampler2D _MainTex;
    float4 _MainTex_ST;
    float4 _TintColor;
    
    ...

    fixed4 frag (v2f i) : SV_Target {
        fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
        return col;
    }
    ENDCG
}
```

## Transparent

+ Fragment Shader를 이용한 `투명 적용`.

```
...

Properties {
    _MainTex ("Texture", 2D) = "white" {}
    _TintColor("Tint Color", Color) = (1,1,1,1)
    _Transparency("Transparency", Range(0.0,0.5)) = 0.25
}

SubShader {
    Tags {"Queue"="Transparent" "RenderType"="Transparent" }

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        ...

        struct v2f {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;
        float4 _TintColor;
        float _Transparency;
        
        ...

        fixed4 frag (v2f i) : SV_Target {
            fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
            col.a = _Transparency;
            return col;
        }
        ENDCG
    }
}
```

## Vertex

+ Vertex Shader를 이용한 `위치 변환`.

```
...

Properties {
    ...
    
    _Distance("Distance", Float) = 1
    _Amplitude("Amplitude", Float) = 1
    _Speed ("Speed", Float) = 1
    _Amount("Amount", Range(0.0,1.0)) = 1
}

SubShader {
    Tags {"Queue"="Transparent" "RenderType"="Transparent" }

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        struct appdata {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        ...

        float _Distance;
        float _Amplitude;
        float _Speed;
        float _Amount;
        
        v2f vert (appdata v) {
            v2f o;
            v.vertex.x += sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target {
            ...
        }
        ENDCG
    }
}
```