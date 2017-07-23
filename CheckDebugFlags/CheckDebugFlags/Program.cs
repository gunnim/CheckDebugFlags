using System;
using System.Reflection;
using System.IO;

namespace CheckDebugFlags
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                string path;

                if (args.Length > 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (args.Length == 1)
                {
                    path = args[0];
                }
                else
                {
                    path = ".";
                }

                var curDirFiles = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);

                foreach (var file in curDirFiles)
                {
                    string assemblyFullName = Path.GetFullPath(file);

                    try
                    {
                        Assembly assembly = Assembly.LoadFile(assemblyFullName);
                        var assemblyInformation = new AssemblyFlags(assembly);

                        if (assemblyInformation.JitTrackingEnabled
                        && !assemblyInformation.JitOptimized
                        && assemblyInformation.EditAndContinueEnabled)
                        {
                            Console.WriteLine(assemblyFullName + " Debug");
                        }
                    }
                    catch { }
                }

                return 0;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine(
                    @"CheckDebugFlags only accepts a single optional parameter. 
                        The directory to start check from.
                        When executed without parameters, the current directory is used."
                );

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
