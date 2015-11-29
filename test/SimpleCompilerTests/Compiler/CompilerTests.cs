namespace SimpleCompilerTests.Compiler
{
    using SimpleCompiler.Compiler;
    using System;
    using Xunit;

    public class CompilerTests
    {
        [Fact]
        public void ReturnsString()
        {
            ICompiler comiler = new Compiler();

            var myProgram = "1";

            var output = comiler.Compile(myProgram);

            Assert.IsType(typeof(string), output);
        }

        [Fact]
        public void SingleInt()
        {
            ICompiler comiler = new Compiler();

            var myProgram = "1";

            var output = comiler.Compile(myProgram);
            var results = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.Equal("MOVW a1, 0x1", results[0]);
            Assert.Equal("PUSH a1", results[1]);
        }

        [Fact]
        public void BasicMath()
        {
            ICompiler comiler = new Compiler();

            var myProgram = "1 + 2";

            var output = comiler.Compile(myProgram);
            var results = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.Equal("MOVW a1, 0x1", results[0]);
            Assert.Equal("MOVW a2, 0x10", results[1]);
            Assert.Equal("ADDS a1, a1, a2", results[2]);
            Assert.Equal("PUSH a1", results[3]);
        }
    }
}