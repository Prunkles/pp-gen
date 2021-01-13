module PpGen.Web.Program

open Browser
open Feliz


[<ReactComponent>]
let App () =
    Html.text "AAA"


ReactDOM.render(App, Dom.document.getElementById("app"))
