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
        public void SingleIntOverFFFF()
        {
            ICompiler comiler = new Compiler();

            var myProgram = "65536";

            var output = comiler.Compile(myProgram);
            var results = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.Equal("MOVW a1, 0x0", results[0]);
            Assert.Equal("MOVT a1, 0x1", results[1]);
            Assert.Equal("PUSH a1", results[2]);
        }

        [Fact]
        public void BasicMathAddition()
        {
            ICompiler comiler = new Compiler();

            var myProgram = "1 + 2";

            var output = comiler.Compile(myProgram);
            var results = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.Equal("MOVW a1, 0x1", results[0]);
            Assert.Equal("PUSH a1", results[1]);
            Assert.Equal("MOVW a1, 0x2", results[2]);
            Assert.Equal("PUSH a1", results[3]);
            Assert.Equal("POP a1", results[4]);
            Assert.Equal("POP a2", results[5]);
            Assert.Equal("ADDS a1, a1, a2", results[6]);
            Assert.Equal("PUSH a1", results[7]);
        }

        [Fact]
        public void BasicMathSubtraction()
        {
            ICompiler comiler = new Compiler();

            var myProgram = "2 - 1";

            var output = comiler.Compile(myProgram);
            var results = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.Equal("MOVW a1, 0x2", results[0]);
            Assert.Equal("PUSH a1", results[1]);
            Assert.Equal("MOVW a1, 0x1", results[2]);
            Assert.Equal("PUSH a1", results[3]);
            Assert.Equal("POP a1", results[4]);
            Assert.Equal("POP a2", results[5]);
            Assert.Equal("SUBS a1, a1, a2", results[6]);
            Assert.Equal("PUSH a1", results[7]);
        }
    }
}