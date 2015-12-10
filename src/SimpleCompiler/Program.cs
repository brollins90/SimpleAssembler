namespace SimpleCompiler
{
    using Compiler;
    using System;
    using System.IO;

    public class Program
    {
        public void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                // Values are available here
                if (options.Verbose) Console.WriteLine("Source: {0}", options.SourceFile);
                if (options.Verbose) Console.WriteLine("AssemblyFile: {0}", options.AssemblyFile);
                if (options.Verbose) Console.WriteLine("Output: {0}", options.OutputFile);

                Program p = new Program();
                p.Compile(options.SourceFile, options.AssemblyFile);
                p.Assemble(options.AssemblyFile, options.OutputFile);
            }
        }

        private void Compile(string sourceFile, string assemblyFile)
        {
            var comiler = new Compiler.Compiler();

            var myProgram = File.ReadAllText(sourceFile);

            var outputAssembly = comiler.Compile(myProgram);

            File.WriteAllText(assemblyFile, outputAssembly);
        }

        private void Assemble(string assemblyFile, string outputFile)
        {
            var parser = new SimpleAssembler.Parser.Parser();

            string fileText = File.ReadAllText(assemblyFile);

            try
            {
                var outputArray = parser.Parse(fileText);

                using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
                {
                    foreach (int i in outputArray)
                    {
                        binaryWriter.Write(i);
                    }
                }
                Console.WriteLine("***********");
                foreach (var labelPair in parser.LabelTable)
                {
                    Console.WriteLine($"{labelPair.Key}: {labelPair.Value}");
                }
            }
            catch
            {
                Console.WriteLine("error");
            }
        }
    }
}
