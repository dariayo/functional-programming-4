module LogCollector.Parser

open System.Text.RegularExpressions
open System

type LogEntry =
    { Timestamp: DateTime
      LogLevel: string
      Message: string
      Source: string }

let logPattern =
    @"^(?<timestamp>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}Z)\s+(?<level>[A-Z]+)\s+\d+\s+---\s+\[\s*(?<thread>[^\]]+)\]\s+(?<source>[^:]+)\s*:\s+(?<message>.+)"



let parseLog (log: string) =
    let matchResult = Regex.Match(log, logPattern)
    printfn "%A" matchResult

    if matchResult.Success then
        Some
            { Timestamp = DateTime.Now // Логи без метки времени, поэтому используем текущую дату.
              LogLevel = matchResult.Groups.["level"].Value.ToUpper() // Преобразование уровня в верхний регистр.
              Message = matchResult.Groups.["message"].Value
              Source = matchResult.Groups.["source"].Value.Trim() }
    else
        None




let filterErrors (logs: string seq) =
    logs
    |> Seq.choose parseLog
    |> Seq.filter (fun log -> log.LogLevel = "ERROR")
