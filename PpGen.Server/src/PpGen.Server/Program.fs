module Server

open Microsoft.AspNetCore.Builder
open PpGen.Api.FableSignalR
open PpGen.Server.FableSignalR
open Saturn
open Config
open Fable.SignalR

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
//    use_config (fun _ -> {connectionString = "DataSource=database.sqlite"} ) //TODO: Set development time configuration
    
    use_router Router.disqFableRemotingHandler
    use_router Router.disqRouter
//    use_signalr (
//        configure_signalr {
//            endpoint DiSq.endpoint
//            send Hub.send
//            invoke Hub.invoke
//            stream_from Hub.Stream.streamFrom
//        }
//    )
}

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code