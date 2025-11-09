using System;
using System.IO;
using UrbanIndicatorsSystem.CodeGeneration;

namespace UrbanIndicatorsSystem.CodeGeneration.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║    Urban Indicators System - Code Generator           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            var projectRoot = FindProjectRoot();
            if (projectRoot == null)
            {
                Console.WriteLine("ERROR: Could not find project root directory");
                return;
            }

            var configPath = Path.Combine(projectRoot, "CodeGeneration", "EntityConfiguration.json");
            var outputPath = Path.Combine(projectRoot, "Generated");

            Console.WriteLine($"Project Root: {projectRoot}");
            Console.WriteLine($"Config Path:  {configPath}");
            Console.WriteLine($"Output Path:  {outputPath}");
            Console.WriteLine();

            var generator = new CodeGenerator(configPath, outputPath);
            
            try
            {
                generator.Generate();
                Console.WriteLine();
                Console.WriteLine("✓ Code generation completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"✗ Error during code generation: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Environment.Exit(1);
            }
        }

        static string FindProjectRoot()
        {
            var currentDir = Directory.GetCurrentDirectory();
            
            while (currentDir != null)
            {
                if (File.Exists(Path.Combine(currentDir, "UrbanIndicatorsSystem.csproj")))
                {
                    return currentDir;
                }
                
                var parent = Directory.GetParent(currentDir);
                currentDir = parent?.FullName;
            }
            
            return null;
        }
    }
}
