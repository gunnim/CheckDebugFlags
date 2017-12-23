using Microsoft.Test.CommandLineParsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CheckDebugFlags
{
    class CommandLineArguments
    {
        /// <summary>
        /// Optionally specify the directory to begin scanning
        /// </summary>
        public string Path { get; set; }
        public string P { get; set; }

        /// <summary>
        /// Current directory and all subdirectories
        /// </summary>
        public bool? Subdirectories { get; set; }
        public bool? S { get; set; }

        /// <summary>
        /// Treat assemblies from different directories but with the same name as distinct
        /// </summary>
        public bool? Distinct { get; set; }
        public bool? D { get; set; }

        /// <summary>
        /// Optional search pattern. If included, this string is prepended to '.dll' and used for filename matching
        /// </summary>
        public string Regex { get; set; }
        public string R { get; set; }
    }

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var cliArgs = new CommandLineArguments();
                CommandLineParser.ParseArguments(cliArgs, args);

                var path = cliArgs.Path ?? cliArgs.P ?? ".";
                var searchOption = cliArgs?.Subdirectories == true || cliArgs?.S == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var searchPattern = cliArgs.Regex + cliArgs.R + "*.dll";
                var treatAllDistinct = cliArgs?.Distinct == true || cliArgs?.D == true;

                var curDirFiles = Directory.EnumerateFiles(path, searchPattern, searchOption);


                var assemblyList = new List<string>();
                var assemblyDic = new Dictionary<string, string>();

                foreach (var file in curDirFiles)
                {
                    string assemblyFullPath = Path.GetFullPath(file);

                    try
                    {
                        Assembly assembly = Assembly.LoadFile(assemblyFullPath);
                        var assemblyInformation = new AssemblyFlags(assembly);

                        if (assemblyInformation.JitTrackingEnabled
                        && !assemblyInformation.JitOptimized
                        && assemblyInformation.EditAndContinueEnabled)
                        {
                            if (treatAllDistinct)
                            {
                                assemblyList.Add(assemblyFullPath);
                            }
                            else
                            {
                                var assemblyName = Path.GetFileNameWithoutExtension(assemblyFullPath);

                                assemblyDic[assemblyName] = assemblyFullPath;
                            }
                        }
                    }
                    catch { }
                }

                if (Console.ForegroundColor == ConsoleColor.Gray)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (treatAllDistinct)
                {
                    foreach (var assembly in assemblyList)
                    {
                        Console.WriteLine(assembly);
                    }
                }
                else
                {
                    foreach (var assembly in assemblyDic)
                    {
                        Console.WriteLine(assembly.Value);
                    }
                }

                return 0;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("/P[ath]=<relative or absolute path> Optionally specify the directory to begin scanning");
                Console.WriteLine("/S[ubdirectories] Current directory and all subdirectories");
                Console.WriteLine("/D[istinct] Treat assemblies from different directories but with the same name as distinct");
                Console.WriteLine("/R[egex] Optional search pattern. If included, this string is prepended to '.dll' and used for filename matching");

                return 1;
            }
            catch (Exception)
            {
                Console.WriteLine(
                    "CheckDebugFlags hit an unhandled exception.."
                );

                return 2;
            }
        }
    }
}
