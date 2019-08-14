namespace Yeah

open System
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.UI
open Aardvark.UI.Primitives
open Aardvark.Base.Rendering
open Yeah.Model

type Message =
    | ToggleModel
    | More


module App =
    let f a b = a + b

    let t = (1,2)
    let (one,two) = t


    let initial = { count = 0; currentModel = Box; cameraState = FreeFlyController.initial }

    let update (m : Model) (msg : Message) =
        match msg with
            | ToggleModel -> 
                match m.currentModel with
                    | Box -> { m with currentModel = Sphere }
                    | Sphere -> { m with currentModel = Box }
            | More -> 
                { m with count = m.count + 1 }

    let myView (m : Model) = 
        div [] [
            button [onClick (fun () -> More)] [text "s"]
            br []
            Svg.svg [attribute "width" "200"; attribute "height" "100"] [
                Svg.rect [
                    attribute "x" (string (m.count * 10))
                    attribute "width" "100"; 
                    attribute "height" "100"; 
                    attribute "style" "fill:blue;stroke:pink;stroke-width:5;fill-opacity:0.1;stroke-opacity:0.9"]
            ]
            ul [] [
                for i in 0 .. m.count - 1 do
                    yield li [] [text "a"]
            ]
        ]


    let view (m : MModel) =
        body [] [

            Incremental.div AttributeMap.empty
                ( 
                    let a = m.Current |> Mod.map myView 
                    AList.ofModSingle a
                )
        ]

    let app =
        {
            initial = initial
            update = update
            view = view
            threads = fun _ -> ThreadPool.empty
            unpersist = Unpersist.instance
        }