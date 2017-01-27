using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NuGist.Nuget
{
    public class PackParams
    {
        public string BasePath { get; set; }
        public string NuspecFileName { get; set; }
        public string Version { get; set; }
        public string OutputDirectory { get; set; }
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
    }

    public static class Commands
    {
        static string MinClientVersion = "3.0.0";
        static Version _minClientVersionValue;

        public static void Pack(string root, PackParams p)
        {
            var packArgs = new PackArgs();
            packArgs.Logger = new ConsoleLogger();
            packArgs.Arguments = new string[]
            {
                Path.Combine(p.BasePath, p.NuspecFileName),
                "-Version", p.Version
            };
            packArgs.OutputDirectory = p.OutputDirectory;
            packArgs.BasePath = p.BasePath;

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

        public static async Task Push(string path, ILogger logger)
        {
            var settings = new NullSettings();
            await PushRunner.Run(
                settings,
                new PackageSourceProvider(settings),
                path,
                "https://www.myget.org/F/nugist-test/api/v3/index.json", "",
                null, null, 60, false, true, logger);
        }
    }
}
