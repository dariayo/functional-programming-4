module LogsView

open Giraffe.ViewEngine

let indexPage =
    html [] [
        head [] [
            title [] [ str "Logs" ]
        ]
        body [] [
            h1 [] [ str "Logs" ]
            p [] [ str "Здесь будут отображаться логи." ]
        ]
    ]
