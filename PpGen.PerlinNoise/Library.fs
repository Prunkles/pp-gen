namespace PpGen.PerlinNoise

open System


module Perlin =
    
    [<Struct>]
    type Vector2(x: float, y: float) =
        member _.X = x
        member _.Y = y
    
    let dot (a: Vector2) (b: Vector2) : float =
        a.X * b.X + a.Y * b.Y
    
    let lerp a b t : float =
        a + (b - a) * t
    
    let qunticCurve t =
        t * t * t * (t * (t * 6. - 15.) + 10.)
    
    let randomVector x y (seed: uint64) =
        let hash = 641374891
        let hash = hash * 31 + x
        let hash = hash * 31 + y
        let hash = hash * 31 + ((int)seed)
        
        match hash % 4 with
        | 0 -> Vector2( 1.,  0.)
        | 1 -> Vector2( 0.,  1.)
        | 2 -> Vector2(-1.,  0.)
        | 3 -> Vector2( 0., -1.)
        | _ -> invalidOp "ops"
    
    let noise (x: float) (y: float) (seed: uint64) =
        let left = floor x |> int
        let top = floor y |> int
        
        let pointInQuadX = x % 1.
        let pointInQuadY = y % 1.
        
        // извлекаем градиентные векторы для всех вершин квадрата:
        let topLeftGradient     = randomVector (left  ) (top  ) seed
        let topRightGradient    = randomVector (left+1) (top  ) seed
        let bottomLeftGradient  = randomVector (left  ) (top+1) seed
        let bottomRightGradient = randomVector (left+1) (top+1) seed

        // вектора от вершин квадрата до точки внутри квадрата:
        let distanceToTopLeft     = Vector2(pointInQuadX   , pointInQuadY   )
        let distanceToTopRight    = Vector2(pointInQuadX-1., pointInQuadY   )
        let distanceToBottomLeft  = Vector2(pointInQuadX   , pointInQuadY-1.)
        let distanceToBottomRight = Vector2(pointInQuadX-1., pointInQuadY-1.)

        // считаем скалярные произведения между которыми будем интерполировать
        
        // tx1--tx2
        //  |    |
        // bx1--bx2
        
        let tx1 = dot distanceToTopLeft topLeftGradient
        let tx2 = dot distanceToTopRight topRightGradient
        let bx1 = dot distanceToBottomLeft bottomLeftGradient
        let bx2 = dot distanceToBottomRight bottomRightGradient

        // готовим параметры интерполяции, чтобы она не была линейной:
        let pointInQuadX = qunticCurve pointInQuadX
        let pointInQuadY = qunticCurve pointInQuadY

        // собственно, интерполяция:
        let tx = lerp tx1 tx2 pointInQuadX
        let bx = lerp bx1 bx2 pointInQuadX
        let tb = lerp tx  bx  pointInQuadY
        
        tb
    
    let generate (x: int) (y: int) (w: int) (h: int) seed =
        let hmap = Array2D.create w h 0.0
        
        let scale = 128.0
        
        for dx in 0..w do
            for dy in 0..h do
                let gfx = x + dx |> float
                let gfy = y + dy |> float
                hmap.[dx, dy] <- noise (gfx / scale) (gfy / scale) seed
        
        hmap
    
    let generateReactive (x: int) (y: int) (w: int) (h: int) seed =
        
        let sub (obv: IObserver<float[,]>) =
            let hm = generate x y w h seed
            obv.OnNext(hm)a
            { new IDisposable with member _.Dispose() = () }
        
        { new IObservable<float[,]> with member _.Subscribe(obv) = sub obv }