namespace SimpleCompilerTests.Compiler
{
    using SimpleCompiler.Compiler;
    using System;
    using Xunit;

    public class CompilerTests
    {
        [Fact]
        public void ReturnsUintArray()
        {
            ICompiler comiler = new Compiler();

            var myProgram = "1 + 2";

            var output = comiler.Compile(myProgram);

            Assert.IsType(typeof(uint[]), output);
        }
        [Fact]
        public void CompilesBasicAdition()
        {
            ICompiler comiler = new Compiler();

            var myProgram = "1 + 2";

            var output = comiler.Compile(myProgram);

            //Assert.Contains("loop", comiler.LabelTable.Keys);
            //Assert.Equal((uint)0, comiler.LabelTable["loop"]);
        }
    }
}