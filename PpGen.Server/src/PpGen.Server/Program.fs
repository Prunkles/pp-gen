module PpGen.Server.Program

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open PpGen.Api.Fabrics
open PpGen.Server.Generators.DiamondSquare
open Saturn

open PpGen.Server.Router


type ApplicationBuilder with
    [<CustomOperation "use_websockets">]
    member _.UseWebSockets(state: ApplicationState) =
        let middleware (app: IApplicationBuilder) =
            app.UseWebSockets()
        { state with AppConfigs = middleware :: state.AppConfigs }
    
    [<CustomOperation "use_websockets_options">]
    member _.UseWebSocketsOptions(state: ApplicationState, options) =
        let middleware (app: IApplicationBuilder) =
            app.UseWebSockets(options)
        { state with AppConfigs = middleware :: state.AppConfigs }
    

let endpointPipe = pipeline {
    plug head
    plug requestId
}

let app = application {
    pipe_through endpointPipe
    
//    error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
    url "http://0.0.0.0:8085/"
    memory_cache
    use_static "static"
    use_gzip
    use_websockets
    
    service_config (fun services ->
        services.AddTransient<IDiamondSquareHeightmapGeneratorFabric, DiamondSquareHeightmapGeneratorFabric>() |> ignore
        services
    )
    
    use_router DiamondSquare.router
}

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code