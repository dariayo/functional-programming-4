module UserInterface.WebApp

open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open System.IO
open Microsoft.Extensions.FileProviders


let webApp =
    choose [ route "/"
             >=> text "Welcome to the Log Collector Dashboard!"
             route "/logs" >=> htmlView LogsView.indexPage
             route "/reports"
             >=> htmlView ReportsView.indexPage ]

let configureServices (builder: WebApplicationBuilder) = builder.Services.AddGiraffe() |> ignore

let configureApp (app: WebApplication) =
    let staticPath =
        Path.Combine(Directory.GetCurrentDirectory(), "UserInterface", "Static")

    printfn "Looking for static files in: %s" staticPath

    if not (Directory.Exists(staticPath)) then
        printfn "Directory not found: %s" staticPath
    else
        printfn "Serving static files from: %s" staticPath

    app
        .UseStaticFiles()
        .UseFileServer(FileServerOptions(FileProvider = new PhysicalFileProvider(staticPath)))
        .UseGiraffe(webApp)
