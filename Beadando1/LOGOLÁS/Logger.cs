using System;
using System.IO;

public static class Logger
{
    private static readonly string logFilePath = "log.txt";

    public static void Log(string message)
    {
        try
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
        catch
        {
            // Hiba logolás közben (opcionális: ide is lehet másodlagos logot írni)
        }
    }
}
