module UserInterface.API

open Giraffe
open LogCollector.Parser
open ReportGenerator.MarkdownReport
open Microsoft.AspNetCore.Http
open System
open System.IO

type ApiResponse<'T> =
    { Data: 'T
      Success: bool
      ErrorMessage: string option }

let getLogsHandler (next: HttpFunc) (ctx: HttpContext) =
    task {
        let logs =
            [ { Timestamp = DateTime.Now
                LogLevel = "INFO"
                Message = "Example log entry"
                Source = "Service1" } ]

        let response =
            { Data = logs
              Success = true
              ErrorMessage = None }

        return! json response next ctx
    }

let getReportsHandler (next: HttpFunc) (ctx: HttpContext) =
    task {
        let reportDir = "./reports"

        let reports =
            Directory.EnumerateFiles(reportDir, "*.md")
            |> Seq.map Path.GetFileName
            |> Seq.toList

        let response =
            { Data = reports
              Success = true
              ErrorMessage = None }

        return! json response next ctx
    }

let getReportContentHandler (reportName: string) (next: HttpFunc) (ctx: HttpContext) =
    task {
        let reportPath = Path.Combine("./reports", reportName)

        if File.Exists(reportPath) then
            let content = File.ReadAllText(reportPath)

            let response =
                { Data = content
                  Success = true
                  ErrorMessage = None }

            return! json response next ctx
        else
            let errorResponse =
                { Data = ""
                  Success = false
                  ErrorMessage = Some "Report not found" }

            return! json errorResponse next ctx
    }

let apiRoutes () =
    choose [ route "/api/logs" >=> getLogsHandler
             route "/api/reports" >=> getReportsHandler
             routef "/api/reports/%s" getReportContentHandler ]
