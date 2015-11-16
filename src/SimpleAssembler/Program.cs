namespace SimpleAssembler
{
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
                if (options.Verbose) Console.WriteLine("Input: {0}", options.InputFile);
                if (options.Verbose) Console.WriteLine("Output: {0}", options.OutputFile);

                Program p = new Program();
                p.Go(options.InputFile, options.OutputFile);
            }
        }

        private void Go(string inputFile, string outputFile)
        {
            var parser = new Parser.Parser();

            string fileText = File.ReadAllText(inputFile);

            var outputArray = parser.Parse(fileText);

            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                foreach (int i in outputArray)
                {
                    binaryWriter.Write(i);
                }
            }
        }
    }
}
