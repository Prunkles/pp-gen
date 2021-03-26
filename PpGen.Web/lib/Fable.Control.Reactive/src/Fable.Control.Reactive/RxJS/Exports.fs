namespace Fable.Import.RxJS

module Exports =
    
    open Fable.Core.JsInterop
    
    let rxjs: obj = importAll "rxjs"
    let rxjs_operators: obj = importAll "rxjs/operators/index.js" // TODO: Import directory
    let rxjs_webSocket: obj = importAll "rxjs/webSocket"
