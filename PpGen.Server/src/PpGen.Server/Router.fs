module Router

open Fable.Remoting.Giraffe
open Fable.Remoting.Server
open PpGen.Api
open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters
open PpGen.Server.DiSq


let remotingApi: IApi =
    { GenerateDiSq = generateDiSq }

let remotingApiHandler =
    Remoting.createApi ()
    |> Remoting.fromValue remotingApi
    |> Remoting.buildHttpHandler

let api = pipeline {
    plug acceptJson
    set_header "x-pipeline-type" "Api"
}

let apiRouter = router {
    not_found_handler (text "Api 404")
    pipe_through api

    pipe_through remotingApiHandler
}

let appRouter = router {
    pipe_through apiRouter
//    forward "/api" apiRouter
}