namespace SimpleCompiler.Compiler
{
    using System;
    using System.Collections.Generic;
    using Parser.Expressions;
    using System.Text;
    public class Compiler : ICompiler
    {
        int KERNEL_SIZE = 0x3000;

        public List<uint> Kernel { get; private set; }
        public uint KernelIndex { get; private set; }
        public uint LineNumber { get; private set; }
        public Dictionary<string, uint> LabelTable { get; }

        public Compiler()
        {
            KernelIndex = 0;
            LineNumber = 1;
            LabelTable = new Dictionary<string, uint>();
        }

        public string Compile(string program)
        {
            Kernel = new List<uint>(KERNEL_SIZE);
            for (int i = 0; i < KERNEL_SIZE; i++) { Kernel.Add(0); }

            Parser.Parser parser = new Parser.Parser();
            string output;

            try
            {
                var tree = parser.Parse(program);
                output = CompileTree(tree);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error {e.Message} on line {LineNumber}");
                throw;
            }
            return output;
        }

        public string CompileTree(Expression tree)
        {
            string result = tree.GenerateCode();
            return result;
        }
    }
}