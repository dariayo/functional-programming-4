module ReportsView

open Giraffe.ViewEngine

let indexPage =
    html [] [
        head [] [
            title [] [ str "Reports" ]
        ]
        body [] [
            h1 [] [ str "Reports" ]
            p [] [ str "Здесь будут отображаться отчёты." ]
        ]
    ]
