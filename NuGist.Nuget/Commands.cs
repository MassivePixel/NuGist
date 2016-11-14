using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NuGist.Nuget
{
    class ConsoleLogger : ILogger
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

    public class PackParams
    {
        public string NuspecFileName { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
    }

    public class Commands
    {
        static string MinClientVersion = "3.0.0";
        static Version _minClientVersionValue;

        public void Pack(string root, PackParams p)
        {
            var packArgs = new PackArgs();
            packArgs.Logger = new ConsoleLogger();
            packArgs.Arguments = new string[] { Path.Combine(root, "input", p.NuspecFileName), "-Version", p.Version };
            packArgs.OutputDirectory = Path.Combine(root, "output");
            packArgs.BasePath = Path.Combine(root, "input");

            // The directory that contains msbuild
            packArgs.MsBuildDirectory = null;// new Lazy<string>(() => @"C:\Program Files (x86)\MSBuild\14.0\Bin");

            // Get the input file
            packArgs.Path = PackCommandRunner.GetInputFile(packArgs);

            // Set the current directory if the files being packed are in a different directory
            PackCommandRunner.SetupCurrentDirectory(packArgs);

            Console.WriteLine(packArgs.Path);

            if (!string.IsNullOrEmpty(MinClientVersion))
            {
                if (!Version.TryParse(MinClientVersion, out _minClientVersionValue))
                {
                    throw new Exception("invalid version");
                }
            }

            packArgs.Build = false;
            packArgs.Exclude = new string[0];
            packArgs.ExcludeEmptyDirectories = true;
            packArgs.IncludeReferencedProjects = false;

            packArgs.LogLevel = LogLevel.Verbose;

            packArgs.MinClientVersion = _minClientVersionValue;
            packArgs.NoDefaultExcludes = true;
            packArgs.NoPackageAnalysis = true;
            if (p.Properties.Any())
            {
                packArgs.Properties.AddRange(p.Properties);
            }
            packArgs.Suffix = string.Empty;
            packArgs.Symbols = false;
            packArgs.Tool = false;

            if (!string.IsNullOrEmpty(p.Version))
            {
                NuGetVersion version;
                if (!NuGetVersion.TryParse(p.Version, out version))
                {
                    throw new Exception();
                }
                packArgs.Version = version.ToNormalizedString();
            }

            var packCommandRunner = new PackCommandRunner(packArgs, null);
            packCommandRunner.BuildPackage();
        }
    }
}
