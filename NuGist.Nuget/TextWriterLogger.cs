using NuGet.Common;
using System.IO;
using System.Text;

namespace NuGist.Nuget
{
    class TextWriterLogger : ILogger
    {
        private StringBuilder sb;

        public StringWriter Writer { get; set; }

        public TextWriterLogger()
        {
            this.sb = new StringBuilder();
            Writer = new StringWriter(sb);
        }

        public void LogDebug(string data)
        {
            Writer.WriteLine($"DEBUG: {data}");
        }

        public void LogError(string data)
        {
            Writer.WriteLine($"ERROR: {data}");
        }

        public void LogErrorSummary(string data)
        {
            Writer.WriteLine($"ERROR SUMMARY: {data}");
        }

        public void LogInformation(string data)
        {
            Writer.WriteLine($"INFORMATION: {data}");
        }

        public void LogInformationSummary(string data)
        {
            Writer.WriteLine($"INFORMATION SUMMARY: {data}");
        }

        public void LogMinimal(string data)
        {
            Writer.WriteLine($"MINIMAL: {data}");
        }

        public void LogVerbose(string data)
        {
            Writer.WriteLine($"VERBOSE: {data}");
        }

        public void LogWarning(string data)
        {
            Writer.WriteLine($"WARNING: {data}");
        }
    }
}
