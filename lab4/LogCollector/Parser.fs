module LogCollector.Parser

open System.Text.RegularExpressions
open System

type LogEntry =
    { Timestamp: DateTime
      LogLevel: string
      Message: string
      Source: string }

let logPattern =
    @"^(?<timestamp>\S+ \S+) (?<level>[A-Z]+) \[(?<source>[^\]]+)\] (?<message>.+)$"

let parseLog (log: string) =
    let matchResult = Regex.Match(log, logPattern)

    if matchResult.Success then
        Some
            { Timestamp = DateTime.Parse(matchResult.Groups.["timestamp"].Value)
              LogLevel = matchResult.Groups.["level"].Value
              Message = matchResult.Groups.["message"].Value
              Source = matchResult.Groups.["source"].Value }
    else
        None

let filterErrors (logs: string seq) =
    logs
    |> Seq.choose parseLog
    |> Seq.filter (fun log -> log.LogLevel = "ERROR")
