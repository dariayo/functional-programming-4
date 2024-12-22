module LogCollector.Storage

open System.IO
open System.Data.SQLite

let logDirectory = "./logs"

let initializeStorage () =
    if not (Directory.Exists(logDirectory)) then
        Directory.CreateDirectory(logDirectory) |> ignore

let saveLogsToFile containerName logs =
    let filePath = Path.Combine(logDirectory, $"{containerName}.log")
    File.WriteAllText(filePath, string logs)


let saveLogsToDatabase containerName logs =
    let connectionString = "Data Source=logdb.sqlite;Version=3;"
    use connection = new SQLiteConnection(connectionString)
    connection.Open()

    use cmd = connection.CreateCommand()
    cmd.CommandText <- "CREATE TABLE IF NOT EXISTS Logs (Id INTEGER PRIMARY KEY, ContainerName TEXT, LogText TEXT)"
    cmd.ExecuteNonQuery() |> ignore

    cmd.CommandText <- "INSERT INTO Logs (ContainerName, LogText) VALUES (@containerName, @logText)"

    cmd.Parameters.AddWithValue("@containerName", containerName)
    |> ignore

    cmd.Parameters.AddWithValue("@logText", logs)
    |> ignore

    cmd.ExecuteNonQuery() |> ignore
