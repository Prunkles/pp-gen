module GenericNumber

module Operators =
    let inline ( ! ) a = a ()
    let inline ( + ) (a: ^a) (b: ^a) : ^a = a + b
    let inline ( * ) (a: ^a) (b: ^a) : ^a = a * b

module Constants =
    open Operators
    
    let inline _0() = LanguagePrimitives.GenericZero
    let inline _1() = LanguagePrimitives.GenericOne
    let inline _2() = !_1 + !_1
    let inline _3() = !_2 + !_1
    let inline _4() = !_2 + !_2
    let inline _5() = !_3 + !_2
    let inline _8() = !_4 + !_4
    let inline _16() = !_8 + !_8
    let inline _32() = !_16 + !_16
    let inline _64() = !_32 + !_32
    let inline _128() = !_64 + !_64
    
    let inline _180() = !_128 + !_32 + !_16 + !_4

module NumericLiteralG =
    let inline FromZero() = Constants._0()
    let inline FromOne() = Constants._1()
