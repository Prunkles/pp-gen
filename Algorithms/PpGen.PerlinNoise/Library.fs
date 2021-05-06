namespace PpGen.PerlinNoise

open System


module Perlin =
    
    [<AutoOpen>]
    module private Helpers =
        
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
            let hash = HashCode.Combine(x, y, (int)seed, 641374891)
            match abs hash % 4 with
            | 0 -> Vector2( 1.,  0.)
            | 1 -> Vector2( 0.,  1.)
            | 2 -> Vector2(-1.,  0.)
            | 3 -> Vector2( 0., -1.)
            | _ -> invalidOp "Unreachable"
    
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
        
        let h = tb // (-1; 1)
        let h = (h + 1.) / 2. // (0; 1)
        h
    
    let generate (x: int) (y: int) (w: int) (h: int) seed octaveCount =
        let hmap = Array2D.create w h 0.0
        
        let order (octave: int) = Math.Pow(2.0, float octave)
        let baseScale (octave: int) = 1. / order(octave)
        let scale (octave: int) = ((w + h) / 2 |> float) / 4. * (baseScale octave)
        
        let mutable maxHeight =
            Seq.init octaveCount id
            |> Seq.map baseScale
            |> Seq.sum
        
        for dx in 0..(w-1) do
            for dy in 0..(h-1) do
                let gfx = x + dx |> float
                let gfy = y + dy |> float
                
                let mutable height = 0.0
                for octave in 0..(octaveCount - 1) do
                    let mapScale = scale octave
                    let dh = noise (gfx * mapScale) (gfy * mapScale) seed
                    height <- height + dh
                
                hmap.[dx, dy] <- height / maxHeight
        
        hmap
    
    let generateReactive (x: int) (y: int) (w: int) (h: int) seed octaveCount =
        
        let sub (obv: IObserver<float[,]>) =
            let hm = generate x y w h seed octaveCount
            obv.OnNext(hm)
            { new IDisposable with member _.Dispose() = () }
        
        { new IObservable<float[,]> with member _.Subscribe(obv) = sub obv }
