namespace rec PpGen.TetrahedralSubdivision


[<Struct>]
type Vector3<'a> =
    { X: 'a; Y: 'a; Z: 'a }

module Vector3 =
    let inline add lhs rhs =
        { X = lhs.X + rhs.X; Y = lhs.Y + rhs.Y; Z = lhs.Z + rhs.Z }
    
    let inline sub lhs rhs =
        { X = lhs.X - rhs.X; Y = lhs.Y - rhs.Y; Z = lhs.Z - rhs.Z }

    let inline dist2 (a: Vector3<_>) (b: Vector3<_>) =
        let ab = a - b
        ab.X * ab.X + ab.Y * ab.Y + ab.Z * ab.Z

type Vector3<'a> with
    static member inline ( + ) (lhs, rhs) = Vector3.add lhs rhs
    static member inline ( - ) (lhs, rhs) = Vector3.sub lhs rhs
