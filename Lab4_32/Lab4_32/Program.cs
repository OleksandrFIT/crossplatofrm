using ClassLibrary;

using McMaster.Extensions.CommandLineUtils;

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Lab4_32
{
    [Command(Name = "labs", Description = "Lab 4 project tool"),
    Subcommand(typeof(RunCommand), typeof(Version), typeof(SetPathCommand))]
    class Lab4Command
    {
        const string labPathStr = "LAB_PATH";
        public static void Main(string[] args) => CommandLineApplication.Execute<Lab4Command>(args);
        public static string GetPath()
        {
            var path = Environment.GetEnvironmentVariable(labPathStr);
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".labcfg");
                if (File.Exists(configPath))
                {
                    string config = File.ReadAllText(configPath);
                    var data = config.Split(':');
                    if (data.Length == 2 && data[0] == labPathStr)
                        return data[1];
                }
            }
            return "";
        }
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine("You must specify at a command.");
            console.WriteLine($"OSVersion.Platform: {Environment.OSVersion.Platform}");
            app.ShowHelp();
            return 1;
        }

        [Command("version", Description = "Show Lab 4 tool version")]
        private class Version
        {
            private int OnExecute(IConsole console)
            {
                console.WriteLine($"Version: {Assembly.GetExecutingAssembly().GetName().Version}");
                console.WriteLine("Lab 4 tool author: Oleksandr Drahan IPZ-43");

                console.WriteLine($"LAB_PATH: {GetPath()}");
                return 1;
            }
        }
        [Command("set-path", Description = "Set LAB_PATH for input.txt and output.txt")]
        private class SetPathCommand
        {
            [Option("--path -p", Description = "Set path to input and output directory")]
            [Required(ErrorMessage = "You must specify the path to files directory")]
            public string Path { get; } = null!;
            private int OnExecute(IConsole console)
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    string configPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".labcfg");

                    File.WriteAllText(configPath, $"{labPathStr}:{Path}");
                }
                else
                {
                    Environment.SetEnvironmentVariable(labPathStr, Path);
                }

                return 1;
            }
        }
        [Command("run", Description = "Starts the selected lab"), Subcommand(typeof(Lab1Command)), Subcommand(typeof(Lab2Command)), Subcommand(typeof(Lab3Command))
        ]
        private class RunCommand
        {

            private int OnExecute(IConsole console)
            {
                console.Error.WriteLine("You must specify lab to start. See run --help for more details.");
                return 1;
            }


            abstract class Lab
            {
                [Option("--input -i", Description = "Specify a path to input.txt")]
                public string InputPath { get; } = null!;

                [Option("--output -o", Description = "Specify a path to output.txt")]
                public string OutputPath { get; } = null!;

                protected List<string> ReadInputFile()
                {
                    string path = InputPath;
                    if (!File.Exists(InputPath))
                    {
                        var labpath = GetPath();
                        if (string.IsNullOrWhiteSpace(labpath))
                        {
                            labpath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        }
                        path = Path.Combine(labpath, "input.txt");
                    }

                    if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                    {
                        return new List<string>();
                    }
                    Console.WriteLine($"input file: {path}");

                    return File.ReadAllLines(path).ToList();
                }

                protected string WriteOuputFile(string output)
                {
                    string outputPath = OutputPath;
                    if (string.IsNullOrWhiteSpace(outputPath))
                    {
                        var labpath = GetPath();
                        if (string.IsNullOrWhiteSpace(labpath))
                        {
                            labpath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        }
                        if (Directory.Exists(labpath))
                        {
                            outputPath = Path.Combine(labpath, "output.txt");
                        }
                        else
                        {
                            outputPath = "output.txt";
                        }

                    }

                    File.WriteAllText(outputPath, output);
                    return outputPath;
                }
            }

            [Command("lab1", Description = "Run lab1")]
            private class Lab1Command : Lab
            {
                protected int OnExecute(IConsole console)
                {
                    List<string> inputData = ReadInputFile();
                    if (inputData.Count == 0)
                    {
                        console.WriteLine($"Not fount or empty input.txt");

                        return -1;
                    }
                    console.WriteLine($"Run lab1");

                    var result = Lab1.RunLab(inputData);

                    console.WriteLine($"Result: {result}");

                    var outputpath = WriteOuputFile(result);
                    console.WriteLine($"Write output in: {outputpath}");

                    return 1;
                }
            }

            [Command("lab2", Description = "Run lab2")]
            private class Lab2Command : Lab
            {
                protected int OnExecute(IConsole console)
                {
                    List<string> inputData = ReadInputFile();
                    if (inputData.Count == 0)
                    {
                        console.WriteLine($"Not fount or empty input.txt");

                        return -1;
                    }
                    console.WriteLine($"Run lab2");

                    var result = Lab2.RunLab(inputData);

                    console.WriteLine($"Result: {result}");

                    var outputpath = WriteOuputFile(result);
                    console.WriteLine($"Write output in: {outputpath}");
                    return 1;
                }
            }

            [Command("lab3", Description = "Run lab3")]
            private class Lab3Command : Lab
            {
                protected int OnExecute(IConsole console)
                {
                    List<string> inputData = ReadInputFile();
                    if (inputData.Count == 0)
                    {
                        console.WriteLine($"Not fount or empty input.txt");

                        return -1;
                    }
                    console.WriteLine($"Run lab3");

                    var result = Lab3.RunLab(inputData);

                    console.WriteLine($"Result: {result}");

                    var outputpath = WriteOuputFile(result);
                    console.WriteLine($"Write output in: {outputpath}");
                    return 1;
                }
            }
        }
    }
}