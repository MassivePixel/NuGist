using NuGet.Common;
using System;

namespace NuGist.Nuget
{
    public class ConsoleLogger : ILogger
    {
        public void LogDebug(string data) => Console.WriteLine($"Debug: {data}");

        public void LogError(string data) => Console.WriteLine($"Error: {data}");

        public void LogErrorSummary(string data) => Console.WriteLine($"Error summary: {data}");

        public void LogInformation(string data) => Console.WriteLine($"Information: {data}");

        public void LogInformationSummary(string data) => Console.WriteLine($"Information summary: {data}");

        public void LogMinimal(string data) => Console.WriteLine($"Minimal: {data}");

        public void LogVerbose(string data) => Console.WriteLine($"Verbose: {data}");

        public void LogWarning(string data) => Console.WriteLine($"Warning: {data}");
    }
}
