// Adapted code from http://hjemmesider.diku.dk/~torbenm/Planet/

namespace PpGen.TetrahedralSubdivision


[<Struct>]
type Vertex<'a> =
    { H: 'a; S: 'a
      V: Vector3<'a> }
    member inline this.X = this.V.X
    member inline this.Y = this.V.Y
    member inline this.Z = this.V.Z


type Tetrahedron<'a> =
    { V0: Vertex<'a>; V1: Vertex<'a>
      V2: Vertex<'a>; V3: Vertex<'a> }

module Tetrahedron =
    let item tetra i = match i with 0 -> tetra.V0 | 1 -> tetra.V1 | 2 -> tetra.V2 | 3 -> tetra.V3 | _ -> invalidOp ""

type Tetrahedron<'a> with
    member this.Item(i) = Tetrahedron.item this i


type Heightmap<'a> private (data: 'a[,]) =
    let height = data.GetLength(0)
    let width = data.GetLength(1)
    new (width, height, defaultValue) =
        let data = Array2D.create width height defaultValue
        Heightmap(data)
    member _.Data = data
    member _.Item with get (x, y) = data.[x, y] and set (x, y) value = data.[x, y] <- value
    member _.Width = width
    member _.Height = height


module Functions =

    open System
    open type Math
    
    let squareProjection (hm: Heightmap<'a>) lat scale  =
        let k = 0.5 * lat * float hm.Width * scale / Math.PI + 0.5 |> int
        for j in 0 .. hm.Height - 1 do
            let y = (2 * (j - k) - hm.Height |> float) / float hm.Width / scale * Math.PI
            
            ()
    
    type IPlanetEnv =
        abstract rand2: float * float -> float
        abstract dd1: float // Weight for altitude difference
        abstract POWA: float // Power for altitude difference
        abstract dd2: float // Weight for distance
        abstract POW: float // Power for distance function
        abstract ssh: Tetrahedron<float> with get, set // ssa, ssb, ssc, ssd
        abstract matchMap: bool
        abstract matchSize: float
        abstract cl0: float[,] // search map
        abstract doshade: int
        abstract shadeAngle: float // shade_angle
        abstract shadeAngle2: float // shade_angle2
        abstract shade: int with set
    
//    let private mutable ssa, ssb, ssc, ssd = 
    let rec planet
        (env: IPlanetEnv)
        (a: Vertex<_>) (b: Vertex<_>) (c: Vertex<_>) (d: Vertex<_>) // Tetrahedron vertices
        (x: float) (y: float) (z: float) // Goal point
        (level: int) // Levels to go
        =
        let planet a b c d x y z level = planet env a b c d x y z level
        if level > 0 then
            // Make sure ab is longest edge
            let lab = Vector3.dist2 a.V b.V
            let lac = Vector3.dist2 a.V c.V
            let lad = Vector3.dist2 a.V d.V
            let lbc = Vector3.dist2 b.V c.V
            let lbd = Vector3.dist2 b.V d.V
            let lcd = Vector3.dist2 c.V d.V
            
            let maxlength =
                lab |> max lac |> max lad
                |> max lbc |> max lbd |> max lcd
            match maxlength with
            | x when x = lac -> planet a c b d x y z level
            | x when x = lad -> planet a d b c x y z level
            | x when x = lbc -> planet b c a d x y z level
            | x when x = lbd -> planet b d a c x y z level
            | x when x = lcd -> planet c d a b x y z level
            | _ ->
                if level = 11 then // Save tetrahedron for caching
                    env.ssh <- { V0 = a; V1 = b; V2 = c; V3 = d }
                
                // ab is longest, so cut ab
                let es' = env.rand2 (a.S, b.S)
                let es1 = env.rand2 (es', es')
                let es2 = 0.5 + 0.1 * env.rand2 (es1, es1)
                let es3 = 1.0 - es2
                
                let ex', ey', ez' =
                    if a.S < b.S then
                        es2 * a.X + es3 * b.X,
                        es2 * a.Y + es3 * b.Y,
                        es2 * a.Z + es3 * b.Z
                    elif a.S > b.S then
                        es3 * a.X + es2 * b.X,
                        es3 * a.Y + es2 * b.Y,
                        es3 * a.Z + es2 * b.Z
                    else // as = bs , very unlikely to ever happen
                        0.5 * a.X + 0.5 * b.X,
                        0.5 * a.Y + 0.5 * b.Y,
                        0.5 * a.Z + 0.5 * b.Z
                
                // New altitude is:
                let eh' =
                    if env.matchMap && lab > env.matchSize then // Use map height
                        let l = sqrt (ex'*ex' + ey'*ey' + ez'*ez')
                        let yy = asin (ey' / l) * 23.0 / PI + 11.5
                        let xx = atan2 ex' ez' * 23.5 / PI + 23.5
                        env.cl0.[xx+0.5 |> int, yy+0.5 |> int] * 0.1 / 8.0
                    else
                        let lab = if lab > 1.0 then lab ** 0.5 else lab
                        // Decrease contribution for very long distances
                        0.5 * (a.H + b.H) // average of end points
                        + es' * env.dd1 * (abs (a.H - b.H) ** env.POWA) // plus contribution for altitude diff
                        + es1 * env.dd2 * (lab ** env.POW) // plus contribution for distance
                
                let e =
                    { S = es'; H = eh'
                      V = { X = ex'; Y = ey'; Z = ez' } }
                
                // Find out in which new tetrahedron target point is
                let eax, eay, eaz = a.X - e.X, a.Y - e.Y, a.Z - e.Z
                let ecx, ecy, ecz = c.X - e.X, c.Y - e.Y, c.Z - e.Z
                let edx, edy, edz = d.X - e.X, d.Y - e.Y, d.Z - e.Z
                let epx, epy, epz =   x - e.X,   y - e.Y,   z - e.Z
                if ( eax*ecy*edz + eay*ecz*edx + eaz*ecx*edy
                   - eaz*ecy*edx - eay*ecx*edz - eax*ecz*edy ) *
                   ( epx*ecy*edz + epy*ecz*edx + epz*ecx*edy
                   - epz*ecy*edx - epy*ecx*edz - epx*ecz*edy ) > 0.0
                then // Point is inside acde
                    planet c d a e x y z (level - 1)
                else // Point is inside bcde
                    planet c d b e x y z (level - 1)
        else // level = 0
            let shade =
                match env.doshade with
                | 1 | 2 ->
                    let x1 = 0.25 * (a.X + b.X + c.X + d.X)
                    let x1 = a.H * (x1 - a.X) + b.H * (x1 - b.X) + c.H * (x1 - c.X) + d.H * (x1 - d.X)
                    let y1 = 0.25 * (a.Y + b.Y + c.Y + d.Y)
                    let y1 = a.H * (y1 - a.Y) + b.H * (y1 - b.Y) + c.H * (y1 - c.Y) + d.H * (y1 - d.Y)
                    let z1 = 0.25 * (a.Z + b.Z + c.Z + d.Z)
                    let z1 = a.H * (z1 - a.Z) + b.H * (z1 - b.Z) + c.H * (z1 - c.Z) + d.H * (z1 - d.Z)
                    let l1 = sqrt (x1*x1 + y1*y1 + z1*z1)
                    let l1 = if l1 = 0.0 then 1.0 else l1
                    let tmp = sqrt (1.0 - y*y) |> max 0.0001
                    let x2 = x*x1 + y*y1 + z*z1
                    let y2 = -x*y / tmp*x1 + tmp*y1 - z*y / tmp*z1
                    let z2 = -z / tmp*x1 + x / tmp*z1
                    let shade =
                        ( - sin (env.shadeAngle * PI/180.0) * y2
                          - cos (env.shadeAngle * PI/180.0) * z2 )
                        / l1 * 48.0 + 128.0
                        |> int
                        |> max 10 |> min 255
                    let shade =
                        if env.doshade = 2 &&  (a.H + b.H + c.H + d.H) < 0.0
                        then 150
                        else shade
                    shade
                | 3 ->
                    let x1, y1, z1 =
                        if a.H + b.H + c.H + d.H < 0.0 then
                            x, y, z
                        else
                            let l1 = 50.0 / sqrt 0.0 // (lad + lac + lad + lbc + lbd + lcd)
                            let x1 = 0.25 * (a.X + b.X + c.X + d.X)
                            let x1 = l1 * (a.H*(x1-a.X) + b.H*(x1-b.X) + c.H*(x1-c.X) + d.H*(x1-d.X)) + x
                            let y1 = 0.25 * (a.Y + b.Y + c.Y + d.Y)
                            let y1 = l1 * (a.H*(y1-a.Y) + b.H*(y1-b.Y) + c.H*(y1-c.Y) + d.H*(y1-d.Y)) + y
                            let z1 = 0.25 * (a.Z + b.Z + c.Z + d.Z)
                            let z1 = l1 * (a.H*(z1-a.Z) + b.H*(z1-b.Z) + c.H*(z1-c.Z) + d.H*(z1-d.Z)) + z
                            x1, y1, z1
                    let l1 = sqrt (x1*x1 + y1*y1 + z1*z1)
                    let l1 = if l1 = 0.0 then 1.0 else l1
                    let x2 = cos (env.shadeAngle * PI/180.0 - 0.5*PI) * cos (env.shadeAngle2 * PI/180.0)
                    let y2 = -sin (env.shadeAngle2 * PI/180.0)
                    let z2 = -sin (env.shadeAngle * PI/180.0 - 0.5*PI) * cos (env.shadeAngle2 * PI/180.0);
                    let shade =
                        (x1*x2 + y1*y2 + z1*z2) / l1 * 170.0 + 10.0
                        |> int
                        |> max 10 |> min 255
                    shade
                | _ -> invalidOp "shade != 1|2|3"
            env.shade <- shade
            0.25 * (a.H + b.H + c.H + d.H)
    
    let planet1 env (x: float) (y: float) (z: float) =
        let ssa, ssb, ssc, ssd : Vertex<_> * Vertex<_> * Vertex<_> * Vertex<_> = failwith "ssa, ssb, ssc, ssd"
        
        // Check if point is inside cached tetrahedron
        let abx, aby, abz = ssb.X - ssa.X, ssb.Y - ssa.Y, ssb.Z - ssa.Z
        let acx, acy, acz = ssc.X - ssa.X, ssc.Y - ssa.Y, ssc.Z - ssa.Z
        let adx, ady, adz = ssd.X - ssa.X, ssd.Y - ssa.Y, ssd.Z - ssa.Z
        let apx, apy, apz = x - ssa.X, y - ssa.Y, z - ssa.Z
        
        if  ( adx*aby*acz + ady*abz*acx + adz*abx*acy
            - adz*aby*acx - ady*abx*acz - adx*abz*acy ) *
            ( apx*aby*acz + apy*abz*acx + apz*abx*acy
            - apz*aby*acx - apy*abx*acz - apx*abz*acy ) > 0.0
            &&
            // p is on same side of abc as d
            ( acx*aby*adz + acy*abz*adx + acz*abx*ady
            - acz*aby*adx - acy*abx*adz - acx*abz*ady ) *
            ( apx*aby*adz + apy*abz*adx + apz*abx*ady
            - apz*aby*adx - apy*abx*adz - apx*abz*ady ) > 0.0
            &&
            // p is on same side of abd as c
            ( abx*ady*acz + aby*adz*acx + abz*adx*acy
            - abz*ady*acx - aby*adx*acz - abx*adz*acy ) *
            ( apx*ady*acz + apy*adz*acx + apz*adx*acy
            - apz*ady*acx - apy*adx*acz - apx*adz*acy ) > 0.0
            &&
            // p is on same side of acd as b
            (
                let bax, bay, baz = -abx, -aby, -abz
                let bcx, bcy, bcz = ssc.X-ssb.X, ssc.Y-ssb.Y, ssc.Z-ssb.Z
                let bdx, bdy, bdz = ssd.X-ssb.X, ssd.Y-ssb.Y, ssd.Z-ssb.Z
                let bpx, bpy, bpz = x-ssb.X, y-ssb.Y, z-ssb.Z
                ( bax*bcy*bdz + bay*bcz*bdx + baz*bcx*bdy
                - baz*bcy*bdx - bay*bcx*bdz - bax*bcz*bdy ) *
                ( bpx*bcy*bdz + bpy*bcz*bdx + bpz*bcx*bdy
                - bpz*bcy*bdx - bpy*bcx*bdz - bpx*bcz*bdy ) > 0.0
             )
        then
            // p is on same side of bcd as a
            // Hence, p is inside cached tetrahedron
            // so we start from there
            planet env ssa ssb ssc ssd x y z 11
        else
            // otherwise, we start from scratch
            let tetra: Tetrahedron<_> = failwith "tetra"
            let Depth = failwith "Depth"
            planet
                env
                tetra.V0 tetra.V1 tetra.V2 tetra.V3 // vertices of tetrahedron
                x y z // coordinated of point we want color of
                Depth // subdivision depth
    
    let planet0 x y z i j env =
        let alt = planet1 x y z env
        let nonLinear = failwith "nonLinear"
        let alt =
            if nonLinear then
                // non-linear scaling to make flatter near sea level
                alt*alt*alt * 300.0
            else alt
        
        // /* store height for heightfield */
        // if (file_type == heightfield) heights[i][j] = 10000000*alt;
        
        let y2 = y * y
        let y2 = y2 * y2
        let y2 = y2 * y2
        
        failwith ""